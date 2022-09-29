using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting.Hosting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using RoslynPad.Roslyn;

namespace CodeManagementSample;

public class CodeVm : INotifyPropertyChanged
{
    private readonly RoslynHost host;
    private string? consoleOutput;
    private string? result;

    public CodeVm(RoslynHost host)
    {
        this.host = host;
    }

    public string Text { get; set; }

    public Script<object> Script { get; private set; }

    public string? Result
    {
        get => result;
        private set => SetProperty(ref result, value);
    }

    public string? ConsoleOutput
    {
        get => consoleOutput;
        private set => SetProperty(ref consoleOutput, value);
    }

    public bool HasError { get; private set; }

    private static MethodInfo? HasSubmissionResult { get; } =
        typeof(Compilation).GetMethod(nameof(HasSubmissionResult), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

    private static PrintOptions PrintOptions { get; } = new() { MemberDisplayFormat = MemberDisplayFormat.SeparateLines };

    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task<bool> TryRunScript()
    {
        if (!Compile())
        {
            return false;
        }

        var compilationResult = Script.GetCompilation();
        var hasResult = (bool?)HasSubmissionResult?.Invoke(compilationResult, null);

        await Run(hasResult);

        return true;
    }

    public bool Compile()
    {
        Script = CSharpScript.Create(
            Text,
            ScriptOptions.Default
                .WithReferences(host.DefaultReferences)
                .AddReferences(Assembly.GetAssembly(typeof(Console)))
                .WithImports(host.DefaultImports)
                .AddImports("System.Console"));

        var diagnostics = Script.Compile();
        if (diagnostics.Any(t => t.Severity == DiagnosticSeverity.Error))
        {
            Result = string.Join(Environment.NewLine, diagnostics.Select(FormatReturnValue));
            return false;
        }

        Result = null;
        return true;
    }

    private static string FormatException(Exception ex)
    {
        return CSharpObjectFormatter.Instance.FormatException(ex);
    }

    private static string FormatReturnValue(object o)
    {
        return CSharpObjectFormatter.Instance.FormatObject(o, PrintOptions);
    }

    private async Task Run(bool? hasResult)
    {
        if (!hasResult.HasValue)
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
                    HasError = true;
                    Result = FormatException(scriptResult.Exception);
                }
                else
                {
                    Result = hasResult.Value ? FormatReturnValue(scriptResult.ReturnValue) : null;
                }
            }
            finally
            {
                Console.SetOut(previousConsoleOut);
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            Result = FormatException(ex);
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
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
