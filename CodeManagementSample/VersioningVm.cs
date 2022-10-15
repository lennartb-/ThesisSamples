﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using CodeManagementSample.GitWrapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibGit2Sharp;

namespace CodeManagementSample;

internal class VersioningVm : ObservableObject
{
    private readonly GitProcessWrapper gitProcessWrapper;
    private readonly VersioningModel model;
    private string? previewText;
    private CommitModel? selectedItem;
    private string? commitMessage;
    public string? CheckedOutText { get; private set; }

    public VersioningVm(VersioningModel model)
    {
        this.model = model;
        OkCommand = new RelayCommand(ApplyCheckout, () => SelectedItem != null);
        CancelCommand = new RelayCommand(CancelCheckout);
        RefreshCommand = new RelayCommand(GetHistory);
        PushCommand = new RelayCommand(CommitCode);

        using var repo = new Repository(model.RepositoryPath);
        gitProcessWrapper = new GitProcessWrapper(repo.Info.WorkingDirectory);

        RefreshCommand.Execute(null);
    }
    public event Action RequestClose = delegate { };
    private void CancelCheckout()
    {
        CheckedOutText = null;
        RequestClose();
    }

    private void ApplyCheckout()
    {
        CheckoutVersion();
        RequestClose();
    }

    public RelayCommand OkCommand { get; }
    public RelayCommand CancelCommand { get; }
    public RelayCommand PushCommand { get; }
    public RelayCommand RefreshCommand { get; }

    public string? CommitMessage
    {
        get => commitMessage;
        set => SetProperty(ref commitMessage, value);
    }

    public string? PreviewText
    {
        get => previewText;
        set => SetProperty(ref previewText, value);
    }

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

            var selectedText = GetStringOfSelectedCommit();

            PreviewText = selectedText ?? "[No content available]";

            OkCommand.NotifyCanExecuteChanged();
        }
    }

    public ObservableCollection<CommitModel> History { get; } = new();

    private void GetHistory()
    {
        History.Clear();
        using var repo = new Repository(model.RepositoryPath);
        gitProcessWrapper.Pull();
        foreach (var c in repo.Commits.Take(15))
        {
            History.Add(new CommitModel(c.Id.Sha[..7], c.Author.Name, c.Message, c.Author.When.DateTime));
        }
    }

    private string? DeserializeBlob(Blob blob)
    {
        var contentStream = blob.GetContentStream();

        using var tr = new StreamReader(contentStream, Encoding.UTF8);
        return JsonSerializer.Deserialize<string>(tr.ReadToEnd());
    }

    public void CommitCode()
    {
        var blob = SerializeBlob();

        using var repo = new Repository(model.RepositoryPath);

        var committer = new Signature(model.Author, "test@example.com", DateTime.Now);

        TreeDefinition td = new TreeDefinition();
        td.Add(model.BlobId.ToString(), blob, Mode.NonExecutableFile);
        Tree tree = repo.ObjectDatabase.CreateTree(td);
        repo.Index.Add(blob, model.BlobId.ToString(), Mode.NonExecutableFile);
        repo.Index.Write();

        var commit = repo.ObjectDatabase.CreateCommit(
            committer,
            committer,
            CommitMessage,
            tree,
            repo.Commits,
        prettifyMessage: false);

        // Update the HEAD reference to point to the latest commit
        repo.Refs.UpdateTarget(repo.Refs.Head, commit.Id);

        var d = repo.Diff.Compare<TreeChanges>().Count > 0;

        gitProcessWrapper.Push();
        GetHistory();
    }

    private Blob SerializeBlob()
    {
        using var repo = new Repository(model.RepositoryPath);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(model.BlobContent);
        var ms = new MemoryStream(bytes);
        return repo.ObjectDatabase.CreateBlob(ms);
    }

    public void CheckoutVersion()
    {
        CheckedOutText = GetStringOfSelectedCommit();
    }

    private string? GetStringOfSelectedCommit()
    {
        using var repo = new Repository(model.RepositoryPath);

        var selectedCommit = repo.Commits.Single(c => c.Id.Sha[..7] == SelectedItem.Id);
        var gitObject = selectedCommit.Tree.SingleOrDefault(t => t.Name == model.BlobId.ToString())?.Target;
        if (gitObject is Blob blob)
        {
            return DeserializeBlob(blob);
        }

        return null;
    }
}
