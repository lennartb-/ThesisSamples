using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AugmentationFramework.AdviceDisplay;
using AugmentationFramework.Generators;
using AugmentationFramework.LeftMargins;
using AugmentationFramework.Renderer;
using AugmentationFramework.Transformers;

namespace AugmentationFramework.Augmentations;

/// <summary>
///     Provides extension methods to build <see cref="Augmentation" />s.
/// </summary>
public static class AugmentationExtensions
{
    /// <summary>
    ///     Sets the text the augmentation applies to.
    /// </summary>
    /// <param name="augmentation">The augmentation that applies to the text.</param>
    /// <param name="regexMatcher">
    ///     A delegate taking a <see cref="Match" /> and returning a value indicating whether the result
    ///     should apply to the augmentation.
    /// </param>
    /// <returns>The same instance of <paramref name="augmentation" />.</returns>
    public static Augmentation ForDelegate(this Augmentation augmentation, Func<Match, bool> regexMatcher)
    {
        augmentation.MatchingDelegate = regexMatcher;

        return augmentation;
    }

    /// <summary>
    ///     Sets the regular expressions the augmentation applies to.
    /// </summary>
    /// <param name="augmentation">The augmentation that applies to the matches found by <paramref name="textMatch" />.</param>
    /// <param name="textMatch">One or more regular expressions that the augmentation will match to.</param>
    /// <returns>The same instance of <paramref name="augmentation" />.</returns>
    public static Augmentation ForText(this Augmentation augmentation, params Regex[] textMatch)
    {
        augmentation.TextMatchesRegex = textMatch;

        return augmentation;
    }

    /// <summary>
    ///     Sets the text the augmentation applies to.
    /// </summary>
    /// <param name="augmentation">The augmentation that applies to the text.</param>
    /// <param name="textMatches">One or more strings with text.</param>
    /// <returns>The same instance of <paramref name="augmentation" />.</returns>
    public static Augmentation ForText(this Augmentation augmentation, params string[] textMatches)
    {
        augmentation.TextMatches = textMatches;

        return augmentation;
    }

    /// <summary>
    /// Places an augmentation with an image directly in the code editor.
    /// </summary>
    /// <param name="augmentation">The augmentation to place in the code editor.</param>
    /// <param name="image">The image to display.</param>
    /// <returns>The same instance of <paramref name="augmentation" />.</returns>
    public static Augmentation InCodeArea(this Augmentation augmentation, ImageSource image)
    {
        if (augmentation.Renderers.OfType<DecorationRenderer>().Any())
        {
            foreach (var existingRenderer in augmentation.Renderers.OfType<DecorationRenderer>())
            {
                existingRenderer.DrawInCodeArea = true;
            }

            return augmentation.WithImage(image);
        }

        var imageRenderer = new DecorationRenderer(augmentation) { DrawInCodeArea = true };
        augmentation.AddDecorationRenderer(imageRenderer);

        return augmentation.WithImage(image);
    }

    /// <summary>
    /// Places an augmentation with an image directly in a left margin.
    /// </summary>
    /// <param name="augmentation">The augmentation to place in a left margin.</param>
    /// <param name="image">The image to display.</param>
    /// <returns>The same instance of <paramref name="augmentation" />.</returns>
    public static Augmentation InLeftMargin(this Augmentation augmentation, ImageSource image)
    {
        if (augmentation.LeftMargins.OfType<MarginDecoration>().Any())
        {
            foreach (var existingRenderer in augmentation.LeftMargins.OfType<MarginDecoration>())
            {
                existingRenderer.Image = image;
            }

            return augmentation;
        }

        var marginDecoration = new MarginDecoration(augmentation) { Image = image };
        augmentation.AddLeftMargin(marginDecoration);

        return augmentation;
    }

    /// <summary>
    /// Places an augmentation with text directly in a left margin.
    /// </summary>
    /// <param name="augmentation">The augmentation to place in the code editor.</param>
    /// <param name="text">The text to display.</param>
    /// <param name="foreground">The text brush.</param>
    /// <param name="outline">The text outline brush.</param>
    /// <param name="fontName">The name of the font for the text.</param>
    /// <param name="fontSize">The size of the font for the text.</param>
    /// <returns>The same instance of <paramref name="augmentation" />.</returns>
    public static Augmentation InLeftMargin(this Augmentation augmentation, string text, Brush foreground, Brush outline, string fontName, int fontSize)
    {
        var textRun = new FormattedText(text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(fontName), fontSize, foreground, 96);

        var visual = new DrawingVisual();

        using (var drawingContext = visual.RenderOpen())
        {
            drawingContext.DrawText(textRun, new Point(0, 0));
            drawingContext.DrawGeometry(null, new Pen(outline, 1), textRun.BuildGeometry(new Point(0, 0)));
        }

        var bitmap = new RenderTargetBitmap(
            fontSize,
            fontSize,
            96,
            96,
            PixelFormats.Default);
        bitmap.Render(visual);

        return augmentation.InLeftMargin(bitmap);
    }

