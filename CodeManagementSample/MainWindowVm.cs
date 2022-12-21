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
    private readonly TextDocument document = null!;

    private CodeVm? code;
    private string? compilerOutput;
    private string? consoleOutput;

    private RoslynHost? host;
    private bool isApplicationExecuting;
    private bool isCompilationRunning;

    public MainWindowVm()
    {
        OnLoadedCommand = new RelayCommand(OnLoaded);
        ExecuteCommand = new AsyncRelayCommand(OnExecute, () => Code != null);
        CompileCommand = new RelayCommand(OnCompile, () => Code != null);
        VersioningCommand = new RelayCommand(OnVersioning, () => Code != null);
        Document = new TextDocument("Console.WriteLine(\"Hello World\");");
    }

    public CodeVm? Code
    {
        get => code;
        private set
        {
            SetProperty(ref code, value);
            ExecuteCommand.NotifyCanExecuteChanged();
            VersioningCommand.NotifyCanExecuteChanged();
            CompileCommand.NotifyCanExecuteChanged();
        }
    }

    public RelayCommand CompileCommand { get; }

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

    public TextDocument Document
    {
        get => document;
        private init => SetProperty(ref document, value);
    }

    public AsyncRelayCommand ExecuteCommand { get; }

    public bool IsApplicationExecuting
    {
        get => isApplicationExecuting;
        set => SetProperty(ref isApplicationExecuting, value);
    }

    public bool IsCompilationRunning
    {
        get => isCompilationRunning;
        set => SetProperty(ref isCompilationRunning, value);
    }

    public RelayCommand OnLoadedCommand { get; }

    public RelayCommand VersioningCommand { get; }

    private void OnCompile()
    {
        if (Code == null)
        {
            return;
        }

        Code.Text = Document.Text;
        IsCompilationRunning = true;
        Code.Compile();
        IsCompilationRunning = false;
        CompilerOutput = Code.Result ?? "✅";
        ConsoleOutput = Code.ConsoleOutput;
    }

    private async Task OnExecute()
    {
        if (Code == null)
        {
            return;
        }

        Code.Text = Document.Text;
        IsApplicationExecuting = true;
        await Code.TryRunScript();
        IsApplicationExecuting = false;
        CompilerOutput = Code.Result ?? "✅";
        ConsoleOutput = Code.ConsoleOutput ?? string.Empty;
    }

    private void OnLoaded()
    {
        host = new RoslynHost(
            new[] { Assembly.Load("RoslynPad.Roslyn.Windows"), Assembly.Load("RoslynPad.Editor.Windows") },
            RoslynHostReferences.NamespaceDefault.With(assemblyReferences: new[] { typeof(object).Assembly, typeof(Regex).Assembly, typeof(Enumerable).Assembly }));

        Code = new CodeVm(host);
    }

    private void OnVersioning()
    {
        var versioningWindow = new Versioning();
        var model = new VersioningModel(Author, FileGuid, Document.Text, SampleRepoPath);
        var versioningVm = new VersioningVm(model);
        versioningWindow.DataContext = versioningVm;
        versioningVm.RequestClose += () => versioningWindow.Close();
        versioningWindow.ShowDialog();

        if (versioningVm.CheckedOutText != null)
        {
            Document.Text = versioningVm.CheckedOutText;
        }
    }
}