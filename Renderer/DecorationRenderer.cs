using System.Text.RegularExpressions;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using Brushes = System.Windows.Media.Brushes;

namespace RoslynPadTest.Renderer;

public class DecorationRenderer : IBackgroundRenderer
{
    private readonly Augmentation parent;

    public DecorationRenderer(Augmentation parent)
    {
        this.parent = parent;
    }

    public void Draw(TextView textView, DrawingContext drawingContext)
    {
        var builder = new BackgroundGeometryBuilder();

        var area = DetermineBackgroundArea();

        foreach (var (startOffset, endOffset) in area)
        {
            builder.AddSegment(textView, new TextSegment() { StartOffset = startOffset, EndOffset = endOffset});
            builder.CloseFigure();
        }

        var geometry = builder.CreateGeometry();
        if (geometry != null)
        {
            drawingContext.DrawGeometry(null, new Pen(Brushes.Purple,1d), geometry);
        }

    }

    public KnownLayer Layer { get; } = KnownLayer.Selection;

    private IEnumerable<(int startOffset, int endOffset)> DetermineBackgroundArea()
    {
        var visualLines = parent.TextView.VisualLines;

        int viewStart = visualLines.First().FirstDocumentLine.Offset;
        int viewEnd = visualLines.Last().LastDocumentLine.EndOffset;

        if (parent.TextMatchRegex is { } regex)
        {
            return DetermineRegexTextMatches(regex);
        }

        if (parent.TextMatch is { } text)
        {
            return DetermineTextMatches(text);
        }

        return new[] { (viewStart, viewEnd) };
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
        var visualLines = parent.TextView.VisualLines;
        int viewStart = visualLines.First().FirstDocumentLine.Offset;
        int viewEnd = visualLines.Last().LastDocumentLine.EndOffset;

        var startOffset = viewStart;
        var endOffset = viewEnd;
        var document = parent.TextView.Document;
        var relevantText = document.GetText(startOffset, endOffset - startOffset);
        return regex.Match(relevantText);
    }

    private int FindTextMatch(string text, int offset = 0)
    {
        var visualLines = parent.TextView.VisualLines;

        int viewStart = visualLines.First().FirstDocumentLine.Offset;
        int viewEnd = visualLines.Last().LastDocumentLine.EndOffset;

        var startOffset = viewStart;
        var endOffset = viewEnd;
        var document = parent.TextView.Document;
        var relevantText = document.GetText(startOffset, endOffset - startOffset);
        return relevantText.IndexOf(text, offset, StringComparison.CurrentCulture);
    }
}
