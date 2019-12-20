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

        // get model objects
        // params:
        // filter   dictionary <string,string> that contains the column names and respective values
        //
        // to correctly use the filter, the method assumes a GetItemsSQLString from the DataMapper that
        // does NOT contain a where clause
        public IEnumerable<T> GetItems<T>(Dictionary<string,string> filter = null) where T: BasicModelObject, new()
        {
            using (SqlConnection conn = NewConnection)
            {
                conn.Open();
                IDataMapper mapper = GetMapper<T>();
                string sqlstring = mapper.GetSelectSqlString();

                if (filter != null)
                {
                    string wherestring = " WHERE ";

                    // put together the where-string
                    // if the filter value contains a '%'
                    // use wildcard search
                    wherestring += String.Join(" AND ",
                        filter.Keys.Select(k =>
                        {
                            string comparison = "=";
                            if (filter[k].Contains('%'))
                                comparison = "LIKE";
                            return String.Format("{0} {1} {2}", k, comparison, filter[k]);
                        }));
                    sqlstring += wherestring;
                }
                return (IEnumerable<T>)mapper.ReadItems(
                        ExecuteSelectStatement(conn, sqlstring));
            }
        }
        public IEnumerable<T> GetItems<T>(string filter) where T : BasicModelObject, new()
        {
            using (SqlConnection conn = NewConnection)
            {
                conn.Open();
                IDataMapper mapper = GetMapper<T>();
                string sqlstring = mapper.GetSelectSqlString();

                // possible to do: the filter string is not checked
                // -> might be a security risk
                sqlstring += " WHERE " + filter;

                return (IEnumerable<T>)mapper.ReadItems(
                        ExecuteSelectStatement(conn, sqlstring));
            }
        }
        public int InsertItem<T>(T model) where T: BasicModelObject, new()
        {
            IDataMapper m = GetMapper<T>();
            using (SqlConnection conn = NewConnection)
            {
                conn.Open();
                return ExecuteUpdateStatement(conn, m.GetInsertSqlString(model));
            }
        }
        // SqlConnection
        // use within a using{}-statement for connection pooling
        // (each connection gets closed after the using block)
        public SqlConnection NewConnection
        {
            get => new SqlConnection(connString);
        }

        // singleton pattern
        private static SQLServer singleton;
        public static SQLServer Instance
        { get => GetSingletonInstance(); }
        private static SQLServer GetSingletonInstance()
        {
            if (singleton == null)
            {
                singleton = new SQLServer(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
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