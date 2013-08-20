using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SWPCCBilling.Models;
using SWPCCBilling.ViewModels;

namespace SWPCCBilling.Infrastructure
{
    public class DiscountStore
    {
        private readonly DatabaseFactory _dbFactory;

        public DiscountStore(DatabaseFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IEnumerable<Discount> LoadAll()
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            var discounts = con.Query<Discount>("SELECT * FROM Discount ORDER BY Id").ToList();

            con.Close();

            return discounts;
        }

        public Discount Load(int id)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            var discount = con.Query<Discount>("SELECT * FROM Discount WHERE Id=?", new {id}).Single();

            con.Close();

            return discount;
        }

        public IEnumerable<DiscountViewModel> LoadAllLinks(FamilyStore familyStore, FeeStore feeStore)
        {
            throw new System.NotImplementedException();
        }

        public void Save(Discount discount)
        {
            throw new System.NotImplementedException();
        }
    }
}