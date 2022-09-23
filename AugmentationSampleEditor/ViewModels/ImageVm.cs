using System;
using AugmentationFramework.Augmentations;
using ICSharpCode.AvalonEdit.Document;
using ReactiveUI;
using RoslynPad.Editor;
using System.Collections.Generic;
using System.Reactive;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AugmentationSampleEditor.ViewModels;

public class ImageVm : ISampleContent
{
    private readonly IList<Augmentation> augmentations = new List<Augmentation>();
    private bool isEnabled;
    private const string Text = @"save here";
    public ImageVm()
    {
        var stringTextSource = new StringTextSource(Text);
        Document = new TextDocument(stringTextSource);
        EditorLoadedCommand = ReactiveCommand.Create<CodeTextEditor>(OnLoaded);
    }

    public ReactiveCommand<CodeTextEditor, Unit> EditorLoadedCommand { get; }
    public TextDocument Document { get; }

    public bool IsEnabled
    {
        get => isEnabled;
        set
        {
            isEnabled = value;
            if (isEnabled)
            {
                foreach (var augmentation in augmentations) augmentation.Enable();
            }
            else
            {
                foreach (var augmentation in augmentations) augmentation.Disable();
            }
        }
    }

    public string Title => "Image Decoration Demo";

    private void OnLoaded(CodeTextEditor editor)
    {
        var image = new BitmapImage(
            new Uri("pack://application:,,,/Resources/ic_menu_save.png"));
        var imageAugmentation = new Augmentation(editor.TextArea)
            .WithImage(image)
        .OnRight()
        .ForText(Text);
        augmentations.Add(imageAugmentation);
        
        var imageAugmentation2 = new Augmentation(editor.TextArea)
            .InLeftMargin(image)
            .ForText(Text);
        augmentations.Add(imageAugmentation2);
    }
}
