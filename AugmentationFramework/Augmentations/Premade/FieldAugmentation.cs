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
            ("T1000", "Products"),
            ("F1000", "Product ID"),
            ("F1001", "Category ID"),
            ("F1002", "Name"),
            ("T2000", "Categories"),
            ("F2000", "Category ID"),
            ("F2001", "Name"),
        };
    }
}