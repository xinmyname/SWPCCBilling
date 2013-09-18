using System;
using System.Data;
using System.Reflection;
using System.Linq;

namespace SWPCCBilling.Infrastructure
{
    public class DatabaseFactory
    {
        private readonly string _dbPath;
        private readonly string _conStr;
        private static readonly Type _sqliteConnectionType;
		private static readonly Type _sqliteConnectionStringBuilderType;

        static DatabaseFactory()
        {
			if (Environment.OSVersion.Platform == PlatformID.Unix) 
			{
				_sqliteConnectionType = typeof(Mono.Data.Sqlite.SqliteConnection);
				_sqliteConnectionStringBuilderType = typeof(Mono.Data.Sqlite.SqliteConnectionStringBuilder);
				Mono.Data.Sqlite.SqliteConnection.SetConfig(Mono.Data.Sqlite.SQLiteConfig.Serialized);
			}
			else 
			{
				_sqliteConnectionType = typeof(Community.CsharpSqlite.SQLiteClient.SqliteConnection);
				_sqliteConnectionStringBuilderType = typeof(Community.CsharpSqlite.SQLiteClient.SqliteConnectionStringBuilder);
			}
        }

        public DatabaseFactory(SettingsStore settingsStore)
        {
            _dbPath = settingsStore.Load().DatabasePath;

            object conBuilder = Activator.CreateInstance(_sqliteConnectionStringBuilderType);
            _sqliteConnectionStringBuilderType.GetProperty("Uri").SetValue(conBuilder, new Uri(_dbPath).AbsoluteUri, null);
            _conStr = (string)_sqliteConnectionStringBuilderType.GetProperty("ConnectionString").GetValue(conBuilder, null);
        }

        public IDbConnection OpenDatabase()
        {
			var con = (IDbConnection)Activator.CreateInstance(_sqliteConnectionType);
            con.ConnectionString = _conStr;
            con.Open();

            return con;
        }
    }
}