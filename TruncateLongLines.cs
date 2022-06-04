using System.Windows.Media;
using ICSharpCode.AvalonEdit.Rendering;

namespace RoslynPadTest;

public class TruncateLongLines : VisualLineElementGenerator
{
    private readonly int _maxLength;
    const string Ellipsis = " ... ";

    public TruncateLongLines(int? maxLength = null)
    {
        _maxLength = maxLength ?? 10000;
    }

    public override int GetFirstInterestedOffset(int startOffset)
    {
        var line = CurrentContext.VisualLine.LastDocumentLine;
        if (line.Length > _maxLength)
        {
            var ellipsisOffset = line.Offset + _maxLength - Ellipsis.Length;
            if (startOffset <= ellipsisOffset)
            {
                return ellipsisOffset;
            }
        }
        return -1;
    }

    public override VisualLineElement ConstructElement(int offset)
    {
        var formattedTextElement = new FormattedTextElement(Ellipsis, CurrentContext.VisualLine.LastDocumentLine.EndOffset - offset)
        {
            BackgroundBrush = new SolidColorBrush(Color.FromArgb(153, 238, 144, 144))
        };

        return formattedTextElement;
    }
}