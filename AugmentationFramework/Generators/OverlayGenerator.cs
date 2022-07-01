using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using AugmentationFramework.AdviceDisplay;
using AugmentationFramework.Augmentations;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Generators;

public class OverlayGenerator : VisualLineElementGenerator
{
    public Func<UIElement>? CustomOverlay { get; internal set; }
    public Func<UIElement>? CustomTooltip { get; internal set; }
    public IAdviceModel? AdviceModel { get; internal set; }
    public Brush? TooltipBackground { get; internal set; }
    public string? TooltipText { get; internal set; }
    public string? OverlayText { get; internal set; }
    private readonly RoiFinder roiFinder;

    public OverlayGenerator(Augmentation parent)
    {
        roiFinder = new RoiFinder(parent);
    }

    public OverlayGenerator(Augmentation parent, Func<UIElement>? customOverlay)
    {
        CustomOverlay = customOverlay;
        roiFinder = new RoiFinder(parent);
    }

    public override int GetFirstInterestedOffset(int startOffset)
    {
        var area = roiFinder.DetermineRangesOfInterest(CurrentContext.Document.Text[startOffset..]).FirstOrDefault();

        if (area.startOffset + startOffset < startOffset)
        {
            return -1;
        }

        //TODO: Possible issue if match is at (0,0)
        if (area == default)
        {
            return -1;
        }

        return area.startOffset + startOffset;
    }

    public override VisualLineElement ConstructElement(int offset)
    {
        var (startOffset, endOffset) = roiFinder.DetermineRangesOfInterest(CurrentContext.Document.Text[offset..]).FirstOrDefault();

        var length = endOffset - startOffset;

        UIElement element;
        object? customTooltip = CustomTooltip != null ? CustomTooltip() : TooltipText;

        if (CustomOverlay is not null)
        {
            element = CustomOverlay();
            if (element is FrameworkElement fe)
            {
                fe.ToolTip = customTooltip;
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
                Background = TooltipBackground ?? Brushes.Transparent
            };

            if (AdviceModel is not null)
            {
                var model = AdviceModel.Clone();
                var popup = new ClosableAdvicePopup(model);
                model.WarningSource = CurrentContext.Document.GetText(offset, length) + "@ L" + CurrentContext.Document.GetLineByOffset(offset).LineNumber;
                var sp = new StackPanel();
                sp.Children.Add(element);
                sp.Children.Add(popup);

                sp.MouseDown += (sender, args) =>
                {
                    popup.IsOpen = true;
                };

                element = sp;
            }
        }

        var overlay = new OverlayElement(length, element);

        return overlay;
    }
}
