using System;

namespace SaveSystem
{
    public interface IStorageService
    {
        void Save<T>(T data);
        T Load<T>(T defaultValue = default);
    }
}