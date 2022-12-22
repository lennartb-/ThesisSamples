using System.Text.RegularExpressions;
using System.Windows.Media;
using AugmentationFramework.AdviceDisplay;
using AugmentationFramework.Renderer.Premade;
using ICSharpCode.AvalonEdit.Editing;

namespace AugmentationFramework.Augmentations.Premade;

/// <summary>
///     Provides a pre-made demo augmentation that displays an advice over incorrect string comparisons based on the data
///     type of a database field.
/// </summary>
public static class TypedFieldAugmentation
{
    private static readonly IEnumerable<DemoField> Fields = GetFieldMapping();

    private enum DataType
    {
        Int,
        Double,
        String,
    }

    /// <summary>
    ///     Gets a completely built augmentation that advises of incorrect string comparisons.
    /// </summary>
    /// <param name="textArea">The text area the augmentation applies to.</param>
    /// <returns>A pre-built augmentation.</returns>
    public static Augmentation GetAugmentation(TextArea textArea)
    {
        return new Augmentation(textArea)
            .ForDelegate(MatchingDelegate)
            .ForText(new Regex(@"\b(F\d+)[ ]*(?<!=)==(?!=)[ ]*(F\d+)\b"))
            .WithDecoration(UnderlineBracket.Geometry, Brushes.Red)
            .WithAdviceOverlay(new StringEqualityAdviceModel());
    }

    private static IEnumerable<DemoField> GetFieldMapping()
    {
        return new List<DemoField>
        {
            new(DataType.Int, "F1000"),
            new(DataType.Int, "F1001"),
            new(DataType.String, "F2000"),
            new(DataType.String, "F2001"),
            new(DataType.Double, "F3000"),
            new(DataType.Double, "F3001"),
        };
    }

    private static bool MatchingDelegate(Match match)
    {
        if (!match.Success)
        {
            return false;
        }

        if (match.Groups.Count != 3)
        {
            return false;
        }

        var left = match.Groups[1];
        var right = match.Groups[2];

        var leftField = Fields.FirstOrDefault(f => f.Id == left.Value);
        var rightField = Fields.FirstOrDefault(f => f.Id == right.Value);

        return (leftField?.DataType == DataType.String) && (rightField?.DataType == DataType.String);
    }

    private record DemoField(DataType DataType, string Id);
}