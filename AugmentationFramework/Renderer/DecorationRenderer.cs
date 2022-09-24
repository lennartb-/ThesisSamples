using System.Windows;
using System.Windows.Media;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Renderer.Premade;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Pen = System.Windows.Media.Pen;

namespace AugmentationFramework.Renderer;

public class DecorationRenderer : IBackgroundRenderer
{
    private readonly Augmentation parent;
    public Brush DecorationColor { get; set; } = Brushes.Transparent;
    public ImageSource? Image { get; set; }
    public bool OnRight { get; set; }
    private readonly RoiFinder roiFinder;
    public Func<Rect, Geometry> GeometryDelegate { get; set; } = UnderlineBracket.Geometry;

    public DecorationRenderer(Augmentation parent)
    {
        this.parent = parent;
        roiFinder = new RoiFinder(parent);
    }

    public void Draw(TextView textView, DrawingContext drawingContext)
    {
        var area = roiFinder.DetermineRangesOfInterest(textView.Document.Text);
        
        foreach (var (startOffset, endOffset) in area)
        {
            var textSegment = new TextSegment { StartOffset = startOffset, EndOffset = endOffset };

            var rects = BackgroundGeometryBuilder.GetRectsForSegment(parent.TextView, textSegment).ToList();
            if (!rects.Any())
            {
                continue;
            }

            var rect = rects.First();

            drawingContext.DrawGeometry(null, new Pen(DecorationColor, 2d), GeometryDelegate(rect));
            DrawImage(drawingContext, startOffset, endOffset);
        }
    }

    private void DrawImage(DrawingContext drawingContext, int startOffset, int endOffset)
    {
        if (Image == null || (!OnRight))
        {
            return;
        }

        var textSegment = new TextSegment { StartOffset = startOffset, EndOffset = endOffset };

        var rects = BackgroundGeometryBuilder.GetRectsForSegment(parent.TextView, textSegment).ToList();
        if (!rects.Any())
        {
            return;
        }
        var rect = rects.First();

        var scale = Math.Min(rect.Width / Image.Width, rect.Height / Image.Height);

        var scaleWidth = (int)(Image.Width * scale);
        var scaleHeight = (int)(Image.Height * scale);

        if (OnRight)
        {
            var r = new Rect(rect.X + rect.Width, rect.Y + (rect.Height - scaleHeight) / 2, scaleWidth, scaleHeight);
            drawingContext.DrawImage(Image, r);
        }

    }

    public KnownLayer Layer => KnownLayer.Selection;

}
