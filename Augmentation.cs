using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit.Rendering;

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
    }

    public void Disable()
    {
        foreach (var visualLineTransformer in transformers)
        {
            TextView.LineTransformers.Remove(visualLineTransformer);
        }
    }

}
