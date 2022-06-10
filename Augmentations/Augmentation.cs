using System.Text.RegularExpressions;
using AugmentationFramework.Renderer;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Augmentations;

public class Augmentation
{
    private readonly IList<IBackgroundRenderer> renderers = new List<IBackgroundRenderer>();

    public IList<IVisualLineTransformer> Transformers { get; } = new List<IVisualLineTransformer>();

    public Augmentation(TextView textView)
    {
        TextView = textView;
    }

    public IList<VisualLineElementGenerator> Generators { get; } = new List<VisualLineElementGenerator>();

    public Regex? TextMatchRegex { get; set; }
    public IEnumerable<Regex>? TextMatchesRegex { get; set; }

    public string? TextMatch { get; set; }
    public IEnumerable<string>? TextMatches { get; set; }

    public TextView TextView { get; }

    internal void AddLineTransformer(IVisualLineTransformer transformer)
    {
        Transformers.Add(transformer);
    }

    public void Enable()
    {
        foreach (var visualLineTransformer in Transformers)
        {
            TextView.LineTransformers.Add(visualLineTransformer);
        }

        foreach (var renderer in renderers)
        {
            TextView.BackgroundRenderers.Add(renderer);
        }

        foreach (var generator in Generators)
        {
            TextView.ElementGenerators.Add(generator);
        }
    }

    public void Disable()
    {
        foreach (var visualLineTransformer in Transformers)
        {
            TextView.LineTransformers.Remove(visualLineTransformer);
        }

        foreach (var renderer in renderers)
        {
            TextView.BackgroundRenderers.Remove(renderer);
        }

        foreach (var generator in Generators)
        {
            TextView.ElementGenerators.Remove(generator);
        }
    }

    internal void AddBackgroundRenderer(DecorationRenderer renderer)
    {
        renderers.Add(renderer);
    }

    internal void AddElementGenerator(VisualLineElementGenerator generator)
    {
        Generators.Add(generator);
    }
}
