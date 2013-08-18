using System.Collections.Generic;
using System.Data;
using System.Linq;
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

            var families = con.Query<Family>("SELECT * FROM Family").ToList();

            foreach (Family family in families)
            {
                LoadParents(con, family);
                LoadChildren(con, family);
            }
            
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

            family.Id = (long)cmd.ExecuteScalar();

            foreach (Parent parent in family.Parents)
                AddParent(con, family.Id, parent);

            foreach (Child child in family.Children)
                AddChild(con, family.Id, child);

            con.Close();

            return family;
        }

        private void AddParent(IDbConnection con, long familyId, Parent parent)
        {
            IDbCommand cmd = con.CreateCommand("INSERT INTO Parent (FamilyId,FirstName,LastName,Email) VALUES (?,?,?,?)")
                .AddParameters(new {familyId, parent.FirstName, parent.LastName, parent.Email});

            cmd.ExecuteNonQuery();

            cmd = con.CreateCommand("SELECT MAX(Id) FROM Parent");

            parent.Id = (long) cmd.ExecuteScalar();
        }

        private void AddChild(IDbConnection con, long familyId, Child child)
        {
            IDbCommand cmd = con.CreateCommand("INSERT INTO Child(FamilyId,FirstName,LastName,Room,Joined) VALUES (?,?,?,?,?)")
                .AddParameters(new {familyId, child.FirstName, child.LastName, child.Room, child.Joined});

            cmd.ExecuteNonQuery();

            cmd = con.CreateCommand("SELECT MAX(Id) FROM Child");

            child.Id = (long) cmd.ExecuteScalar();

            cmd = con.CreateCommand(
                "INSERT INTO ChildDays (ChildId,Mon,Tue,Wed,Thu,Fri,Effective) VALUES (?,?,?,?,?,?,?)")
                .AddParameters(
                    new {child.Id, child.Mon, child.Tue, child.Wed, child.Thu, child.Fri, child.Effective});

            cmd.ExecuteNonQuery();
        }

        private void LoadParents(IDbConnection con, Family family)
        {
            var parents = con.Query<Parent>("SELECT * FROM Parent WHERE FamilyId=? ORDER BY Id",
                new {family.Id});

            foreach (Parent parent in parents.ToList())
                family.Parents.Add(parent);
        }

        private void LoadChildren(IDbConnection con, Family family)
        {
            var children = con.Query<Child>("SELECT * FROM Child WHERE FamilyId=? ORDER BY Id",
                new { family.Id });

            const string weekQuery =
                "SELECT   Mon,Tue,Wed,Thu,Fri,MAX(Effective) AS Effective " +
                "FROM     ChildDays " +
                "WHERE    ChildId=? " +
                "GROUP BY Mon,Tue,Wed,Thu,Fri";

            foreach (Child child in children.ToList())
            {
                var week = con.Query<Week>(weekQuery, new {child.Id}).Single();

                child.Mon = week.Mon > 0;
                child.Tue = week.Tue > 0;
                child.Wed = week.Wed > 0;
                child.Thu = week.Thu > 0;
                child.Fri = week.Fri > 0;
                child.Effective = week.Effective;

                family.Children.Add(child);
            }
        }
    }
}