using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using AugmentationFramework.Augmentations;
using AugmentationSampleEditor.ViewModels;

namespace AugmentationSampleEditor;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IList<Augmentation> plugins = new List<Augmentation>();

    public MainWindow()
    {
        InitializeComponent();
        ViewModels.Add(new SynonymDisplayVm());
        ViewModels.Add(new SmalltalkHighlightingVm());
        ViewModels.Add(new DecorationsVm());
        ViewModels.Add(new HashingVm());
        ViewModels.Add(new AdviceVm());
        DataContext = this;

        var image = new BitmapImage(
            new Uri("pack://application:,,,/Resources/ic_menu_save.png"));

        //var underlineAugmentation = new Augmentation(Editor.TextArea.TextView)
        //.WithImage(image)
        //.WithImagePosition(ImagePosition.Left)
        //.ForText(new Regex(@"\btristique\b"));
        //plugins.Add(underlineAugmentation);
    }

    public IList<ISampleContent> ViewModels { get; } = new List<ISampleContent>();
}
