using System.Windows;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework;

/// <summary>
///     Calculates the bounds of a text area from offsets.
/// </summary>
internal static class DrawingBoundsCalculator
{
    /// <summary>
    ///     Gets the bounds of a text area from <paramref name="startOffset" /> to <paramref name="endOffset" /> in
    ///     <paramref name="textView" />.
    /// </summary>
    /// <param name="startOffset">The start offset.</param>
    /// <param name="endOffset">The end offset.</param>
    /// <param name="textView">The <see cref="TextView" /> which contains the offsets.</param>
    /// <returns>A <see cref="Rect" /> representing the bounds of the text between the offsets, or null.</returns>
    public static Rect? GetBoundsFromTextOffset(int startOffset, int endOffset, TextView textView)
    {
        var textSegment = new TextSegment { StartOffset = startOffset, EndOffset = endOffset };

        var rects = BackgroundGeometryBuilder.GetRectsForSegment(textView, textSegment).ToList();
        if (!rects.Any())
        {
            return null;
        }

        return rects.First();
    }

    /// <summary>
    ///     Gets the bounds of a text area from <paramref name="startOffset" /> to <paramref name="endOffset" /> in
    ///     <paramref name="textView" />, scaled by the factors of <paramref name="width" /> and <paramref name="height" />.
    /// </summary>
    /// <param name="startOffset">The start offset.</param>
    /// <param name="endOffset">The end offset.</param>
    /// <param name="width">The width to scale to.</param>
    /// <param name="height">The height to scale to.</param>
    /// <param name="textView">The <see cref="TextView" /> which contains the offsets.</param>
    /// <returns>A <see cref="Rect" /> representing the bounds of the text between the offsets, or null.</returns>
    public static Rect? GetScaledImageBoundsFromTextOffset(int startOffset, int endOffset, double width, double height, TextView textView)
    {
        if (GetBoundsFromTextOffset(startOffset, endOffset, textView) is not { } rect)
        {
            return null;
        }

        var scale = Math.Min(rect.Width / width, rect.Height / height);

        var scaledWidth = (int)(width * scale);
        var scaledHeight = (int)(height * scale);

        return new Rect(rect.X + rect.Width, rect.Y + ((rect.Height - scaledHeight) / 2), scaledWidth, scaledHeight);
    }
}