using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    /*public class JSonToFileStorageService : IStorageService
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

            try
            {
                using (var fileStream = new StreamReader(path))
                {
                    var json = fileStream.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<T>(json);

                    return data;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                //return data;
            }
        }

        private string BuildPath(string key)
        {
            key = key + "Saves/GameSave.save";
            return Path.Combine(Application.persistentDataPath, key);
        }
    }*/
}