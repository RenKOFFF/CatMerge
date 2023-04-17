using System;
using UnityEngine;

namespace SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public static IStorageService Instance { get; private set; }

        private void Awake()
        {
            Instance = new BinaryFormatterStorage(Application.persistentDataPath + "/saves/");
        }
    }
}