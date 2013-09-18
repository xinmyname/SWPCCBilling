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

        public IEnumerable<DiscountViewModel> LoadAllLinks(IList<Family> families, IList<Fee> fees)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            var links = con
                .Query<Discount>("SELECT * FROM Discount ORDER BY Id")
                .Select(discount => new DiscountViewModel
                {
                    Discount = discount, 
                    FamilyName = families.Single(f => f.Id == discount.FamilyId).FamilyName, 
                    FeeName = fees.Single(f => f.Id == discount.FeeId).Name
                }).ToList();

            con.Close();

            return links;
        }

        public void Save(Discount discount)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            if (discount.Id < 0)
            {
                con.Execute("INSERT INTO Discount (FamilyId,FeeId,Percent) VALUES (?,?,?)",
                    new {discount.FamilyId, discount.FeeId, discount.Percent});
            }
            else
            {
                con.Execute("UPDATE Discount SET FamilyId=?, FeeId=?, Percent=? WHERE Id=?",
                    new { discount.FamilyId, discount.FeeId, discount.Percent, discount.Id });
            }

            con.Close();
        }
    }
}