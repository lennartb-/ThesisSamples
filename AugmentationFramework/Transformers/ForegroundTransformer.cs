using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Generators;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Transformers;

public class ForegroundTransformer : DocumentColorizingTransformer
{
    private readonly RoiFinder roiFinder;

    public ForegroundTransformer(Augmentation parent)
    {
        roiFinder = new RoiFinder(parent);
    }

    public Brush? Foreground { get; set; }
    public Brush? Background { get; set; }
    public FontFamily? FontFamily { get; set; }
    public double? FontSize { get; set; }
    public FontWeight? FontWeight { get; set; }
    public FontStyle? FontStyle { get; set; }

    protected override void ColorizeLine(DocumentLine line)
    {
        var lineStartOffset = line.Offset;

        var regions = roiFinder.DetermineRangesOfInterest(CurrentContext.Document.Text).OrderBy(tuple => tuple.startOffset);

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
                        if (Background != null)
                        {
                            tb.Background = Background;
                        }

                        if (Foreground != null)
                        {
                            tb.Foreground = Foreground;
                        }

                        if (FontSize.HasValue)
                        {
                            tb.FontSize = FontSize.Value;
                        }

                        if (FontFamily != null)
                        {
                            tb.FontFamily = FontFamily;
                        }

                        if (FontWeight.HasValue)
                        {
                            tb.FontWeight = FontWeight.Value;
                        }

                        if (FontStyle.HasValue)
                        {
                            tb.FontStyle = FontStyle.Value;
                        }
                    }
                    else
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
                                tf.Stretch
                            ));
                    }
                });
        }
    }
}

internal static class Ext
{
    public static T IfNotNull<T>(this T obj, T? val) where T : struct
    {
        if (val.HasValue)
        {
            obj = val.Value;
        }

        return obj;
    }

    public static T IfNotNull<T>(this T obj, T? val) where T : class
    {
        if (val != null)
        {
            obj = val;
        }

        return obj;
    }
}

