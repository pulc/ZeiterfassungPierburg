using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;

namespace ZeiterfassungPierburg.Models.ViewModels

{
    public class ProduktivitätViewModel
    {
        public int ProduktionsanlageID { get; set; }

        public string Produktionsanlage;

        public string Fertigungsteil;

        public int FertigungsteilID { get; set; }

        public string Mitarbeiter;

        [Display(Name = "Mitarbeiter")]
        public int MitarbeiterID { get; set; }

        [Display(Name = "Schicht")]
        public int Art { get; set; }

        public decimal Produktivität { get; set; }

        [Display(Name = "Gesamte Produktivität")]
        public int ProduktivitätGesamt { get; set; }

        [Display(Name = "Tag")]
        public int Day { get; set; }
        [Display(Name = "Monat")]
        public int Month{ get; set; }

        [Display(Name = "Jahr")]
        public int Year { get; set; }

        [Display(Name = "Fertigungsteil")]
        public IEnumerable<SelectListItem> FertigungteilList
        {
            get => SQLServer.Instance.GetDictionaryProduktivität("Fertigungsteil", "Bezeichnung", null)
                .Select(s => new SelectListItem()
                {
                    Text = s.Value,
                    Value = s.Key.ToString()
                });
        }
        [Display(Name = "Mitarbeiter")]
        public IEnumerable<SelectListItem> MitarbeiterList
        {
            get => SQLServer.Instance.GetDictionaryProduktivität("Mitarbeiter", "Nachname + ' ' + Vorname", null)
                .Select(s => new SelectListItem()
                {
                    Text = s.Value,
                    Value = s.Key.ToString()
                });
        }

        [Display(Name = "Produktionsanlage")]
        public IEnumerable<SelectListItem> ProduktionsanlageList
        {
            get => SQLServer.Instance.GetDictionaryProduktivität("Produktionsanlage", "Bezeichner", "istEineMaschine = 'false' AND istAktiv = 'true'")
                    .Select(s => new SelectListItem()
                    {
                        Text = s.Value,
                        Value = s.Key.ToString()
                    });
        }

        [Display(Name = "Monat")]
        public IEnumerable<SelectListItem> MonthList { get; set; } = new List<SelectListItem>()
        {
        new SelectListItem{ Text="alle Monate", Value="0"},
        new SelectListItem{ Text="Januar", Value="1"},
        new SelectListItem{ Text="Februar", Value="2"},
        new SelectListItem{ Text="März", Value="3"},
        new SelectListItem{ Text="April", Value="4"},
        new SelectListItem{ Text="Mai", Value="5"},
        new SelectListItem{ Text="Juni", Value="6"},
        new SelectListItem{ Text="Juli", Value="7"},
        new SelectListItem{ Text="August", Value="8"},
        new SelectListItem{ Text="September", Value="9"},
        new SelectListItem{ Text="Oktober", Value="10"},
        new SelectListItem{ Text="November", Value="11"},
        new SelectListItem{ Text="Dezember", Value="12"}
        };
        [Display(Name = "Tag")]
        public IEnumerable<SelectListItem> DayList { get; set; } = new List<SelectListItem>()
        {
        new SelectListItem{ Text="alle Tage", Value="0"},
        new SelectListItem{ Text="1", Value="1"},
        new SelectListItem{ Text="2", Value="2"},
        new SelectListItem{ Text="3", Value="3"},
        new SelectListItem{ Text="4", Value="4"},
        new SelectListItem{ Text="5", Value="5"},
        new SelectListItem{ Text="6", Value="6"},
        new SelectListItem{ Text="7", Value="7"},
        new SelectListItem{ Text="8", Value="8"},
        new SelectListItem{ Text="9", Value="9"},
        new SelectListItem{ Text="10", Value="10"},
        new SelectListItem{ Text="11", Value="11"},
        new SelectListItem{ Text="12", Value="12"},
        new SelectListItem{ Text="13", Value="13"},
        new SelectListItem{ Text="14", Value="14"},
        new SelectListItem{ Text="15", Value="15"},
        new SelectListItem{ Text="16", Value="16"},
        new SelectListItem{ Text="17", Value="17"},
        new SelectListItem{ Text="18", Value="18"},
        new SelectListItem{ Text="19", Value="19"},
        new SelectListItem{ Text="20", Value="20"},
        new SelectListItem{ Text="21", Value="21"},
        new SelectListItem{ Text="22", Value="22"},
        new SelectListItem{ Text="23", Value="23"},
        new SelectListItem{ Text="24", Value="24"},
        new SelectListItem{ Text="25", Value="25"},
        new SelectListItem{ Text="26", Value="26"},
        new SelectListItem{ Text="27", Value="27"},
        new SelectListItem{ Text="28", Value="28"},
        new SelectListItem{ Text="29", Value="29"},
        new SelectListItem{ Text="30", Value="30"},
        new SelectListItem{ Text="31", Value="31"},
        };

        [Display(Name = "Schicht")]
        public IEnumerable<SelectListItem> ArtList { get; set; } = new List<SelectListItem>()
        {
        new SelectListItem{ Text="alle Schichten", Value="0"},
        new SelectListItem{ Text="Früh", Value="1"},
        new SelectListItem{ Text="Spät", Value="2"},
        new SelectListItem{ Text="Nacht", Value="3"},
        };
    }
}