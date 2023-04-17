using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveSystem
{
    public class BinaryFormatterStorage : IStorageService
    {
        private string _directory;
        private BinaryFormatter _formatter = new();

        public BinaryFormatterStorage(string directory)
        {
            _directory = directory;
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }
        }

        public void Save<T>(T data)
        {
            var fileName = $"{typeof(T)}.save";

            using (FileStream file = File.Create(_directory + fileName))
            {
                _formatter.Serialize(file, data);
            }
        }

        public T Load<T>(T defaultValue = default)
        {
            var fileName = $"{typeof(T)}.save";

            if (!File.Exists(_directory + fileName))
            {
                Save(defaultValue);
                return defaultValue;
            }
            
            using (FileStream file = File.Open(_directory + fileName, FileMode.Open))
            {
                var loadedData = _formatter.Deserialize(file);
                T saveData = (T)loadedData;

                return saveData;
            }
        }
    }
}