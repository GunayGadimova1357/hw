using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ConsoleApp7.Utils
{
    public class JsonRepository<T> : IRepository<T>
    {
        private readonly string _filePath;
        private List<T> _data;

        public JsonRepository(string filePath)
        {
            _filePath = filePath;
            _data = Load();
        }

        public void Add(T item)
        {
            _data.Add(item);
            Save();
        }

        public void Remove(Guid id)
        {
            var item = _data.FirstOrDefault(i => (i as dynamic).Id == id);
            if (item != null)
            {
                _data.Remove(item);
                Save();
            }
        }

        public T Get(Guid id)
        {
            return _data.FirstOrDefault(i => (i as dynamic).Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _data;
        }

        private void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_data, options));
        }

        private List<T> Load()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            return new List<T>();
        }
    }
}