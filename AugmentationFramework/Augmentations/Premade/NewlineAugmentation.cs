using System.Text.RegularExpressions;
using System.Windows.Media;
using AugmentationFramework.AdviceDisplay;
using AugmentationFramework.Renderer.Premade;
using ICSharpCode.AvalonEdit.Editing;

namespace AugmentationFramework.Augmentations.Premade;

/// <summary>
/// Provides a pre-made demo augmentation that displays an advice over "\n" strings in a text.
/// </summary>
public static class NewlineAugmentation
{
    /// <summary>
    /// Gets a completely built augmentation that displays an advice over "\n" strings in a text.
    /// </summary>
    /// <param name="textArea">The text area the augmentation applies to.</param>
    /// <returns>A pre-built augmentation.</returns>
    public static Augmentation GetAugmentation(TextArea textArea)
    {
        return new Augmentation(textArea)
            .ForText(new Regex(@"(?<=\"").*(\\n).*(?=\"")"))
            .WithDecoration(UnderlineBracket.Geometry, Brushes.Orange)
            .WithAdviceOverlay(new NewlineAdviceModel());
    }
}