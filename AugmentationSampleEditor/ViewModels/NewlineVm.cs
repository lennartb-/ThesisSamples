using System.Collections.Generic;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Augmentations.Premade;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

/// <summary>
/// View model for the newline warning sample.
/// </summary>
public class NewlineVm : ISampleContent
{
    private const string Text = "\"This is a newline \\n in a string\"\n" +
                                "\"This is a newline\\nin a string\"\n" +
                                "This is a newline \\n in a regular line\n" +
                                "This is a newline\\nin a regular line\n";

    private readonly IList<Augmentation> augmentations = new List<Augmentation>();
    private bool isEnabled;

    /// <summary>
    /// Initializes a new instance of the <see cref="NewlineVm"/> class.
    /// </summary>
    public NewlineVm()
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
    public string Title => "Newlines";

    private void OnLoaded(CodeTextEditor? editor)
    {
        if (editor == null)
        {
            return;
        }

        augmentations.Add(NewlineAugmentation.GetAugmentation(editor.TextArea));
    }
}