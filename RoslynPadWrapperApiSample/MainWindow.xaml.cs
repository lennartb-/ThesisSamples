using System.Reflection;
using System.Windows;
using RoslynPad.Editor;
using RoslynPad.Roslyn;

namespace RoslynPadWrapperApiSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnEditorLoaded(object sender, RoutedEventArgs e)
        {
            var editor = (RoslynCodeEditor)sender;
            Loaded -= OnEditorLoaded;

            var host = new RoslynHost(additionalAssemblies: new[]
            {
                Assembly.Load("RoslynPad.Roslyn.Windows"),
                Assembly.Load("RoslynPad.Editor.Windows")
            }, RoslynHostReferences.NamespaceDefault.With(assemblyReferences: new[]
            {
                typeof(object).Assembly,
                typeof(WrapperApiSample.HashFunctions).Assembly,
            }));

            var documentId = editor.Initialize(host, new ClassificationHighlightColors(),
                "C:\\WorkingDirectory", string.Empty);
        }
    }
}
