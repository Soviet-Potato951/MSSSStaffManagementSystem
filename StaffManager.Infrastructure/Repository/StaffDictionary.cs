using StaffManager.Core.Abstractions;

namespace StaffManager.Infrastructure.Repository
{
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
        
        public void UpdateName(int id, string name)
        {
            if (!_staffRecords.ContainsKey(id))
                throw new ArgumentException($"No staff member found with ID {id}.");
            _staffRecords[id] = name;
        }

        public bool Remove(int id)
        {
            if (!_staffRecords.ContainsKey(id))
                return false;
            return _staffRecords.Remove(id);
        }

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
