using System.Windows;
using System.Windows.Media;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Renderer.Premade;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Renderer;

/// <summary>
///     Renders <see cref="Geometry" /> and <see cref="ImageSource" />.
/// </summary>
public class DecorationRenderer : IBackgroundRenderer
{
    private readonly Augmentation augmentation;
    private readonly RoiFinder roiFinder;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DecorationRenderer" /> class.
    /// </summary>
    /// <param name="augmentation">The <see cref="Augmentation" /> this instance is based on.</param>
    public DecorationRenderer(Augmentation augmentation)
    {
        this.augmentation = augmentation;
        roiFinder = new RoiFinder(augmentation);
    }

    /// <summary>
    ///     Gets or sets a value indicating whether <see cref="Image" /> is drawn or not.
    /// </summary>
    public bool DrawInCodeArea { get; set; }

    /// <summary>
    ///     Gets or sets the <see cref="Brush" /> used to draw <see cref="GeometryDelegate" />.
    /// </summary>
    public Brush GeometryBrush { get; set; } = Brushes.Transparent;

    /// <summary>
    ///     Gets or sets a delegate that takes a <see cref="Rect" /> as input and returns a <see cref="Geometry" /> based on
    ///     it.
    /// </summary>
    public Func<Rect, Geometry> GeometryDelegate { get; set; } = UnderlineBracket.Geometry;

    /// <summary>
    ///     Gets or sets an <see cref="ImageSource" /> that is rendered.
    /// </summary>
    public ImageSource? Image { get; set; }

    /// <inheritdoc />
    public KnownLayer Layer => KnownLayer.Selection;

    /// <inheritdoc />
    public void Draw(TextView textView, DrawingContext drawingContext)
    {
        var area = roiFinder.DetermineRangesOfInterest(textView.Document.Text);

        foreach (var (startOffset, endOffset) in area)
        {
            if (DrawingBoundsCalculator.GetBoundsFromTextOffset(startOffset, endOffset, augmentation.TextView) is { } rect)
            {
                drawingContext.DrawGeometry(null, new Pen(GeometryBrush, 2d), GeometryDelegate(rect));
            }
            else
            {
                continue;
            }

            DrawImage(drawingContext, startOffset, endOffset);
        }
    }

    private void DrawImage(DrawingContext drawingContext, int startOffset, int endOffset)
    {
        if ((Image == null) || !DrawInCodeArea)
        {
            return;
        }

        if (DrawingBoundsCalculator.GetScaledImageBoundsFromTextOffset(startOffset, endOffset, Image.Width, Image.Height, augmentation.TextView) is { } imageBounds)
        {
            drawingContext.DrawImage(Image, imageBounds);
        }
    }
}