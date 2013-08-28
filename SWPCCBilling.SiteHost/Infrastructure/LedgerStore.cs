using System.Collections.Generic;
using System.Data;
using System.Linq;
using SWPCCBilling.Models;

namespace SWPCCBilling.Infrastructure
{
    public class LedgerStore
    {
        private readonly DatabaseFactory _dbFactory;

        public LedgerStore(DatabaseFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IEnumerable<LedgerLine> LoadAll()
        {
            IDbConnection con = _dbFactory.OpenDatabase();
            var lines = con.Query<LedgerLine>("SELECT * FROM Ledger").ToList();
            con.Close();
            return lines;
        }

        public IEnumerable<LedgerLine> LoadForFamily(long familyId)
        {
            IDbConnection con = _dbFactory.OpenDatabase();
            var lines = con.Query<LedgerLine>("SELECT * FROM Ledger WHERE FamilyId=?", 
                new { familyId }).ToList();
            con.Close();
            return lines;
        }


        public LedgerLine Add(LedgerLine line)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            con.Execute("INSERT INTO Ledger (FamilyId,Date,FeeId,PaymentId,UnitPrice,Quantity,Amount,Notes) VALUES (?,?,?,?,?,?,?,?)",
                new { line.FamilyId, line.Date, line.FeeId, line.PaymentId, line.UnitPrice, line.Quantity, line.Amount, line.Notes });

            con.Close();

            return line;
        }
    }
}
