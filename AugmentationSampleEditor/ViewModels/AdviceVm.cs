using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Media;
using AugmentationFramework.AdviceDisplay;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Renderer.Premade;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

public class AdviceVm : ISampleContent
{
    private const string Text = @"var hashedString = HashingAlgorithms.HashWithSha1(""Lorem ipsum dolor sit amet"")";
    private readonly IList<Augmentation> augmentations = new List<Augmentation>();
    private bool isEnabled;

    public AdviceVm()
    {
        var stringTextSource = new StringTextSource(Text);
        Document = new TextDocument(stringTextSource);
        EditorLoadedCommand = new RelayCommand<CodeTextEditor>(OnLoaded);
    }

    /// <inheritdoc />
    public IRelayCommand<CodeTextEditor> EditorLoadedCommand { get; }

    /// <inheritdoc />
    public TextDocument Document { get; }

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
    public string Title => "Security Advice";

    private void OnLoaded(CodeTextEditor editor)
    {
        var underlineAugmentation = new Augmentation(editor.TextArea)
            .WithDecorationColor(Brushes.Red)
            .WithDecoration(UnderlineBracket.Geometry)
            .WithAdviceOverlay(new SampleAdviceModel())
            .ForText(new Regex(@"\.HashWithSha1(.*)?\)"));

        augmentations.Add(underlineAugmentation);
    }
}