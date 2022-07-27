using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DynamicData;
using GoonPlusPlus.Models;
using ReactiveUI;

namespace GoonPlusPlus.ViewModels;

public class RunViewModel : ViewModelBase
{
    public static RunViewModel Instance { get; private set; } = null!;
    public SourceList<string> StdOut { get; } = new();
    public SourceList<string> StdErr { get; } = new();
    public Process? RunProcess { get; set; }
    private readonly Queue<string> _stdIn = new();
    public StringBuilder StdInBuilder { get; } = new();

    public void Enqueue()
    {
        if (RunProcess == null) _stdIn.Enqueue(StdInBuilder.ToString());
        else RunProcess.StandardInput.WriteLine(StdInBuilder.ToString());
        StdInBuilder.Clear();
    }

    public RunViewModel()
    {
        Instance = this;
        this.WhenAnyValue(x => x.RunProcess)
            .WhereNotNull()
            .Subscribe(p =>
            {
                _stdIn.ToList().ForEach(l => p.StandardInput.WriteLineAsync(l));
                _stdIn.Clear();
            });
    }
}