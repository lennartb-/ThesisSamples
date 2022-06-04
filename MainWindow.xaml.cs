using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace RoslynPadTest;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IList<Augmentation> plugins = new List<Augmentation>();

    public MainWindow()
    {
        InitializeComponent();
        var plugin = new Augmentation(Editor.TextArea.TextView);
        plugin.WithBackground(Brushes.HotPink);
        //plugin.ForText("bonk");
        plugin.ForTextMatch(new Regex(@"\bsit\b"));

        plugins.Add(plugin);

        var plugin2 = new Augmentation(Editor.TextArea.TextView);
        plugin2.WithDecoration(Brushes.Blue);
        //plugin.ForText("bonk");
        plugin2.ForTextMatch(new Regex(@"\but\b"));

        plugins.Add(plugin2);
    }

    private void OnOverlayChecked(object sender, RoutedEventArgs e)
    {
        foreach (var augmentation in plugins)
        {
            augmentation.Enable();
        }
    }

    private void OnOverlayUnchecked(object sender, RoutedEventArgs e)
    {
        foreach (var augmentation in plugins)
        {
            augmentation.Disable();
        }
    }
}
