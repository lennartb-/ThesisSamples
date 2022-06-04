using System.Text.RegularExpressions;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace RoslynPadTest.Transformers;

public class BackgroundTransformer : DocumentColorizingTransformer
{
    private readonly Augmentation parent;
    private readonly Brush background;

    public BackgroundTransformer(Augmentation parent, Brush background)
    {
        this.parent = parent;
        this.background = background;
    }

    protected override void ColorizeLine(DocumentLine line)
    {

        var lineStartOffset = line.Offset;

        var area = DetermineBackgroundArea();

        foreach (var (startOffset, endOffset) in area)
        {
            ChangeLinePart(
                lineStartOffset + startOffset,
                lineStartOffset + endOffset,
                element =>
                {
                    element.TextRunProperties.SetBackgroundBrush(background);
                });
        }
    }

    private IEnumerable<(int startOffset, int endOffset)> DetermineBackgroundArea()
    {
        if (parent.TextMatchRegex is { } regex)
        {
            return DetermineRegexTextMatches(regex);
        }

        if (parent.TextMatch is { } text)
        {
            return DetermineTextMatches(text);
        }

        return new[] { (CurrentContext.VisualLine.LastDocumentLine.Offset, CurrentContext.VisualLine.LastDocumentLine.EndOffset) };
    }

    private IEnumerable<(int startOffset, int endOffset)> DetermineTextMatches(string text)
    {
        var list = new List<(int, int)>();
        var startIndex = FindTextMatch(text);
        while (startIndex >= 0)
        {
            list.Add((startIndex, startIndex + text.Length));
            startIndex = FindTextMatch(text, startIndex + text.Length);
        }

        return list;
    }

    private IEnumerable<(int startOffset, int endOffset)> DetermineRegexTextMatches(Regex regex)
    {
        var list = new List<(int, int)>();
        var match = FindTextRegexMatch(regex);

        while (match.Success)
        {
            list.Add((match.Index, match.Index + match.Value.Length));

            match = match.NextMatch();
        }

        return list;
    }

    private Match FindTextRegexMatch(Regex regex)
    {
        var startOffset = CurrentContext.VisualLine.LastDocumentLine.Offset;
        var endOffset = CurrentContext.VisualLine.LastDocumentLine.EndOffset;
        var document = CurrentContext.Document;
        var relevantText = document.GetText(startOffset, endOffset - startOffset);
        return regex.Match(relevantText);
    }

    private int FindTextMatch(string text, int offset = 0)
    {
        var startOffset = CurrentContext.VisualLine.LastDocumentLine.Offset;
        var endOffset = CurrentContext.VisualLine.LastDocumentLine.EndOffset;
        var document = CurrentContext.Document;
        var relevantText = document.GetText(startOffset, endOffset - startOffset);
        return relevantText.IndexOf(text, offset, StringComparison.CurrentCulture);
    }
}
