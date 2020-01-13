using Dapper;
using Dapper.Contrib.Extensions;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Models;

namespace ZeiterfassungPierburg.Data
{
    public class SQLServer
    {
        private readonly string connString;
        private SQLServer(string connectionString)
        {
            connString = connectionString;
            dataMappers = new Dictionary<Type, IDataMapper>();
        }
        private Dictionary<Type, IDataMapper> dataMappers;
        private IDataMapper GetMapper<T>() where T: BasicModelObject, new()
        {
            if (!dataMappers.ContainsKey(typeof(T)))
            {
                // create a generic data mapper, if none has been registered yet
                dataMappers.Add(typeof(T), new DataMapper<T>(typeof(T).Name));
            }
            return dataMappers[typeof(T)];
        }

        // dictionary methods for dropdown lists
        public Dictionary<int,string> GetDictionary(string tableName, string labelString)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            using (SqlConnection conn = NewOpenConnection)
            {
                conn.Open();

                string sql = $"SELECT Id, {labelString} As Label FROM {tableName}";
                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    result.Add(r.GetInt32(0), r.GetString(1));
                }
            }
            return result;
        }

        // get model objects
        // params:
        // filter   dictionary <string,string> that contains the column names and respective values
        //
        // to correctly use the filter, the method assumes a GetItemsSQLString from the DataMapper that
        // does NOT contain a where clause
        public IEnumerable<T> GetItems<T>(string whereClause = null)
        {
            using (var c = NewOpenConnection)
            {
                string sql = $"SELECT * FROM {GetTableNameForModel<T>()}";
                if (String.IsNullOrEmpty(whereClause))
                {
                    sql += $" WHERE {whereClause}";
                }

                return c.Query<T>(sql).ToList();
            }
        }
        public T GetItem<T>(int id) where T: BasicModelObject, new()
        {
            using (var c = NewOpenConnection)
            {
                var result = c.Get<T>(id);
                return result;
            }
        }

        // helper methods
        private string GetTableNameForModel<T>()
        {
            return typeof(T).Name;
        }

        // data manipulation methods
        public int InsertItem<T>(T model) where T: BasicModelObject, new()
        {
            IDataMapper m = GetMapper<T>();
            using (SqlConnection conn = NewOpenConnection)
            {
                conn.Open();
                return ExecuteUpdateStatement(conn, m.GetInsertSqlString(model));
            }
        }
        public void EditItem<T>(T model) where T: BasicModelObject, new()
        {
            IDataMapper m = GetMapper<T>();
            using (SqlConnection conn = NewOpenConnection)
            {
                conn.Open();
                ExecuteUpdateStatement(conn, m.GetUpdateSqlString(model));
            }
        }
        public void RemoveItem<T>(T model) where T : BasicModelObject, new()
        {
            RemoveItem<T>(model.ID);
        }
        public void RemoveItem<T>(int id) where T : BasicModelObject, new()
        {
            IDataMapper m = GetMapper<T>();
            using (SqlConnection conn = NewOpenConnection)
            {
                conn.Open();
                ExecuteUpdateStatement(conn, m.GetDeleteSqlString(id));
            }
        }
        // SqlConnection
        // use within a using{}-statement for connection pooling
        // (each connection gets closed after the using block)
        public SqlConnection NewOpenConnection
        {
            get
            {
                SqlConnection c = new SqlConnection(connString);
                c.Open();
                return c;
            }
        }

        // singleton pattern
        private static SQLServer singleton;
        public static SQLServer Instance
        { get => GetSingletonInstance(); }
        private static SQLServer GetSingletonInstance()
        {
            if (singleton == null)
            {
                // DEBUG switches for specific connection strings
                // in RELEASE just use default connection string in web.config
                string connectionString;
#if DEBUG_PAVEL
                connectionString = @"Server =.\MSSQLSERVER01; Database = zeiterfassung; Trusted_Connection = True";
#elif DEBUG_MARTIN
                connectionString = @"Server =.; Database = zeiterfassung; Trusted_Connection = True";
#else
                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
#endif

                singleton = new SQLServer(connectionString);
            }
            return singleton;
        }

        // helper methods for routine operations
        protected SqlDataReader ExecuteSelectStatement(SqlConnection connection, string sqlstring)
        {
            SqlCommand c = new SqlCommand(sqlstring, connection);
            return c.ExecuteReader();
        }
        protected int ExecuteUpdateStatement(SqlConnection connection, string sqlstring)
        {
            SqlCommand c = new SqlCommand(sqlstring, connection);
            Decimal result = (Decimal)c.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        // for quick testing of SQL commands
        public static void RunSqlCommand(String cmmd)
        {
            SqlConnection cnn;

            cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            cnn.Open();
            SqlCommand command;
            SqlDataReader dataReader;

            command = new SqlCommand(cmmd, cnn);
            dataReader = command.ExecuteReader();

            dataReader.Close();
            cnn.Close();
        }


    }
}