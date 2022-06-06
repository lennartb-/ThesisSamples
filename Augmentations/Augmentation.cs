using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit.Rendering;
using RoslynPadTest.Renderer;

namespace RoslynPadTest.Augmentations;

public class Augmentation
{
    public IList<VisualLineElementGenerator> Generators { get; } = new List<VisualLineElementGenerator>();
    private readonly IList<IBackgroundRenderer> renderers = new List<IBackgroundRenderer>();

    private readonly IList<IVisualLineTransformer> transformers = new List<IVisualLineTransformer>();

    public Augmentation(TextView textView)
    {
        TextView = textView;
    }

    public Regex? TextMatchRegex { get; set; }

    public string? TextMatch { get; set; }

    public TextView TextView { get; }

    internal void AddLineTransformer(IVisualLineTransformer transformer)
    {
        transformers.Add(transformer);
    }

    public void Enable()
    {
        foreach (var visualLineTransformer in transformers)
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
        foreach (var visualLineTransformer in transformers)
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

    public void AddBackgroundRenderer(DecorationRenderer renderer)
    {
        renderers.Add(renderer);
    }

    public void AddElementGenerator(VisualLineElementGenerator generator)
    {
        Generators.Add(generator);
    }
}
