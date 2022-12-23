using ICSharpCode.AvalonEdit.Highlighting;

namespace CodeManagementSample;

/// <summary>
///     Interaction logic for Versioning.xaml.
/// </summary>
public partial class Versioning
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Versioning"/> class.
    /// </summary>
    public Versioning()
    {
        InitializeComponent();
        Preview.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
    }
}