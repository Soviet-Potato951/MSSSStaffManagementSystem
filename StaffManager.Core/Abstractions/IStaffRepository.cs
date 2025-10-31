using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffManager.Core.Abstractions
{
    public interface IStaffRepository
    {
        bool Exists(int id);
        bool TryGet(int id, out string name);
        IEnumerable<KeyValuePair<int, string>> All();
        IEnumerable<KeyValuePair<int, string>> AllOrdered();
        void Add(int id, string name);
        void UpdateName(int id, string name);
        bool Remove(int id);
        void Clear();
        void ReplaceAll(IEnumerable<KeyValuePair<int, string>> records);
    }
}
