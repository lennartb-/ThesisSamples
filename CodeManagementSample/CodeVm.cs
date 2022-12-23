using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting.Hosting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using RoslynPad.Roslyn;

namespace CodeManagementSample;

/// <summary>
/// Viewmodel for code compilation.
/// </summary>
public class CodeVm : INotifyPropertyChanged
{
    private readonly RoslynHost host;

    private readonly StringBuilder resultBuilder = new();
    private string? consoleOutput;
    private string? compilationResult;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CodeVm" /> class.
    /// </summary>
    /// <param name="host">The <see cref="RoslynHost" /> environment used to execute code.</param>
    public CodeVm(RoslynHost host)
    {
        this.host = host;
    }

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    ///     Gets the output from the console.
    /// </summary>
    public string? ConsoleOutput
    {
        get => consoleOutput;
        private set => SetProperty(ref consoleOutput, value);
    }

    /// <summary>
    ///     Gets the diagnostic results from the compilation. Null if no diagnostics were emitted.
    /// </summary>
    public string? CompilationResult
    {
        get => compilationResult;
        private set => SetProperty(ref compilationResult, value);
    }

    /// <summary>
    ///     Gets or sets the code to compile.
    /// </summary>
    public string? Code { get; set; }

    private static PrintOptions PrintOptions { get; } = new() { MemberDisplayFormat = MemberDisplayFormat.Hidden };

    private Script<object>? Script { get; set; }

    /// <summary>
    ///     Compiles the code in <see cref="Code" />.
    /// </summary>
    /// <returns>True if compilation was successful, false if not.</returns>
    public bool Compile()
    {
        CompilationResult = null;
        ConsoleOutput = null;
        resultBuilder.Clear();
        Script = CSharpScript.Create(
            Code,
            ScriptOptions.Default
                .WithReferences(host.DefaultReferences)
                .AddReferences(Assembly.GetAssembly(typeof(Console)))
                .WithImports(host.DefaultImports)
                .AddImports("System.Console"));

        var compileDiagnostics = Script.Compile();
        var allDiagnostics = Script.GetCompilation().GetDiagnostics();
        if (compileDiagnostics.Any(t => t.Severity == DiagnosticSeverity.Error))
        {
            CompilationResult = string.Join(Environment.NewLine, compileDiagnostics.Select(FormatReturnValue));
            return false;
        }

        if (allDiagnostics.Any())
        {
            resultBuilder.AppendLine(string.Join(Environment.NewLine, allDiagnostics.Select(FormatReturnValue)));
        }

        CompilationResult = resultBuilder.ToString();
        return true;
    }

    /// <summary>
    ///     Compiles the script and runs it if compilation was successful.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}" /> representing the result of the asynchronous operation.</returns>
    public async Task TryRunScript()
    {
        if (!Compile())
        {
            return;
        }

        await Run();
    }

    private static string FormatException(Exception ex)
    {
        return CSharpObjectFormatter.Instance.FormatException(ex);
    }

    private static string FormatReturnValue(object o)
    {
        return CSharpObjectFormatter.Instance.FormatObject(o, PrintOptions);
    }

    private async Task Run()
    {
        if (Script == null)
        {
            return;
        }

        try
        {
            var previousConsoleOut = Console.Out;
            try
            {
                await using var writer = new StringWriter();
                Console.SetOut(writer);
                var scriptResult = await Script.RunAsync();
                await writer.FlushAsync();
                ConsoleOutput = writer.GetStringBuilder().ToString();

                if (scriptResult.Exception != null)
                {
                    resultBuilder.AppendLine(FormatException(scriptResult.Exception));
                }
                else
                {
                    if (scriptResult.ReturnValue is { } returnValue)
                    {
                        resultBuilder.AppendLine(FormatReturnValue(returnValue));
                    }
                }
            }
            finally
            {
                Console.SetOut(previousConsoleOut);
            }
        }
        catch (Exception ex)
        {
            resultBuilder.AppendLine(FormatException(ex));
        }
        finally
        {
            CompilationResult = resultBuilder.ToString();
        }
    }

    private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value) && (value != null))
        {
            return false;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}