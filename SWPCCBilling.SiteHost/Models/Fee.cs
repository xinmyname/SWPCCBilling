namespace SWPCCBilling.SiteHost.Models
{
    public class Fee
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }

        public Fee()
        {
        }

        public Fee(int id, string name, string type, decimal amount)
        {
            Id = id;
            Name = name;
            Type = type;
            Amount = amount;
        }
    }
}