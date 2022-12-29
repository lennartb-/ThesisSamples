using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.Threading;
using RoslynPad.Editor;
using RoslynPad.Roslyn;
using WrapperApi;

namespace WrapperApiSampleApp;

/// <summary>
///     Interaction logic for MainWindow.xaml.
/// </summary>
public partial class MainWindow
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        var joinableTaskFactory = new JoinableTaskContext().Factory;
        Editor.Loaded += (s, _) => joinableTaskFactory.RunAsync(() => InitializeEditorAsync(s));
    }

    private static AnalyzerReference GetAnalyzerReference(IRoslynHost host, string analyzerPath)
    {
        var loader = host.GetService<IAnalyzerAssemblyLoader>();
        return new AnalyzerFileReference(analyzerPath, loader);
    }

    private static async Task InitializeEditorAsync(object sender)
    {
        var editor = (RoslynCodeEditor)sender;

        var host = new RoslynHost(
            new[] { Assembly.Load("RoslynPad.Roslyn.Windows"), Assembly.Load("RoslynPad.Editor.Windows") },
            RoslynHostReferences.NamespaceDefault.With(
                assemblyReferences: new[] { typeof(object).Assembly, typeof(HashFunctions).Assembly },
                imports: new[] { nameof(WrapperApi) }));

        var documentId = await editor.InitializeAsync(
            host,
            new ClassificationHighlightColors(),
            "C:\\WorkingDirectory",
            string.Empty,
            SourceCodeKind.Script);

        if (host.GetDocument(documentId) is not { } document)
        {
            return;
        }

        var analyzerReference = GetAnalyzerReference(host, new FileInfo("SampleAnalyzer.dll").FullName);
        var codeFixReference = GetAnalyzerReference(host, new FileInfo("SampleAnalyzer.CodeFixes.dll").FullName);

        var project = document.Project.AddAnalyzerReferences(new[] { analyzerReference, codeFixReference });

        if (project.GetDocument(documentId) is not { } updatedDocument)
        {
            return;
        }

        host.UpdateDocument(updatedDocument);
    }
}