using AugmentationFramework.AdviceDisplay;

namespace AugmentationFramework.Generators;

public class ClosableAdvicePopup : ClosablePopup
{
    public IAdviceModel AdviceModel { get; }

    public ClosableAdvicePopup(IAdviceModel adviceModel)
    {
        AdviceModel = adviceModel;
        Child = new AdviceView() { DataContext = this };
    }
}
