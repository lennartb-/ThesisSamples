using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AugmentationFramework.Augmentations;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

/// <summary>
///     View model for the image display sample.
/// </summary>
public sealed class ImageVm : SampleContentBase
{
    private const string TextForRightAugmentation = @"public bool IsSaved {get; set;}";

    private const string TextForLeftAugmentation = @"public string Name {get; set;}";

    /// <summary>
    ///     Initializes a new instance of the <see cref="ImageVm" /> class.
    /// </summary>
    public ImageVm()
    {
        var stringTextSource = new StringTextSource(TextForLeftAugmentation + Environment.NewLine + TextForRightAugmentation);
        Document = new TextDocument(stringTextSource);
    }

    /// <inheritdoc />
    public override TextDocument Document { get; }

    /// <inheritdoc />
    public override string Title => "Image Decoration Demo";

    /// <inheritdoc />
    protected override void OnLoaded(CodeTextEditor? editor)
    {
        if (editor == null)
        {
            return;
        }

        var image = new BitmapImage(
            new Uri("pack://application:,,,/Resources/ic_menu_save.png"));
        var imageAugmentation = new Augmentation(editor.TextArea)
            .InCodeArea(image)
            .ForText(TextForRightAugmentation);
        Augmentations.Add(imageAugmentation);

        var imageAugmentation2 = new Augmentation(editor.TextArea)
            .InLeftMargin("\xE735", Brushes.Yellow, Brushes.Black, "Segoe MDL2 Assets", 25)
            .ForText(TextForLeftAugmentation);
        Augmentations.Add(imageAugmentation2);
    }
}