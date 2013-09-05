using System;
using System.Collections.Generic;
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

            var payments = new List<Payment>();

            IDbConnection con = _dbFactory.OpenDatabase();

            payments.AddRange(con.Query<Payment>("SELECT * FROM Payment WHERE Received BETWEEN ? AND ? AND FamilyId=? ",
                new{ monthStart, monthEnd, familyId }));

            con.Close();

            return payments;
        }
    }
}