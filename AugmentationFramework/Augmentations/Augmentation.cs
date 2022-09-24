using System.Text.RegularExpressions;
using AugmentationFramework.Renderer;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Augmentations;

public class Augmentation
{
    public TextArea TextArea { get; }
    public IList<IBackgroundRenderer> Renderers { get; } = new List<IBackgroundRenderer>();

    public IList<IVisualLineTransformer> Transformers { get; } = new List<IVisualLineTransformer>();
    public IList<AbstractMargin> LeftMargins { get; } = new List<AbstractMargin>();

    public Augmentation(TextArea textArea)
    {
        TextArea = textArea;
        TextView = textArea.TextView;
    }

    public IList<VisualLineElementGenerator> Generators { get; } = new List<VisualLineElementGenerator>();

    public Regex? TextMatchRegex { get; set; }
    public IEnumerable<Regex>? TextMatchesRegex { get; set; }

    public string? TextMatch { get; set; }
    public IEnumerable<string>? TextMatches { get; set; }
    public Func<IEnumerable<string>>? TextMatchesFunc { get; set; }
    public Func<Match,bool>? MatchingDelegate { get; set; }

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

        foreach (var renderer in Renderers)
        {
            TextView.BackgroundRenderers.Add(renderer);
        }

        foreach (var generator in Generators)
        {
            TextView.ElementGenerators.Add(generator);
        }

        foreach (var leftMargin in LeftMargins)
        {
            TextArea.LeftMargins.Add(leftMargin);
        }
    }

    public void Disable()
    {
        foreach (var visualLineTransformer in Transformers)
        {
            TextView.LineTransformers.Remove(visualLineTransformer);
        }

        foreach (var renderer in Renderers)
        {
            TextView.BackgroundRenderers.Remove(renderer);
        }

        foreach (var generator in Generators)
        {
            TextView.ElementGenerators.Remove(generator);
        }

        foreach (var leftMargin in LeftMargins)
        {
            TextArea.LeftMargins.Remove(leftMargin);
        }
    }

    internal void AddBackgroundRenderer(DecorationRenderer renderer)
    {
        Renderers.Add(renderer);
    }

    internal void AddElementGenerator(VisualLineElementGenerator generator)
    {
        Generators.Add(generator);
    }

    internal void AddLeftMargin(AbstractMargin leftMargin)
    {
        LeftMargins.Add(leftMargin);
    }
}
