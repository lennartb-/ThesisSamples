using System.Reactive;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

/// <summary>
/// Defines a page of sample content.
/// </summary>
public interface ISampleContent
{
    /// <summary>
    /// Gets the text document this instance uses to display its content.
    /// </summary>
    TextDocument Document { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the sample content is enabled.
    /// </summary>
    bool IsEnabled { get; set; }

    /// <summary>
    /// Gets the title of the sample content.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Gets a command that is called when the underlying editor fires its <see cref="FrameworkElement.LoadedEvent"/>.
    /// </summary>
    IRelayCommand<CodeTextEditor> EditorLoadedCommand { get; }
}