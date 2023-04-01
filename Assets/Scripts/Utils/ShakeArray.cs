using System;

namespace Utils
{
    public static class ShakeArray<T>
    {
        public static T[] Shake(T[] array)
        {
            var random = new Random();

            for (int i = array.Length - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                (array[j], array[i]) = (array[i], array[j]);
            }

            return array;
        }
    }
}