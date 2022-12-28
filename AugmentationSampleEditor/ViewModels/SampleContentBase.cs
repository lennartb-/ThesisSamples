using System.Collections.Generic;
using System.Windows;
using AugmentationFramework.Augmentations;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using RoslynPad.Editor;

namespace AugmentationFrameworkSampleApp.ViewModels;

/// <summary>
///     Base class for sample content.
/// </summary>
public abstract class SampleContentBase : ISampleContent
{
    private bool isEnabled;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SampleContentBase" /> class.
    /// </summary>
    protected SampleContentBase()
    {
        EditorLoadedCommand = new RelayCommand<CodeTextEditor>(OnLoaded);
    }

    /// <inheritdoc />
    public abstract TextDocument Document { get; }

    /// <inheritdoc />
    public IRelayCommand<CodeTextEditor> EditorLoadedCommand { get; }

    /// <inheritdoc />
    public bool IsEnabled
    {
        get => isEnabled;
        set
        {
            isEnabled = value;
            if (isEnabled)
            {
                foreach (var augmentation in Augmentations)
                {
                    augmentation.Enable();
                }
            }
            else
            {
                foreach (var augmentation in Augmentations)
                {
                    augmentation.Disable();
                }
            }
        }
    }

    /// <inheritdoc />
    public abstract string Title { get; }

    /// <summary>
    ///     Gets the list of augmentations of this sample content.
    /// </summary>
    protected IList<Augmentation> Augmentations { get; } = new List<Augmentation>();

    /// <summary>
    ///     Called when the underlying editor fires its <see cref="FrameworkElement.LoadedEvent" />.
    /// </summary>
    /// <param name="editor">The editor that is passed in from the event arguments.</param>
    protected abstract void OnLoaded(CodeTextEditor? editor);
}