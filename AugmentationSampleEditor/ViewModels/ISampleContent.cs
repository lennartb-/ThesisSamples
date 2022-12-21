using System.Reactive;
using ICSharpCode.AvalonEdit.Document;
using ReactiveUI;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

public interface ISampleContent
{
    TextDocument Document { get; }

    bool IsEnabled { get; set; }

    string Title { get; }

    ReactiveCommand<CodeTextEditor, Unit> EditorLoadedCommand { get; }
}