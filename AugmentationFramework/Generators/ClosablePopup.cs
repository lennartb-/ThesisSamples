using System.Windows.Controls.Primitives;
using CommunityToolkit.Mvvm.Input;

namespace AugmentationFramework.Generators;

public class ClosablePopup : Popup
{
    public ClosablePopup()
    {
        CloseCommand = new RelayCommand(() => IsOpen = false);
        StaysOpen = true;
        MouseDown += (s, e) => e.Handled = true;
    }

    public IRelayCommand CloseCommand { get; }
}