using System.Windows;
using ICSharpCode.AvalonEdit.Highlighting;

namespace CodeManagementSample;

/// <summary>
///     Interaction logic for MainWindow.xaml.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowVm();
        Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
    }
}