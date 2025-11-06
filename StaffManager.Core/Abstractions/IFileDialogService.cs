namespace StaffManager.Core.Abstractions
{
    public interface IFileDialogService
    {
        string? ShowOpenCsvDialog(string? initialPath = null);
        string? ShowSaveCsvDialog(string? suggestedFileName = null);
    }
}
