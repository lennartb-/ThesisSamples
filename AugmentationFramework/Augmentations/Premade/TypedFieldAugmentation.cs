using System.Text.RegularExpressions;
using System.Windows.Media;
using AugmentationFramework.AdviceDisplay;
using AugmentationFramework.Renderer.Premade;
using ICSharpCode.AvalonEdit.Editing;

namespace AugmentationFramework.Augmentations.Premade;

public class TypedFieldAugmentation
{
    private static readonly IEnumerable<DemoField> Fields = GetFieldMapping();

    public static Augmentation GetAugmentation(TextArea textArea)
    {
        return new Augmentation(textArea)
            .ForDelegate(MatchingDelegate)
            .ForText(new Regex(@"\b(F\d+)[ ]*(?<!=)==(?!=)[ ]*(F\d+)\b"))
            .WithDecoration(UnderlineBracket.Geometry)
            .WithDecorationColor(Brushes.Red)
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

    private enum DataType
    {
        Int,
        Double,
        String,
    }
}