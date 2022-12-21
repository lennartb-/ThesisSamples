using System.Reactive;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

public interface ISampleContent
{
    TextDocument Document { get; }

    bool IsEnabled { get; set; }

    string Title { get; }

    IRelayCommand<CodeTextEditor> EditorLoadedCommand { get; }
}