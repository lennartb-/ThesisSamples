using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit.Rendering;

namespace RoslynPadTest.Generators;

public class ToolTipGenerator : VisualLineElementGenerator
{
    private readonly Augmentation parent;
    private readonly Brush tooltipBackground;
    private readonly string tooltipText;

    public ToolTipGenerator(Augmentation parent, string tooltipText, Brush tooltipBackground)
    {
        this.parent = parent;
        this.tooltipText = tooltipText;
        this.tooltipBackground = tooltipBackground;
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

        var text = "";

        if (parent.TextMatchRegex is { } regex)
        {
            text = FindTextRegexMatch(regex, startOffset).Value;
        }

        if (parent.TextMatch is { } parentText)
        {
            text = parentText;
        }

        var tb = new TextBlock
        {
            VerticalAlignment = VerticalAlignment.Center,
            Text = text,
            FontSize = new VisualLineElementTextRunProperties(CurrentContext.GlobalTextRunProperties).FontHintingEmSize,
            ToolTip = tooltipText,
            Background = tooltipBackground
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
            return (match.Index, match.Index + match.Value.Length);
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
