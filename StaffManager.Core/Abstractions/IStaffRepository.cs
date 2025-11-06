namespace StaffManager.Core.Abstractions
{
    public interface IStaffRepository
    {
        IEnumerable<KeyValuePair<int, string>> All();
        IEnumerable<KeyValuePair<int, string>> AllFiltered();
        void Add(int id, string name);
        void UpdateName(int id, string name);
        bool Remove(int id);
        void ReplaceAll(IEnumerable<KeyValuePair<int, string>> records);
    }
}
