using AugmentationFramework.Augmentations.Premade;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

/// <summary>
///     View model for the synonym display sample.
/// </summary>
public sealed class SynonymDisplayVm : SampleContentBase
{
    private const string Text = "foreach(var name in T1000.F1001.T2000.F2001.T3000.F3001)\n{\n\tConsole.WriteLine($\"Name of the category of the product is {name}\");\n}";

    /// <summary>
    ///     Initializes a new instance of the <see cref="SynonymDisplayVm" /> class.
    /// </summary>
    public SynonymDisplayVm()
    {
        var stringTextSource = new StringTextSource(Text);
        Document = new TextDocument(stringTextSource);
    }

    /// <inheritdoc />
    public override TextDocument Document { get; }

    /// <inheritdoc />
    public override string Title => "Synonym Display";

    /// <inheritdoc />
    protected override void OnLoaded(CodeTextEditor? editor)
    {
        if (editor == null)
        {
            return;
        }

        foreach (var augmentation in FieldAugmentation.GetAugmentations(editor.TextArea))
        {
            Augmentations.Add(augmentation);
        }
    }
}