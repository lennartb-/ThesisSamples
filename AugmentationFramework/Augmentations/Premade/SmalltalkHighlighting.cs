using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Editing;

namespace AugmentationFramework.Augmentations.Premade;

public partial class SmalltalkHighlighting
{
    public static Augmentation[] GetAugmentation(TextArea textArea)
    {
        var parameterRegex = ParameterRegex();
        var symbolRegex = SymbolRegex();
        var numberRegex = NumberRegex();
        var messagesRegex = MessagesRegex();
        var commentRegex = CommentRegex();
        var stringRegex = StringRegex();
        var keywordsRegex = KeywordRegex();

        var comments = new Augmentation(textArea)
            .ForText(commentRegex)
            .WithForeground(Brushes.LightGreen)
            .WithFontStyle(FontStyles.Italic);

        var keywords = new Augmentation(textArea)
            .ForText(keywordsRegex)
            .WithFontWeight(FontWeights.Bold)
            .WithForeground(Brushes.RoyalBlue);

        var messages = new Augmentation(textArea)
            .ForText(messagesRegex)
            .WithForeground(Brushes.Orange);

        var numbersAndStrings = new Augmentation(textArea)
            .ForText(numberRegex, stringRegex)
            .WithForeground(Brushes.MediumAquamarine);

        var symbols = new Augmentation(textArea)
            .ForText(symbolRegex)
            .WithFontWeight(FontWeights.Bold)
            .WithForeground(Brushes.DarkRed);

        var parameters = new Augmentation(textArea)
            .ForText(parameterRegex)
            .WithFontWeight(FontWeights.Bold);

        return new[] { keywords, messages, numbersAndStrings, symbols, parameters, comments };
    }

    [GeneratedRegex("\"(.|\r|\n)*?\"")]
    private static partial Regex CommentRegex();

    [GeneratedRegex("\\b(self|super|true|false|nil)\\b")]
    private static partial Regex KeywordRegex();

    [GeneratedRegex("\\w+:")]
    private static partial Regex MessagesRegex();

    [GeneratedRegex("\\b\\d+(\\.\\d+)?\\b")]
    private static partial Regex NumberRegex();

    [GeneratedRegex(":\\w+")]
    private static partial Regex ParameterRegex();

    [GeneratedRegex("\\'((.|\\r|\\n)*?)\\'")]
    private static partial Regex StringRegex();

    [GeneratedRegex("[$#]\\w+")]
    private static partial Regex SymbolRegex();
}