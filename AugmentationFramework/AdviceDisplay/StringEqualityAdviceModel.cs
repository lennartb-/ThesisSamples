namespace AugmentationFramework.AdviceDisplay;

/// <summary>
///     Provides an advice for incorrect string comparisons.
/// </summary>
public class StringEqualityAdviceModel : IAdviceModel
{
    /// <inheritdoc />
    public string Information => "http://example.com/comparing-two-strings";

    /// <inheritdoc />
    public Uri InformationLink => new("http://example.com/comparing-two-strings");

    /// <inheritdoc />
    public string InsecureAdvice => "To really compare two strings by reference, you can continue to use \"==\"";

    /// <inheritdoc />
    public string InsecureSample => "F1000 == F1002";

    /// <inheritdoc />
    public string SecureAdvice => "To compare the content of two strings, use the Str.IsEqual(string1, string2) method";

    /// <inheritdoc />
    public string SecureSample => "Str.IsEqual(F1000,F1001)";

    /// <inheritdoc />
    public string WarningRisk => "Comparing two strings with \"==\" may have undesired behavior";

    /// <inheritdoc />
    public string WarningSource { get; set; } = string.Empty;

    /// <inheritdoc />
    public string WarningText => "Use Str.IsEqual(string1, string2) method to compare two strings";

    /// <inheritdoc />
    public string WarningTitle => "Warning!";
}