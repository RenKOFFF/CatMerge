using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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

        /// <summary>
        /// Save... damn, why a u asked this 
        /// </summary>
        /// <param name="data">save data: (new ...SaveData(dataParam from this SaveData)</param>
        /// <param name="key">needed if u need save more 1 data</param>
        /// <typeparam name="T"></typeparam>
        public void Save<T>(T data, string key = "")
        {
            var fileName = GetFileName<T>(key);

            using (FileStream file = File.Create(_directory + fileName))
            {
                _formatter.Serialize(file, data);
            }
        }

        public T LoadOrDefault<T>(T defaultValue = default, string key = "")
        {
            var fileName = GetFileName<T>(key);

            if (!File.Exists(_directory + fileName))
            {
                Save(defaultValue, key);
                return defaultValue;
            }

            using (FileStream file = File.Open(_directory + fileName, FileMode.Open))
            {
                var loadedData = _formatter.Deserialize(file);
                T saveData = (T)loadedData;

                return saveData;
            }
        }

        private string GetFileName<T>(string key = "")
        {
            var fileName = $"{typeof(T)}{key}.save";
            return fileName;
        }
    }
}