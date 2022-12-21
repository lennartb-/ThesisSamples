using System.Text.RegularExpressions;
using AugmentationFramework.Renderer;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Augmentations;

/// <summary>
///     Describes a single augmentation that can be applied to a text string.
/// </summary>
public class Augmentation
{
    /// <summary>
    ///     Creates a new instance of the <see cref="Augmentation" /> class.
    /// </summary>
    /// <param name="textArea">The <see cref="TextArea" /> this augmentation is rendered on.</param>
    public Augmentation(TextArea textArea)
    {
        TextArea = textArea;
        TextView = textArea.TextView;
    }

    /// <summary>
    ///     Gets the <see cref="TextArea" /> this augmentation is rendered on.
    /// </summary>
    private TextArea TextArea { get; }

    /// <summary>
    ///     Gets a list of the <see cref="IBackgroundRenderer" />s of this augmentation.
    /// </summary>
    public IList<IBackgroundRenderer> Renderers { get; } = new List<IBackgroundRenderer>();

    /// <summary>
    ///     Gets a list of the <see cref="IVisualLineTransformer" />s of this augmentation.
    /// </summary>
    public IList<IVisualLineTransformer> Transformers { get; } = new List<IVisualLineTransformer>();

    /// <summary>
    ///     Gets a list of the <see cref="AbstractMargin" />s of this augmentation.
    /// </summary>
    public IList<AbstractMargin> LeftMargins { get; } = new List<AbstractMargin>();

    /// <summary>
    ///     Gets a list of the <see cref="VisualLineElementGenerator" />s of this augmentation.
    /// </summary>
    public IList<VisualLineElementGenerator> Generators { get; } = new List<VisualLineElementGenerator>();

    /// <summary>
    ///     Gets or sets the <see cref="Regex" /> this augmentation matches.
    /// </summary>
    public Regex? TextMatchRegex { get; set; }

    /// <summary>
    ///     Gets or sets a list of <see cref="Regex" />es this augmentation matches.
    /// </summary>
    public IEnumerable<Regex>? TextMatchesRegex { get; set; }

    /// <summary>
    ///     Gets or sets a string this augmentation matches.
    /// </summary>
    public string? TextMatch { get; set; }

    /// <summary>
    ///     Gets or sets a list of strings this augmentation matches.
    /// </summary>
    public IEnumerable<string>? TextMatches { get; set; }

    /// <summary>
    ///     Gets or sets a delegate that takes a regex <see cref="Match" /> and returns a value whether this augmentation
    ///     should be applied.
    /// </summary>
    public Func<Match, bool>? MatchingDelegate { get; set; }

    /// <summary>
    ///     Gets the <see cref="TextView" /> this augmentation is rendered on.
    /// </summary>
    public TextView TextView { get; }

    internal void AddLineTransformer(IVisualLineTransformer transformer)
    {
        Transformers.Add(transformer);
    }

    /// <summary>
    ///     Enables the effects of this augmentation.
    /// </summary>
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

    /// <summary>
    ///     Disables the effects of this augmentation.
    /// </summary>
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