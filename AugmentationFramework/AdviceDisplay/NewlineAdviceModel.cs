namespace AugmentationFramework.AdviceDisplay;

public class NewlineAdviceModel : IAdviceModel
{
    public string WarningTitle => "Warning!";
    public string WarningText => "Use .Equals() method to compare two strings";
    public string WarningSource { get; set; } = string.Empty;

    public string WarningRisk => "Comparing two strings with \"==\" may have undesired behavior";
    public string Information => "company.confluence.com/comparing-two-strings";
    public string SecureAdvice => "To compare the content of two strings, use the .Equals() method";
    public string SecureSample => "F1000.Equals(F1001)";
    public string InsecureAdvice => "To really compare two strings by reference, you can continue to use \"==\"";
    public string InsecureSample => "F1000 == F1002";
}
