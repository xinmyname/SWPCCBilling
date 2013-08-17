using System.Collections.Generic;
using System.Data;
using Dapper;
using SWPCCBilling.Models;

namespace SWPCCBilling.Infrastructure
{
    public class FamilyStore
    {
        private readonly DatabaseFactory _dbFactory;

        public FamilyStore(DatabaseFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IEnumerable<Family> LoadAll()
        {
            IDbConnection con = _dbFactory.OpenDatabase();
            var families = con.Query<Family>("SELECT * FROM Family");
            



            con.Close();

            return families;
        }

        public Family Add(Family family)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            IDbCommand cmd = con.CreateCommand("INSERT INTO Family (StreetAddress,City,State,ZIP,Joined,DueDay) VALUES (?,?,?,?,?,?)")
                .AddParameters(new { family.StreetAddress, family.City, family.State, family.ZIP, family.Joined, family.DueDay });

            cmd.ExecuteNonQuery();

            cmd = con.CreateCommand("SELECT MAX(Id) FROM Family");

            family.Id = (long) cmd.ExecuteScalar();

            foreach (Parent parent in family.Parents)
                AddParent(con, family.Id, parent);

            foreach (Child child in family.Children)
                AddChild(con, family.Id, child);

            con.Close();

            return family;
        }

        private void AddParent(IDbConnection con, long familyId, Parent parent)
        {
            IDbCommand cmd = con.CreateCommand("INSERT INTO Parent (FirstName,LastName,Email) VALUES (?,?,?)")
                .AddParameters(new {parent.FirstName, parent.LastName, parent.Email});

            cmd.ExecuteNonQuery();

            cmd = con.CreateCommand("SELECT MAX(Id) FROM Parent");

            parent.Id = (long) cmd.ExecuteScalar();

            cmd = con.CreateCommand("INSERT INTO FamilyParent (FamilyId,ParentId) VALUES (?,?)")
                .AddParameters(new {familyId, parent.Id});

            cmd.ExecuteNonQuery();
        }

        private void AddChild(IDbConnection con, long familyId, Child child)
        {
            IDbCommand cmd = con.CreateCommand("INSERT INTO Child(FirstName,LastName,Room,Joined) VALUES (?,?,?,?)")
                .AddParameters(new {child.FirstName, child.LastName, child.Room, child.Joined});

            cmd.ExecuteNonQuery();

            cmd = con.CreateCommand("SELECT MAX(Id) FROM Child");

            child.Id = (long) cmd.ExecuteScalar();

            cmd = con.CreateCommand("INSERT INTO FamilyChild (FamilyId,ChildId) VALUES (?,?)")
                .AddParameters(new {familyId, child.Id});

            cmd.ExecuteNonQuery();

            cmd = con.CreateCommand(
                "INSERT INTO ChildDays (ChildId,Mon,Tue,Wed,Thu,Fri,EffectiveDate) VALUES (?,?,?,?,?,?,?)")
                .AddParameters(
                    new {child.Id, child.Mon, child.Tue, child.Wed, child.Thu, child.Fri, child.EffectiveDate});

            cmd.ExecuteNonQuery();
        }
    }
}