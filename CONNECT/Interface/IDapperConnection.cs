﻿using System.Data;

namespace CONNECTION.Interface
{
    public interface IDapperConnection
    {
        //public IDbConnection DbConnection { get; }
        //public DynamicParameters DynamicParameters { get; }
        //public IDbTransaction ITransaction { get; }

        public IDapperConnection CreateConnection(string connectionString = "");

        public void OpenConnect();

        public void CloseConnect();

        public void CreateNewStoredProcedure(string nameStore);

        public void AddParameter(string field, object value);

        public int Execute();

        public IEnumerable<dynamic> Query();
    }
}