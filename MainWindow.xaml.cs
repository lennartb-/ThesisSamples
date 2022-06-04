using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace RoslynPadTest;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Augmentation plugin;

    public MainWindow()
    {
        InitializeComponent();
        plugin = new Augmentation(Editor.TextArea.TextView);
        plugin.WithBackground(Brushes.HotPink);
        //plugin.ForText("bonk");
        plugin.ForTextMatch(new Regex(@"\bbonk\b"));
    }

    private void OnOverlayChecked(object sender, RoutedEventArgs e)
    {
        plugin.Enable();
    }

    private void OnOverlayUnchecked(object sender, RoutedEventArgs e)
    {
        plugin.Disable();
    }
}
