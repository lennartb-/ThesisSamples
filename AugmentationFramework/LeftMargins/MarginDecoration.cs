using System.Windows;
using System.Windows.Media;
using AugmentationFramework.Augmentations;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.LeftMargins;

/// <summary>
///     Places images in an additional margin on the left side.
/// </summary>
public class MarginDecoration : AbstractMargin
{
    private readonly Augmentation parent;
    private readonly RoiFinder roiFinder;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MarginDecoration" /> class.
    /// </summary>
    /// <param name="parent">The <see cref="Augmentation" /> this instance is based on.</param>
    public MarginDecoration(Augmentation parent)
    {
        this.parent = parent;
        roiFinder = new RoiFinder(parent);
    }

    /// <summary>
    ///     Gets or sets an <see cref="ImageSource" /> to display.
    /// </summary>
    public ImageSource? Image { get; set; }

    /// <inheritdoc />
    protected override Size MeasureOverride(Size availableSize)
    {
        if (Image == null)
        {
            return availableSize;
        }

        if (TextView is not { VisualLinesValid: true })
        {
            return availableSize;
        }

        var ranges = roiFinder.DetermineRangesOfInterest(TextView.Document.Text);
        var (startOffset, endOffset) = ranges.First();

        if (DrawingBoundsCalculator.GetScaledImageBoundsFromTextOffset(startOffset, endOffset, Image.Width, Image.Height, parent.TextView) is not { } imageBounds)
        {
            return availableSize;
        }

        return new Size(imageBounds.Width, imageBounds.Height);
    }

    /// <inheritdoc />
    protected override void OnRender(DrawingContext drawingContext)
    {
        if (Image == null)
        {
            return;
        }

        if (TextView is { VisualLinesValid: true })
        {
            var area = roiFinder.DetermineRangesOfInterest(TextView.Document.Text);

            foreach (var (startOffset, endOffset) in area)
            {
                DrawImage(drawingContext, startOffset, endOffset);
            }
        }
    }

    /// <inheritdoc />
    protected override void OnTextViewChanged(TextView? oldTextView, TextView? newTextView)
    {
        if (oldTextView != null)
        {
            oldTextView.VisualLinesChanged -= OnTextViewVisualLinesChanged;
        }

        base.OnTextViewChanged(oldTextView, newTextView);

        if (newTextView != null)
        {
            newTextView.VisualLinesChanged += OnTextViewVisualLinesChanged;
        }

        InvalidateVisual();
    }

    private void DrawImage(DrawingContext drawingContext, int startOffset, int endOffset)
    {
        if (Image == null)
        {
            return;
        }

        if (DrawingBoundsCalculator.GetScaledImageBoundsFromTextOffset(startOffset, endOffset, Image.Width, Image.Height, parent.TextView) is { } imageBounds)
        {
            // Clamp X to 0 since it should not be translated inside the margin.
            drawingContext.DrawImage(Image, imageBounds with { X = 0 });
        }
    }

    private void OnTextViewVisualLinesChanged(object? sender, EventArgs e)
    {
        InvalidateVisual();
    }
}