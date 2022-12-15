using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.Threading;
using RoslynPad.Editor;
using RoslynPad.Roslyn;
using WrapperApiSample;

namespace RoslynPadWrapperApiSample;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        var joinableTaskFactory = new JoinableTaskContext().Factory;
        Editor.Loaded += (s, _) => joinableTaskFactory.RunAsync(() => InitializeEditorAsync(s));
    }

    private static async Task InitializeEditorAsync(object sender)
    {
        var editor = (RoslynCodeEditor)sender;

        var host = new RoslynHost(
            new[] { Assembly.Load("RoslynPad.Roslyn.Windows"), Assembly.Load("RoslynPad.Editor.Windows") },
            RoslynHostReferences.NamespaceDefault.With(
                assemblyReferences: new[] { typeof(object).Assembly, typeof(HashFunctions).Assembly },
                imports: new[] { "WrapperApiSample" }));

        var documentId = await editor.InitializeAsync(
            host,
            new ClassificationHighlightColors(),
            "C:\\WorkingDirectory",
            string.Empty,
            SourceCodeKind.Script);

        var analyzerRef = GetAnalyzerReference(host, new FileInfo("SampleAnalyzer.dll").FullName);
        var analyzerRef2 = GetAnalyzerReference(host, new FileInfo("SampleAnalyzer.CodeFixes.dll").FullName);

        var document = host.GetDocument(documentId);

        if (document == null)
        {
            return;
        }

        var project = document.Project.AddAnalyzerReferences(new[] { analyzerRef, analyzerRef2 });

        document = project.GetDocument(documentId);

        if (document == null)
        {
            return;
        }

        host.UpdateDocument(document);
    }

    private static AnalyzerReference GetAnalyzerReference(IRoslynHost host, string analyzerPath)
    {
        var loader = host.GetService<IAnalyzerAssemblyLoader>();
        return new AnalyzerFileReference(analyzerPath, loader);
    }
}

