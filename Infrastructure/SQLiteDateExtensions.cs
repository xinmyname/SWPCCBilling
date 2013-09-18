using System;
using System.Globalization;

namespace SWPCCBilling.Infrastructure
{
    public static class SQLiteDateExtensions
    {
        public static DateTime? ToSQLiteDateTime(this string date)
        {
            if (String.IsNullOrEmpty(date))
                return null;

            return DateTime.Parse(date, CultureInfo.InvariantCulture);
        }

        public static DateTime? ToSQLiteDate(this string date)
        {
			if (String.IsNullOrEmpty(date))
                return null;

            return DateTime.Parse(date, CultureInfo.InvariantCulture);
        }

        public static string ToSQLiteDateTime(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToSQLiteDate(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

    }
}