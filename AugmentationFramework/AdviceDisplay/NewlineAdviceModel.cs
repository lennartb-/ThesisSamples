namespace AugmentationFramework.AdviceDisplay;

/// <summary>
///     Provides a <see cref="IAdviceModel" /> implementation to warn about wrong usage of newline characters.
/// </summary>
public class NewlineAdviceModel : IAdviceModel
{
    /// <inheritdoc />
    public string Information => "http://example.com/using-newlines";

    /// <inheritdoc />
    public Uri InformationLink => new("http://example.com/using-newlines");

    /// <inheritdoc />
    public string InsecureAdvice => "Continuing with newline characters in a string has likely not the desired effects in the resulting application.";

    /// <inheritdoc />
    public string InsecureSample => "\"This is a newline \\n in a string.\"\n";

    /// <inheritdoc />
    public string SecureAdvice => "To add newlines to a string, use the Str.LF() method.";

    /// <inheritdoc />
    public string SecureSample => "\"This is a \" + Str.LF() + \" newline in a string.\"";

    /// <inheritdoc />
    public string WarningRisk => "Using explicit newline characters may have unintended effects in the generated application.";

    /// <inheritdoc />
    public string WarningSource { get; set; } = string.Empty;

    /// <inheritdoc />
    public string WarningText => "Usage of newline characters in strings is not supported";

    /// <inheritdoc />
    public string WarningTitle => "Warning!";
}