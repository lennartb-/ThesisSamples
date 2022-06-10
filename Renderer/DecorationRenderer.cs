using System.Text.RegularExpressions;
using System.Windows.Media;
using AugmentationFramework.Augmentations;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Renderer;

public class DecorationRenderer : IBackgroundRenderer
{
    private readonly Augmentation parent;
    private readonly Brush decorationColor;
    private readonly RoiFinder roiFinder;

    public DecorationRenderer(Augmentation parent, Brush decorationColor)
    {
        this.parent = parent;
        this.decorationColor = decorationColor;
        roiFinder = new RoiFinder(parent);
    }

    public void Draw(TextView textView, DrawingContext drawingContext)
    {
        var area = roiFinder.DetermineRangesOfInterest();
        
        foreach (var (startOffset, endOffset) in area)
        {
            var textSegment = new TextSegment { StartOffset = startOffset, EndOffset = endOffset };

            var rects = BackgroundGeometryBuilder.GetRectsForSegment(parent.TextView, textSegment);
            var rect = rects.First();

            var underlineGeometry = new StreamGeometry();

            using (var ctx = underlineGeometry.Open())
            {
                ctx.BeginFigure(rect.BottomLeft with { Y = rect.BottomLeft.Y - 3 }, false, false);
                ctx.LineTo(rect.BottomLeft with { Y = rect.BottomLeft.Y  }, true, false);
                ctx.LineTo(rect.BottomRight with { Y = rect.BottomRight.Y  }, true, false);
                ctx.LineTo(rect.BottomRight with { Y = rect.BottomRight.Y -3 }, true, false);
            }

            underlineGeometry.Freeze();
            drawingContext.DrawGeometry(null, new Pen(decorationColor, 2d), underlineGeometry);
        }
    }

    public KnownLayer Layer => KnownLayer.Selection;

}
