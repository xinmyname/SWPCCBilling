namespace SWPCCBilling.Models
{
    public class InvoiceLine
    {
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public long Quantity { get; set; }
        public decimal Amount { get; set; }

        public string UnitPriceText
        {
            get { return UnitPrice.ToString("C"); }
        }

        public string AmountText
        {
            get { return Amount.ToString("C"); }
        }

        public InvoiceLine(string description, decimal unitPrice, long quantity, decimal amount)
        {
            Description = description;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Amount = amount;
        }

        public InvoiceLine(string description, decimal unitPrice, long quantity)
        {
            Description = description;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Amount = unitPrice * quantity;
        }

        public InvoiceLine(string description, decimal amount)
        {
            Description = description;
            Amount = amount;
            Quantity = 1;
            UnitPrice = Amount;
        }
    }
}