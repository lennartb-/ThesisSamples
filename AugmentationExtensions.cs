using System.Text.RegularExpressions;
using System.Windows.Media;
using RoslynPadTest.Generators;
using RoslynPadTest.Renderer;
using RoslynPadTest.Transformers;

namespace RoslynPadTest;

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

    public static Augmentation WithTooltip(this Augmentation augmentation, string tooltipText, Brush decorationColor)
    {
        var toolTipGenerator = new ToolTipGenerator(augmentation, tooltipText, decorationColor);
        augmentation.AddElementGenerator(toolTipGenerator);

        return augmentation;
    }
}
