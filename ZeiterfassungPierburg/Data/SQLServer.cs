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
        public Dictionary<int, string> GetDictionary(string tableName, string labelString, string where)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            using (SqlConnection conn = NewOpenConnection)
            {
                string sql;

                if (where != null)
                {
                    sql = $"SELECT Id, {labelString} As Label FROM {tableName} WHERE {where} order by Label";
                }
                else sql = $"SELECT Id, {labelString} As Label FROM {tableName}  order by Label";

                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    result.Add(r.GetInt32(0), r.GetString(1));
                }
            }
            return result;
        }
        public Dictionary<string, float> GetProduktivitätLast12Months()
        {
            Dictionary<string, float> result = new Dictionary<string, float>();

            for (int i = 11; i >= 0; i--)
            {
                using (SqlConnection conn = NewOpenConnection)
                {
                    string sql;

                    sql = @"select  ''+MONTH(DATEADD(MONTH, -" + i + @", GETDATE())),YEAR(DATEADD(MONTH, -" + i + @", GETDATE())),

sum(Stück) *100/(sum(f.teZEIT*(DirStunden+InDirStunden)))

  FROM[zeiterfassung].[dbo].[MitarbeiterInSchicht] t
LEFT OUTER JOIN Mitarbeiter m  ON t.MitarbeiterID = m.ID
LEFT OUTER JOIN Produktionsanlage p ON t.ProduktionsanlageID = p.ID
LEFT OUTER JOIN Fertigungsteil f ON t.FertigungsteilID = f.ID
LEFT OUTER JOIN Schichtinfo s ON t.SchichtInfoID = s.ID

where p.IstEineMaschine = 'false' and (MONTH(s.Datum) = MONTH(DATEADD(MONTH, -" + i + @", GETDATE())))
";

                    // select ID from Produktionsanlage where IstEineMaschine = 'false'
                    SqlDataReader r = ExecuteSelectStatement(conn, sql);
                    while (r.Read())
                    {
                        int month = r.GetInt32(0);
                        int year = r.GetInt32(1);
                        string date = month + "/" + year;


                        try
                        {
                            result.Add(date, (float)r.GetDecimal(2));
                        }
                        catch
                        {
                            result.Add(date, 0);

                        }
                    }
                }
            }

            return result;
        }
        public Dictionary<string, float> GetProduktivitätMaschinenGesamt()
        {
            Dictionary<string, float> result = new Dictionary<string, float>();

            using (SqlConnection conn = NewOpenConnection)
            {
                string sql;

                sql = @"
select   p.Bezeichner, f.Bezeichnung,((sum(Stück) / (sum(DirStunden) + sum(InDirStunden)))*100)/(f.teZEIT)


  FROM[zeiterfassung].[dbo].[MitarbeiterInSchicht] t
LEFT OUTER JOIN Mitarbeiter m  ON t.MitarbeiterID = m.ID
LEFT OUTER JOIN Produktionsanlage p ON t.ProduktionsanlageID = p.ID
LEFT OUTER JOIN Fertigungsteil f ON t.FertigungsteilID = f.ID
Left Outer Join TeileInProduktionsanlage i ON f.ID = i.FertigungsteilID

--LEFT OUTER JOIN TeileInProduktionsanlage i ON p.ID = i.ProduktionsanlageID

where p.IstEineMaschine = 'true'

group by p.Bezeichner, f.teZEIT, f.Bezeichnung
order by Bezeichner
";

                // select ID from Produktionsanlage where IstEineMaschine = 'false'
                SqlDataReader r = ExecuteSelectStatement(conn, sql);


                while (r.Read())
                {

                    try
                    {
                        result.Add(r.GetString(0) + "_" + r.GetString(1), (float)r.GetDecimal(2));
                    }
                    catch
                    {
                        result.Add("", 0);
                    }
                }
            }
            return result;
        }

        public Dictionary<string, float> GetProduktivität()
        {
            List<int> BandIDs = SQLServer.Instance.GetIntList("ID", "Produktionsanlage", "IstEineMaschine = 'false'");

            Dictionary<string, float> result = new Dictionary<string, float>();

            foreach (int id in BandIDs)
            {
                using (SqlConnection conn = NewOpenConnection)
                {
                    string sql;

                    sql = @"select   p.Bezeichner,((sum(Stück) / (sum(DirStunden) + sum(InDirStunden)))*100)/(select f.teZEIT

  FROM[zeiterfassung].[dbo].TeileInProduktionsanlage i

left outer JOIN Fertigungsteil f  ON f.ID = i.FertigungsteilID

where ProduktionsanlageID = " + id + @")

  FROM[zeiterfassung].[dbo].[MitarbeiterInSchicht] t
LEFT OUTER JOIN Mitarbeiter m  ON t.MitarbeiterID = m.ID
LEFT OUTER JOIN Produktionsanlage p ON t.ProduktionsanlageID = p.ID
LEFT OUTER JOIN Fertigungsteil f ON t.FertigungsteilID = f.ID

--LEFT OUTER JOIN TeileInProduktionsanlage i ON p.ID = i.ProduktionsanlageID

where t.ProduktionsanlageID = " + id + @"

group by p.Bezeichner
";
                    // select ID from Produktionsanlage where IstEineMaschine = 'false'
                    SqlDataReader r = ExecuteSelectStatement(conn, sql);
                    while (r.Read())
                    {
                        result.Add(r.GetString(0), (float)r.GetDecimal(1));
                    }
                }
            }

            return result;
        }

        // dictionary methods for dropdown lists
        public List<List<string>> GetDictionaryTeileInProduktionsanlageEdit(int ID)
        {
            List<List<string>>  result = new List<List<string>>();

            using (SqlConnection conn = NewOpenConnection)
            {
                string sql = @"
select

p.Bezeichner as Produktionsanlage,
t.ProduktionsanlageID,
f.Bezeichnung as Fertigungsteil

from [zeiterfassung].[dbo].TeileInProduktionsanlage t
LEFT OUTER JOIN Produktionsanlage p  ON t.ProduktionsanlageID= p.ID 
LEFT OUTER JOIN Fertigungsteil f  ON t.FertigungsteilID = f.ID
 where t.ID = " + ID;

                
                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    List<string> temp = new List<string>();
                    temp.Add(r.GetString(0));
                    temp.Add(r.GetInt32(1).ToString());
                    temp.Add(r.GetString(2));

                    result.Add(temp);
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




        public T GetItem<T>(int id) where T : BasicModelObject, new()
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
,t.Auswertung
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

        public IEnumerable<T> GetProduktivitätViewModel<T>(int day, int month, int year, int ProduktionsanlageID, int FertigungsteilID, int MitarbeiterID, int Art)
        {
            using (var c = NewOpenConnection)
            {
                string groupyByString = @"group by p.Bezeichner, f.Bezeichnung ";
                string select = @" 
select  p.Bezeichner as Produktionsanlage,  f.Bezeichnung as Fertigungsteil,

sum(Stück)*100/(sum(f.teZEIT*(DirStunden+InDirStunden))) as Produktivität";

                string dayCondition = "'true'='true'";
                if (day != 0) dayCondition = "DAY(s.Datum) = " + day;
                string monthCondition = "'true'='true'";
                if (month != 0) monthCondition = "MONTH(s.Datum) = " + month;
                string yearCondition = "'true'='true'";
                if (year != 0) yearCondition = "YEAR(s.Datum) = " + year;
                string anlageCondition = "'true'='true'";
                if (ProduktionsanlageID != 0) anlageCondition = "ProduktionsanlageID = " + ProduktionsanlageID;
                string teilCondition = "'true'='true'";
                if (FertigungsteilID != 0) teilCondition = "FertigungsteilID = " + FertigungsteilID;

                string mitarbeiterCondition = "'true'='true'";
                if (MitarbeiterID != 0)
                {
                    mitarbeiterCondition = "MitarbeiterID = " + MitarbeiterID;
                    groupyByString = groupyByString + " ,MitarbeiterID,m.Nachname,m.Vorname";
                    select = select + " , m.Nachname + ' ' + m.Vorname as Mitarbeiter";
                }
                string schichtCondition = "'true'='true'";
                if (Art != 0)
                {
                    schichtCondition = "Art = " + Art;
                    groupyByString = groupyByString + " ,Art";
                }


                string sql = @"

  FROM[zeiterfassung].[dbo].[MitarbeiterInSchicht] t
LEFT OUTER JOIN Mitarbeiter m  ON t.MitarbeiterID = m.ID
LEFT OUTER JOIN Produktionsanlage p ON t.ProduktionsanlageID = p.ID
LEFT OUTER JOIN Fertigungsteil f ON t.FertigungsteilID = f.ID
LEFT OUTER JOIN Schichtinfo s ON t.SchichtInfoID = s.ID";


                sql = sql + " where " + dayCondition + " and " + monthCondition + " and " + yearCondition + " and " + anlageCondition + " and " + teilCondition + " and " + mitarbeiterCondition + " and " + schichtCondition;

                string orderBy = " order by Produktionsanlage,Fertigungsteil";

                string cmd = select + sql + groupyByString + orderBy;

                return c.Query<T>(cmd).ToList();
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
,t.Auswertung

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
        public int InsertItem<T>(T model) where T : BasicModelObject, new()
        {
            using (SqlConnection conn = NewOpenConnection)
            {
                return Convert.ToInt32(conn.Insert<T>(model));
            }
        }
        public void EditItem<T>(T model) where T : BasicModelObject, new()
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

        // TODO: Needs to be corrected
        public int ExecuteCommand(string command)
        {
            using (SqlConnection conn = NewOpenConnection)
            {
                try { 
                SqlCommand c = new SqlCommand(command, conn);
                object result = c.ExecuteScalar();

                return 1;
                }
                catch (Exception e)
                { 
                return 0;
                }
            }
        }


        public int GetNumber(string command)
        {
            int result = 0;
            try
            {
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
                    return result;
                }
            }
            catch
            {

                return result;
            }
        }

        public decimal GetDecimal(string command)
        {
            decimal result = 0;
            try
            {
                using (SqlConnection conn = NewOpenConnection)
                {
                    string sql;

                    sql = command;

                    SqlDataReader r = ExecuteSelectStatement(conn, sql);

                    // TO DO: Nullbehandlung

                    while (r.Read())
                    {
                        result = r.GetDecimal(0);
                    }
                    return result;
                }
            }
            catch
            {

                return result;
            }
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
LEFT OUTER JOIN Fertigungsteil f  ON t.FertigungsteilID = f.ID
 where p.Bezeichner is not null and f.Bezeichnung is not null"
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
" + ProduktionsanlageID + " and f.Bezeichnung is not null ";

                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    result.Add(r.GetString(0));
                }
            }
            return result;
        }

        public List<int> GetIntList(string column, string table, string where)
        {
            List<int> result = new List<int>();

            using (SqlConnection conn = NewOpenConnection)
            {
                string sql = $"select {column} from {table} where {where} order by ID";

                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    result.Add(r.GetInt32(0));
                }
            }
            return result;
        }
        public List<string> GetStringList(string column, string table, string where)
        {
            List<string> result = new List<string>();

            using (SqlConnection conn = NewOpenConnection)
            {
                string sql = $"select {column} from {table} where {where} order by ID";

                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    result.Add(r.GetString(0));
                }
            }
            return result;
        }
        public string GetString(string column, string table, string where)
        {
            string result = "";

            using (SqlConnection conn = NewOpenConnection)
            {
                string sql = $"select {column} from {table} where {where} order by ID";

                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    result = result + "," + (r.GetString(0));
                }
            }
            return result.Substring(0, result.Length - 1); //substring to remove the last comma
        }
        public Dictionary<int, string> GetDictionaryProduktivität(string tableName, string labelString, string where)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            using (SqlConnection conn = NewOpenConnection)
            {
                string sql;
                result.Add(0, "alle");

                if (where != null)
                {
                    sql = $"SELECT Id, {labelString} As Label FROM {tableName} WHERE {where} order by Label";
                }
                else sql = $"SELECT Id, {labelString} As Label FROM {tableName}  order by Label";

                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    result.Add(r.GetInt32(0), r.GetString(1));
                }
            }
            return result;
        }

    }
}