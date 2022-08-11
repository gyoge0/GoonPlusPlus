using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class ViewModelBase : ReactiveObject
{
    public CustomCfg Custom { get; } = CustomCfg.Instance;
}
