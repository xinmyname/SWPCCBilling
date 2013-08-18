namespace SWPCCBilling.Models
{
    public class Parent
    {
        public long Id { get; set; }
        public long FamilyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Parent()
        {
        }

        public Parent(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}