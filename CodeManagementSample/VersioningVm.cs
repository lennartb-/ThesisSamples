using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using LibGit2Sharp;

namespace CodeManagementSample;
internal class VersioningVm
{
    private string repositoryPath;

    public RelayCommand OkCommand { get; }
    public RelayCommand CancelCommand { get; }
    public RelayCommand PushCommand { get; }
    public RelayCommand PullCommand { get; }

    public string EntityName { get; set; }
    public TextDocument PreviewDocument { get; set; }

    public IEnumerable<Commit> History { get; }

    public VersioningVm(string repositoryPath)
    {
        this.repositoryPath = repositoryPath;
    }

    public void InitOrOpenRepository()
    {
        var existingRepo = Repository.Discover(repositoryPath);

        if (string.IsNullOrEmpty(existingRepo))
        {
            Repository.Init(repositoryPath);
        }
    }

    public void CommitCode()
    {

    }

    public void PullCode()
    {

    }
}
