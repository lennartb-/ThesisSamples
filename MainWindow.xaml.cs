using System.Text.RegularExpressions;
using System.Windows;

namespace RoslynPadTest;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly TestGenerator testGenerator = new TestGenerator();
    private readonly WordHighlighter testHighlighter = new WordHighlighter(new Regex(@"\bzonk\b"));
    public MainWindow()
    {
        InitializeComponent();
        //var testGenerator = new WordHighlighter("bonk");
        //var testGenerator = new TestGenerator();
        //Editor.TextArea.TextView.LineTransformers.Add(testGenerator);

    }

    private void OnOverlayChecked(object sender, RoutedEventArgs e)
    {
        Editor.TextArea.TextView.ElementGenerators.Add(testGenerator);
        Editor.TextArea.TextView.LineTransformers.Add(testHighlighter);

    }

    private void OnOverlayUnchecked(object sender, RoutedEventArgs e)
    {
        Editor.TextArea.TextView.ElementGenerators.Remove(testGenerator);
        Editor.TextArea.TextView.LineTransformers.Remove(testHighlighter);
    }
}
