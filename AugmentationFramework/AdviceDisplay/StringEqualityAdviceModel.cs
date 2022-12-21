namespace AugmentationFramework.AdviceDisplay;

public class StringEqualityAdviceModel : IAdviceModel
{
    public string WarningTitle => "Warning!";

    public string WarningText => "Use Str.IsEqual(string1, string2) method to compare two strings";

    public string WarningSource { get; set; } = string.Empty;

    public string WarningRisk => "Comparing two strings with \"==\" may have undesired behavior";

    public string Information => "http://example.com/comparing-two-strings";

    public Uri InformationLink => new("http://example.com/comparing-two-strings");

    public string SecureAdvice => "To compare the content of two strings, use the Str.IsEqual(string1, string2) method";

    public string SecureSample => "Str.IsEqual(F1000,F1001)";

    public string InsecureAdvice => "To really compare two strings by reference, you can continue to use \"==\"";

    public string InsecureSample => "F1000 == F1002";
}