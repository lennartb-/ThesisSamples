using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using LibGit2Sharp;

namespace CodeManagementSample;

internal class VersioningVm : ObservableObject
{
    private readonly VersioningModel model;
    private CommitModel? selectedItem;

    public VersioningVm(VersioningModel model)
    {
        this.model = model;
        RefreshCommand = new RelayCommand(GetHistory);
        PushCommand = new RelayCommand(CommitCode);
        PreviewDocument = new TextDocument();
        InitOrOpenRepository();
        RefreshCommand.Execute(null);
    }

    public RelayCommand OkCommand { get; }
    public RelayCommand CancelCommand { get; }
    public RelayCommand PushCommand { get; }
    public RelayCommand PullCommand { get; }
    public RelayCommand RefreshCommand { get; }

    public string EntityName { get; set; }
    public TextDocument PreviewDocument { get; set; }

    public CommitModel? SelectedItem
    {
        get => selectedItem;
        set
        {
            SetProperty(ref selectedItem, value);
            if (selectedItem == null)
            {
                return;
            }

            using var repo = new Repository(model.RepositoryPath);
            var entry = repo.Commits.Single(c => c.Id.Sha[..7] == selectedItem.Id);
            ;
            var blob = entry.Tree.SingleOrDefault(t => t.Name == model.BlobId.ToString())?.Target;

            if (blob is Blob b)
            {
                var text = DeserializeBlob(b);

                PreviewDocument.Text = text;
            }
        }
    }

    public ObservableCollection<CommitModel> History { get; } = new();

    public void InitOrOpenRepository()
    {
        var existingRepo = Repository.Discover(model.RepositoryPath);

        if (string.IsNullOrEmpty(existingRepo))
        {
            Repository.Init(model.RepositoryPath, true);
        }
    }

    private void GetHistory()
    {
        History.Clear();
        using var repo = new Repository(model.RepositoryPath);

        foreach (var c in repo.Commits.Take(15))
        {
            History.Add(new CommitModel(c.Id.Sha[..7], c.Author.Name, c.Message, c.Author.When.DateTime));
            //Console.WriteLine("commit {0}", c.Id);

            //if (c.Parents.Count() > 1)
            //{
            //    Console.WriteLine("Merge: {0}", string.Join(" ", c.Parents.Select(p => p.Id.Sha.Substring(0, 7)).ToArray()));
            //}

            //Console.WriteLine("Author: {0} <{1}>", c.Author.Name, c.Author.Email);
            //Console.WriteLine("Date:   {0}", c.Author.When.ToString(RFC2822Format, CultureInfo.InvariantCulture));
            //Console.WriteLine();
            //Console.WriteLine(c.Message);
            //Console.WriteLine();
        }
    }

    private string DeserializeBlob(Blob blob)
    {
        var contentStream = blob.GetContentStream();

        using (var tr = new StreamReader(contentStream, Encoding.UTF8))
        {
            return JsonSerializer.Deserialize<string>(tr.ReadToEnd());
        }
    }

    public void CommitCode()
    {
        using var repo = new Repository(model.RepositoryPath);

        var bytes = JsonSerializer.SerializeToUtf8Bytes(model.BlobContent);
        var ms = new MemoryStream(bytes);
        var newBlob = repo.ObjectDatabase.CreateBlob(ms);

        // Put the blob in a tree
        var td = new TreeDefinition();
        td.Add(model.BlobId.ToString(), newBlob, Mode.NonExecutableFile);
        var tree = repo.ObjectDatabase.CreateTree(td);

        // Committer and author
        var committer = new Signature(model.Author, "test@example.com", DateTime.Now);

        repo.Index.Add(newBlob, model.BlobId.ToString(), Mode.NonExecutableFile);

        repo.Commit("Update", committer, committer);

        var proc = new Process();

        var processStartInfo = new ProcessStartInfo("cmd", "/c git push");
        processStartInfo.RedirectStandardError = processStartInfo.RedirectStandardInput = processStartInfo.RedirectStandardOutput = true;
        processStartInfo.UseShellExecute = false;
        processStartInfo.CreateNoWindow = true;
        processStartInfo.WorkingDirectory = repo.Info.Path;
        proc.StartInfo = processStartInfo;
        proc.Start();

        var sb = new StringBuilder();
        proc.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
        {
            sb.AppendLine(e.Data);
        };
        proc.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
        {
            sb.AppendLine(e.Data);
        };

        proc.BeginOutputReadLine();
        proc.BeginErrorReadLine();
        proc.WaitForExit();

        ;
    }

    public void PullCode()
    {
    }
}
