using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            var fees = con.Query<Fee>("SELECT * FROM Fee").ToList();
            con.Close();
            return fees;
        }

        public Fee Add(Fee fee)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            IDbCommand cmd = con.CreateCommand("INSERT INTO Fee (Name,Type,Amount,Category) VALUES (?,?,?,?)")
                .AddParameters(new {fee.Name, fee.Type, fee.Amount, fee.Category});

            cmd.ExecuteNonQuery();

            cmd = con.CreateCommand("SELECT MAX(Id) FROM Fee");
            fee.Id = (long)cmd.ExecuteScalar();

            con.Close();

            return fee;
        }

        public void Save(Fee fee)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            IDbCommand cmd = con.CreateCommand("UPDATE Fee SET Name=?, Type=?, Amount=?, Category=? WHERE Id=?")
                .AddParameters(new { fee.Name, fee.Type, fee.Amount, fee.Category, fee.Id });

            fee.Id = cmd.ExecuteNonQuery();

            con.Close();
        }

        public Fee Load(long feeId)
        {
            IDbConnection con = _dbFactory.OpenDatabase();
            var fee = con.Query<Fee>("SELECT * FROM Fee WHERE Id=?", new{ feeId }).Single();
            con.Close();
            return fee;
        }

        public Fee LoadDonation()
        {
            IDbConnection con = _dbFactory.OpenDatabase();
            var fee = con.Query<Fee>("SELECT * FROM Fee WHERE Category='Donations'").SingleOrDefault();
            con.Close();
            return fee;
        }
    }
}