using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Rendering;

namespace AugmentationFramework.Augmentations.Premade;

public class SmalltalkHighlighting
{
    private static readonly Regex Keywords = new(@"\b(self|super|true|false|nil)\b");

    public static Augmentation[] GetAugmentation(TextView textView)
    {

        var parameters = new Regex(@":\w+");
        var constants = new Regex(@"[$#]\w+");
        var numbers = new Regex(@"\b\d+(\.\d+)?\b");
        var keymes = new Regex(@"\w+:");
        var comment = new Regex("\"(.*?)\"");
        var str = new Regex(@"\'(.*?)\'");

        var comments = new Augmentation(textView)
            .ForText(comment)
            .WithBackground(Brushes.DarkGreen)
            .WithForeground(Brushes.White)
            .WithFontSize(16);

        var keywords = new Augmentation(textView)
            .ForText(Keywords)
            .WithFontWeight(FontWeights.Bold)
            .WithForeground(Brushes.RoyalBlue);

        var messages = new Augmentation(textView)
            .ForText(keymes)
            .WithForeground(Brushes.Orange);

        var numsAndStrings = new Augmentation(textView)
            .ForText(numbers, str)
            .WithForeground(Brushes.MediumAquamarine);

        var consts = new Augmentation(textView)
            .ForText(constants)
            .WithFontWeight(FontWeights.Bold)
            .WithForeground(Brushes.DarkRed);

        var ps = new Augmentation(textView)
            .ForText(parameters)
            .WithFontWeight(FontWeights.Bold);

        return new[] { keywords, messages, numsAndStrings, consts, ps, comments };
    }
}
