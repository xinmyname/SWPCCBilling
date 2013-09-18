using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
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

            con.Execute("INSERT INTO Payment (FamilyId,CheckNum,Amount,Received) VALUES (?,?,?,?)",
                new {payment.FamilyId, payment.CheckNum, payment.Amount, payment.Received});

            payment.Id = con.ExecuteScalar<long>("SELECT MAX(Id) FROM Payment");

            con.Close();

            return payment;
        }

        public IList<Payment> Load(DateTime month, long familyId)
        {
            string monthStart = month.ToSQLiteDate();
            string monthEnd = month.AddMonths(1).AddDays(-1).ToSQLiteDate();

            IDbConnection con = _dbFactory.OpenDatabase();

            var payments = con.Query<Payment>(
                "SELECT * FROM Payment WHERE Received BETWEEN ? AND ? AND FamilyId=? ",
                new{ monthStart, monthEnd, familyId }).ToList();

            con.Close();

            return payments;
        }

        public IList<Payment> Load(DateTime month)
        {
            string monthStart = month.ToSQLiteDate();
            string monthEnd = month.AddMonths(1).AddDays(-1).ToSQLiteDate();

            IDbConnection con = _dbFactory.OpenDatabase();

            var payments = con.Query<Payment>(
                "SELECT * FROM Payment WHERE Received BETWEEN ? AND ? ORDER BY Received",
                new { monthStart, monthEnd }).ToList();

            con.Close();

            return payments;
        }

        public IList<Payment> LoadUndeposited()
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            var payments = con.Query<Payment>("SELECT * FROM Payment WHERE Deposited IS NULL")
                .ToList();

            con.Close();

            return payments;
        }

        public void MarkAsDeposited(IEnumerable<long> paymentIds)
        {
            var ids = new StringBuilder();

            foreach (long id in paymentIds)
            {
                if (ids.Length > 0)
                    ids.Append(",");
                ids.Append(id);
            }

            string now = DateTime.Now.ToSQLiteDateTime();

            var query = String.Format("UPDATE Payment SET Deposited=? WHERE Id IN ({0})", ids);

            IDbConnection con = _dbFactory.OpenDatabase();

            con.Execute(query, new {now});

            con.Close();
        }
    }
}