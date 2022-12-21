using System.Diagnostics;
using System.Windows.Navigation;

namespace AugmentationFramework.AdviceDisplay;

/// <summary>
///     Interaction logic for AdviceView.xaml.
/// </summary>
public partial class AdviceView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AdviceView" /> class.
    /// </summary>
    public AdviceView()
    {
        InitializeComponent();
    }

    private void OnHyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        e.Handled = true;
    }
}