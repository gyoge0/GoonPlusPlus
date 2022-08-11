/*
 * https://stackoverflow.com/a/4501659/18944758
 */

using System;
using System.IO;
using System.Text;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
#pragma warning disable CS8618

namespace GoonPlusPlus.Util;

public class ConsoleInputReadEventArgs : EventArgs
{
    public ConsoleInputReadEventArgs(string input) => Input = input;

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string Input { get; }
}

public interface IConsoleAutomator
{
    StreamWriter StandardInput { get; }

    event EventHandler<ConsoleInputReadEventArgs> StandardInputRead;
}

public abstract class ConsoleAutomatorBase : IConsoleAutomator
{
    protected readonly byte[] Buffer = new byte[256];
    protected readonly StringBuilder InputAccumulator = new();

    protected volatile bool StopAutomation;

    protected StreamReader StandardOutput { get; set; }

    protected StreamReader StandardError { get; set; }

    public StreamWriter StandardInput { get; protected set; }

    public event EventHandler<ConsoleInputReadEventArgs> StandardInputRead;

    protected void BeginReadAsync()
    {
        if (!StopAutomation)
        {
            var res = StandardOutput.BaseStream.BeginRead(Buffer, 0, Buffer.Length, ReadHappened, null);
        }
    }

    protected virtual void OnAutomationStopped()
    {
        StopAutomation = true;
        StandardOutput.DiscardBufferedData();
    }

    private void ReadHappened(IAsyncResult asyncResult)
    {
        var bytesRead = StandardOutput.BaseStream.EndRead(asyncResult);
        if (bytesRead == 0)
        {
            OnAutomationStopped();
            return;
        }

        var input = StandardOutput.CurrentEncoding.GetString(Buffer, 0, bytesRead);
        InputAccumulator.Append(input);

        if (bytesRead < Buffer.Length) OnInputRead(InputAccumulator.ToString());

        BeginReadAsync();
    }

    private void OnInputRead(string input)
    {
        var handler = StandardInputRead;
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (handler == null) return;

        handler(this, new ConsoleInputReadEventArgs(input));
        InputAccumulator.Clear();
    }
}

public class ConsoleAutomator : ConsoleAutomatorBase
{
    public ConsoleAutomator(StreamWriter standardInput, StreamReader standardOutput)
    {
        StandardInput = standardInput;
        StandardOutput = standardOutput;
    }

    public void StartAutomating()
    {
        StopAutomation = false;
        BeginReadAsync();
    }

    public void StopAutomating() => OnAutomationStopped();
}
