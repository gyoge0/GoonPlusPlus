using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class CompileViewModel : ViewModelBase
{
    private string _compileOutput = string.Empty;

    public CompileViewModel() => Instance = this;

    public static CompileViewModel Instance { get; private set; } = null!;

    public string CompileOutput
    {
        get => _compileOutput;
        set => this.RaiseAndSetIfChanged(ref _compileOutput, value);
    }
}
