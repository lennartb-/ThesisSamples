using System.Windows.Controls.Primitives;
using CommunityToolkit.Mvvm.Input;

namespace AugmentationFramework.Generators;

/// <summary>
///     A <see cref="Popup" /> that can be explicitly closed.
/// </summary>
public class ClosablePopup : Popup
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ClosablePopup" /> class.
    /// </summary>
    public ClosablePopup()
    {
        CloseCommand = new RelayCommand(() => IsOpen = false);
        StaysOpen = true;
        MouseDown += (_, e) => e.Handled = true;
    }

    /// <summary>
    ///     Gets the command that closes this popup.
    /// </summary>
    public IRelayCommand CloseCommand { get; }
}