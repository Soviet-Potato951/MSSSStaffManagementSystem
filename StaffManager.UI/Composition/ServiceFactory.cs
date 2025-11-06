using StaffManager.Core.Abstractions;
using StaffManager.Core.Domain;
using StaffManager.Infrastructure.Csv;
using StaffManager.Infrastructure.Repository;

namespace StaffManager.UI.Composition
{
    public static class ServiceFactory
    {
        // used at application startup to create initial services
        public static ( ICsvSerialiser csv, IStaffRepository repo) Create(StoreMode mode)
        {
            ICsvSerialiser csv = new CsvSerialiser();

            IDictionary<int, string> map = mode == StoreMode.Tree
                ? new SortedDictionary<int, string>()
                : new Dictionary<int, string>();

            IStaffRepository repo = new StaffDictionary(map);
            return (csv, repo);
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
