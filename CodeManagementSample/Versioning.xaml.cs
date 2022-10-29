using System.Windows;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.CodeAnalysis.Differencing;

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
