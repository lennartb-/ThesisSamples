using System.Windows;
using System.Windows.Media;

namespace AugmentationFramework.Renderer.Premade;

/// <summary>
/// Provides a pre-made <see cref="Geometry"/> representing an underline that looks like a horizontal bracket.
/// </summary>
public static class UnderlineBracket
{
    /// <summary>
    /// Gets a <see cref="Geometry"/>  representing an underline that looks like a horizontal bracket.
    /// </summary>
    /// <param name="rect">A <see cref="Rect"/> representing the bounds of the area to underline.</param>
    /// <returns>A <see cref="Geometry"/> sized according to the bounds of <paramref name="rect"/>.</returns>
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