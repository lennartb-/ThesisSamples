using System.Windows;
using System.Windows.Media;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Renderer.Premade;
using ICSharpCode.AvalonEdit.Document;
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
    /// Initializes a new instance of the <see cref="DecorationRenderer"/> class.
    /// </summary>
    /// <param name="augmentation">The <see cref="Augmentation"/> this instance is based on.</param>
    public DecorationRenderer(Augmentation augmentation)
    {
        this.augmentation = augmentation;
        roiFinder = new RoiFinder(augmentation);
    }

    /// <summary>
    ///     Gets or sets the <see cref="Brush" /> used to draw <see cref="GeometryDelegate" />.
    /// </summary>
    public Brush GeometryBrush { get; set; } = Brushes.Transparent;

    /// <summary>
    ///     Gets or sets an <see cref="ImageSource" /> that is rendered.
    /// </summary>
    public ImageSource? Image { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether <see cref="Image" /> is drawn or not.
    /// </summary>
    public bool DrawInCodeArea { get; set; }

    /// <summary>
    ///     Gets or sets a delegate that takes a <see cref="Rect" /> as input and returns a <see cref="Geometry" /> based on
    ///     it.
    /// </summary>
    public Func<Rect, Geometry> GeometryDelegate { get; set; } = UnderlineBracket.Geometry;

    /// <inheritdoc />
    public void Draw(TextView textView, DrawingContext drawingContext)
    {
        var area = roiFinder.DetermineRangesOfInterest(textView.Document.Text);

        foreach (var (startOffset, endOffset) in area)
        {
            var textSegment = new TextSegment { StartOffset = startOffset, EndOffset = endOffset };

            var rects = BackgroundGeometryBuilder.GetRectsForSegment(augmentation.TextView, textSegment).ToList();
            if (!rects.Any())
            {
                continue;
            }

            var rect = rects.First();

            drawingContext.DrawGeometry(null, new Pen(GeometryBrush, 2d), GeometryDelegate(rect));
            DrawImage(drawingContext, startOffset, endOffset);
        }
    }

    /// <inheritdoc />
    public KnownLayer Layer => KnownLayer.Selection;

    private void DrawImage(DrawingContext drawingContext, int startOffset, int endOffset)
    {
        if ((Image == null) || !DrawInCodeArea)
        {
            return;
        }

        var textSegment = new TextSegment { StartOffset = startOffset, EndOffset = endOffset };

        var rects = BackgroundGeometryBuilder.GetRectsForSegment(augmentation.TextView, textSegment).ToList();

        if (!rects.Any())
        {
            return;
        }

        var rect = rects.First();

        var scale = Math.Min(rect.Width / Image.Width, rect.Height / Image.Height);

        var scaleWidth = (int)(Image.Width * scale);
        var scaleHeight = (int)(Image.Height * scale);

        if (DrawInCodeArea)
        {
            var r = new Rect(rect.X + rect.Width, rect.Y + ((rect.Height - scaleHeight) / 2), scaleWidth, scaleHeight);
            drawingContext.DrawImage(Image, r);
        }
    }
}

