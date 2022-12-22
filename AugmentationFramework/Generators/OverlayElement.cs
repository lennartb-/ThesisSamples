using System.Windows;
using System.Windows.Media.TextFormatting;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Generators;

/// <summary>
/// A <see cref="VisualLineElement"/> that represents inline text and overlays text at a specific offset. Used by <see cref="OverlayGenerator"/>.
/// </summary>
internal class OverlayElement : VisualLineElement
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OverlayElement"/> class.
    /// </summary>
    /// <param name="documentLength">The length of the element in the document. Must be non-negative.</param>
    /// <param name="element">The element that this overlay displays.</param>
    public OverlayElement(int documentLength, UIElement element)
        : base(1, documentLength)
    {
        Element = element;
    }

    /// <summary>
    /// Gets the element this overlay displays.
    /// </summary>
    public UIElement Element { get; }

    /// <inheritdoc />
    public override TextRun CreateTextRun(int startVisualColumn, ITextRunConstructionContext context)
    {
        var textRunProperties = new VisualLineElementTextRunProperties(context.GlobalTextRunProperties);
        textRunProperties.SetBaselineAlignment(BaselineAlignment.TextBottom);

        return new InlineObjectRun(1, textRunProperties, Element);
    }
}