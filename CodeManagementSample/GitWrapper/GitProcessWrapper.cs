﻿using System.Diagnostics;
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

    public void Pull()
    {
        RunGitCommand("pull");
    }

    private void RunGitCommand(params string[] args)
    {
        var process = new Process();

        var processStartInfo = new ProcessStartInfo("cmd", "/c git " + args)
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
