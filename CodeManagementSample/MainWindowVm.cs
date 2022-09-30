using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Roslyn;

namespace CodeManagementSample;

public class MainWindowVm : INotifyPropertyChanged
{
    private const string Author = "J Doe";
    private static readonly Guid FileGuid = Guid.Parse("9320336C07D54E8FB0E9B132EFCCEFEF");
    private const string SampleRepoPath = @"..\..\..\VersioningSampleRepo";

    private CodeVm? code;
    private TextDocument consoleOutputDocument;
    private TextDocument document;

    private RoslynHost? host;
    private TextDocument outputDocument;

    public MainWindowVm()
    {
        OnLoadedCommand = new RelayCommand(OnLoaded);
        CompileCommand = new AsyncRelayCommand(OnCompile, () => Code != null);
        AnalyzeCommand = new RelayCommand(OnAnalyze, () => Code != null);
        VersioningCommand = new RelayCommand(OnVersioning, () => Code != null);
        Document = new TextDocument("Console.WriteLine(\"Hello World\");");
        OutputDocument = new TextDocument();
        ConsoleOutputDocument = new TextDocument();
    }

    public CodeVm? Code
    {
        get => code;
        private set
        {
            SetField(ref code, value);
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
        private set => SetField(ref document, value);
    }

    public TextDocument OutputDocument
    {
        get => outputDocument;
        private set => SetField(ref outputDocument, value);
    }

    public TextDocument ConsoleOutputDocument
    {
        get => consoleOutputDocument;
        private set => SetField(ref consoleOutputDocument, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private async Task OnCompile()
    {
        if (Code == null)
        {
            return;
        }

        Code.Text = Document.Text;
        await Code.TryRunScript();

        OutputDocument.Text = Code.Result ?? "✅";
        ConsoleOutputDocument.Text = Code.ConsoleOutput ?? string.Empty;
        OnPropertyChanged(nameof(OutputDocument));
    }

    private void OnAnalyze()
    {
        if (Code == null)
        {
            return;
        }

        Code.Text = Document.Text;
        Code.Compile();

        OutputDocument.Text = Code.Result ?? "✅";

        OnPropertyChanged(nameof(OutputDocument));
    }

    private void OnVersioning()
    {
        var versioningWindow = new Versioning();
        var model = new VersioningModel("John Doe", FileGuid, Document.Text, SampleRepoPath);
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

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
