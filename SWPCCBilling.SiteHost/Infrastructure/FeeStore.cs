﻿using System.Collections.Generic;
using System.Data;
using Dapper;
using SWPCCBilling.Models;

namespace SWPCCBilling.Infrastructure
{
    public class FeeStore
    {
        private readonly DatabaseFactory _dbFactory;

        public FeeStore(DatabaseFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IEnumerable<Fee> LoadAll()
        {
            IDbConnection con = _dbFactory.OpenDatabase();
            var fees = con.Query<Fee>("SELECT * FROM Fee");
            con.Close();
            return fees;
        }

        public void Add(Fee fee)
        {
            IDbConnection con = _dbFactory.OpenDatabase();
            IDbCommand cmd = con.CreateCommand();
            cmd.CommandText = "INSERT INTO Fee (Name,Type,Amount) VALUES (?,?,?)";

            var p1 = cmd.CreateParameter();
            p1.Value = fee.Name;
            var p2 = cmd.CreateParameter();
            p2.Value = fee.Type;
            var p3 = cmd.CreateParameter();
            p3.Value = fee.Amount;

            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);

            fee.Id = cmd.ExecuteNonQuery();

            con.Close();
        }
    }
}