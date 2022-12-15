using AugmentationFramework.AdviceDisplay;

namespace AugmentationFramework.Generators;

public class ClosableAdvicePopup : ClosablePopup
{
    public ClosableAdvicePopup(IAdviceModel adviceModel)
    {
        AdviceModel = adviceModel;
        Child = new AdviceView { DataContext = this };
    }

    public IAdviceModel AdviceModel { get; }
}