    /// <summary>
    /// Adds an advice overlay to the augmentation.
    /// </summary>
    /// <param name="augmentation">The augmentation to add the advice overlay to.</param>
    /// <param name="model">The advice model containing information for the advice display.</param>
    /// <returns>The same instance of <paramref name="augmentation" />.</returns>
    public static Augmentation WithAdviceOverlay(this Augmentation augmentation, IAdviceModel model)
    {
        if (augmentation.Generators.OfType<OverlayGenerator>().Any())
        {
            foreach (var existingGenerator in augmentation.Generators.OfType<OverlayGenerator>())
            {
                existingGenerator.AdviceModel = model;
            }

            return augmentation;
        }

        var toolTipGenerator = new OverlayGenerator(augmentation) { AdviceModel = model };
        augmentation.AddElementGenerator(toolTipGenerator);

        return augmentation;
    }

    /// <summary>
    ///     Adds a color background to the augmentation.
    /// </summary>
    /// <param name="augmentation">The augmentation to add the background color to.</param>
    /// <param name="background">A brush for the background.</param>
    /// <returns>The same instance of <paramref name="augmentation" />.</returns>
    public static Augmentation WithBackground(this Augmentation augmentation, Brush background)
    {
        if (augmentation.Transformers.OfType<TextTransformer>().Any())
        {
            foreach (var existingGenerator in augmentation.Transformers.OfType<TextTransformer>())
            {
                existingGenerator.Background = background;
            }

            return augmentation;
        }

        var foregroundTransformer = new TextTransformer(augmentation) { Background = background };

        augmentation.AddLineTransformer(foregroundTransformer);

        return augmentation;
    }

    /// <summary>
    ///     Adds a color decoration to the augmentation.
    /// </summary>
    /// <param name="augmentation">The augmentation that applies to the text.</param>
    /// <param name="geometry">A delegate taking a <see cref="Rect"/> and returning a <see cref="Geometry"/> that represents the decoration.</param>
    /// <param name="decorationColor">A brush for the decoration.</param>
    /// <returns>The same instance of <paramref name="augmentation" />.</returns>
    public static Augmentation WithDecoration(this Augmentation augmentation, Func<Rect, Geometry> geometry, Brush decorationColor)
    {
        if (augmentation.Renderers.OfType<DecorationRenderer>().Any())
        {
            foreach (var existingGenerator in augmentation.Renderers.OfType<DecorationRenderer>())
            {
                existingGenerator.GeometryDelegate = geometry;
                existingGenerator.GeometryBrush = decorationColor;
            }

            return augmentation;
        }

        var toolTipGenerator = new DecorationRenderer(augmentation) { GeometryDelegate = geometry, GeometryBrush = decorationColor};
        augmentation.AddDecorationRenderer(toolTipGenerator);

        return augmentation;
    }

    /// <summary>
    ///     Sets the font family for the augmentation.
    /// </summary>
    /// <param name="augmentation">The augmentation to apply the font family to.</param>
    /// <param name="fontFamily">The font family.</param>
    /// <returns>The same instance of <paramref name="augmentation" />.</returns>
    public static Augmentation WithFontFamily(this Augmentation augmentation, FontFamily fontFamily)
    {
        if (augmentation.Transformers.OfType<TextTransformer>().Any())
        {
            foreach (var existingGenerator in augmentation.Transformers.OfType<TextTransformer>())
            {
                existingGenerator.FontFamily = fontFamily;
            }

            return augmentation;
        }

        var foregroundTransformer = new TextTransformer(augmentation) { FontFamily = fontFamily };

        augmentation.AddLineTransformer(foregroundTransformer);

        return augmentation;
    }

    /// <summary>
    ///     Sets the font size for the augmentation.
    /// </summary>
    /// <param name="augmentation">The augmentation to apply the font size to.</param>
    /// <param name="fontSize">The font size in device independent units.</param>
    /// <returns>The same instance of <paramref name="augmentation" />.</returns>
    public static Augmentation WithFontSize(this Augmentation augmentation, double fontSize)
    {
        if (augmentation.Transformers.OfType<TextTransformer>().Any())
        {
            foreach (var existingGenerator in augmentation.Transformers.OfType<TextTransformer>())
            {
                existingGenerator.FontSize = fontSize;
            }

            return augmentation;
        }

        var foregroundTransformer = new TextTransformer(augmentation) { FontSize = fontSize };

        augmentation.AddLineTransformer(foregroundTransformer);

        return augmentation;
    }

