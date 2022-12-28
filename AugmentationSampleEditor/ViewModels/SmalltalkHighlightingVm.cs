using AugmentationFramework.Augmentations.Premade;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

/// <summary>
///     View model for the smalltalk highlighting sample.
/// </summary>
public sealed class SmalltalkHighlightingVm : SampleContentBase
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
                                "\t\t\t\tshow: ' '].\n" + "^x < y";

    /// <summary>
    ///     Initializes a new instance of the <see cref="SmalltalkHighlightingVm" /> class.
    /// </summary>
    public SmalltalkHighlightingVm()
    {
        var stringTextSource = new StringTextSource(Text);
        Document = new TextDocument(stringTextSource);
    }

    /// <inheritdoc />
    public override TextDocument Document { get; }

    /// <inheritdoc />
    public override string Title => "Smalltalk Syntax Highlighting";

    /// <inheritdoc />
    protected override void OnLoaded(CodeTextEditor? editor)
    {
        if (editor == null)
        {
            return;
        }

        foreach (var augmentation in SmalltalkHighlighting.GetAugmentation(editor.TextArea))
        {
            Augmentations.Add(augmentation);
        }
    }
}