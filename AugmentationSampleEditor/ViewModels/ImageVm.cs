﻿using System;
using System.Collections.Generic;
using System.Reactive;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AugmentationFramework.Augmentations;
using ICSharpCode.AvalonEdit.Document;
using ReactiveUI;
using RoslynPad.Editor;

namespace AugmentationSampleEditor.ViewModels;

public class ImageVm : ISampleContent
{
    private const string TextForRightAugmentation = @"public bool IsSaved {get; set;}";

    private const string TextForLeftAugmentation = @"public string Name {get; set;}";
    private readonly IList<Augmentation> augmentations = new List<Augmentation>();
    private bool isEnabled;

    public ImageVm()
    {
        var stringTextSource = new StringTextSource(TextForLeftAugmentation + Environment.NewLine + TextForRightAugmentation);
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

    public string Title => "Image Decoration Demo";

    private void OnLoaded(CodeTextEditor editor)
    {
        var image = new BitmapImage(
            new Uri("pack://application:,,,/Resources/ic_menu_save.png"));
        var imageAugmentation = new Augmentation(editor.TextArea)
            .WithImage(image)
            .OnRight()
            .ForText(TextForRightAugmentation);
        augmentations.Add(imageAugmentation);

        var imageAugmentation2 = new Augmentation(editor.TextArea)
            .InLeftMargin("\xE735", Brushes.Yellow, Brushes.Black, "Segoe MDL2 Assets", 25)
            .ForText(TextForLeftAugmentation);
        augmentations.Add(imageAugmentation2);
    }
}

