using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
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
            if (!rects.Any()) continue;

            var rect = rects.First();

            drawingContext.DrawGeometry(null, new Pen(DecorationColor, 2d), GeometryDelegate(rect));
        }
    }

    public KnownLayer Layer => KnownLayer.Selection;

}
