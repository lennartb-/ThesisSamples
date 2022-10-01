using System.Diagnostics;
using System.Text;

namespace CodeManagementSample.GitWrapper;

public class GitProcessWrapper
{
    private readonly string workingDirectory;

    public GitProcessWrapper(string workingDirectory)
    {
        this.workingDirectory = workingDirectory;
    }

    public void Push()
    {
        RunGitCommand("push");

    }

    public void Commit(string message)
    {
        RunGitCommand("commit", "-m", message);
    }

    public void Pull()
    {
        RunGitCommand("pull");
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
            WorkingDirectory = workingDirectory
        };
        process.StartInfo = processStartInfo;
        process.Start();

        var stringBuilder = new StringBuilder();
        process.OutputDataReceived += (_, e) => stringBuilder.AppendLine(e.Data);
        process.ErrorDataReceived += (_, e) => stringBuilder.AppendLine(e.Data);

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
    }
}
