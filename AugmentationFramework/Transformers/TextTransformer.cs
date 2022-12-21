using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Generators;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Transformers;

/// <summary>
/// Formats text elements.
/// </summary>
public class TextTransformer : DocumentColorizingTransformer
{
    private readonly RoiFinder roiFinder;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextTransformer"/> class.
    /// </summary>
    /// <param name="parent">The <see cref="Augmentation"/> this instance is based on.</param>
    public TextTransformer(Augmentation parent)
    {
        roiFinder = new RoiFinder(parent);
    }

    /// <summary>
    /// Gets or sets the foreground brush to apply.
    /// </summary>
    public Brush? Foreground { get; set; }

    /// <summary>
    /// Gets or sets the background brush to apply.
    /// </summary>
    public Brush? Background { get; set; }

    /// <summary>
    /// Gets or sets the font family to apply.
    /// </summary>
    public FontFamily? FontFamily { get; set; }

    /// <summary>
    /// Gets or sets the font size to apply.
    /// </summary>
    public double? FontSize { get; set; }

    /// <summary>
    /// Gets or sets the font weight to apply.
    /// </summary>
    public FontWeight? FontWeight { get; set; }

    /// <summary>
    /// Gets or sets the font style to apply.
    /// </summary>
    public FontStyle? FontStyle { get; set; }

    /// <inheritdoc />
    protected override void ColorizeLine(DocumentLine line)
    {
        var lineStartOffset = line.Offset;

        var regions = roiFinder.DetermineRangesOfInterest(CurrentContext.Document.Text).OrderBy(tuple => tuple.StartOffset);

        foreach (var (startOffset, endOffset) in regions)
        {
            // Skip if region doesn't start on this line
            if (startOffset > line.EndOffset)
            {
                continue;
            }

            var clampedEnd = line.EndOffset < endOffset ? line.EndOffset : Math.Max(lineStartOffset, endOffset);
            var clampedStart = Math.Max(lineStartOffset, startOffset);
            ChangeLinePart(
                clampedStart,
                clampedEnd,
                element =>
                {
                    if (element is OverlayElement { Element: TextBlock tb })
                    {
                        SetOverlayElementProperties(tb);
                    }
                    else
                    {
                        SetProperties(element);
                    }
                });
        }
    }

    private void SetProperties(VisualLineElement element)
    {
        if (Foreground is not null)
        {
            element.TextRunProperties.SetForegroundBrush(Foreground);
        }

        if (Background is not null)
        {
            element.TextRunProperties.SetBackgroundBrush(Background);
        }

        if (FontSize.HasValue)
        {
            element.TextRunProperties.SetFontRenderingEmSize(FontSize.Value);
        }

        var tf = element.TextRunProperties.Typeface;
        element.TextRunProperties.SetTypeface(
            new Typeface(
                tf.FontFamily.IfNotNull(FontFamily),
                tf.Style.IfNotNull(FontStyle),
                tf.Weight.IfNotNull(FontWeight),
                tf.Stretch));
    }

    private void SetOverlayElementProperties(TextBlock textBlock)
    {
        if (Background != null)
        {
            textBlock.Background = Background;
        }

        if (Foreground != null)
        {
            textBlock.Foreground = Foreground;
        }

        if (FontSize.HasValue)
        {
            textBlock.FontSize = FontSize.Value;
        }

        if (FontFamily != null)
        {
            textBlock.FontFamily = FontFamily;
        }

        if (FontWeight.HasValue)
        {
            textBlock.FontWeight = FontWeight.Value;
        }

        if (FontStyle.HasValue)
        {
            textBlock.FontStyle = FontStyle.Value;
        }
    }
}