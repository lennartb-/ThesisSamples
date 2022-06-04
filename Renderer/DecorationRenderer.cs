using System.Text.RegularExpressions;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace RoslynPadTest.Renderer;

public class DecorationRenderer : IBackgroundRenderer
{
    private readonly Augmentation parent;
    private readonly Brush decorationColor;

    public DecorationRenderer(Augmentation parent, Brush decorationColor)
    {
        this.parent = parent;
        this.decorationColor = decorationColor;
    }

    public void Draw(TextView textView, DrawingContext drawingContext)
    {
        var area = DetermineBackgroundArea();

        foreach (var (startOffset, endOffset) in area)
        {
            var textSegment = new TextSegment { StartOffset = startOffset, EndOffset = endOffset };

            var rects = BackgroundGeometryBuilder.GetRectsForSegment(parent.TextView, textSegment);
            var rect = rects.First();

            var underlineGeometry = new StreamGeometry();

            using (var ctx = underlineGeometry.Open())
            {
                ctx.BeginFigure(rect.BottomLeft with { Y = rect.BottomLeft.Y - 3 }, false, false);
                ctx.LineTo(rect.BottomLeft with { Y = rect.BottomLeft.Y  }, true, false);
                ctx.LineTo(rect.BottomRight with { Y = rect.BottomRight.Y  }, true, false);
                ctx.LineTo(rect.BottomRight with { Y = rect.BottomRight.Y -3 }, true, false);
            }

            underlineGeometry.Freeze();
            drawingContext.DrawGeometry(null, new Pen(decorationColor, 2d), underlineGeometry);
        }
    }

    public KnownLayer Layer => KnownLayer.Selection;

    private IEnumerable<(int startOffset, int endOffset)> DetermineBackgroundArea()
    {
        var visualLines = parent.TextView.VisualLines;

        var viewStart = visualLines.First().FirstDocumentLine.Offset;
        var viewEnd = visualLines.Last().LastDocumentLine.EndOffset;

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
        var viewStart = visualLines.First().FirstDocumentLine.Offset;
        var viewEnd = visualLines.Last().LastDocumentLine.EndOffset;

        var startOffset = viewStart;
        var endOffset = viewEnd;
        var document = parent.TextView.Document;
        var relevantText = document.GetText(startOffset, endOffset - startOffset);
        return regex.Match(relevantText);
    }

    private int FindTextMatch(string text, int offset = 0)
    {
        var visualLines = parent.TextView.VisualLines;

        var viewStart = visualLines.First().FirstDocumentLine.Offset;
        var viewEnd = visualLines.Last().LastDocumentLine.EndOffset;

        var startOffset = viewStart;
        var endOffset = viewEnd;
        var document = parent.TextView.Document;
        var relevantText = document.GetText(startOffset, endOffset - startOffset);
        return relevantText.IndexOf(text, offset, StringComparison.CurrentCulture);
    }
}
