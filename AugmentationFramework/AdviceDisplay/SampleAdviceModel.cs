namespace AugmentationFramework.AdviceDisplay;

/// <summary>
///     Provides a sample implementation of <see cref="IAdviceModel" />.
/// </summary>
public class SampleAdviceModel : IAdviceModel
{
    /// <inheritdoc />
    public string WarningTitle => "Insecure API!";

    /// <inheritdoc />
    public string WarningText => "Usage of this API can lead to security issues.";

    /// <inheritdoc />
    public string WarningSource { get; set; } = string.Empty;

    /// <inheritdoc />
    public string WarningRisk => "Potential leak of user data.";

    /// <inheritdoc />
    public string Information => "www.google.de";

    /// <inheritdoc />
    public Uri InformationLink => new("http://www.google.de");

    /// <inheritdoc />
    public string SecureAdvice => "Use this:";

    /// <inheritdoc />
    public string SecureSample => "public static void Main (string[] args){}";

    /// <inheritdoc />
    public string InsecureAdvice => "Not this";

    /// <inheritdoc />
    public string InsecureSample => "public static void Main (string[] args){}";
}