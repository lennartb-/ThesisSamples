using System.Text.RegularExpressions;
using System.Windows.Media;
using RoslynPadTest.Generators;
using RoslynPadTest.Renderer;
using RoslynPadTest.Transformers;

namespace RoslynPadTest.Augmentations;

public static class AugmentationExtensions
{
    public static Augmentation WithBackground(this Augmentation augmentation, Brush background)
    {
        var backgroundTransformer = new BackgroundTransformer(augmentation, background);

        augmentation.AddLineTransformer(backgroundTransformer);

        return augmentation;
    }

    public static Augmentation ForText(this Augmentation augmentation, string text)
    {
        augmentation.TextMatch = text;

        return augmentation;
    }

    public static Augmentation ForTextMatch(this Augmentation augmentation, Regex textMatch)
    {
        augmentation.TextMatchRegex = textMatch;

        return augmentation;
    }

    public static Augmentation WithDecoration(this Augmentation augmentation, Brush decorationColor)
    {
        var backgroundTransformer = new DecorationRenderer(augmentation, decorationColor);
        augmentation.AddBackgroundRenderer(backgroundTransformer);

        return augmentation;
    }

    public static Augmentation WithTooltip(this Augmentation augmentation, string tooltipText)
    {
        if (augmentation.Generators.OfType<OverlayGenerator>().Any())
        {
            foreach (var existingGenerator in augmentation.Generators.OfType<OverlayGenerator>())
            {
                existingGenerator.TooltipText = tooltipText;
            }

            return augmentation;
        }

        var toolTipGenerator = new OverlayGenerator(augmentation)
        {
            TooltipText = tooltipText
        };
        augmentation.AddElementGenerator(toolTipGenerator);

        return augmentation;
    }

    public static Augmentation WithOverlay(this Augmentation augmentation, string overlayText)
    {
        if (augmentation.Generators.OfType<OverlayGenerator>().Any())
        {
            foreach (var existingGenerator in augmentation.Generators.OfType<OverlayGenerator>())
            {
                existingGenerator.OverlayText = overlayText;
            }

            return augmentation;
        }

        var toolTipGenerator = new OverlayGenerator(augmentation)
        {
            OverlayText = overlayText
        };
        augmentation.AddElementGenerator(toolTipGenerator);

        return augmentation;
    }
}
