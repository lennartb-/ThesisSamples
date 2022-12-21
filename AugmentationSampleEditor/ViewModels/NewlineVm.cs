using System.Collections.Generic;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Augmentations.Premade;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

public class NewlineVm : ISampleContent
{
    private const string Text = "\"This is a newline \\n in a string\"\n" +
                                "\"This is a newline\\nin a string\"\n" +
                                "This is a newline \\n in a regular line\n" +
                                "This is a newline\\nin a regular line\n";

    private readonly IList<Augmentation> augmentations = new List<Augmentation>();
    private bool isEnabled;

    public NewlineVm()
    {
        var stringTextSource = new StringTextSource(Text);
        Document = new TextDocument(stringTextSource);
        EditorLoadedCommand = new RelayCommand<CodeTextEditor>(OnLoaded);
    }

    public IRelayCommand<CodeTextEditor> EditorLoadedCommand { get; }

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

    public string Title => "Newlines";

    private void OnLoaded(CodeTextEditor editor)
    {
        augmentations.Add(NewlineAugmentation.GetAugmentation(editor.TextArea));
    }
}