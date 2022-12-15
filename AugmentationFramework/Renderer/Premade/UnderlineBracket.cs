using System.Windows;
using System.Windows.Media;

namespace AugmentationFramework.Renderer.Premade;

public static class UnderlineBracket
{
    public static Geometry Geometry(Rect rect)
    {
        var underlineGeometry = new StreamGeometry();

        using (var ctx = underlineGeometry.Open())
        {
            ctx.BeginFigure(rect.BottomLeft with { Y = rect.BottomLeft.Y - 3 }, false, false);
            ctx.LineTo(rect.BottomLeft with { Y = rect.BottomLeft.Y }, true, false);
            ctx.LineTo(rect.BottomRight with { Y = rect.BottomRight.Y }, true, false);
            ctx.LineTo(rect.BottomRight with { Y = rect.BottomRight.Y - 3 }, true, false);
        }

        underlineGeometry.Freeze();
        return underlineGeometry;
    }
}

