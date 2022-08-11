using GoonPlusPlus.Customization;
using ReactiveUI;

namespace GoonPlusPlus;

public class ViewModelBase : ReactiveObject
{
    public CustomCfg Custom { get; } = CustomCfg.Instance;
}
