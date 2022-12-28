using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AugmentationFramework.Augmentations;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

/// <summary>
///     View model for the decoration sample.
/// </summary>
public sealed class DecorationsVm : SampleContentBase
{
    private const string Text =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.\n" +
        "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.\n" +
        "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.\n" +
        "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

    /// <summary>
    ///     Initializes a new instance of the <see cref="DecorationsVm" /> class.
    /// </summary>
    public DecorationsVm()
    {
        var stringTextSource = new StringTextSource(Text);
        Document = new TextDocument(stringTextSource);
    }

    /// <inheritdoc />
    public override TextDocument Document { get; }

    /// <inheritdoc />
    public override string Title => "Decorations Demo";

    /// <inheritdoc />
    protected override void OnLoaded(CodeTextEditor? editor)
    {
        if (editor == null)
        {
            return;
        }

        var backgroundAugmentation = new Augmentation(editor.TextArea)
            .WithForeground(Brushes.RoyalBlue)
            .WithFontWeight(FontWeights.Bold)
            .WithFontFamily(new FontFamily("Consolas"))
            .ForText(new Regex(@"\bsit\b"));
        Augmentations.Add(backgroundAugmentation);

        var tooltipAugmentation = new Augmentation(editor.TextArea)
            .WithTooltip(() => new Calendar())
            .WithOverlay(
                () => new Button { Content = "Hallo" })
            .WithBackground(Brushes.LightGreen)
            .ForText(new Regex(@"\besse\b"));
        Augmentations.Add(tooltipAugmentation);
    }
}