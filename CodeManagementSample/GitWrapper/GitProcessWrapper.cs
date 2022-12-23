using System.Diagnostics;
using System.Text;
using Serilog;

namespace CodeManagementSample.GitWrapper;

/// <summary>
///     Executes Git commands via process invocation.
/// </summary>
public class GitProcessWrapper
{
    private readonly string workingDirectory;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GitProcessWrapper" /> class.
    /// </summary>
    /// <param name="workingDirectory">The working directory of the Git repository.</param>
    public GitProcessWrapper(string workingDirectory)
    {
        this.workingDirectory = workingDirectory;
    }

    /// <summary>
    ///     Executes git pull.
    /// </summary>
    public void Pull()
    {
        RunGitCommand("pull");
    }

    /// <summary>
    ///     Executes git push.
    /// </summary>
    public void Push()
    {
        RunGitCommand("push");
    }

    private void RunGitCommand(params string[] args)
    {
        var process = new Process();

        var argumentString = string.Join(' ', args);

        var processStartInfo = new ProcessStartInfo("cmd", "/c git " + argumentString)
        {
            RedirectStandardError = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = workingDirectory,
        };
        process.StartInfo = processStartInfo;

        Log.Logger.Information("About to run {Process} {CommandLine}", processStartInfo.FileName, processStartInfo.Arguments);
        process.Start();

        var stringBuilder = new StringBuilder();
        process.OutputDataReceived += (_, e) => stringBuilder.AppendLine(e.Data);
        process.ErrorDataReceived += (_, e) => stringBuilder.AppendLine(e.Data);

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        Log.Logger.Information("git returned: {Status}", stringBuilder.ToString());
    }
}