using System.Text.RegularExpressions;
using System.Windows.Media;
using AugmentationFramework.AdviceDisplay;
using AugmentationFramework.Renderer.Premade;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Augmentations.Premade;

public class NewlineAugmentation
{
    public static Augmentation GetAugmentation(TextView textView)
    {
        return new Augmentation(textView)
            .ForText(new Regex(@"(?<=\"").*(\\n).*(?=\"")"))
            .WithDecoration(UnderlineBracket.Geometry)
            .WithDecorationColor(Brushes.Orange)
            .WithAdviceOverlay(new NewlineAdviceModel());

    }
}
