using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit.Rendering;

namespace RoslynPadTest;

public class TestGenerator : VisualLineElementGenerator
{
    private static readonly Regex imageRegex = new(@"\bbonk\b");

    public override int GetFirstInterestedOffset(int startOffset)
    {
        var match = FindMatch(startOffset);
        return match.Success ? startOffset + match.Index : -1;
    }

    public override VisualLineElement ConstructElement(int offset)
    {
        var match = FindMatch(offset);
        if (match.Success && (match.Index == 0))
        {
            var rect = new Rectangle() { Width = 30, Height = 10, Fill = Brushes.RoyalBlue, Opacity = 125};
            rect.ToolTip = "Nope";
            var overlay = new OverlayElement(match.Length, rect);

            return overlay;
        }

        return null;
    }

    private Match FindMatch(int startOffset)
    {
        var endOffset = CurrentContext.VisualLine.LastDocumentLine.EndOffset;
        var document = CurrentContext.Document;
        var relevantText = document.GetText(startOffset, endOffset - startOffset);
        return imageRegex.Match(relevantText);
    }
}
