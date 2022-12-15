using ICSharpCode.AvalonEdit.Highlighting;

namespace CodeManagementSample;

/// <summary>
///     Interaction logic for Versioning.xaml
/// </summary>
public partial class Versioning
{
    public Versioning()
    {
        InitializeComponent();
        Preview.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
    }
}

