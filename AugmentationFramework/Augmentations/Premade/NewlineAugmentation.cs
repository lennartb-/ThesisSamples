using System.Text.RegularExpressions;
using System.Windows.Media;
using AugmentationFramework.AdviceDisplay;
using AugmentationFramework.Renderer.Premade;
using ICSharpCode.AvalonEdit.Editing;

namespace AugmentationFramework.Augmentations.Premade;

public class NewlineAugmentation
{
    public static Augmentation GetAugmentation(TextArea textArea)
    {
        return new Augmentation(textArea)
            .ForText(new Regex(@"(?<=\"").*(\\n).*(?=\"")"))
            .WithDecoration(UnderlineBracket.Geometry)
            .WithDecorationColor(Brushes.Orange)
            .WithAdviceOverlay(new NewlineAdviceModel());
    }
}