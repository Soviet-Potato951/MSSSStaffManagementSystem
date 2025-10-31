using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool Exists(int id)
        {
            return _staffRecords.ContainsKey(id);
        }

        public bool TryGet(int id, out string name)
        {
            return _staffRecords.TryGetValue(id, out name);
        }

        public IEnumerable<KeyValuePair<int, string>> All()
        {
            return _staffRecords;
        }

        public IEnumerable<KeyValuePair<int, string>> AllOrdered()
        {
            return _staffRecords.OrderBy(kvp => kvp.Key);
        }

        public void Add(int id, string name)
        {
            if (_staffRecords.ContainsKey(id))
                throw new ArgumentException($"A staff member with ID {id} already exists.");
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

        public void Clear()
        {
            _staffRecords.Clear();
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
