namespace AugmentationFramework.AdviceDisplay;

public class NewlineAdviceModel : IAdviceModel
{
    public string WarningTitle => "Warning!";
    public string WarningText => "Usage of newline characters in strings is not supported";
    public string WarningSource { get; set; } = string.Empty;

    public string WarningRisk => "Using explicit newline characters may have unintended effects in the generated application.";
    public string Information => "http://example.com/using-newlines";
    public Uri InformationLink => new("http://example.com/using-newlines");
    public string SecureAdvice => "To add newlines to a string, use the Str.LF() method.";
    public string SecureSample => "\"This is a \" + Str.LF() + \" newline in a string.\"";
    public string InsecureAdvice => "Continuing with newline characters in a string has likely not the desired effects in the resulting application.";
    public string InsecureSample => "\"This is a newline \\n in a string.\"\n";
}
