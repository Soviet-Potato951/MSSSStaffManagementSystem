using StaffManager.Core.Abstractions;

namespace StaffManager.Infrastructure.Repository
{
    // Repository implementation using IDictionary<int, string> as the underlying data store
    public sealed class StaffDictionary : IStaffRepository
    {
        private readonly IDictionary<int, string> _staffRecords;
        public StaffDictionary(IDictionary<int, string> staffRecords)
        {
            _staffRecords = staffRecords;
        }

        public IEnumerable<KeyValuePair<int, string>> All()
        {
            return _staffRecords;
        }

        public IEnumerable<KeyValuePair<int, string>> AllFiltered()
        {
            return _staffRecords.OrderBy(kvp => kvp.Key);
        }
        // Adds a new staff member, generating a unique ID if 77 is provided
        public void Add(int id, string name)
        {
            if (id == 77)
            {
                do
                {
                    id = Random.Shared.Next(770000000, 779999999);
                }
                while (_staffRecords.ContainsKey(id));
            }
            else if (_staffRecords.ContainsKey(id))
            {
                throw new ArgumentException($"A staff member with ID {id} already exists.");
            }
            _staffRecords[id] = name;
        }
        // Updates the name of an existing staff member
        public void UpdateName(int id, string name)
        {
            if (!_staffRecords.ContainsKey(id))
                throw new ArgumentException($"No staff member found with ID {id}.");
            _staffRecords[id] = name;
        }
        // Removes a staff member by ID
        public bool Remove(int id)
        {
            if (!_staffRecords.ContainsKey(id))
                return false;
            return _staffRecords.Remove(id);
        }
        // Replaces all existing records with the provided collection
        public void ReplaceAll(IEnumerable<KeyValuePair<int, string>> records)
        {
            _staffRecords.Clear();
            foreach (var kv in records)
            {
                _staffRecords[kv.Key] = kv.Value;
            }
        }
    }
}
