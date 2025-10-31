using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
