using AugmentationFramework.Augmentations.Premade;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

/// <summary>
///     View model for the string comparison sample.
/// </summary>
public sealed class FieldComparisonVm : SampleContentBase
{
    private const string Text = "var intCompare = (F1000==F1001);\n" +
                                "var stringCompare = (F2000==F2001);\n" +
                                "var mixedCompare = (F1000==F2000);\n" +
                                "var assign = (F1000=F1002);\n" +
                                "var spacing = (F2000 == F2001);\n";

    /// <summary>
    ///     Initializes a new instance of the <see cref="FieldComparisonVm" /> class.
    /// </summary>
    public FieldComparisonVm()
    {
        var stringTextSource = new StringTextSource(Text);
        Document = new TextDocument(stringTextSource);
    }

    /// <inheritdoc />
    public override TextDocument Document { get; }

    /// <inheritdoc />
    public override string Title => "Field Comparison";

    /// <inheritdoc />
    protected override void OnLoaded(CodeTextEditor? editor)
    {
        if (editor == null)
        {
            return;
        }

        Augmentations.Add(TypedFieldAugmentation.GetAugmentation(editor.TextArea));
    }
}