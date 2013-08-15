using System;
using System.Data;
using Community.CsharpSqlite.SQLiteClient;
using SWPCCBilling.Properties;

namespace SWPCCBilling.Infrastructure
{
    public class DatabaseFactory
    {
        public IDbConnection OpenDatabase()
        {

            var conBuilder = new SqliteConnectionStringBuilder
            {
                Uri = new Uri(Settings.Default.DatabasePath).AbsoluteUri
            };

            string conStr = conBuilder.ConnectionString;

            IDbConnection con = new SqliteConnection(conStr);
            con.Open();
            return con;
        }
    }
}