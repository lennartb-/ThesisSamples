using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Augmentations.Premade;

public class SmalltalkHighlighting
{
    public static Augmentation[] GetAugmentation(TextView textView)
    {
        var parameterRegex = new Regex(@":\w+");
        var symbolRegex = new Regex(@"[$#]\w+");
        var numberRegex = new Regex(@"\b\d+(\.\d+)?\b");
        var messagesRegex = new Regex(@"\w+:");
        var commentRegex = new Regex("\"(.|\r|\n)*?\"");
        var stringRegex = new Regex(@"\'((.|\r|\n)*?)\'");
        var keywordsRegex = new Regex(@"\b(self|super|true|false|nil)\b");

        var comments = new Augmentation(textView)
            .ForText(commentRegex)
            .WithForeground(Brushes.LightGreen)
            .WithFontStyle(FontStyles.Italic);

        var keywords = new Augmentation(textView)
            .ForText(keywordsRegex)
            .WithFontWeight(FontWeights.Bold)
            .WithForeground(Brushes.RoyalBlue);

        var messages = new Augmentation(textView)
            .ForText(messagesRegex)
            .WithForeground(Brushes.Orange);

        var numbersAndStrings = new Augmentation(textView)
            .ForText(numberRegex, stringRegex)
            .WithForeground(Brushes.MediumAquamarine);

        var symbols = new Augmentation(textView)
            .ForText(symbolRegex)
            .WithFontWeight(FontWeights.Bold)
            .WithForeground(Brushes.DarkRed);

        var parameters = new Augmentation(textView)
            .ForText(parameterRegex)
            .WithFontWeight(FontWeights.Bold);

        return new[] { keywords, messages, numbersAndStrings, symbols, parameters, comments };
    }
}
