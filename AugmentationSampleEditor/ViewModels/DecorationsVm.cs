using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AugmentationFramework.Augmentations;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

public class DecorationsVm : ISampleContent
{
    private const string Text =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.\n" +
        "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.\n" +
        "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.\n" +
        "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

    private readonly IList<Augmentation> augmentations = new List<Augmentation>();
    private bool isEnabled;

    public DecorationsVm()
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
    public string Title => "Decorations Demo";

    private void OnLoaded(CodeTextEditor editor)
    {
        var backgroundAugmentation = new Augmentation(editor.TextArea)
            .WithForeground(Brushes.RoyalBlue)
            .WithFontWeight(FontWeights.Bold)
            .WithFontFamily(new FontFamily("Consolas"))
            .ForText(new Regex(@"\bsit\b"));
        augmentations.Add(backgroundAugmentation);

        var tooltipAugmentation = new Augmentation(editor.TextArea)
            .WithTooltip(() => new Calendar())
            .WithOverlay(
                () => new Button { Content = "Hallo" })
            .WithBackground(Brushes.LightGreen)
            .ForText(new Regex(@"\besse\b"));
        augmentations.Add(tooltipAugmentation);
    }
}