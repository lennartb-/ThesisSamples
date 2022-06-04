using System.Text.RegularExpressions;

namespace RoslynPadTest;

public class WordHighlightPlugin
{
    public Regex TextMatch { get; set; }

}

public enum Position
{
    Line, Word, Custom
}
