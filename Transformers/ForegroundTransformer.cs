
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Generators;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AugmentationFramework.Transformers;

public class ForegroundTransformer : DocumentColorizingTransformer
{
    private readonly RoiFinder roiFinder;

    public Brush? Foreground { get; set; }
    public Brush? Background { get; set; }
    public FontFamily? FontFamily { get; set; }
    public double? FontSize { get; set; }
    public FontWeight? FontWeight { get; set; }

    public ForegroundTransformer(Augmentation parent)
    {
        roiFinder = new RoiFinder(parent);
    }

    protected override void ColorizeLine(DocumentLine line)
    {
        var lineStartOffset = line.Offset;
        
        var area = roiFinder.DetermineRangesOfInterest(CurrentContext.Document.GetText(line.Offset,line.Length)).OrderBy(tuple => tuple.startOffset);
        
        foreach (var (startOffset, endOffset) in area)
        {
            ChangeLinePart(
                lineStartOffset + startOffset,
                lineStartOffset + endOffset,
                element =>
                {
                    if (element is OverlayElement { Element: TextBlock tb })
                    {
                        if (Foreground is not null)
                        {
                            tb.Foreground = Foreground;
                        }

                        if (Background is not null)
                        {
                            tb.Background = Background;
                        }

                        tb.FontSize.IfNotNull(FontSize);

                        if (FontFamily is not null)
                        {
                            tb.FontFamily = FontFamily;
                        }

                        tb.FontWeight.IfNotNull(FontWeight);
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

                        if (FontWeight.HasValue)
                        {
                            var tf = element.TextRunProperties.Typeface;
                            element.TextRunProperties.SetTypeface(new Typeface(
                                tf.FontFamily,
                                tf.Style,
                                FontWeight.Value,
                                tf.Stretch
                            ));
                        }

                    }
                });
        }
    }
}

internal static class Ext
{
    public static object IfNotNull(this object obj, object? val)
    {
        if (val != null)
        {
            obj = val;
        }

        return obj;
    }
}

