using StaffManager.Core.Abstractions;

namespace StaffManager.Infrastructure.Csv
{
    public class CsvSerialiser : ICsvSerialiser
    {
        public IEnumerable<KeyValuePair<int, string>> Load(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var result = new List<KeyValuePair<int, string>>();
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length >= 2 &&
                    int.TryParse(parts[0], out int id))
                {
                    var name = parts[1];
                    result.Add(new KeyValuePair<int, string>(id, name));
                }
            }
            return result;
        }

        public void Save(string filepath, IEnumerable<KeyValuePair<int, string>> records)
        {
            var lines = records.Select(kvp => $"{kvp.Key},{kvp.Value}");
            File.WriteAllLines(filepath, lines);
        }
    }
}
