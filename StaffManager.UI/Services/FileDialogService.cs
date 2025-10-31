using Microsoft.Win32;
using StaffManager.Core.Abstractions;

namespace StaffManager.UI.Services
{
    public sealed class FileDialogService : IFileDialogService
    {
        public string? ShowOpenCsvDialog(string? initialPath)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Open staff CSV",
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                InitialDirectory = initialPath
            };
            return dlg.ShowDialog() == true ? dlg.FileName : null;
        }

        public string? ShowSaveCsvDialog(string? suggestedFileName)
        {
            var dlg = new SaveFileDialog
            {
                Title = "Save staff CSV",
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                FileName = suggestedFileName ?? "staff_master.csv"
            };
            return dlg.ShowDialog() == true ? dlg.FileName : null;
        }
    }
}
