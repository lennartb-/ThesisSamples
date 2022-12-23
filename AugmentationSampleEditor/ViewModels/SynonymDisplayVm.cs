﻿using System.Collections.Generic;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Augmentations.Premade;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

/// <summary>
/// View model for the synonym display sample.
/// </summary>
public class SynonymDisplayVm : ISampleContent
{
    private const string Text = "foreach(var name in T1000.F1001.T2000.F2001.T3000.F3001)\n{\n\tConsole.WriteLine($\"Name of the category of the product is {name}\");\n}";
    private readonly IList<Augmentation> augmentations = new List<Augmentation>();
    private bool isEnabled;

    /// <summary>
    /// Initializes a new instance of the <see cref="SynonymDisplayVm"/> class.
    /// </summary>
    public SynonymDisplayVm()
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
    public string Title => "Synonym Display";

    private void OnLoaded(CodeTextEditor? editor)
    {
        if (editor == null)
        {
            return;
        }

        foreach (var augmentation in FieldAugmentation.GetAugmentations(editor.TextArea))
        {
            augmentations.Add(augmentation);
        }
    }
}