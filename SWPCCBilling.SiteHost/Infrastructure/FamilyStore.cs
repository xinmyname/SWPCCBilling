using System;
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

        public Family Load(long familyId)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            var family = con.Query<Family>("SELECT * FROM Family WHERE Id=?", new {familyId}).Single();

            LoadParents(con, family);
            LoadChildren(con, family);

            con.Close();

            return family;
        }

        public Family Add(Family family)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            con.Execute("INSERT INTO Family (StreetAddress,City,State,ZIP,Joined,DueDay) VALUES (?,?,?,?,?,?)",
                new { family.StreetAddress, family.City, family.State, family.ZIP, family.Joined, family.DueDay });

            family.Id = con.ExecuteScalar<long>("SELECT MAX(Id) FROM Family");

            foreach (Parent parent in family.Parents)
            {
                parent.FamilyId = family.Id;
                AddParent(con, parent);
            }

            foreach (Child child in family.Children)
            {
                child.FamilyId = family.Id;
                AddChild(con, child);
            }

            con.Close();

            return family;
        }

        public Parent AddParent(Parent parent)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            AddParent(con, parent);

            con.Close();

            return parent;
        }

        public Child AddChild(Child child)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            AddChild(con, child);

            con.Close();

            return child;
        }

        private void AddParent(IDbConnection con, Parent parent)
        {
            con.Execute("INSERT INTO Parent (FamilyId,FirstName,LastName,Email) VALUES (?,?,?,?)",
                new {parent.FamilyId, parent.FirstName, parent.LastName, parent.Email});

            parent.Id = con.ExecuteScalar<long>("SELECT MAX(Id) FROM Parent");
        }

        private void AddChild(IDbConnection con, Child child)
        {
            con.Execute("INSERT INTO Child(FamilyId,FirstName,LastName,Room,Joined) VALUES (?,?,?,?,?)",
                new {child.FamilyId, child.FirstName, child.LastName, child.Room, child.Joined});

            child.Id = con.ExecuteScalar<long>("SELECT MAX(Id) FROM Child");

            con.Execute("INSERT INTO ChildDays (ChildId,Mon,Tue,Wed,Thu,Fri,Effective) VALUES (?,?,?,?,?,?,?)",
                    new {child.Id, child.Mon, child.Tue, child.Wed, child.Thu, child.Fri, child.Effective});
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

            const string query =
                "SELECT   Mon,Tue,Wed,Thu,Fri,MAX(Effective) AS Effective " +
                "FROM     ChildDays " +
                "WHERE    ChildId=? " +
                "GROUP BY Mon,Tue,Wed,Thu,Fri";

            foreach (Child child in children.ToList())
            {
                var week = con.Query<Week>(query, new {child.Id}).Single();

                child.Mon = week.Mon > 0;
                child.Tue = week.Tue > 0;
                child.Wed = week.Wed > 0;
                child.Thu = week.Thu > 0;
                child.Fri = week.Fri > 0;
                child.Effective = week.Effective;

                family.Children.Add(child);
            }
        }

        public void Save(Family family)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            const string query =
                "UPDATE Family " +
                "SET StreetAddress=?, " +
                "    City=?, " +
                "    State=?, " +
                "    ZIP=?, " +
                "    Joined=?, " +
                "    Departed=?, " +
                "    DueDay=?, " +
                "    Notes=? " +
                "WHERE Id=? ";

            con.Execute(query, new
            {
                family.StreetAddress, family.City, family.State, family.ZIP,
                family.Joined, family.Departed, family.DueDay, family.Notes,
                family.Id
            });
            
            con.Close();
        }

        public void SaveParent(Parent parent)
        {
            IDbConnection con = _dbFactory.OpenDatabase();

            const string query =
                "UPDATE Parent " +
                "SET FirstName=? " +
                "SET LastName=? " +
                "SET Email=? " +
                "WHERE Id=? ";

            con.Execute(query, new {parent.FirstName, parent.LastName, parent.Email, parent.Id});

            con.Close();
        }

        private void SaveChild(Child child)
        {
            throw new NotImplementedException();
        }
    }
}