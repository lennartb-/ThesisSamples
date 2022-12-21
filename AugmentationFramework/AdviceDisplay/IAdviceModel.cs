namespace AugmentationFramework.AdviceDisplay;

/// <summary>
///     Specifies a model that contains information about an advice.
/// </summary>
public interface IAdviceModel
{
    /// <summary>
    ///     Gets the text displayed for <see cref="InformationLink" />.
    /// </summary>
    string Information { get; }

    /// <summary>
    ///     Gets a hyperlink with more information about the advice.
    /// </summary>
    Uri InformationLink { get; }

    /// <summary>
    ///     Gets the title for the <see cref="InsecureSample" />.
    /// </summary>
    string InsecureAdvice { get; }

    /// <summary>
    ///     Gets the code for an insecure sample.
    /// </summary>
    string InsecureSample { get; }

    /// <summary>
    ///     Gets the title for the <see cref="SecureSample" />.
    /// </summary>
    string SecureAdvice { get; }

    /// <summary>
    ///     Gets the code for a secure sample.
    /// </summary>
    string SecureSample { get; }

    /// <summary>
    ///     Gets the risk of the advice.
    /// </summary>
    string WarningRisk { get; }

    /// <summary>
    ///     Gets or sets the source of the advice.
    /// </summary>
    string WarningSource { get; set; }

    /// <summary>
    ///     Gets the text of the advice.
    /// </summary>
    string WarningText { get; }

    /// <summary>
    ///     Gets the title of the advice.
    /// </summary>
    string WarningTitle { get; }
}