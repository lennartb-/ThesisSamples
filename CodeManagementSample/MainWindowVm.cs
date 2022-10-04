using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Roslyn;

namespace CodeManagementSample;

public class MainWindowVm : ObservableObject
{
    private const string Author = "J Doe";
    private const string SampleRepoPath = @"..\..\..\VersioningSampleRepo";
    private static readonly Guid FileGuid = Guid.Parse("9320336C07D54E8FB0E9B132EFCCEFEF");

    private CodeVm? code;
    private string? compilerOutput;
    private string? consoleOutput;
    private readonly TextDocument document;

    private RoslynHost? host;

    public MainWindowVm()
    {
        OnLoadedCommand = new RelayCommand(OnLoaded);
        CompileCommand = new AsyncRelayCommand(OnCompile, () => Code != null);
        AnalyzeCommand = new RelayCommand(OnAnalyze, () => Code != null);
        VersioningCommand = new RelayCommand(OnVersioning, () => Code != null);
        Document = new TextDocument("Console.WriteLine(\"Hello World\");");
    }

    public CodeVm? Code
    {
        get => code;
        private set
        {
            SetProperty(ref code, value);
            CompileCommand.NotifyCanExecuteChanged();
            VersioningCommand.NotifyCanExecuteChanged();
            AnalyzeCommand.NotifyCanExecuteChanged();
        }
    }

    public RelayCommand OnLoadedCommand { get; }
    public AsyncRelayCommand CompileCommand { get; }
    public RelayCommand AnalyzeCommand { get; }
    public RelayCommand VersioningCommand { get; }

    public TextDocument Document
    {
        get => document;
        private init => SetProperty(ref document, value);
    }

    public string? CompilerOutput
    {
        get => compilerOutput;
        private set => SetProperty(ref compilerOutput, value);
    }

    public string? ConsoleOutput
    {
        get => consoleOutput;
        private set => SetProperty(ref consoleOutput, value);
    }

    private async Task OnCompile()
    {
        if (Code == null)
        {
            return;
        }

        Code.Text = Document.Text;
        await Code.TryRunScript();

        CompilerOutput = Code.Result ?? "✅";
        ConsoleOutput = Code.ConsoleOutput ?? string.Empty;
    }

    private void OnAnalyze()
    {
        if (Code == null)
        {
            return;
        }

        Code.Text = Document.Text;
        Code.Compile();

        CompilerOutput = Code.Result ?? "✅";
    }

    private void OnVersioning()
    {
        var versioningWindow = new Versioning();
        var model = new VersioningModel(Author, FileGuid, Document.Text, SampleRepoPath);
        versioningWindow.DataContext = new VersioningVm(model);
        versioningWindow.ShowDialog();
    }

    private void OnLoaded()
    {
        host = new RoslynHost(
            new[] { Assembly.Load("RoslynPad.Roslyn.Windows"), Assembly.Load("RoslynPad.Editor.Windows") },
            RoslynHostReferences.NamespaceDefault.With(assemblyReferences: new[] { typeof(object).Assembly, typeof(Regex).Assembly, typeof(Enumerable).Assembly }));

        Code = new CodeVm(host);
    }
}
