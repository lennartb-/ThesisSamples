using System.Diagnostics;
using System.Windows.Navigation;

namespace AugmentationFramework.AdviceDisplay;

/// <summary>
///     Interaction logic for AdviceView.xaml
/// </summary>
public partial class AdviceView 
{
    public AdviceView()
    {
        InitializeComponent();
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        e.Handled = true;
    }
}
