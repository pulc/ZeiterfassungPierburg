using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;
using Dapper.Contrib.Extensions;

namespace ZeiterfassungPierburg.Models.ViewModels
{
    [Table("Dashboard")]
    public class Dashboard: BasicModelObject
    {
        /* Statitcs for tables. It only counts how many rows there are. Can be also possibly skipped
         * as the purpose is only for the Dashboard to look prettier */
        string SelectMitarbiter = @"select count(ID) as MitarbeiterAnzahl
from Mitarbeiter";
        string SelectProduktionsanlagen = @"select count(ID) as MitarbeiterAnzahl
from Produktionsanlage";
        string SelectFertigungsteile = @"select count(ID) as MitarbeiterAnzahl
from Fertigungsteil";
        string SelectZeiterfassungen = @"select count(ID) as MitArbeiterinschicht
from MitArbeiterinschicht";

        // Statistics for Stücke
        // TODO: can be simplified with a loop or completely removed as it doesn't serve a real purpose 
        string selectStückeToday = @"select sum(Stück) 
  FROM  [MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -1, GETDATE()) AND DATEADD(day, -0, GETDATE())";

        string selectStückeMinusOneDay = @"select sum(Stück) 
  FROM  [MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -2, GETDATE()) AND DATEADD(day, -1, GETDATE())";

        string selectStückeMinusTwoDays = @"select sum(Stück) 
  FROM  [MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -3, GETDATE()) AND DATEADD(day, -2, GETDATE())";

        string selectStückeMinusThreeDays = @"select sum(Stück) 
  FROM  [MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -4, GETDATE()) AND DATEADD(day, -3, GETDATE())";

        string selectStückeMinusFourDays = @"select sum(Stück) 
  FROM  [MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -5, GETDATE()) AND DATEADD(day, -4, GETDATE())";

        string selectStückeMinusFiveDays = @"select sum(Stück) 
  FROM  [MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -6, GETDATE()) AND DATEADD(day, -5, GETDATE())";

        string selectStückeMinusSixDays = @"select sum(Stück) 
  FROM  [MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -7, GETDATE()) AND DATEADD(day, -6, GETDATE())";

        string selectStückepastWeek = @"select sum(Stück) 
  FROM  [MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -7, GETDATE()) AND DATEADD(day, -0, GETDATE())";

        // Tables to count the number of queries for each table
        public int MitarbeiterAnzahl{ get => SQLServer.Instance.GetInt(SelectMitarbiter); }
        public int Produktionsanlagen { get => SQLServer.Instance.GetInt(SelectProduktionsanlagen); }
        public int ZeiterfassungenAnzahl { get => SQLServer.Instance.GetInt(SelectZeiterfassungen); }
        public int Fertigungsteile { get => SQLServer.Instance.GetInt(SelectFertigungsteile); }
                 

        // Calculate count of Stücke
        public int StückeToday { get => SQLServer.Instance.GetInt(selectStückeToday); }

        public int StückeMinusOneDay { get => SQLServer.Instance.GetInt(selectStückeMinusOneDay); }

        public int StückeMinusTwoDays { get => SQLServer.Instance.GetInt(selectStückeMinusTwoDays);  }

        public int StückeMinusThreeDays { get => SQLServer.Instance.GetInt(selectStückeMinusThreeDays); }

        public int StückeMinusFourDays { get => SQLServer.Instance.GetInt(selectStückeMinusFourDays); }

        public int StückeMinusFiveDays { get => SQLServer.Instance.GetInt(selectStückeMinusFiveDays); }

        public int StückeMinusSixDays { get => SQLServer.Instance.GetInt(selectStückeMinusSixDays); }

        public int StückeWoche { get => SQLServer.Instance.GetInt(selectStückepastWeek); }


        // Productivity for all Produktionsanlagen, ready to get transformed into a chart 
        public Dictionary<string, float> ProduktivitätLast12Months
        {
            get => SQLServer.Instance.GetProduktivitätLast12Months();
        }

        // Calculate Productivity for each month for each Maschine
        public Dictionary<string, float> ProduktivitätBerechnungProMaschineLastMonth
        {
            get => SQLServer.Instance.CalculateProductivityLastMonthAlleMaschinen();
        }
        // Calculate Productivity for each month for each Band
        public Dictionary<string, float> ProduktivitätBerechnungProBandLastMonth
        {
            get => SQLServer.Instance.CalculateProductivityLastMonthAlleBänder();
        }

    }
}