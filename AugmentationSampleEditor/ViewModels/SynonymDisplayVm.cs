using System.Collections.Generic;
using System.Reactive;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Augmentations.Premade;
using ICSharpCode.AvalonEdit.Document;
using ReactiveUI;
using RoslynPad.Editor;

namespace AugmentationSampleEditor.ViewModels;

public class SynonymDisplayVm : ISampleContent
{
    private const string Text = "foreach(var name in T1000.F1001.T2000.F2001.T3000.F3001)\n{\n\tConsole.WriteLine($\"Name of the category of the product is {name}\");\n}";
    private readonly IList<Augmentation> augmentations = new List<Augmentation>();
    private bool isEnabled;

    public SynonymDisplayVm()
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

    public string Title => "Synonym Display";

    private void OnLoaded(CodeTextEditor editor)
    {
        foreach (var augmentation in FieldAugmentation.GetAugmentations(editor.TextArea))
        {
            augmentations.Add(augmentation);
        }
    }
}
