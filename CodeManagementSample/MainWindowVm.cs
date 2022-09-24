using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using RoslynPad.Roslyn;

namespace CodeManagementSample;

public class MainWindowVm
{
    public RelayCommand OnLoadedCommand { get; }
    public RelayCommand CompileCommand { get; }
    public RelayCommand AnalyzeCommand { get; }
    public RelayCommand VersioningCommand { get; }
    private RoslynHost? host;
    private readonly ObservableCollection<CodeVm> documents = new();

    public MainWindowVm()
    {
        OnLoadedCommand = new RelayCommand(OnLoaded);
        CompileCommand = new RelayCommand(OnCompile);
        AnalyzeCommand = new RelayCommand(OnAnalyze);
        VersioningCommand = new RelayCommand(OnVersioning);
    }

    private void OnCompile()
    {
    }

    private void OnAnalyze()
    {
    }

    private void OnVersioning()
    {
    }

    private void OnLoaded()
    {

        host = new RoslynHost(additionalAssemblies: new[]
        {
            Assembly.Load("RoslynPad.Roslyn.Windows"),
            Assembly.Load("RoslynPad.Editor.Windows")
        }, RoslynHostReferences.NamespaceDefault.With(assemblyReferences: new[]
        {
            typeof(object).Assembly,
            typeof(System.Text.RegularExpressions.Regex).Assembly,
            typeof(System.Linq.Enumerable).Assembly,
        }));

        documents.Add(new CodeVm(host));
    }
}
