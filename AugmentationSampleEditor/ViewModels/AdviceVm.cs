using System.Text.RegularExpressions;
using System.Windows.Media;
using AugmentationFramework.AdviceDisplay;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Renderer.Premade;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

/// <summary>
///     View model for the advice sample.
/// </summary>
public sealed class AdviceVm : SampleContentBase
{
    private const string Text = @"var hashedString = HashingAlgorithms.HashWithSha1(""Lorem ipsum dolor sit amet"")";

    /// <summary>
    ///     Initializes a new instance of the <see cref="AdviceVm" /> class.
    /// </summary>
    public AdviceVm()
    {
        var stringTextSource = new StringTextSource(Text);
        Document = new TextDocument(stringTextSource);
    }

    /// <inheritdoc />
    public override TextDocument Document { get; }

    /// <inheritdoc />
    public override string Title => "Security Advice";

    /// <inheritdoc />
    protected override void OnLoaded(CodeTextEditor? editor)
    {
        if (editor == null)
        {
            return;
        }

        var underlineAugmentation = new Augmentation(editor.TextArea)
            .WithDecoration(UnderlineBracket.Geometry, Brushes.Red)
            .WithAdviceOverlay(new SampleAdviceModel())
            .ForText(new Regex(@"\.HashWithSha1(.*)?\)"));

        Augmentations.Add(underlineAugmentation);
    }
}