using AugmentationFramework.AdviceDisplay;

namespace AugmentationFramework.Generators;

/// <summary>
///     A <see cref="ClosablePopup" /> that contains information from a <see cref="IAdviceModel" />.
/// </summary>
public class ClosableAdvicePopup : ClosablePopup
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ClosableAdvicePopup" /> class.
    /// </summary>
    /// <param name="adviceModel">The <see cref="IAdviceModel" /> to display in this popup.</param>
    public ClosableAdvicePopup(IAdviceModel adviceModel)
    {
        AdviceModel = adviceModel;
        Child = new AdviceView { DataContext = this };
    }

    /// <summary>
    ///     Gets the <see cref="IAdviceModel" /> that is displayed in this popup.
    /// </summary>
    public IAdviceModel AdviceModel { get; }
}