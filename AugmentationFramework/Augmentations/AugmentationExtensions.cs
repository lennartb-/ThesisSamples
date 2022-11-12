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
using Brush = System.Windows.Media.Brush;
using FontFamily = System.Windows.Media.FontFamily;
using FontStyle = System.Windows.FontStyle;

namespace AugmentationFramework.Augmentations;

public static class AugmentationExtensions
{
    public static Augmentation WithBackground(this Augmentation augmentation, Brush background)
    {
        if (augmentation.Transformers.OfType<ForegroundTransformer>().Any())
        {
            foreach (var existingGenerator in augmentation.Transformers.OfType<ForegroundTransformer>())
            {
                existingGenerator.Background = background;
            }

            return augmentation;
        }

        var foregroundTransformer = new ForegroundTransformer(augmentation) { Background = background };

        augmentation.AddLineTransformer(foregroundTransformer);

        return augmentation;
    }

    public static Augmentation WithForeground(this Augmentation augmentation, Brush foreground)
    {
        if (augmentation.Transformers.OfType<ForegroundTransformer>().Any())
        {
            foreach (var existingGenerator in augmentation.Transformers.OfType<ForegroundTransformer>())
            {
                existingGenerator.Foreground = foreground;
            }

            return augmentation;
        }

        var foregroundTransformer = new ForegroundTransformer(augmentation) { Foreground = foreground };

        augmentation.AddLineTransformer(foregroundTransformer);

        return augmentation;
    }

    public static Augmentation WithFontSize(this Augmentation augmentation, double fontSize)
    {
        if (augmentation.Transformers.OfType<ForegroundTransformer>().Any())
        {
            foreach (var existingGenerator in augmentation.Transformers.OfType<ForegroundTransformer>())
            {
                existingGenerator.FontSize = fontSize;
            }

            return augmentation;
        }

        var foregroundTransformer = new ForegroundTransformer(augmentation) { FontSize = fontSize };

        augmentation.AddLineTransformer(foregroundTransformer);

        return augmentation;
    }

    public static Augmentation WithFontWeight(this Augmentation augmentation, FontWeight fontWeight)
    {
        if (augmentation.Transformers.OfType<ForegroundTransformer>().Any())
        {
            foreach (var existingGenerator in augmentation.Transformers.OfType<ForegroundTransformer>())
            {
                existingGenerator.FontWeight = fontWeight;
            }

            return augmentation;
        }

        var foregroundTransformer = new ForegroundTransformer(augmentation) { FontWeight = fontWeight };

        augmentation.AddLineTransformer(foregroundTransformer);

        return augmentation;
    }

    public static Augmentation WithFontFamily(this Augmentation augmentation, FontFamily fontFamily)
    {
        if (augmentation.Transformers.OfType<ForegroundTransformer>().Any())
        {
            foreach (var existingGenerator in augmentation.Transformers.OfType<ForegroundTransformer>())
            {
                existingGenerator.FontFamily = fontFamily;
            }

            return augmentation;
        }

        var foregroundTransformer = new ForegroundTransformer(augmentation) { FontFamily = fontFamily };

        augmentation.AddLineTransformer(foregroundTransformer);

        return augmentation;
    }

    public static Augmentation WithFontStyle(this Augmentation augmentation, FontStyle fontStyle)
    {
        if (augmentation.Transformers.OfType<ForegroundTransformer>().Any())
        {
            foreach (var existingGenerator in augmentation.Transformers.OfType<ForegroundTransformer>())
            {
                existingGenerator.FontStyle = fontStyle;
            }

            return augmentation;
        }

        var foregroundTransformer = new ForegroundTransformer(augmentation) { FontStyle = fontStyle };

        augmentation.AddLineTransformer(foregroundTransformer);

        return augmentation;
    }

    public static Augmentation ForText(this Augmentation augmentation, string text)
    {
        augmentation.TextMatch = text;

        return augmentation;
    }

    public static Augmentation ForText(this Augmentation augmentation, Regex textMatch)
    {
        augmentation.TextMatchRegex = textMatch;

        return augmentation;
    }

    public static Augmentation ForText(this Augmentation augmentation, params Regex[] textMatch)
    {
        augmentation.TextMatchesRegex = textMatch;

        return augmentation;
    }

    public static Augmentation ForDelegate(this Augmentation augmentation, Func<Match, bool> regexMatcher)
    {
        augmentation.MatchingDelegate = regexMatcher;

        return augmentation;
    }

    public static Augmentation ForText(this Augmentation augmentation, params string[] textMatches)
    {
        augmentation.TextMatches = textMatches;

        return augmentation;
    }

    public static Augmentation WithDecorationColor(this Augmentation augmentation, Brush decorationColor)
    {
        if (augmentation.Renderers.OfType<DecorationRenderer>().Any())
        {
            foreach (var existingGenerator in augmentation.Renderers.OfType<DecorationRenderer>())
            {
                existingGenerator.DecorationColor = decorationColor;
            }

            return augmentation;
        }

        var toolTipGenerator = new DecorationRenderer(augmentation) { DecorationColor = decorationColor };
        augmentation.AddBackgroundRenderer(toolTipGenerator);

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
        augmentation.AddBackgroundRenderer(imageRenderer);

        return augmentation;
    }

    public static Augmentation OnRight(this Augmentation augmentation)
    {
        if (augmentation.Renderers.OfType<DecorationRenderer>().Any())
        {
            foreach (var existingRenderer in augmentation.Renderers.OfType<DecorationRenderer>())
            {
                existingRenderer.OnRight = true;
            }

            return augmentation;
        }

        var imageRenderer = new DecorationRenderer(augmentation) { OnRight = true };
        augmentation.AddBackgroundRenderer(imageRenderer);

        return augmentation;
    }

    public static Augmentation InLeftMargin(this Augmentation augmentation, ImageSource image)
    {
        var marginDecoration = new MarginDecoration(augmentation) { Image = image };
        augmentation.AddLeftMargin(marginDecoration);

        return augmentation;
    }

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

    public static Augmentation WithDecoration(this Augmentation augmentation, Func<Rect, Geometry> geometry)
    {
        if (augmentation.Renderers.OfType<DecorationRenderer>().Any())
        {
            foreach (var existingGenerator in augmentation.Renderers.OfType<DecorationRenderer>())
            {
                existingGenerator.GeometryDelegate = geometry;
            }

            return augmentation;
        }

        var toolTipGenerator = new DecorationRenderer(augmentation) { GeometryDelegate = geometry };
        augmentation.AddBackgroundRenderer(toolTipGenerator);

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

        var toolTipGenerator = new OverlayGenerator(augmentation, overlay);
        augmentation.AddElementGenerator(toolTipGenerator);

        return augmentation;
    }

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
}
