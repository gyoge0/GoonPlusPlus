using Avalonia;

namespace GoonPlusPlus.Models;

public record Selection(string? Content, int? Start, int? Length)
{
    public async void Copy()
    {
        if (Content == null) return;
        await Application.Current!.Clipboard!.SetTextAsync(Content);
    }
}