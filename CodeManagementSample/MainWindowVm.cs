using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Roslyn;

namespace CodeManagementSample;

/// <summary>
/// Viewmodel for the main window.
/// </summary>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowVm"/> class.
    /// </summary>
    public MainWindowVm()
    {
        OnLoadedCommand = new RelayCommand(OnLoaded);
        ExecuteCommand = new AsyncRelayCommand(OnExecute, () => Code != null);
        CompileCommand = new RelayCommand(OnCompile, () => Code != null);
        VersioningCommand = new RelayCommand(OnVersioning, () => Code != null);
        Document = new TextDocument("Console.WriteLine(\"Hello World\");");
    }

    /// <summary>
    /// Gets the viewmodel that manages the code compilation.
    /// </summary>
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

    /// <summary>
    /// Gets a command that is executed when code should be compiled.
    /// </summary>
    public RelayCommand CompileCommand { get; }

    /// <summary>
    /// Gets the output of the compiler.
    /// </summary>
    public string? CompilerOutput
    {
        get => compilerOutput;
        private set => SetProperty(ref compilerOutput, value);
    }

    /// <summary>
    /// Gets the output from the console.
    /// </summary>
    public string? ConsoleOutput
    {
        get => consoleOutput;
        private set => SetProperty(ref consoleOutput, value);
    }

    /// <summary>
    /// Gets the document that contains the code to compile.
    /// </summary>
    public TextDocument Document
    {
        get => document;
        private init => SetProperty(ref document, value);
    }

    /// <summary>
    /// Gets a command that is executed when code should be executed.
    /// </summary>
    public AsyncRelayCommand ExecuteCommand { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the code is currently executing.
    /// </summary>
    public bool IsApplicationExecuting
    {
        get => isApplicationExecuting;
        set => SetProperty(ref isApplicationExecuting, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the compilation is currently running.
    /// </summary>
    public bool IsCompilationRunning
    {
        get => isCompilationRunning;
        set => SetProperty(ref isCompilationRunning, value);
    }

    /// <summary>
    /// Gets the command that is executed when the <see cref="FrameworkElement.LoadedEvent"/> is fired.
    /// </summary>
    public RelayCommand OnLoadedCommand { get; }

    /// <summary>
    /// Gets a command that is executed when the versioning window should open.
    /// </summary>
    public RelayCommand VersioningCommand { get; }

    private void OnCompile()
    {
        if (Code == null)
        {
            return;
        }

        Code.Code = Document.Text;
        IsCompilationRunning = true;
        Code.Compile();
        IsCompilationRunning = false;
        CompilerOutput = Code.CompilationResult ?? "✅";
        ConsoleOutput = Code.ConsoleOutput;
    }

    private async Task OnExecute()
    {
        if (Code == null)
        {
            return;
        }

        Code.Code = Document.Text;
        IsApplicationExecuting = true;
        await Code.TryRunScript();
        IsApplicationExecuting = false;
        CompilerOutput = Code.CompilationResult ?? "✅";
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