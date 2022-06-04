using System.Windows;
using System.Windows.Media.TextFormatting;
using ICSharpCode.AvalonEdit.Rendering;

namespace RoslynPadTest;

public class OverlayElement : VisualLineElement
{
    public UIElement Element { get; }

    public OverlayElement(int documentLength, UIElement element)
        : base(1, documentLength)
    {
        Element = element;
    }

    public override TextRun CreateTextRun(int startVisualColumn, ITextRunConstructionContext context)
    {
        var textRunProperties = new VisualLineElementTextRunProperties(context.GlobalTextRunProperties);
        textRunProperties.SetBaselineAlignment(BaselineAlignment.TextBottom);

        return new InlineObjectRun(1, textRunProperties, Element);
    }
}
