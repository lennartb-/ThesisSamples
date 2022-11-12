using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using RoslynPad.Editor;
using RoslynPad.Roslyn;
using RoslynPad.Roslyn.CodeActions;
using RoslynPad.Roslyn.CodeFixes;
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
    }

    private void OnEditorLoaded(object sender, RoutedEventArgs e)
    {
        var editor = (RoslynCodeEditor)sender;
        editor.Text = "var a = \"new\\nline\";";
        Loaded -= OnEditorLoaded;

        var host = new RoslynHost(
            new[] { Assembly.Load("RoslynPad.Roslyn.Windows"), Assembly.Load("RoslynPad.Editor.Windows") },
            RoslynHostReferences.NamespaceDefault.With(assemblyReferences: new[] { typeof(object).Assembly, typeof(HashFunctions).Assembly },
                imports: new []{ "WrapperApiSample" }));

        var documentId = editor.Initialize(
            host,
            new ClassificationHighlightColors(),
            "C:\\WorkingDirectory",
            string.Empty);

        var analyzerRef = GetAnalyzerReference(host, "C:\\Users\\lbrue\\Source\\Repos\\RoslynPadTest\\SampleAnalyzer\\SampleAnalyzer\\bin\\Debug\\netstandard2.0\\SampleAnalyzer.dll");
        var analyzerRef2 = GetAnalyzerReference(host, "C:\\Users\\lbrue\\Source\\Repos\\RoslynPadTest\\SampleAnalyzer\\SampleAnalyzer.CodeFixes\\bin\\Debug\\netstandard2.0\\SampleAnalyzer.CodeFixes.dll");

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
internal sealed class CodeActionsConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((CodeAction)value).GetCodeActions();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
