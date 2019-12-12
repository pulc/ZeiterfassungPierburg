using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Models;

namespace ZeiterfassungPierburg.Data
{
    public class SQLServer
    {
        private readonly SqlConnection connection;
        private SQLServer(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            dataMappers = new Dictionary<Type, IDataMapper>();
        }
        private Dictionary<Type, IDataMapper> dataMappers;

        private IDataMapper GetMapper<T>() where T: BasicModelObject
        {
            if (!dataMappers.ContainsKey(typeof(T)))
            {
                // create a generic data mapper, if none has been registered yet
                dataMappers.Add(typeof(T), new DataMapper<T>(typeof(T).Name));
            }
            return dataMappers[typeof(T)];
        }

        // get model objects
        public IEnumerable<T> GetItems<T>() where T: BasicModelObject
        {
            IDataMapper mapper = GetMapper<T>();
            return null;
        }

        // singleton pattern
        private static SQLServer singleton;
        public SQLServer Instance
        { get => GetConnection(); }
        private SQLServer GetConnection()
        {
            if (singleton == null)
            {
                singleton = new SQLServer(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            }
            return singleton;
        }
    }
}