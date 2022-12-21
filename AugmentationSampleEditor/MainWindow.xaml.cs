using System.Collections.Generic;
using System.Windows;
using AugmentationFrameworkSampleApp.ViewModels;

namespace AugmentationFrameworkSampleApp;

/// <summary>
///     Interaction logic for MainWindow.xaml.
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
    }

    public IList<ISampleContent> ViewModels { get; } = new List<ISampleContent>();
}