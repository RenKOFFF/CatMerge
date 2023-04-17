using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    public class JSonToFileStorageService : IStorageService
    {
        public void Save(string key, object data)
        {
            string path = BuildPath(key);
            string json = JsonConvert.SerializeObject(data);

            using (var fileStream = new StreamWriter(path))
            {
                fileStream.Write(json);
            }
        }

        public T Load<T>(string key)
        {
            string path = BuildPath(key);

            using (var fileStream = new StreamReader(path))
            {
                var json = fileStream.ReadToEnd();
                var data = JsonConvert.DeserializeObject<T>(json);
                
                return data;
            }
        }

        private string BuildPath(string key)
        {
            return Path.Combine(Application.persistentDataPath, key);
        }
    }
}