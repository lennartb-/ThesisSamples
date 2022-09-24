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
    private CodeVm? code;
    private RoslynHost? host;

    public MainWindowVm()
    {
        OnLoadedCommand = new RelayCommand(OnLoaded);
        CompileCommand = new AsyncRelayCommand(OnCompile, () => code != null);
        AnalyzeCommand = new RelayCommand(OnAnalyze,() => code!=null);
        VersioningCommand = new RelayCommand(OnVersioning, () => code != null);
        Document = new TextDocument("Console.WriteLine(\"Hello World\");");
        OutputDocument = new TextDocument();
    }

    public RelayCommand OnLoadedCommand { get; }
    public AsyncRelayCommand CompileCommand { get; }
    public RelayCommand AnalyzeCommand { get; }
    public RelayCommand VersioningCommand { get; }
    public TextDocument Document { get; }

    public TextDocument OutputDocument { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private async Task OnCompile()
    {
        if (code == null)
        {
            return;
        }

        code.Text = Document.Text;
        await code.TryRunScript();

        OutputDocument.Text = code.Result;
        OnPropertyChanged(nameof(OutputDocument));
    }

    private void OnAnalyze()
    {
        if (code == null)
        {
            return;
        }

        code.Text = Document.Text;
        code.Compile();

        OutputDocument.Text = code.Result ?? string.Empty;
        
        OnPropertyChanged(nameof(OutputDocument));
    }

    private void OnVersioning()
    {
    }

    private void OnLoaded()
    {
        host = new RoslynHost(
            new[] { Assembly.Load("RoslynPad.Roslyn.Windows"), Assembly.Load("RoslynPad.Editor.Windows") },
            RoslynHostReferences.NamespaceDefault.With(assemblyReferences: new[] { typeof(object).Assembly, typeof(Regex).Assembly, typeof(Enumerable).Assembly }));

        code = new CodeVm(host);
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
