using System;

namespace Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsPassed(this DateTime dateTime)
            => dateTime.IsBefore(DateTime.UtcNow);

        public static bool IsAfter(this DateTime dateTime, DateTime other)
            => dateTime > other;

        public static bool IsBefore(this DateTime dateTime, DateTime other)
            => dateTime < other;
    }
}
