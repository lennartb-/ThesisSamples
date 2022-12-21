namespace AugmentationFramework.AdviceDisplay;

public interface IAdviceModel
{
    string WarningTitle { get; }

    string WarningText { get; }

    string WarningSource { get; set; }

    string WarningRisk { get; }

    string Information { get; }

    Uri InformationLink { get; }

    string SecureAdvice { get; }

    string SecureSample { get; }

    string InsecureAdvice { get; }

    string InsecureSample { get; }
}