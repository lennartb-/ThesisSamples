using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit.Rendering;

namespace RoslynPadTest.Generators;

public class OverlayGenerator : VisualLineElementGenerator
{
    private readonly Augmentation parent;
    public Brush? TooltipBackground { get; internal set; }
    public string? TooltipText { get; internal set; }
    public string? OverlayText { get; internal set; }

    public OverlayGenerator(Augmentation parent)
    {
        this.parent = parent;
    }

    public override int GetFirstInterestedOffset(int startOffset)
    {
        var area = DetermineBackgroundArea(startOffset);

        if (area.startOffset < startOffset)
        {
            return -1;
        }

        return area.startOffset;
    }

    public override VisualLineElement ConstructElement(int offset)
    {
        var (startOffset, endOffset) = DetermineBackgroundArea(offset);

        var length = endOffset - startOffset;

        var tb = new TextBlock
        {
            VerticalAlignment = VerticalAlignment.Center,
            Text = OverlayText,
            FontSize = new VisualLineElementTextRunProperties(CurrentContext.GlobalTextRunProperties).FontHintingEmSize,
            ToolTip = TooltipText,
            Background = TooltipBackground ?? Brushes.Transparent
        };

        var overlay = new OverlayElement(length, tb);

        return overlay;
    }

    private (int startOffset, int endOffset) DetermineBackgroundArea(int startOffset)
    {
        var visualLines = CurrentContext.VisualLine;

        var viewStart = visualLines.FirstDocumentLine.Offset;
        var viewEnd = visualLines.LastDocumentLine.EndOffset;

        if (parent.TextMatchRegex is { } regex)
        {
            return DetermineRegexTextMatches(regex, startOffset);
        }

        if (parent.TextMatch is { } text)
        {
            return DetermineTextMatches(text, startOffset);
        }

        return (viewStart, viewEnd);
    }

    private (int startOffset, int endOffset) DetermineTextMatches(string text, int startOffset)
    {
        var startIndex = FindTextMatch(text, startOffset);
        if (startIndex >= 0)
        {
            return (startOffset, text.Length);
        }

        return (0, 0);
    }

    private (int startOffset, int endOffset) DetermineRegexTextMatches(Regex regex, int offset)
    {
        var match = FindTextRegexMatch(regex, offset);

        if (match.Success)
        {
            return (match.Index + offset, match.Index + match.Value.Length + offset);
        }

        return (0, 0);
    }

    private Match FindTextRegexMatch(Regex regex, int offset)
    {
        var visualLines = CurrentContext.VisualLine;

        var viewEnd = visualLines.LastDocumentLine.EndOffset;

        var endOffset = viewEnd;
        var document = parent.TextView.Document;
        var relevantText = document.GetText(offset, endOffset - offset);
        return regex.Match(relevantText);
    }

    private int FindTextMatch(string text, int offset = 0)
    {
        var visualLines = CurrentContext.VisualLine;

        var viewStart = visualLines.FirstDocumentLine.Offset;
        var viewEnd = visualLines.LastDocumentLine.EndOffset;

        var startOffset = viewStart;
        var endOffset = viewEnd;
        var document = parent.TextView.Document;
        var relevantText = document.GetText(startOffset, endOffset - startOffset);
        return relevantText.IndexOf(text, offset, StringComparison.CurrentCulture);
    }
}
