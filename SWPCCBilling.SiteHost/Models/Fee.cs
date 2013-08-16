namespace SWPCCBilling.Models
{
    public class Fee
    {
        public long Id { get; set; } 
        public string Name { get; set; }
        public string Type { get; set; }
        public double? Amount { get; set; }

        public Fee()
        {
        }

        public Fee(int id, string name, string type, double amount)
        {
            Id = id;
            Name = name;
            Type = type;
            Amount = amount;
        }
    }
}