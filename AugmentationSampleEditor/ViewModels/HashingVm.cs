using System.Text.RegularExpressions;
using System.Windows.Media;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Renderer.Premade;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

/// <summary>
///     View model for the underline sample.
/// </summary>
public sealed class HashingVm : SampleContentBase
{
    private const string Text = @"var hashedString = HashingAlgorithms.HashWithSha1(""Lorem ipsum dolor sit amet"")";

    /// <summary>
    ///     Initializes a new instance of the <see cref="HashingVm" /> class.
    /// </summary>
    public HashingVm()
    {
        var stringTextSource = new StringTextSource(Text);
        Document = new TextDocument(stringTextSource);
    }

    /// <inheritdoc />
    public override TextDocument Document { get; }

    /// <inheritdoc />
    public override string Title => "Underline Tooltip";

    /// <inheritdoc />
    protected override void OnLoaded(CodeTextEditor? editor)
    {
        if (editor == null)
        {
            return;
        }

        var underlineAugmentation = new Augmentation(editor.TextArea)
            .WithDecoration(UnderlineBracket.Geometry, Brushes.Red)
            .WithTooltip("SHA1 is cryptographically broken, please use a currently secure function like SHA-512.")
            .ForText(new Regex(@"\.HashWithSha1(.*)?\)"));
        Augmentations.Add(underlineAugmentation);
    }
}