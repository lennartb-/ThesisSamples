using System.Windows.Media;
using ICSharpCode.AvalonEdit.Editing;

namespace AugmentationFramework.Augmentations.Premade;

/// <summary>
/// Provides pre-made demo augmentations that replace certain identifiers with more readable strings.
/// </summary>
public static class FieldAugmentation
{
    /// <summary>
    /// Gets a list of completely build augmentations that replaces identifiers with readable strings.
    /// </summary>
    /// <param name="textArea">The text area the augmentations apply to.</param>
    /// <returns>A list of pre-built augmentations.</returns>
    public static IEnumerable<Augmentation> GetAugmentations(TextArea textArea)
    {
        foreach (var (field, name) in GetFieldMapping())
        {
            yield return new Augmentation(textArea)
                .ForText(field)
                .WithBackground(Brushes.DarkGray)
                .WithForeground(Brushes.White)
                .WithOverlay(name);
        }
    }

    private static IEnumerable<(string FieldID, string FieldName)> GetFieldMapping()
    {
        return new List<(string, string)>
        {
            ("T1000.F1000", "Order ID"),
            ("T1000.F1001", "Products"),
            ("T1000.F1002", "Amount"),
            ("T2000.F2000", "Product ID"),
            ("T2000.F2001", "Categories"),
            ("T2000.F2002", "Name"),
            ("T3000.F3000", "Category ID"),
            ("T3000.F3001", "Name"),
        };
    }
}