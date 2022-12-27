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
    ///     Initializes a new instance of the <see cref="Augmentation" /> class.
    /// </summary>
    /// <param name="textArea">The <see cref="TextArea" /> this augmentation is rendered on.</param>
    public Augmentation(TextArea textArea)
    {
        TextArea = textArea;
        TextView = textArea.TextView;
    }

    /// <summary>
    ///     Gets a list of the <see cref="VisualLineElementGenerator" />s of this augmentation.
    /// </summary>
    public IList<VisualLineElementGenerator> Generators { get; } = new List<VisualLineElementGenerator>();

    /// <summary>
    ///     Gets a list of the <see cref="AbstractMargin" />s of this augmentation.
    /// </summary>
    public IList<AbstractMargin> LeftMargins { get; } = new List<AbstractMargin>();

    /// <summary>
    ///     Gets or sets a delegate that takes a regex <see cref="Match" /> and returns a value whether this augmentation
    ///     should be applied.
    /// </summary>
    public Func<Match, bool>? MatchingDelegate { get; set; }

    /// <summary>
    ///     Gets a list of the <see cref="IBackgroundRenderer" />s of this augmentation.
    /// </summary>
    public IList<IBackgroundRenderer> Renderers { get; } = new List<IBackgroundRenderer>();

    /// <summary>
    ///     Gets or sets a list of strings this augmentation matches.
    /// </summary>
    public IEnumerable<string>? TextMatches { get; set; }

    /// <summary>
    ///     Gets or sets a list of <see cref="Regex" />es this augmentation matches.
    /// </summary>
    public IEnumerable<Regex>? TextMatchesRegex { get; set; }

    /// <summary>
    ///     Gets the <see cref="TextView" /> this augmentation is rendered on.
    /// </summary>
    public TextView TextView { get; }

    /// <summary>
    ///     Gets a list of the <see cref="IVisualLineTransformer" />s of this augmentation.
    /// </summary>
    public IList<IVisualLineTransformer> Transformers { get; } = new List<IVisualLineTransformer>();

    /// <summary>
    ///     Gets the <see cref="TextArea" /> this augmentation is rendered on.
    /// </summary>
    private TextArea TextArea { get; }

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
    /// Adds a <see cref="DecorationRenderer"/> to the augmentation.
    /// </summary>
    /// <param name="renderer">The <see cref="DecorationRenderer"/> to add.</param>
    internal void AddDecorationRenderer(DecorationRenderer renderer)
    {
        Renderers.Add(renderer);
    }

    /// <summary>
    /// Adds a <see cref="VisualLineElementGenerator"/> to the augmentation.
    /// </summary>
    /// <param name="generator">The <see cref="VisualLineElementGenerator"/> to add.</param>
    internal void AddElementGenerator(VisualLineElementGenerator generator)
    {
        Generators.Add(generator);
    }

    /// <summary>
    /// Adds an <see cref="AbstractMargin"/> to the augmentation.
    /// </summary>
    /// <param name="leftMargin">The <see cref="AbstractMargin"/> to add.</param>
    internal void AddLeftMargin(AbstractMargin leftMargin)
    {
        LeftMargins.Add(leftMargin);
    }

    /// <summary>
    /// Adds an <see cref="IVisualLineTransformer"/> to the augmentation.
    /// </summary>
    /// <param name="transformer">The <see cref="IVisualLineTransformer"/> to add.</param>
    internal void AddLineTransformer(IVisualLineTransformer transformer)
    {
        Transformers.Add(transformer);
    }
}