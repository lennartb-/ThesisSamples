using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit.Rendering;
using RoslynPadTest.Renderer;

namespace RoslynPadTest;

public class Augmentation
{
    public Regex? TextMatchRegex { get; set; }

    public string? TextMatch { get; set; }

    public TextView TextView { get; }

    public Augmentation(TextView textView)
    {
        TextView = textView;
    }

    private readonly IList<IVisualLineTransformer> transformers = new List<IVisualLineTransformer>();
    private readonly IList<IBackgroundRenderer> renderers = new List<IBackgroundRenderer>();

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
    }

    public void AddBackgroundRenderer(DecorationRenderer renderer)
    {
        renderers.Add(renderer);
    }
}
