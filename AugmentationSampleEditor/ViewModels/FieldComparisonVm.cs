﻿using System.Collections.Generic;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Augmentations.Premade;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

public class FieldComparisonVm : ISampleContent
{
    private const string Text = "var intCompare = F1000==F1001);\n" +
                                "var stringCompare = F2000==F2001);\n" +
                                "var mixedCompare = F1000==F2000);\n" +
                                "var assign = F1000=F1002);\n" +
                                "var spacing = F2000 == F2001);\n";

    private readonly IList<Augmentation> augmentations = new List<Augmentation>();
    private bool isEnabled;

    public FieldComparisonVm()
    {
        var stringTextSource = new StringTextSource(Text);
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
    public string Title => "Field Comparison";

    private void OnLoaded(CodeTextEditor editor)
    {
        augmentations.Add(TypedFieldAugmentation.GetAugmentation(editor.TextArea));
    }
}