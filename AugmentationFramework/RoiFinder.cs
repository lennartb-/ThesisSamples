using System.Text.RegularExpressions;
using AugmentationFramework.Augmentations;
using Serilog;

namespace AugmentationFramework;

/// <summary>
///     Finds text offsets based on the texts or regular expressions an augmentation should be applied to.
/// </summary>
public class RoiFinder
{
    private readonly Augmentation parent;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RoiFinder" /> class.
    /// </summary>
    /// <param name="parent">
    ///     The augmentation that contains the texts or regular expressions whose offsets should be
    ///     determined.
    /// </param>
    public RoiFinder(Augmentation parent)
    {
        this.parent = parent;
    }

    /// <summary>
    ///     Calculates a list of offsets the augmentation will be applied to.
    /// </summary>
    /// <param name="textRegion">The text that will be searched for text and regular expression matches.</param>
    /// <returns>A list of offsets containing the starts and ends of the found matches.</returns>
    public IEnumerable<(int StartOffset, int EndOffset)> DetermineRangesOfInterest(string textRegion)
    {
        if (parent is { MatchingDelegate: { } matchingDelegate, TextMatchesRegex: { } matchingRegex })
        {
            return DetermineDelegateMatches(matchingDelegate, matchingRegex, textRegion);
        }

        if (parent.TextMatchesRegex is { } regexes)
        {
            return DetermineRegexTextMatches(regexes, textRegion);
        }

        if (parent.TextMatches is { } textMatches)
        {
            return DetermineTextMatches(textMatches, textRegion);
        }

        return Enumerable.Empty<(int, int)>();
    }

    private static IEnumerable<(int StartOffset, int EndOffset)> DetermineDelegateMatches(Func<Match, bool> matchingDelegate, IEnumerable<Regex> regexses, string textRegion)
    {
        var matches = regexses.Select(regex => FindTextRegexMatch(regex, textRegion)).ToList();

        foreach (var match in matches)
        {
            var localMatch = match;
            while (localMatch.Success)
            {
                Log.Logger.Information("{Method} found match: {Match}", nameof(DetermineDelegateMatches), localMatch);
                if (matchingDelegate(localMatch))
                {
                    var matchLength = localMatch.Index + localMatch.Value.Length;
                    yield return (localMatch.Index, matchLength);
                }

                localMatch = localMatch.NextMatch();
            }
        }
    }

    private static IEnumerable<(int StartOffset, int EndOffset)> DetermineRegexTextMatches(IEnumerable<Regex> regexes, string textRegion)
    {
        var offsets = new List<(int StartOffset, int EndOffset)>();
        foreach (var regex in regexes)
        {
            offsets.AddRange(DetermineRegexTextMatches(regex, textRegion));
        }

        return offsets;
    }

    private static IEnumerable<(int StartOffset, int EndOffset)> DetermineRegexTextMatches(Regex regex, string textRegion)
    {
        var match = FindTextRegexMatch(regex, textRegion);

        while (match.Success)
        {
            Log.Logger.Information("{Method} found match: {Match}", nameof(DetermineRegexTextMatches), match);
            var matchLength = match.Index + match.Value.Length;
            yield return (match.Index, matchLength);

            match = match.NextMatch();
        }
    }

    private static IEnumerable<(int StartOffset, int EndOffset)> DetermineTextMatches(string searchText, string textRegion)
    {
        var offsetAtMatch = FindTextMatch(searchText, 0, textRegion);
        while (offsetAtMatch >= 0)
        {
            var matchLength = offsetAtMatch + searchText.Length;
            yield return (offsetAtMatch, matchLength);
            offsetAtMatch = FindTextMatch(searchText, matchLength, textRegion);
        }
    }

    private static IEnumerable<(int StartOffset, int EndOffset)> DetermineTextMatches(IEnumerable<string> searchTexts, string textRegion)
    {
        var offsets = new List<(int StartOffset, int EndOffset)>();
        foreach (var searchText in searchTexts)
        {
            offsets.AddRange(DetermineTextMatches(searchText, textRegion));
        }

        return offsets;
    }

    private static int FindTextMatch(string text, int startIndex, string relevantText)
    {
        return relevantText.IndexOf(text, startIndex, StringComparison.CurrentCulture);
    }

    private static Match FindTextRegexMatch(Regex regex, string relevantText)
    {
        return regex.Match(relevantText);
    }
}