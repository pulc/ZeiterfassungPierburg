using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;

namespace ZeiterfassungPierburg.Models.NeueZeiterfassung
{
    public class NeueZeiterfassung
    {

        public DateTime Datum { get; set; }

        public string Name { get; set; }

        public int Schicht { get; set; }

        [Display(Name = "Produktionsanlage")]
        public IEnumerable<SelectListItem> ProduktionsanlageList { get; set; }

        [Display(Name = "Schicht")]
        public IEnumerable<SelectListItem> SchichtList { get; set; } = new List<SelectListItem>()
        {
        new SelectListItem{ Text="Früh", Value="1"},
        new SelectListItem{ Text="Spät", Value="2"},
        new SelectListItem{ Text="Nacht", Value="3"},
        };

        [Display(Name = "Name")]
        public IEnumerable<SelectListItem> NameList { get; set; }



        //public string Produktionsanlage { get; set; }

        [Display(Name = "Teil")]
        public string Fertigungsteil { get; set; }

        public int Stückzahl { get; set; }

        public decimal Zeit { get; set; }

        public string Produktionsanlage { get; set; }

        enum SchichtArt
        {
            Früh,     // 0
            Spät,    // 1
            Nacht,  // 2
        }
    }
}

