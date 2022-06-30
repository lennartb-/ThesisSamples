using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AugmentationFramework.AdviceDisplay;
using AugmentationFramework.Augmentations;
using AugmentationFramework.Augmentations.Premade;
using AugmentationFramework.Renderer.Premade;
using AugmentationSampleEditor.ViewModels;
using Microsoft.CodeAnalysis.Differencing;

namespace AugmentationSampleEditor;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IList<Augmentation> plugins = new List<Augmentation>();

    public IList<ISampleContent> ViewModels { get; } = new List<ISampleContent>();

    public MainWindow()
    {
        InitializeComponent();
        ViewModels.Add(new SynonymDisplayVm());
        DataContext = this;
        //var backgroundAugmentation = new Augmentation(Editor.TextArea)
        //    .WithForeground(Brushes.RoyalBlue)
        //    .WithFontWeight(FontWeights.Bold)
        //    .WithFontFamily(new FontFamily("Consolas"))
        //    .ForText(new Regex(@"\bsit\b"));
        //plugins.Add(backgroundAugmentation);

        var image = new BitmapImage(
            new Uri("pack://application:,,,/Resources/ic_menu_save.png"));

        //var underlineAugmentation = new Augmentation(Editor.TextArea.TextView)
        //.WithImage(image)
        //.WithImagePosition(ImagePosition.Left)
        //.ForText(new Regex(@"\btristique\b"));
        //plugins.Add(underlineAugmentation);

        //var tooltipAugmentation = new Augmentation(Editor.TextArea)
        //.WithTooltip(() => new Calendar())
        //.WithOverlay(
        //    () => new Button
        //        { Content = "Hallo" })
        //.WithBackground(Brushes.LightGreen)
        //.ForText(new Regex(@"\bodio\b"));
        //plugins.Add(tooltipAugmentation);

        //foreach (var augmentation in SmalltalkHighlighting.GetAugmentation(Editor.TextArea.TextView))
        //{
        //    plugins.Add(augmentation);
        //}

        //var underlineAugmentation = new Augmentation(Editor.TextArea)
        //    .WithDecorationColor(Brushes.Red)
        //    .WithDecoration(UnderlineBracket.Geometry)
        //    .WithTooltip("SHA1 is cryptographically broken, please use a currently secure function like SHA-512.")
        //    .ForText(new Regex(@"\.HashWithSha1(.*)?\)"));

        //var underlineAugmentation = new Augmentation(Editor.TextArea.TextView)
        //    .WithDecorationColor(Brushes.Red)
        //    .WithDecoration(UnderlineBracket.Geometry)
        //    .WithAdviceOverlay(new SampleAdviceModel())
        //    .ForText(new Regex(@"\.HashWithSha1(.*)?\)"));

        //plugins.Add(underlineAugmentation);

        //foreach (var augmentation in FieldAugmentation.GetAugmentations(Editor.TextArea.TextView))
        //{
        //    plugins.Add(augmentation);
        //}
    }
}