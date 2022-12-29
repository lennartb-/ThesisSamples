using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using AdysTech.CredentialManager;
using LibGit2Sharp;
using Serilog;

namespace CodeManagementSample.GitWrapper;

/// <summary>
///     Implementation of <see cref="IGitWrapper" /> with LibGit2Sharp.
/// </summary>
public class LibGit2SharpWrapper : IGitWrapper
{
    private const string GithubCredentialAddress = "git:https://github.com";
    private readonly GitProcessWrapper gitProcessWrapper;
    private readonly VersioningModel model;

    /// <summary>
    ///     Initializes a new instance of the <see cref="LibGit2SharpWrapper" /> class.
    /// </summary>
    /// <param name="model">A model containing information about the current state of the code to work with.</param>
    public LibGit2SharpWrapper(VersioningModel model)
    {
        this.model = model;

        using var repo = new Repository(model.RepositoryPath);
        gitProcessWrapper = new GitProcessWrapper(repo.Info.WorkingDirectory);
    }

    /// <inheritdoc />
    public bool IsExternalGitAuthenticationEnabled { get; set; }

    /// <inheritdoc />
    public bool Commit(string? message)
    {
        if (IsContentEqualToLatestVersion())
        {
            Log.Logger.Warning("Text to commit is equal to latest commit. Cancelling.");
            return false;
        }

        var blob = SerializeBlob(model.BlobContent);

        using var repo = new Repository(model.RepositoryPath);

        var committer = new Signature(model.Author, "test@example.com", DateTime.Now);

        repo.Index.Add(blob, model.BlobId.ToString(), Mode.NonExecutableFile);
        repo.Index.Write();

        var repositoryStatus = repo.RetrieveStatus(new StatusOptions());

        if (!repositoryStatus.Any())
        {
            Log.Logger.Information("No changes to commit. Cancelling.");
            return false;
        }

        _ = repo.Commit(message, committer, committer);

        if (IsExternalGitAuthenticationEnabled)
        {
            gitProcessWrapper.Push();
        }
        else
        {
            var credentials = GetOrAskCredentials();
            Push(credentials, repo);
        }

        return true;
    }

    /// <inheritdoc />
    public string? GetContentOfCommitWithId(string? id)
    {
        if (id == null)
        {
            Log.Logger.Warning("Id is null, can't check for selected commit.");
            return null;
        }

        using var repo = new Repository(model.RepositoryPath);

        var selectedCommit = repo.Commits.Single(c => c.Id.Sha[..7] == id);
        var gitObject = selectedCommit.Tree.SingleOrDefault(t => t.Name == model.BlobId.ToString())?.Target;
        if (gitObject is Blob blob)
        {
            return DeserializeBlob(blob);
        }

        Log.Logger.Warning("{Id} is not a blob.", id);
        return null;
    }

    /// <inheritdoc />
    public IEnumerable<CommitModel> GetHistory()
    {
        using var repo = new Repository(model.RepositoryPath);

        if (IsExternalGitAuthenticationEnabled)
        {
            gitProcessWrapper.Pull();
        }
        else
        {
            var merger = new Signature(model.Author, "test@example.com", DateTime.Now);
            var credentials = GetOrAskCredentials();
            Pull(credentials, repo, merger);
        }

        foreach (var c in repo.Commits.Take(15))
        {
            yield return new CommitModel(c.Id.Sha[..7], c.Author.Name, c.Message, c.Author.When.DateTime);
        }
    }

    /// <inheritdoc />
    public bool IsContentEqualToLatestVersion()
    {
        return GetContentOfCommitWithId(GetHistory().First().Id) == model.BlobContent;
    }

    private static string? DeserializeBlob(Blob blob)
    {
        var contentStream = blob.GetContentStream();

        using var tr = new StreamReader(contentStream, Encoding.UTF8);
        return JsonSerializer.Deserialize<string>(tr.ReadToEnd());
    }

    private static NetworkCredential GetOrAskCredentials()
    {
        Log.Logger.Information("Getting credentials via credential manager.");
        var existingCredentials = CredentialManager.GetCredentials(GithubCredentialAddress);

        if (existingCredentials == null)
        {
            Log.Logger.Information("Existing credentials not found, prompting user.");
            var save = false;
            return CredentialManager.PromptForCredentials(GithubCredentialAddress, ref save, "Please provide credentials", "Credentials for service");
        }

        return existingCredentials;
    }

    private static void Pull(NetworkCredential cred, Repository repo, Signature merger)
    {
        Log.Logger.Information("Pulling via libgit2sharp.");
        var options = new PullOptions
        {
            FetchOptions = new FetchOptions
            {
                CredentialsProvider = (_, _, _) =>
                    new SecureUsernamePasswordCredentials { Username = cred.UserName, Password = cred.SecurePassword },
            },
        };
        Commands.Pull(repo, merger, options);
    }

    private static void Push(NetworkCredential cred, Repository repo)
    {
        Log.Logger.Information("Pushing via libgit2sharp.");
        var options = new PushOptions
        {
            CredentialsProvider = (_, _, _) =>
                new SecureUsernamePasswordCredentials { Username = cred.UserName, Password = cred.SecurePassword },
        };
        repo.Network.Push(repo.Head, options);
    }

    private Blob SerializeBlob(string blobContents)
    {
        using var repo = new Repository(model.RepositoryPath);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(blobContents);
        var ms = new MemoryStream(bytes);
        return repo.ObjectDatabase.CreateBlob(ms);
    }
}