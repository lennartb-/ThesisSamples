using System.Collections.Generic;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Augmentations.Premade;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

public class SmalltalkHighlightingVm : ISampleContent
{
    private const string Text = "exampleWithNumber: x\n" +
                                "\"A method that illustrates every part of Smalltalk method syntax\n" +
                                "except primitives. It has unary, binary, and keyboard messages,\n" +
                                "declares arguments and temporaries, accesses a global variable\n" +
                                "(but not an instance variable), uses literals (array, character,\n" +
                                "symbol, string, integer, float), uses the pseudo variables\n" +
                                "true, false, nil, self, and super, and has sequence, assignment,\n" +
                                "return and cascade. It has both zero argument and one argument blocks.\"\n" +
                                "| y |\n" +
                                "true & false not & (nil isNil) ifFalse: [self halt].\n" +
                                "y := self size + super size.\n" +
                                "\t#($a #a \"a\" 1 1.0)\n" +
                                "\t\tdo: [ :each |\n" +
                                "\t\t\tTranscript show: (each class name);\n" +
                                "\t\t\t\tshow: ' '].\n" +
                                "^x < y";

    private readonly IList<Augmentation> augmentations = new List<Augmentation>();
    private bool isEnabled;

    public SmalltalkHighlightingVm()
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
    public string Title => "Smalltalk Syntax Highlighting";

    private void OnLoaded(CodeTextEditor? editor)
    {
        if (editor == null)
        {
            return;
        }

        foreach (var augmentation in SmalltalkHighlighting.GetAugmentation(editor.TextArea))
        {
            augmentations.Add(augmentation);
        }
    }
}