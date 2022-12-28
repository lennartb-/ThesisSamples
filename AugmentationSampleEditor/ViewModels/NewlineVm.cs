using AugmentationFramework.Augmentations.Premade;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

/// <summary>
///     View model for the newline warning sample.
/// </summary>
public sealed class NewlineVm : SampleContentBase
{
    private const string Text = "\"This is a newline \\n in a string\"\n" +
                                "\"This is a newline\\nin a string\"\n" +
                                "This is a newline \\n in a regular line\n" +
                                "This is a newline\\nin a regular line\n";

    /// <summary>
    ///     Initializes a new instance of the <see cref="NewlineVm" /> class.
    /// </summary>
    public NewlineVm()
    {
        var stringTextSource = new StringTextSource(Text);
        Document = new TextDocument(stringTextSource);
    }

    /// <inheritdoc />
    public override TextDocument Document { get; }

    /// <inheritdoc />
    public override string Title => "Newlines";

    /// <inheritdoc />
    protected override void OnLoaded(CodeTextEditor? editor)
    {
        if (editor == null)
        {
            return;
        }

        Augmentations.Add(NewlineAugmentation.GetAugmentation(editor.TextArea));
    }
}