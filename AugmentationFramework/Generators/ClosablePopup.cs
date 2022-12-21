using System.Windows.Controls.Primitives;
using ReactiveUI;

namespace AugmentationFramework.Generators;

public class ClosablePopup : Popup
{
    public ClosablePopup()
    {
        CloseCommand = ReactiveCommand.Create(() => IsOpen = false);
        StaysOpen = true;
        MouseDown += (s, e) => e.Handled = true;
    }

    public IReactiveCommand CloseCommand { get; }
}