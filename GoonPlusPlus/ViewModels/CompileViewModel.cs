using DynamicData;
using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class CompileViewModel : ViewModelBase
{
    public static CompileViewModel Instance { get; private set; } = null!;
    public SourceList<string> Classpath { get; } = new();

    public CompileViewModel()
    {
        Instance = this;
    }

    private string _compileOutput = string.Empty;

    public string CompileOutput
    {
        get => _compileOutput;
        set => this.RaiseAndSetIfChanged(ref _compileOutput, value);
    }

}