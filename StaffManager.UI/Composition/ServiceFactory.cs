// StaffManager.UI/Composition/ServiceFactory.cs
using System.Collections.Generic;
using StaffManager.Core.Abstractions;
using StaffManager.Core.Domain;           // StoreMode
using StaffManager.Infrastructure.Csv;
using StaffManager.Infrastructure.Repository;
using StaffManager.UI.Services;

namespace StaffManager.UI.Composition
{
    public static class ServiceFactory
    {
        public static (IFileDialogService fileDialog, ICsvSerialiser csv, IStaffRepository repo) Create(StoreMode mode)
        {
            IFileDialogService fileDialog = new FileDialogService();
            ICsvSerialiser csv = new CsvSerialiser();

            IDictionary<int, string> map = mode == StoreMode.Tree
                ? new SortedDictionary<int, string>()
                : new Dictionary<int, string>();

            IStaffRepository repo = new StaffDictionary(map);
            return (fileDialog, csv, repo);
        }

        // used when the user switches modes mid-session
        public static IStaffRepository CreateRepository(StoreMode mode, IEnumerable<KeyValuePair<int, string>> seed)
        {
            IDictionary<int,string> map = mode == StoreMode.Tree
                ? new SortedDictionary<int, string>()
                : new Dictionary<int, string>();

            var repo = new StaffDictionary(map);
            repo.ReplaceAll(seed);
            return repo;
        }
    }
}
