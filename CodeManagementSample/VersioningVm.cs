using System;
using System.Collections.ObjectModel;
using System.Linq;
using CodeManagementSample.GitWrapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;

namespace CodeManagementSample;

/// <summary>
///     Viewmodel for the versioning sample.
/// </summary>
internal class VersioningVm : ObservableObject
{
    private readonly IGitWrapper wrapper;

    private string? commitMessage;
    private TextDocument previewDocument = null!;
    private CommitModel? selectedItem;

    /// <summary>
    ///     Initializes a new instance of the <see cref="VersioningVm" /> class.
    /// </summary>
    /// <param name="wrapper">An instance of <see cref="IGitWrapper"/> to provide access to Git.</param>
    public VersioningVm(IGitWrapper wrapper)
    {
        this.wrapper = wrapper;
        OkCommand = new RelayCommand(ApplyCheckout, () => SelectedItem != null);
        CancelCommand = new RelayCommand(CancelCheckout);
        RefreshHistoryCommand = new RelayCommand(GetHistory);

        PreviewDocument = new TextDocument();

        PushCommand = new RelayCommand(CommitCode, () => !wrapper.IsContentEqualToLatestVersion());
        RefreshHistoryCommand.Execute(null);
    }

    /// <summary>
    ///     Invoked when a request to close the connected window is run.
    /// </summary>
    public event Action RequestClose = () => { };

    /// <summary>
    ///     Gets the command that is executed when the Cancel button is clicked.
    /// </summary>
    public RelayCommand CancelCommand { get; }

    /// <summary>
    ///     Gets the contents of the currently checked out commit.
    /// </summary>
    public string? CheckedOutText { get; private set; }

    /// <summary>
    ///     Gets or sets the commit message for the current changes.
    /// </summary>
    public string? CommitMessage
    {
        get => commitMessage;
        set => SetProperty(ref commitMessage, value);
    }

    /// <summary>
    ///     Gets the list of commits from the repository.
    /// </summary>
    public ObservableCollection<CommitModel> History { get; } = new();

    /// <summary>
    ///     Gets or sets a value indicating whether the Windows-integrated Git authentication should be used.
    /// </summary>
    public bool IsExternalGitAuthenticationEnabled
    {
        get => wrapper.IsExternalGitAuthenticationEnabled;
        set => wrapper.IsExternalGitAuthenticationEnabled = value;
    }

    /// <summary>
    ///     Gets the command that is executed when the OK button is clicked.
    /// </summary>
    public RelayCommand OkCommand { get; }

    /// <summary>
    ///     Gets or sets the preview contents of a commit.
    /// </summary>
    public TextDocument PreviewDocument
    {
        get => previewDocument;
        set => SetProperty(ref previewDocument, value);
    }

    /// <summary>
    ///     Gets the command that is executed when a commit should be pushed to git.
    /// </summary>
    public RelayCommand PushCommand { get; }

    /// <summary>
    ///     Gets the command that is executed when the history should be refreshed..
    /// </summary>
    public RelayCommand RefreshHistoryCommand { get; }

    /// <summary>
    ///     Gets or sets the selected commit.
    /// </summary>
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

            var selectedText = wrapper.GetContentOfCommitWithId(SelectedItem?.Id);

            PreviewDocument.Text = selectedText ?? "[No content available]";

            OkCommand.NotifyCanExecuteChanged();
        }
    }

    /// <summary>
    ///     Sets the contents of the commit selected by <see cref="SelectedItem" /> to <see cref="CheckedOutText" />.
    /// </summary>
    public void CheckoutVersion()
    {
        CheckedOutText = wrapper.GetContentOfCommitWithId(SelectedItem?.Id);
    }

    /// <summary>
    ///     Creates a commit and pushes code to the repository.
    /// </summary>
    public void CommitCode()
    {
        if (wrapper.Commit(CommitMessage))
        {
            GetHistory();
            SelectedItem = History.First();
        }
    }

    private void ApplyCheckout()
    {
        CheckoutVersion();
        RequestClose();
    }

    private void CancelCheckout()
    {
        CheckedOutText = null;
        RequestClose();
    }

    private void GetHistory()
    {
        History.Clear();

        foreach (var c in wrapper.GetHistory())
        {
            History.Add(c);
        }

        PushCommand.NotifyCanExecuteChanged();
    }
}