    /// <summary>
    ///     Sets the font style for the augmentation.
    /// </summary>
    /// <param name="augmentation">The augmentation to apply the font style to.</param>
    /// <param name="fontStyle">The font style.</param>
    /// <returns>The same instance of <paramref name="augmentation" />.</returns>
    public static Augmentation WithFontStyle(this Augmentation augmentation, FontStyle fontStyle)
    {
        if (augmentation.Transformers.OfType<TextTransformer>().Any())
        {
            foreach (var existingGenerator in augmentation.Transformers.OfType<TextTransformer>())
            {
                existingGenerator.FontStyle = fontStyle;
            }

            return augmentation;
        }

        var foregroundTransformer = new TextTransformer(augmentation) { FontStyle = fontStyle };

        augmentation.AddLineTransformer(foregroundTransformer);

        return augmentation;
    }

    /// <summary>
    ///     Sets the font weight for the augmentation.
    /// </summary>
    /// <param name="augmentation">The augmentation to apply the font weight to.</param>
    /// <param name="fontWeight">The font weight.</param>
    /// <returns>The same instance of <paramref name="augmentation" />.</returns>
    public static Augmentation WithFontWeight(this Augmentation augmentation, FontWeight fontWeight)
    {
        if (augmentation.Transformers.OfType<TextTransformer>().Any())
        {
            foreach (var existingGenerator in augmentation.Transformers.OfType<TextTransformer>())
            {
                existingGenerator.FontWeight = fontWeight;
            }

            return augmentation;
        }

        var foregroundTransformer = new TextTransformer(augmentation) { FontWeight = fontWeight };

        augmentation.AddLineTransformer(foregroundTransformer);

        return augmentation;
    }

    /// <summary>
    ///     Adds a color foreground to the augmentation.
    /// </summary>
    /// <param name="augmentation">The augmentation to add the foreground color to.</param>
    /// <param name="foreground">A brush for the foreground.</param>
    /// <returns>The same instance of <paramref name="augmentation" />.</returns>
    public static Augmentation WithForeground(this Augmentation augmentation, Brush foreground)
    {
        if (augmentation.Transformers.OfType<TextTransformer>().Any())
        {
            foreach (var existingGenerator in augmentation.Transformers.OfType<TextTransformer>())
            {
                existingGenerator.Foreground = foreground;
            }

            return augmentation;
        }

        var foregroundTransformer = new TextTransformer(augmentation) { Foreground = foreground };

        augmentation.AddLineTransformer(foregroundTransformer);

        return augmentation;
    }

    public static Augmentation WithImage(this Augmentation augmentation, ImageSource image)
    {
        if (augmentation.Renderers.OfType<DecorationRenderer>().Any())
        {
            foreach (var existingRenderer in augmentation.Renderers.OfType<DecorationRenderer>())
            {
                existingRenderer.Image = image;
            }

            return augmentation;
        }

        var imageRenderer = new DecorationRenderer(augmentation) { Image = image };
        augmentation.AddDecorationRenderer(imageRenderer);

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

        var toolTipGenerator = new OverlayGenerator(augmentation) { OverlayText = overlayText };
        augmentation.AddElementGenerator(toolTipGenerator);

        return augmentation;
    }

    public static Augmentation WithOverlay(this Augmentation augmentation, Func<UIElement> overlay)
    {
        if (augmentation.Generators.OfType<OverlayGenerator>().Any())
        {
            foreach (var existingGenerator in augmentation.Generators.OfType<OverlayGenerator>())
            {
                existingGenerator.CustomOverlay = overlay;
            }

            return augmentation;
        }

        var toolTipGenerator = new OverlayGenerator(augmentation) { CustomOverlay = overlay };
        augmentation.AddElementGenerator(toolTipGenerator);

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

        var toolTipGenerator = new OverlayGenerator(augmentation) { TooltipText = tooltipText };
        augmentation.AddElementGenerator(toolTipGenerator);

        return augmentation;
    }

    public static Augmentation WithTooltip(this Augmentation augmentation, Func<UIElement> customTooltip)
    {
        if (augmentation.Generators.OfType<OverlayGenerator>().Any())
        {
            foreach (var existingGenerator in augmentation.Generators.OfType<OverlayGenerator>())
            {
                existingGenerator.CustomTooltip = customTooltip;
            }

            return augmentation;
        }

        var toolTipGenerator = new OverlayGenerator(augmentation) { CustomTooltip = customTooltip };
        augmentation.AddElementGenerator(toolTipGenerator);

        return augmentation;
    }
}