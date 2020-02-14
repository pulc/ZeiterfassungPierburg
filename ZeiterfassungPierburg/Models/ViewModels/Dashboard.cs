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
        string SelectMitarbiter = @"select count(ID) as MitarbeiterAnzahl
from Mitarbeiter";
        string SelectProduktionsanlagen = @"select count(ID) as MitarbeiterAnzahl
from Produktionsanlage";
        string SelectFertigungsteile = @"select count(ID) as MitarbeiterAnzahl
from Fertigungsteil";
        string SelectZeiterfassungen = @"select count(ID) as MitArbeiterinschicht
from MitArbeiterinschicht";


        string selectStückeToday = @"select sum(Stück) 
  FROM [zeiterfassung].[dbo].[MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -1, GETDATE()) AND DATEADD(day, -0, GETDATE())";

        string selectStückeMinusOneDay = @"select sum(Stück) 
  FROM [zeiterfassung].[dbo].[MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -2, GETDATE()) AND DATEADD(day, -1, GETDATE())";

        string selectStückeMinusTwoDays = @"select sum(Stück) 
  FROM [zeiterfassung].[dbo].[MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -3, GETDATE()) AND DATEADD(day, -2, GETDATE())";

        string selectStückeMinusThreeDays = @"select sum(Stück) 
  FROM [zeiterfassung].[dbo].[MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -4, GETDATE()) AND DATEADD(day, -3, GETDATE())";

        string selectStückeMinusFourDays = @"select sum(Stück) 
  FROM [zeiterfassung].[dbo].[MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -5, GETDATE()) AND DATEADD(day, -4, GETDATE())";

        string selectStückeMinusFiveDays = @"select sum(Stück) 
  FROM [zeiterfassung].[dbo].[MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -6, GETDATE()) AND DATEADD(day, -5, GETDATE())";

        string selectStückeMinusSixDays = @"select sum(Stück) 
  FROM [zeiterfassung].[dbo].[MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -7, GETDATE()) AND DATEADD(day, -6, GETDATE())";

        string selectStückepastWeek = @"select sum(Stück) 
  FROM [zeiterfassung].[dbo].[MitarbeiterInSchicht] t
LEFT OUTER JOIN Schichtinfo s  ON t.SchichtInfoID = s.ID 
where Datum BETWEEN DATEADD(day, -7, GETDATE()) AND DATEADD(day, -0, GETDATE())";


        [Display(Name = "Produktionsanlage")]
        public int MitarbeiterAnzahl
        {
            get => SQLServer.Instance.GetNumber(SelectMitarbiter);

        }
        public int Produktionsanlagen { get => SQLServer.Instance.GetNumber(SelectProduktionsanlagen); }

        public int Fertigungsteile { get => SQLServer.Instance.GetNumber(SelectFertigungsteile); }

        public int StückeToday { get => SQLServer.Instance.GetNumber(selectStückeToday); }

        public int StückeMinusOneDay { get => SQLServer.Instance.GetNumber(selectStückeMinusOneDay); }

        public int StückeMinusTwoDays { get => SQLServer.Instance.GetNumber(selectStückeMinusTwoDays);  }

        public int StückeMinusThreeDays { get => SQLServer.Instance.GetNumber(selectStückeMinusThreeDays); }

        public int StückeMinusFourDays { get => SQLServer.Instance.GetNumber(selectStückeMinusFourDays); }

        public int StückeMinusFiveDays { get => SQLServer.Instance.GetNumber(selectStückeMinusFiveDays); }

        public int StückeMinusSixDays { get => SQLServer.Instance.GetNumber(selectStückeMinusSixDays); }

        public int StückeWoche { get => SQLServer.Instance.GetNumber(selectStückepastWeek); }

        public int ZeiterfassungenAnzahl { get => SQLServer.Instance.GetNumber(SelectZeiterfassungen); }

        /*
        public Dictionary<string, float> Produktivität
        {
            get => SQLServer.Instance.GetProduktivität();
        }
        */
        public Dictionary<string, float> ProduktivitätBerechnungProBandLastMonth
        {
            get => SQLServer.Instance.CalculateProductivityLastMonthAlleBänder();
        }

        public Dictionary<string, float> ProduktivitätLast12Months
        {
            get => SQLServer.Instance.GetProduktivitätLast12Months();
        }

        public Dictionary<string, float> ProduktivitätBerechnungProMaschineLastMonth
        {
            get => SQLServer.Instance.CalculateProductivityLastMonthAlleMaschinen();
        }
    }
}