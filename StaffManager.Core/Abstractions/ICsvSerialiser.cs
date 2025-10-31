using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffManager.Core.Abstractions
{
    public interface ICsvSerialiser
    {
        IEnumerable<KeyValuePair<int, string>> Load(string filePath);
        void Save(string filepath, IEnumerable<KeyValuePair<int, string>> records);
    }
}
