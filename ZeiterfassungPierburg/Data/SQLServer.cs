using Dapper;
using Dapper.Contrib.Extensions;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Models;
using MiniProfiler.Integrations;

namespace ZeiterfassungPierburg.Data
{
    public class SQLServer
    {
        private readonly string connString;
        private SQLServer(string connectionString)
        {
            connString = connectionString;
        }

        // dictionary methods for dropdown lists
        public Dictionary<int,string> GetDictionary(string tableName, string labelString, string where)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            using (SqlConnection conn = NewOpenConnection)
            {
                string sql;

                if (where != null)
                { 
                sql = $"SELECT Id, {labelString} As Label FROM {tableName} WHERE {where}";
                }
                else sql = $"SELECT Id, {labelString} As Label FROM {tableName}";

                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    result.Add(r.GetInt32(0), r.GetString(1));
                }
            }
            return result;
        }

        // dictionary methods for dropdown lists
        public Dictionary<int, string> GetDictionaryTest(string tableName, string labelString, string where)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            using (SqlConnection conn = NewOpenConnection)
            {
                string sql;

                if (where != null)
                {
                    sql = $"SELECT t.Id, {labelString} As Label FROM {tableName} WHERE {where}";
                }
                else sql = $"SELECT Id, {labelString} As Label FROM {tableName}";

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
                if (!String.IsNullOrEmpty(whereClause))
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

        public IEnumerable<T> GetMitarbeiterInSchichtModelMEBA<T>()
        {
            using (var c = NewOpenConnection)
            {
                string sql = @" 
SELECT
t.ID,
s.Datum,
s.Art,
m.Nachname + ', ' + m.Vorname as Name,
m.Personalnummer,
m.Kostenstelle,
p.Bezeichner as Anlage,
p.Kostenstelle,
p.SAPAPNr,
f.ZeichenNr

      ,t.Stück
      ,t.DirStunden
      ,t.InDirStunden
      ,t.IstInSAPEingetragen

	  ,Bemerkung
      ,t.ErstelltAm
  FROM [zeiterfassung].[dbo].[MitarbeiterInSchicht] t
LEFT OUTER JOIN Mitarbeiter m  ON t.MitarbeiterID = m.ID 
LEFT OUTER JOIN Produktionsanlage p  ON t.ProduktionsanlageID = p.ID 
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
LEFT OUTER JOIN Fertigungsteil f  ON t.FertigungsteilID = f.ID 
where p.IstEineMaschine = 'True'
";
                return c.Query<T>(sql).ToList();

            }
        }
        public IEnumerable<T> GetMitarbeiterInSchichtModel<T>()
        {
            using (var c = NewOpenConnection)
            {
                string sql = @" 
SELECT 
t.ID,
s.Datum,
s.Art,
m.Nachname + ', ' + m.Vorname as Name,
m.Personalnummer,
m.Kostenstelle,
p.Bezeichner as Anlage,
p.Kostenstelle,
p.SAPAPNr,
f.ZeichenNr

      ,t.Stück
      ,t.DirStunden
      ,t.InDirStunden
      ,t.IstInSAPEingetragen

	  ,Bemerkung
      ,t.ErstelltAm
  FROM [zeiterfassung].[dbo].[MitarbeiterInSchicht] t
LEFT OUTER JOIN Mitarbeiter m  ON t.MitarbeiterID = m.ID 
LEFT OUTER JOIN Produktionsanlage p  ON t.ProduktionsanlageID = p.ID 
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
LEFT OUTER JOIN Fertigungsteil f  ON t.FertigungsteilID = f.ID 
where p.IstEineMaschine = 'False'
";
                return c.Query<T>(sql).ToList();

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
            using (SqlConnection conn = NewOpenConnection)
            {
                return Convert.ToInt32(conn.Insert<T>(model));
            }
        }
        public void EditItem<T>(T model) where T: BasicModelObject, new()
        {
            using (SqlConnection conn = NewOpenConnection)
            {
                conn.Update<T>(model);
            }
        }
        public void RemoveItem<T>(T model) where T : BasicModelObject, new()
        {
            using (var c = NewOpenConnection)
            {
                c.Delete<T>(model);
            }
        }
        public void RemoveItem<T>(int id) where T : BasicModelObject, new()
        {
            T modelById = GetItem<T>(id);
            RemoveItem<T>(modelById);
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

        // dictionary methods for dropdown lists
        public int GetNumber(string command)
        {
            int result = 0;
            using (SqlConnection conn = NewOpenConnection)
            {
                string sql;

                sql = command;
                
                SqlDataReader r = ExecuteSelectStatement(conn, sql);

                // TO DO: Nullbehandlung
                while (r.Read())
                {
                    result = r.GetInt32(0);
                }
                
            }
            return result;
        }
        public IEnumerable<T> GetTeileInProduktionsanlageView<T>()
        {
            using (var c = NewOpenConnection)
            {
                string sql = @" 
select
t.ID,
p.Bezeichner as Produktionsanlage,
f.Bezeichnung as Fertigungsteil

from [zeiterfassung].[dbo].TeileInProduktionsanlage t
LEFT OUTER JOIN Produktionsanlage p  ON t.ProduktionsanlageID= p.ID 
LEFT OUTER JOIN Fertigungsteil f  ON t.FertigungsteilID = f.ID"
;
                return c.Query<T>(sql).ToList();

            }
        }
        // dictionary methods for dropdown lists
        public List<string> GetFertigungsteilList(int ProduktionsanlageID)
        {
            List<string> result = new List<string>();

            using (SqlConnection conn = NewOpenConnection)
            {
                string sql = @"
select  f.Bezeichnung from 
TeileInProduktionsanlage t
LEFT OUTER JOIN Produktionsanlage p  ON t.ProduktionsanlageID = p.ID
LEFT OUTER JOIN Fertigungsteil f  ON t.FertigungsteilID = f.ID
where ProduktionsanlageID = 
"+ ProduktionsanlageID;

                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    result.Add(r.GetString(0));
                }
            }
            return result;
        }
    }
}