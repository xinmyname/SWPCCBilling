using System.Data;

namespace SWPCCBilling.Infrastructure
{
    public static class DbConnectionExtensions
    {
        public static IDbCommand CreateStoredProcedure(this IDbConnection connection, string name)
        {
            IDbCommand cmd = connection.CreateCommand();

            cmd.CommandText = name;
            cmd.CommandType = CommandType.StoredProcedure;

            return cmd;
        }

        public static IDbCommand CreateStoredProcedure(this IDbConnection connection, string name, object paramObj)
        {
            IDbCommand cmd = connection.CreateCommand();

            cmd.CommandText = name;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.AddParameters(paramObj);

            return cmd;
        }

        public static IDbCommand CreateCommand(this IDbConnection connection, string text)
        {
            IDbCommand cmd = connection.CreateCommand();

            cmd.CommandText = text;
            cmd.CommandType = CommandType.Text;

            return cmd;
        }

        public static IDbCommand CreateCommand(this IDbConnection connection, string text, object paramObj)
        {
            IDbCommand cmd = connection.CreateCommand();

            cmd.CommandText = text;
            cmd.CommandType = CommandType.Text;

            cmd.AddParameters(paramObj);

            return cmd;
        }
    }

}