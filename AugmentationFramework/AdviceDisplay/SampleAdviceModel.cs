using System.Windows;

namespace AugmentationFramework.AdviceDisplay;

public class SampleAdviceModel : IAdviceModel
{
    public string WarningTitle => "Insecure API!";

    public string WarningText => "Usage of this API can lead to security issues.";

    public string WarningSource => "";

    public string WarningRisk => "Potential leak of user data.";

    public string Information => "www.google.de";

    public string SecureAdvice => "Use this:";

    public string SecureSample => "public static void Main (string[] args){}";

    public string InsecureAdvice => "Not this";

    public string InsecureSample => "public static void Main (string[] args){}";
}
