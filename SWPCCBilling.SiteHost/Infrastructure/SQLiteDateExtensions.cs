using System;
using System.Globalization;

namespace SWPCCBilling.Infrastructure
{
    public static class SQLiteDateExtensions
    {
        public static DateTime? ToSQLiteDateTime(this string date)
        {
            if (date == null)
                return null;

            return DateTime.ParseExact(date, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
        }

        public static string ToSQLiteDateTime(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}