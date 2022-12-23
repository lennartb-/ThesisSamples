using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AugmentationFramework.Augmentations;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

/// <summary>
/// View model for the image display sample.
/// </summary>
public class ImageVm : ISampleContent
{
    private const string TextForRightAugmentation = @"public bool IsSaved {get; set;}";

    private const string TextForLeftAugmentation = @"public string Name {get; set;}";
    private readonly IList<Augmentation> augmentations = new List<Augmentation>();
    private bool isEnabled;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageVm"/> class.
    /// </summary>
    public ImageVm()
    {
        var stringTextSource = new StringTextSource(TextForLeftAugmentation + Environment.NewLine + TextForRightAugmentation);
        Document = new TextDocument(stringTextSource);
        EditorLoadedCommand = new RelayCommand<CodeTextEditor>(OnLoaded);
    }

    /// <inheritdoc />
    public TextDocument Document { get; }

    /// <inheritdoc />
    public IRelayCommand<CodeTextEditor> EditorLoadedCommand { get; }

    /// <inheritdoc />
    public bool IsEnabled
    {
        get => isEnabled;
        set
        {
            isEnabled = value;
            if (isEnabled)
            {
                foreach (var augmentation in augmentations)
                {
                    augmentation.Enable();
                }
            }
            else
            {
                foreach (var augmentation in augmentations)
                {
                    augmentation.Disable();
                }
            }
        }
    }

    /// <inheritdoc />
    public string Title => "Image Decoration Demo";

    private void OnLoaded(CodeTextEditor? editor)
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
        augmentations.Add(imageAugmentation);

        var imageAugmentation2 = new Augmentation(editor.TextArea)
            .InLeftMargin("\xE735", Brushes.Yellow, Brushes.Black, "Segoe MDL2 Assets", 25)
            .ForText(TextForLeftAugmentation);
        augmentations.Add(imageAugmentation2);
    }
}