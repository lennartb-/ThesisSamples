using System.Windows;
using System.Windows.Media.TextFormatting;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Generators;

public class OverlayElement : VisualLineElement
{
    public FrameworkElement Element { get; }

    public OverlayElement(int documentLength, FrameworkElement element)
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
