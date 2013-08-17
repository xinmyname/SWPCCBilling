using System.Collections.Generic;
using System.Data;
using Dapper;
using SWPCCBilling.Models;

namespace SWPCCBilling.Infrastructure
{
    public class FeeStore
    {
        private readonly DatabaseFactory _dbFactory;

        public FeeStore(DatabaseFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IEnumerable<Fee> LoadAll()
        {
            IDbConnection con = _dbFactory.OpenDatabase();
            var fees = con.Query<Fee>("SELECT * FROM Fee");
            con.Close();
            return fees;
        }

        public void Add(Fee fee)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            IDbCommand cmd = con.CreateCommand("INSERT INTO Fee (Name,Type,Amount) VALUES (?,?,?)")
                .AddParameters(new {fee.Name, fee.Type, fee.Amount});

            fee.Id = cmd.ExecuteNonQuery();

            con.Close();
        }
    }
}