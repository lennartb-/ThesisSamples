using System.Windows;
using System.Windows.Media;
using AugmentationFramework.Augmentations;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.LeftMargins;

public class MarginDecoration : AbstractMargin
{
    private readonly Augmentation parent;
    private readonly RoiFinder roiFinder;

    public MarginDecoration(Augmentation parent)
    {
        this.parent = parent;
        roiFinder = new RoiFinder(parent);
    }

    public ImageSource? Image { get; set; }

    /// <inheritdoc />
    protected override void OnRender(DrawingContext drawingContext)
    {
        if (Image == null)
        {
            return;
        }

        var textView = TextView;
        if ((TextView != null) && textView.VisualLinesValid)
        {
            var area = roiFinder.DetermineRangesOfInterest(textView.Document.Text);

            foreach (var (startOffset, endOffset) in area)
            {
                DrawImage(drawingContext, startOffset, endOffset);
            }
            //var foreground = (Brush)GetValue(Control.ForegroundProperty);
            //foreach (VisualLine line in textView.VisualLines)
            //{
            //    int lineNumber = line.FirstDocumentLine.LineNumber;

            //    double y = line.GetTextLineVisualYPosition(line.TextLines[0], VisualYPosition.TextTop);
            //    drawingContext.DrawImage(text, new Point(renderSize.Width - text.Width, y - textView.VerticalOffset));
            //}
        }
    }

    /// <inheritdoc />
    protected override Size MeasureOverride(Size availableSize)
    {
        if (Image != null)
        {
            if (TextView is { VisualLinesValid: true })
            {
                var area = roiFinder.DetermineRangesOfInterest(TextView.Document.Text);
                var (startOffset, endOffset) = area.First();
                var textSegment = new TextSegment { StartOffset = startOffset, EndOffset = endOffset };

                var rects = BackgroundGeometryBuilder.GetRectsForSegment(parent.TextView, textSegment).ToList();
                if (!rects.Any())
                {
                    return availableSize;
                }

                var rect = rects.First();

                var scale = Math.Min(rect.Width / Image.Width, rect.Height / Image.Height);

                var scaleWidth = (int)(Image.Width * scale);
                var scaleHeight = (int)(Image.Height * scale);
                return new Size(scaleWidth, scaleHeight);
            }
        }

        return availableSize;
    }

    private void DrawImage(DrawingContext drawingContext, int startOffset, int endOffset)
    {
        if (Image == null)
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

        var r = new Rect(0, rect.Y + ((rect.Height - scaleHeight) / 2), scaleWidth, scaleHeight);
        drawingContext.DrawImage(Image, r);
    }
}