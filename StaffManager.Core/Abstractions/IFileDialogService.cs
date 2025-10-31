using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffManager.Core.Abstractions
{
    public interface IFileDialogService
    {
        string? ShowOpenCsvDialog(string? initialPath = null);
        string? ShowSaveCsvDialog(string? suggestedFileName = null);
    }
}
