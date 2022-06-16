using System.Windows.Media;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Augmentations.Premade;

public class FieldAugmentation
{
    public static IEnumerable<Augmentation> GetAugmentations(TextView textView)
    {
        return GetFieldNames().Select(
            fieldMapping => new Augmentation(textView)
                .ForText(fieldMapping.Key)
                .WithBackground(Brushes.DarkGray)
                .WithForeground(Brushes.White)
                .WithOverlay(fieldMapping.Value));
    }

    private static IDictionary<string, string> GetFieldNames()
    {
        return new Dictionary<string, string>
        {
            { "T1000.F1000", "Order ID" },
            { "T1000.F1001", "Products" },
            { "T1000.F1002", "Amount" },
            { "T2000.F2000", "Product ID" },
            { "T2000.F2001", "Categories" },
            { "T2000.F2002", "Name" },
            { "T3000.F3000", "Category ID" },
            { "T3000.F3001", "Name" }
        };
    }
}
