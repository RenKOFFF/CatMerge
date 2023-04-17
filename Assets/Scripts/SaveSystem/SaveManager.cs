using System;
using UnityEngine;

namespace SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public static IStorageService SaveInstance { get; } = new JSonToFileStorageService();
    }
}