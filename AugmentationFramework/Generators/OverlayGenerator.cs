using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using AugmentationFramework.AdviceDisplay;
using AugmentationFramework.Augmentations;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Generators;

/// <summary>
///     Builds overlays over matching elements.
/// </summary>
public class OverlayGenerator : VisualLineElementGenerator
{
    private readonly RoiFinder roiFinder;

    /// <summary>
    ///     Initializes a new instance of the <see cref="OverlayGenerator" /> class.
    /// </summary>
    /// <param name="parent">The <see cref="Augmentation" /> this instance is based on.</param>
    public OverlayGenerator(Augmentation parent)
    {
        roiFinder = new RoiFinder(parent);
    }

    /// <summary>
    ///     Gets or sets an <see cref="IAdviceModel" /> to display.
    /// </summary>
    public IAdviceModel? AdviceModel { get; set; }

    /// <summary>
    ///     Gets or sets a delegate to build an overlay.
    /// </summary>
    public Func<UIElement>? CustomOverlay { get; set; }

    /// <summary>
    ///     Gets or sets a delegate to build a tooltip.
    /// </summary>
    public Func<UIElement>? CustomTooltip { get; set; }

    /// <summary>
    ///     Gets or sets a text to display as overlay.
    /// </summary>
    public string? OverlayText { get; set; }

    /// <summary>
    ///     Gets or sets the background brush of <see cref="CustomTooltip" />.
    /// </summary>
    public Brush? TooltipBackground { get; set; }

    /// <summary>
    ///     Gets or sets a text to display as tooltip.
    /// </summary>
    public string? TooltipText { get; set; }

    /// <inheritdoc />
    public override VisualLineElement ConstructElement(int offset)
    {
        var (startOffset, endOffset) = roiFinder.DetermineRangesOfInterest(CurrentContext.Document.Text[offset..]).FirstOrDefault();

        var length = endOffset - startOffset;

        UIElement element;
        object? customTooltip = CustomTooltip != null ? CustomTooltip() : TooltipText;

        if (CustomOverlay is not null)
        {
            element = CustomOverlay();
            if (element is FrameworkElement frameworkElement)
            {
                frameworkElement.ToolTip = customTooltip;
            }
        }
        else
        {
            element = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                Text = OverlayText ?? CurrentContext.Document.GetText(offset, length),
                FontSize = new VisualLineElementTextRunProperties(CurrentContext.GlobalTextRunProperties).FontHintingEmSize,
                ToolTip = customTooltip,
                Background = TooltipBackground ?? Brushes.Transparent,
            };

            if (BuildAdvicePopup(offset, startOffset, element) is { } advicePopup)
            {
                element = advicePopup;
            }
        }

        return new OverlayElement(length, element);
    }

    /// <inheritdoc />
    public override int GetFirstInterestedOffset(int startOffset)
    {
        var area = roiFinder.DetermineRangesOfInterest(CurrentContext.Document.Text[startOffset..]).FirstOrDefault();

        if (area.StartOffset + startOffset < startOffset)
        {
            return -1;
        }

        if (area == default)
        {
            return -1;
        }

        return area.StartOffset + startOffset;
    }

    private StackPanel? BuildAdvicePopup(int offset, int startOffset, UIElement element)
    {
        if (AdviceModel is null)
        {
            return null;
        }

        var model = AdviceModel.Clone();
        var popup = new ClosableAdvicePopup(model);
        model.WarningSource = CurrentContext.Document.GetText(startOffset, CurrentContext.Document.GetLineByOffset(offset).Length - 1) + "@ Line " +
                              CurrentContext.Document.GetLineByOffset(offset).LineNumber;
        var stackPanel = new StackPanel();
        stackPanel.Children.Add(element);
        stackPanel.Children.Add(popup);

        stackPanel.MouseDown += (_, _) =>
        {
            popup.Placement = PlacementMode.Mouse;
            popup.PlacementTarget = stackPanel;
            popup.IsOpen = true;
        };

        return stackPanel;
    }
}