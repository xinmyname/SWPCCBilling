using System;
using SWPCCBilling.Infrastructure;

namespace SWPCCBilling.Models
{
    public class LedgerLine
    {
        public long Id { get; set; }
        public long FamilyId { get; set; }
        public string Date { get; set; }
        public long? FeeId { get; set; }
        public long? PaymentId { get; set; }
        public double Amount { get; set; }
        public string Notes { get; set; }

        public static LedgerLine CreateCharge(long familyId, long feeId, DateTime date, decimal amount, string notes)
        {
            return new LedgerLine
            {
                FamilyId = familyId,
                Date = date.ToSQLiteDateTime(),
                FeeId = feeId,
                Amount = (double)amount,
                Notes = notes
            };
        }

        public static LedgerLine CreatePayment(long familyId, long paymentId, DateTime date, decimal amount, string notes)
        {
            return new LedgerLine
            {
                FamilyId = familyId,
                Date = date.ToSQLiteDateTime(),
                PaymentId = paymentId,
                Amount = (double)amount,
                Notes = notes
            };
        }
    }
}
