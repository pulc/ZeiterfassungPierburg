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

        // dictionary methods for dropdown lists and productivity calculation
        public Dictionary<int,string> GetFertigungsteilDictionary(int ProduktionsanlageID)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            using (SqlConnection conn = NewOpenConnection)
            {
                string sql = @"
select  f.ID,f.Bezeichnung from 
TeileInProduktionsanlage t
LEFT OUTER JOIN Produktionsanlage p  ON t.ProduktionsanlageID = p.ID
LEFT OUTER JOIN Fertigungsteil f  ON t.FertigungsteilID = f.ID
where ProduktionsanlageID = 
" + ProduktionsanlageID + " and f.Bezeichnung is not null ";

                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    try
                    {
                        result.Add(r.GetInt32(0), r.GetString(1));
                    }
                    // if there are doubled Fertigungsteile in the database, an error of doubled keys in the dictionary occurs
                    // it just get's ignored
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            return result;
        }



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

            List<int> AlleBänderList = new List<int>(); //get List of all Produktionsanlagen

            using (SqlConnection conn = NewOpenConnection)
            {
                string sql =
                   @"select ID
from Produktionsanlage
";

                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    AlleBänderList.Add(r.GetInt32(0));
                }
            }

            for (int i = 11; i >= 0; i--)
            {
                float avgProduktivität = 0.0f;
                float SummeProduktivZeit = 0.0f;
                float Anwesenheit = 0.0f;


                foreach (var BandID in AlleBänderList)
                {
                    bool? istEineMaschine = SQLServer.Instance.GetBoolean("select IstEineMaschine from Produktionsanlage where ID = " + BandID);

                    if (istEineMaschine == true)
                    {

                        List<int> SchichtInfoList = new List<int>(); //get List of all SchichtInfos in the last month for each Band
                        List<float> ProduktivitätSchichtInfoList = new List<float>(); //List of all calculated Produktivität for each Schicht

                        using (SqlConnection conn = NewOpenConnection)
                        {
                            string sql =
                               @"
select  SchichtinfoID
from MitarbeiterInSchicht m
left outer join Schichtinfo i on m.SchichtInfoID = i.ID
left outer join Produktionsanlage p on m.ProduktionsanlageID = p.ID
where p.ID = " + BandID + "and (MONTH(i.Datum) = MONTH(DATEADD(MONTH, -" + i + @", GETDATE())));";

                            SqlDataReader r = ExecuteSelectStatement(conn, sql);
                            while (r.Read())
                            {
                                SchichtInfoList.Add(r.GetInt32(0));
                            }
                        }

                        foreach (var SchichtInfoID in SchichtInfoList)
                        {

                            using (SqlConnection conn = NewOpenConnection)
                            {
                                string sql =
                                   @"
select   
sum(t.Stück) * sum(f.Tezeit) * sum(f.AnzahlMA)/100/count(MitarbeiterID)/COUNT(MitarbeiterID)/COUNT(MitarbeiterID) as ProduktivZeit, --H5
(sum(t.dirstunden)*60) as Anwesenheit	 


FROM[zeiterfassung].[dbo].[MitarbeiterInSchicht] t


LEFT OUTER JOIN Mitarbeiter m  ON t.MitarbeiterID = m.ID
LEFT OUTER JOIN Produktionsanlage p ON t.ProduktionsanlageID = p.ID
LEFT OUTER JOIN Fertigungsteil f ON t.FertigungsteilID = f.ID
LEFT OUTER JOIN Schichtinfo s ON t.SchichtInfoID = s.ID

where SchichtInfoID = " + SchichtInfoID +
@" 

group by SchichtInfoID, f.ID
order by SchichtInfoID";

                                // int CountTeile = 0;

                                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                                while (r.Read())
                                {
                                    SummeProduktivZeit = SummeProduktivZeit + (float)r.GetDecimal(0);
                                    Anwesenheit = Anwesenheit + (float)r.GetDecimal(1);
                                    //CountTeile++;
                                }
                            }
                        }
                    }
                    else if (istEineMaschine == false)
                    {
                        List<int> SchichtInfoList = new List<int>(); //List of all SchichtInfos in the last month for each Band
                        List<float> ProduktivitätSchichtInfoList = new List<float>(); //List of all calculated Produktivität for each Schicht

                        using (SqlConnection conn = NewOpenConnection)
                        {
                            string sql =
                               @"
select  SchichtinfoID
from MitarbeiterInSchicht m
left outer join Schichtinfo i on m.SchichtInfoID = i.ID
left outer join Produktionsanlage p on m.ProduktionsanlageID = p.ID
where p.ID = " + BandID + "and (MONTH(i.Datum) = MONTH(DATEADD(MONTH, -" + i + @", GETDATE())));";
                            
                            SqlDataReader r = ExecuteSelectStatement(conn, sql);
                            while (r.Read())
                            {
                                SchichtInfoList.Add(r.GetInt32(0));
                            }
                        }

                        foreach (var SchichtInfoID in SchichtInfoList)
                        {

                            using (SqlConnection conn = NewOpenConnection)
                            {
                                string sql =
                                   @"
select   
sum(t.Stück) * sum(f.Tezeit) * sum(f.AnzahlMA)/100/count(MitarbeiterID)/COUNT(MitarbeiterID)/COUNT(MitarbeiterID) as ProduktivZeit, --H5
(sum(t.dirstunden)*60) as Anwesenheit	 --I5


FROM[zeiterfassung].[dbo].[MitarbeiterInSchicht] t


LEFT OUTER JOIN Mitarbeiter m  ON t.MitarbeiterID = m.ID
LEFT OUTER JOIN Produktionsanlage p ON t.ProduktionsanlageID = p.ID
LEFT OUTER JOIN Fertigungsteil f ON t.FertigungsteilID = f.ID
LEFT OUTER JOIN Schichtinfo s ON t.SchichtInfoID = s.ID

where SchichtInfoID = " + SchichtInfoID + @" 

group by SchichtInfoID, f.ID
order by SchichtInfoID";

                                // int CountTeile = 0;
                                float AnwesenheitTemp = 0f;

                                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                                while (r.Read())
                                {
                                    SummeProduktivZeit = SummeProduktivZeit + (float)r.GetDecimal(0);
                                    AnwesenheitTemp = (float)r.GetDecimal(1); // should be each time the same
                                                                              //CountTeile++;
                                }
                                Anwesenheit = Anwesenheit + AnwesenheitTemp; //add only one time for Schicht
                            }
                        }
                    }
                    avgProduktivität = SummeProduktivZeit * 100 / Anwesenheit;
                }

                using (SqlConnection conn = NewOpenConnection)
                {
                    string sql;

                    sql = @"select  ''+MONTH(DATEADD(MONTH, -" + i + @", GETDATE())),YEAR(DATEADD(MONTH, -" + i + @", GETDATE()))";

                    // select ID from Produktionsanlage where IstEineMaschine = 'false'
                    SqlDataReader r = ExecuteSelectStatement(conn, sql);
                    while (r.Read())
                    {
                        int month = r.GetInt32(0);
                        int year = r.GetInt32(1);
                        string date = month + "/" + year;

                        try
                        {
                            result.Add(date, avgProduktivität);
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
        public Dictionary<string, float> GetProduktivitätCustomDictionary(int day, int month, int year, int ProduktionsanlageID, int FertigungsteilID, int MitarbeiterID, int Art)
        {
            Dictionary<string, float> result = new Dictionary<string, float>();

            string dayCondition = "'true'='true'";
            if (day != 0) dayCondition = "DAY(i.Datum) = " + day;
            string monthCondition = "'true'='true'";
            if (month != 0) monthCondition = "MONTH(i.Datum) = " + month;
            string yearCondition = "'true'='true'";
            if (year != 0) yearCondition = "YEAR(i.Datum) = " + year;
            string anlageCondition = "'true'='true'";
            if (ProduktionsanlageID != 0) anlageCondition = "ProduktionsanlageID = " + ProduktionsanlageID;
            string teilCondition = "'true'='true'";
            if (FertigungsteilID != 0) teilCondition = "FertigungsteilID = " + FertigungsteilID;
            string mitarbeiterCondition = "'true'='true'";
            if (MitarbeiterID != 0) mitarbeiterCondition = "MitarbeiterID = " + MitarbeiterID;
            string schichtCondition = "'true'='true'";
            if (Art != 0) schichtCondition = "Art = " + Art;
            string sql = "";


            List<int> AlleBänderList = new List<int>();

            if (ProduktionsanlageID == 0)
            {
                using (SqlConnection conn = NewOpenConnection)
                {
                    sql =
                       @"select ID
from Produktionsanlage
";

                    SqlDataReader r = ExecuteSelectStatement(conn, sql);
                    while (r.Read())
                    {
                        AlleBänderList.Add(r.GetInt32(0));
                    }
                }
            }
            else
            {
                AlleBänderList.Add(ProduktionsanlageID);
            }

            foreach (var BandID in AlleBänderList)
            {
                bool? istEineMaschine = SQLServer.Instance.GetBoolean("select IstEineMaschine from Produktionsanlage where ID = " + BandID);
                string BandName = SQLServer.Instance.GetOneString("Bezeichner", "Produktionsanlage", "ID = " + BandID);

                if (istEineMaschine == true)
                {

                    List<int> SchichtInfoList = new List<int>(); //get List of all SchichtInfos in the last month for each Band
                    List<float> ProduktivitätSchichtInfoList = new List<float>(); //List of all calculated Produktivität for each Schicht

                    float avgProduktivität = 0.0f;
                    float SummeProduktivZeit = 0.0f;
                    float Anwesenheit = 0.0f;

                    using (SqlConnection conn = NewOpenConnection)
                    {
                        sql =
                           @"
select  SchichtinfoID
from MitarbeiterInSchicht m
left outer join Schichtinfo i on m.SchichtInfoID = i.ID
left outer join Produktionsanlage p on m.ProduktionsanlageID = p.ID
where p.ID = " + BandID + " and " + dayCondition + " and " + monthCondition + " and " + yearCondition + " and " + teilCondition + " and " + mitarbeiterCondition + " and " + schichtCondition;
                        ;

                        SqlDataReader r = ExecuteSelectStatement(conn, sql);
                        while (r.Read())
                        {
                            SchichtInfoList.Add(r.GetInt32(0));
                        }
                    }

                    foreach (var SchichtInfoID in SchichtInfoList)
                    {

                        using (SqlConnection conn = NewOpenConnection)
                        {
                            sql =
                               @"
select   
sum(t.Stück) * sum(f.Tezeit) * sum(f.AnzahlMA)/100/count(MitarbeiterID)/COUNT(MitarbeiterID)/COUNT(MitarbeiterID) as ProduktivZeit, --H5
(sum(t.dirstunden)*60) as Anwesenheit	 


FROM[zeiterfassung].[dbo].[MitarbeiterInSchicht] t


LEFT OUTER JOIN Mitarbeiter m  ON t.MitarbeiterID = m.ID
LEFT OUTER JOIN Produktionsanlage p ON t.ProduktionsanlageID = p.ID
LEFT OUTER JOIN Fertigungsteil f ON t.FertigungsteilID = f.ID
LEFT OUTER JOIN Schichtinfo s ON t.SchichtInfoID = s.ID

where SchichtInfoID = " + SchichtInfoID +
@" 

group by SchichtInfoID, f.ID
order by SchichtInfoID";

                            // int CountTeile = 0;

                            SqlDataReader r = ExecuteSelectStatement(conn, sql);
                            while (r.Read())
                            {
                                SummeProduktivZeit = SummeProduktivZeit + (float)r.GetDecimal(0);
                                Anwesenheit = Anwesenheit + (float)r.GetDecimal(1);
                                //CountTeile++;
                            }
                        }
                    }
                    avgProduktivität = SummeProduktivZeit / Anwesenheit * 100;

                    result.Add(BandName, avgProduktivität);
                }
                else if (istEineMaschine == false)
                {
                    List<int> SchichtInfoList = new List<int>(); //List of all SchichtInfos in the last month for each Band
                    List<float> ProduktivitätSchichtInfoList = new List<float>(); //List of all calculated Produktivität for each Schicht

                    float avgProduktivität = 0.0f;

                    using (SqlConnection conn = NewOpenConnection)
                    {
                        sql =
                           @"
select  SchichtinfoID
from MitarbeiterInSchicht m
left outer join Schichtinfo i on m.SchichtInfoID = i.ID
left outer join Produktionsanlage p on m.ProduktionsanlageID = p.ID
where p.ID = " + BandID + " and " + dayCondition + " and " + monthCondition + " and " + yearCondition + " and " + teilCondition + " and " + mitarbeiterCondition + " and " + schichtCondition;
                        //
                        SqlDataReader r = ExecuteSelectStatement(conn, sql);
                        while (r.Read())
                        {
                            SchichtInfoList.Add(r.GetInt32(0));
                        }
                    }
                    float SummeProduktivZeit = 0.0f; //add all Produktivzeit together
                    float Anwesenheit = 0.0f; //add all Anwesenheit together 


                    foreach (var SchichtInfoID in SchichtInfoList)
                    {

                        using (SqlConnection conn = NewOpenConnection)
                        {
                            sql =
                               @"
select   
sum(t.Stück) * sum(f.Tezeit) * sum(f.AnzahlMA)/100/count(MitarbeiterID)/COUNT(MitarbeiterID)/COUNT(MitarbeiterID) as ProduktivZeit, --H5
(sum(t.dirstunden)*60) as Anwesenheit	 --I5


FROM[zeiterfassung].[dbo].[MitarbeiterInSchicht] t


LEFT OUTER JOIN Mitarbeiter m  ON t.MitarbeiterID = m.ID
LEFT OUTER JOIN Produktionsanlage p ON t.ProduktionsanlageID = p.ID
LEFT OUTER JOIN Fertigungsteil f ON t.FertigungsteilID = f.ID
LEFT OUTER JOIN Schichtinfo s ON t.SchichtInfoID = s.ID

where SchichtInfoID = " + SchichtInfoID + @" 

group by SchichtInfoID, f.ID
order by SchichtInfoID";

                            // int CountTeile = 0;
                            float AnwesenheitTemp = 0f;

                            SqlDataReader r = ExecuteSelectStatement(conn, sql);
                            while (r.Read())
                            {
                                SummeProduktivZeit = SummeProduktivZeit + (float)r.GetDecimal(0);
                                AnwesenheitTemp = (float)r.GetDecimal(1); // should be each time the same
                                                                          //CountTeile++;
                            }
                            Anwesenheit = Anwesenheit + AnwesenheitTemp; //add only one time for Schicht

                            //float ProduktivitätProSchichtInfo = SummeProduktivZeit / Anwesenheit * 100;
                            //sumProduktivität = sumProduktivität + ProduktivitätProSchichtInfo;

                        }
                    }

                    // avgProduktivität = sumProduktivität / SchichtInfoList.Count;

                    avgProduktivität = SummeProduktivZeit * 100 / Anwesenheit;
                    result.Add(BandName, avgProduktivität);
                }

            }
            return result;
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
        public Dictionary<string, float> CalculateProductivityLastMonthAlleBänder()
        {

            Dictionary<string, float> result = new Dictionary<string, float>();


            List<int> AlleBänderList = new List<int>();
            using (SqlConnection conn = NewOpenConnection)
            {
                string sql =
                    @"select ID
from Produktionsanlage
where IstEineMaschine = 'false' and istAktiv = 'true'";

                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    AlleBänderList.Add(r.GetInt32(0));
                }
            }

            foreach (var BandID in AlleBänderList)
            {
                string BandName = SQLServer.Instance.GetOneString("Bezeichner", "Produktionsanlage", "ID = " + BandID);

                List<int> SchichtInfoList = new List<int>(); //List of all SchichtInfos in the last month for each Band
                List<float> ProduktivitätSchichtInfoList = new List<float>(); //List of all calculated Produktivität for each Schicht
                float avgProduktivität = 0.0f;

                using (SqlConnection conn = NewOpenConnection)
                {
                    string sql =
                        @"
select  SchichtinfoID
from MitarbeiterInSchicht m
left outer join Schichtinfo i on m.SchichtInfoID = i.ID
left outer join Produktionsanlage p on m.ProduktionsanlageID = p.ID
where p.ID = " + BandID + " and (Datum BETWEEN  DATEADD(m, -1, getdate()) AND getdate());";
                    //
                    SqlDataReader r = ExecuteSelectStatement(conn, sql);
                    while (r.Read())
                    {
                        SchichtInfoList.Add(r.GetInt32(0));
                    }
                }
                float SummeProduktivZeit = 0.0f; //add all Produktivzeit together
                float Anwesenheit = 0.0f; //add all Anwesenheit together 


                foreach (var SchichtInfoID in SchichtInfoList)
                {

                    using (SqlConnection conn = NewOpenConnection)
                    {
                        string sql =
                            @"
select   
sum(t.Stück) * sum(f.Tezeit) * sum(f.AnzahlMA)/100/count(MitarbeiterID)/COUNT(MitarbeiterID)/COUNT(MitarbeiterID) as ProduktivZeit, --H5
(sum(t.dirstunden)*60) as Anwesenheit	 --I5


FROM[zeiterfassung].[dbo].[MitarbeiterInSchicht] t


LEFT OUTER JOIN Mitarbeiter m  ON t.MitarbeiterID = m.ID
LEFT OUTER JOIN Produktionsanlage p ON t.ProduktionsanlageID = p.ID
LEFT OUTER JOIN Fertigungsteil f ON t.FertigungsteilID = f.ID
LEFT OUTER JOIN Schichtinfo s ON t.SchichtInfoID = s.ID

where SchichtInfoID = " + SchichtInfoID + @" 

group by SchichtInfoID, f.ID
order by SchichtInfoID";

                        // int CountTeile = 0;
                        float AnwesenheitTemp = 0f;

                        SqlDataReader r = ExecuteSelectStatement(conn, sql);
                        while (r.Read())
                        {
                            SummeProduktivZeit = SummeProduktivZeit + (float)r.GetDecimal(0);
                            AnwesenheitTemp = (float)r.GetDecimal(1); // should be each time the same
                            //CountTeile++;
                        }
                        Anwesenheit = Anwesenheit + AnwesenheitTemp; //add only one time for Schicht

                        //float ProduktivitätProSchichtInfo = SummeProduktivZeit / Anwesenheit * 100;
                        //sumProduktivität = sumProduktivität + ProduktivitätProSchichtInfo;

                    }
                }

                // avgProduktivität = sumProduktivität / SchichtInfoList.Count;

                avgProduktivität = SummeProduktivZeit * 100 / Anwesenheit;
                result.Add(BandName, avgProduktivität);
            }
            return result;
        }
        public Dictionary<string, float> CalculateProductivityLastMonthAlleMaschinen()
        {

            Dictionary<string, float> result = new Dictionary<string, float>();


            List<int> AlleBänderList = new List<int>();
            using (SqlConnection conn = NewOpenConnection)
            {
                string sql =
                    @"select ID
from Produktionsanlage
where IstEineMaschine = 'true' and istAktiv = 'true'";

                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    AlleBänderList.Add(r.GetInt32(0));
                }
            }

            foreach (var BandID in AlleBänderList)
            {
                string BandName = SQLServer.Instance.GetOneString("Bezeichner", "Produktionsanlage", "ID = " + BandID);

                List<int> SchichtInfoList = new List<int>(); //get List of all SchichtInfos in the last month for each Band
                List<float> ProduktivitätSchichtInfoList = new List<float>(); //List of all calculated Produktivität for each Schicht

                float avgProduktivität = 0.0f;
                float SummeProduktivZeit = 0.0f;
                float Anwesenheit = 0.0f;

                using (SqlConnection conn = NewOpenConnection)
                {
                    string sql =
                        @"
select  SchichtinfoID
from MitarbeiterInSchicht m
left outer join Schichtinfo i on m.SchichtInfoID = i.ID
left outer join Produktionsanlage p on m.ProduktionsanlageID = p.ID
where p.ID = " + BandID + " and (Datum BETWEEN  DATEADD(m, -1, getdate()) AND getdate());";

                    SqlDataReader r = ExecuteSelectStatement(conn, sql);
                    while (r.Read())
                    {
                        SchichtInfoList.Add(r.GetInt32(0));
                    }
                }

                foreach (var SchichtInfoID in SchichtInfoList)
                {

                    using (SqlConnection conn = NewOpenConnection)
                    {
                        string sql =
                            @"
select   
sum(t.Stück) * sum(f.Tezeit) * sum(f.AnzahlMA)/100/count(MitarbeiterID)/COUNT(MitarbeiterID)/COUNT(MitarbeiterID) as ProduktivZeit, --H5
(sum(t.dirstunden)*60) as Anwesenheit	 


FROM[zeiterfassung].[dbo].[MitarbeiterInSchicht] t


LEFT OUTER JOIN Mitarbeiter m  ON t.MitarbeiterID = m.ID
LEFT OUTER JOIN Produktionsanlage p ON t.ProduktionsanlageID = p.ID
LEFT OUTER JOIN Fertigungsteil f ON t.FertigungsteilID = f.ID
LEFT OUTER JOIN Schichtinfo s ON t.SchichtInfoID = s.ID

where SchichtInfoID = " + SchichtInfoID + @" 

group by SchichtInfoID, f.ID
order by SchichtInfoID";

                        // int CountTeile = 0;

                        SqlDataReader r = ExecuteSelectStatement(conn, sql);
                        while (r.Read())
                        {
                            SummeProduktivZeit = SummeProduktivZeit + (float)r.GetDecimal(0);
                            Anwesenheit = Anwesenheit + (float)r.GetDecimal(1);
                            //CountTeile++;
                        }
                    }
                }
                avgProduktivität = SummeProduktivZeit / Anwesenheit * 100;

                result.Add(BandName, avgProduktivität);
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
        private string GetTableNameForModel<T>()
        {
            return typeof(T).Name;
        }

        // helper methods 

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
        public int CountResults (string cmd)
        {
            using (SqlConnection conn = NewOpenConnection)
            {
                try
                {
                    SqlCommand c = new SqlCommand(cmd, conn);
                    object result = c.ExecuteScalar();

                    return (int)result;
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
        }
        // TODO: Needs to be corrected
        public int ExecuteCommand(string command)
        {
            using (SqlConnection conn = NewOpenConnection)
            {
                try
                {
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


        // helper methods to get a single query result with a specific data type
        public int GetInt(string command)
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
        public bool? GetBoolean(string command)
        {
            bool? result = null;
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
                        result = r.GetBoolean(0);
                    }
                    return result;
                }
            }
            catch
            {

                return result;
            }
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
        public string GetOneString(string column, string table, string where)
        {
            string result = "";

            using (SqlConnection conn = NewOpenConnection)
            {
                string sql = $"select {column} from {table} where {where} order by ID";

                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    result = r.GetString(0);
                }
            }
            return result;
        }


        //helper methods to get a List with a specific data type
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

        // IEnurable methods for loading of View Models
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
        public IEnumerable<T> GetMitarbeiterInSchichtModel<T>(string where = null)
        {
            string con = "'true' = 'true'";

            if (where != null)
            {
                con = where;
            }
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
where  
" + con;
                return c.Query<T>(sql).ToList();
            }
        }
                          
        // Methods for generating HTML code 
        // TODO: refactoring of the method 
        public string generateHtmlProduktionsanlagen(string condition)
        {
            string result = @"

            <select class=""form-control text-box single-line"" id=""anlageFilter"">
                            <option value = """"> alle Anlagen </option>";

            

            using (SqlConnection conn = NewOpenConnection)
            {
                string sql = "";

                if(condition == null)
                {
                    return null;
                }
                else
                {
                     sql =
                    @"select Bezeichner
from Produktionsanlage
where "+condition;

                }
                
                SqlDataReader r = ExecuteSelectStatement(conn, sql);
                while (r.Read())
                {
                    result = result +   @"<option value =  """+ r.GetString(0)+ @""" > " + r.GetString(0) + "</option>";
                }
            }

            return result + " </select>";
        }
    }
}