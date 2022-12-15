using System.Collections.Generic;
using System.Windows;
using AugmentationSampleEditor.ViewModels;

namespace AugmentationSampleEditor;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ViewModels.Add(new SynonymDisplayVm());
        ViewModels.Add(new SmalltalkHighlightingVm());
        ViewModels.Add(new DecorationsVm());
        ViewModels.Add(new HashingVm());
        ViewModels.Add(new AdviceVm());
        ViewModels.Add(new FieldComparisonVm());
        ViewModels.Add(new NewlineVm());
        ViewModels.Add(new ImageVm());
        DataContext = this;

        //var image = new BitmapImage(
        //    new Uri("pack://application:,,,/Resources/ic_menu_save.png"));

        //var underlineAugmentation = new Augmentation(Editor.TextArea.TextView)
        //.WithImage(image)
        //.WithImagePosition(ImagePosition.Left)
        //.ForText(new Regex(@"\btristique\b"));
        //plugins.Add(underlineAugmentation);
    }

    public IList<ISampleContent> ViewModels { get; } = new List<ISampleContent>();
}

