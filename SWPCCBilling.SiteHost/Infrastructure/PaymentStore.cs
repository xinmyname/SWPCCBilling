using System.Data;
using SWPCCBilling.Models;

namespace SWPCCBilling.Infrastructure
{
    public class PaymentStore
    {
        private readonly DatabaseFactory _dbFactory;

        public PaymentStore(DatabaseFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public Payment Add(Payment payment)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            IDbCommand cmd = con.CreateCommand("INSERT INTO Payment (FamilyId,CheckNum,Amount,Received) VALUES (?,?,?,?)",
                new {payment.FamilyId, payment.CheckNum, payment.Amount, payment.Received});

            cmd.ExecuteNonQuery();

            cmd = con.CreateCommand("SELECT MAX(Id) FROM Payment");
            payment.Id = (long)cmd.ExecuteScalar();

            con.Close();

            return payment;
        }
    }
}