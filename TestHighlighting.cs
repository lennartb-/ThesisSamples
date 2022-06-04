using System.Collections.Generic;
using ICSharpCode.AvalonEdit.Highlighting;

namespace RoslynPadTest;

public class TestHighlighting : IHighlightingDefinition
{
    public HighlightingRuleSet GetNamedRuleSet(string name)
    {
        throw new System.NotImplementedException();
    }

    public HighlightingColor GetNamedColor(string name)
    {
        throw new System.NotImplementedException();
    }

    public string Name => "TestRule";
    public HighlightingRuleSet MainRuleSet { get; }
    public IEnumerable<HighlightingColor> NamedHighlightingColors { get; }
    public IDictionary<string, string> Properties { get; }
}