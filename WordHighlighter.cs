using System.Text.RegularExpressions;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace RoslynPadTest;

public class WordHighlighter : DocumentColorizingTransformer
{
    private readonly Regex textToHighlight;

    public WordHighlighter(Regex textToHighlight)
    {
        this.textToHighlight = textToHighlight;
    }

    protected override void ColorizeLine(DocumentLine line)
    {
       
        var lineStartOffset = line.Offset;
        var textOfCurrentLine = CurrentContext.Document.GetText(line);
        var start = 0;

        var match = FindMatch();
        if (!match.Success)
        {
            return;
        }

        int index;
        var matchedText = match.Value;
        while ((index = textOfCurrentLine.IndexOf(matchedText, start, StringComparison.Ordinal)) >= 0)
        {
            ChangeLinePart(
                lineStartOffset + index, 
                lineStartOffset + index + matchedText.Length, 
                element =>
                {
                    element.TextRunProperties.SetBackgroundBrush(Brushes.LightSkyBlue);
                });
            
            start = index + 1; 
        }

    }

    private Match FindMatch()
    {
        var startOffset = CurrentContext.VisualLine.LastDocumentLine.Offset;
        var endOffset = CurrentContext.VisualLine.LastDocumentLine.EndOffset;
        var document = CurrentContext.Document;
        var relevantText = document.GetText(startOffset, endOffset - startOffset);
        return textToHighlight.Match(relevantText);
    }
}
