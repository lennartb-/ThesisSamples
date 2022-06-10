using System.Text.RegularExpressions;
using AugmentationFramework.Augmentations;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework;

public class RoiFinder
{
    private readonly Augmentation parent;
    private readonly TextView textView;

    public RoiFinder(Augmentation parent)
    {
        this.parent = parent;
        textView = this.parent.TextView;
    }

    public IEnumerable<(int startOffset, int endOffset)> DetermineRangesOfInterest()
    {
        var viewStart = textView.VisualLines.First().FirstDocumentLine.Offset;
        var viewEnd = textView.VisualLines.Last().LastDocumentLine.EndOffset;

        var relevantText = textView.Document.GetText(viewStart, viewEnd - viewStart);

        if (parent.TextMatchRegex is { } regex)
        {
            return DetermineRegexTextMatches(regex, relevantText);
        }

        if (parent.TextMatch is { } text)
        {
            return DetermineTextMatches(text, relevantText);
        }

        return new[] { (viewStart, viewEnd) };
    }

    private static IEnumerable<(int startOffset, int endOffset)> DetermineTextMatches(string searchText, string textRegion)
    {
        var offsetAtMatch = FindTextMatch(searchText, 0, textRegion);
        while (offsetAtMatch >= 0)
        {
            var matchLength = offsetAtMatch + searchText.Length;
            yield return (offsetAtMatch, matchLength);
            offsetAtMatch = FindTextMatch(searchText, matchLength, textRegion);
        }

    }

    private static IEnumerable<(int startOffset, int endOffset)> DetermineRegexTextMatches(Regex regex, string textRegion)
    {
        var match = FindTextRegexMatch(regex, textRegion);

        while (match.Success)
        {
            var matchLength = match.Index + match.Value.Length;
            yield return (match.Index, matchLength);

            match = match.NextMatch();
        }
    }

    private static Match FindTextRegexMatch(Regex regex, string relevantText)
    {
        return regex.Match(relevantText);
    }

    private static int FindTextMatch(string text, int startIndex, string relevantText)
    {
        return relevantText.IndexOf(text, startIndex, StringComparison.CurrentCulture);
    }
}
