using System.Collections.Generic;

namespace CodeManagementSample.GitWrapper;

/// <summary>
///     Describes the access to a Git repository.
/// </summary>
public interface IGitWrapper
{
    /// <summary>
    ///     Gets or sets a value indicating whether the Windows-integrated Git authentication should be used.
    /// </summary>
    bool IsExternalGitAuthenticationEnabled { get; set; }

    /// <summary>
    ///     Creates a commit and pushes code to the repository.
    /// </summary>
    /// <param name="message">A commit message.</param>
    /// <returns>True if commiting and pushing succeeded, false if not.</returns>
    bool Commit(string? message);

    /// <summary>
    ///     Retrieves the blob content of a specific commit.
    /// </summary>
    /// <param name="id">The ID of the commit.</param>
    /// <returns>The content of the commit, or null if <paramref name="id" /> is null or the commit with the ID is not a blob.</returns>
    string? GetContentOfCommitWithId(string? id);

    /// <summary>
    ///     Gets a list of commits from the repository.
    /// </summary>
    /// <returns>The last 15 commits from the repository.</returns>
    IEnumerable<CommitModel> GetHistory();

    /// <summary>
    ///     Checks whether the content of the current blob is equal to the latest commit in the repository.
    /// </summary>
    /// <returns>True if the content is equal, false if not.</returns>
    bool IsContentEqualToLatestVersion();
}