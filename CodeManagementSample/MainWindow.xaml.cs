using System.Windows;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.CodeAnalysis.Differencing;

namespace CodeManagementSample;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowVm();
        Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
    }
}
