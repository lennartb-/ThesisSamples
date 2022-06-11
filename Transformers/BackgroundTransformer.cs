using System.Windows.Controls;
using System.Windows.Media;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Generators;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Transformers;

public class BackgroundTransformer : DocumentColorizingTransformer
{
    private readonly Brush background;
    private readonly RoiFinder roiFinder;

    public BackgroundTransformer(Augmentation parent, Brush background)
    {
        this.background = background;
        roiFinder = new RoiFinder(parent);
    }

    protected override void ColorizeLine(DocumentLine line)
    {
        var lineStartOffset = line.Offset;

        var regions = roiFinder.DetermineRangesOfInterest(CurrentContext.Document.Text).OrderBy(tuple => tuple.startOffset);

        foreach (var (startOffset, endOffset) in regions)
        {
            // Skip if region doesn't start on this line
            if (startOffset > line.EndOffset) continue;
            var clampedEnd = Math.Min(line.EndOffset, endOffset);
            var clampedStart = Math.Max(lineStartOffset, startOffset);

            ChangeLinePart(
                clampedStart,
                clampedEnd,
                element =>
                {
                    if (element is OverlayElement { Element: TextBlock tb })
                    {
                        tb.Background = background;
                    }
                    else
                    {
                        element.TextRunProperties.SetBackgroundBrush(background);
                    }
                });
        }
    }
}
