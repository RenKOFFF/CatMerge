using System;

namespace SaveSystem
{
    public interface IStorageService
    {
        void Save<T>(T data, string key = "");
        T LoadOrDefault<T>(T defaultValue = default, string key = "");
    }
}