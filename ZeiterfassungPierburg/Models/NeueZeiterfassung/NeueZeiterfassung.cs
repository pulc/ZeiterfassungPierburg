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

        //public string Produktionsanlage { get; set; }

        [Display(Name = "Teil")]
        public string Fertigungsteil { get; set;  }

        public int Stückzahl { get; set; }

        public decimal Zeit { get; set; }

        public string Produktionsanlage { get; set; }
        /*
        public Fertigungsteil fertigungsteil;
        public Produktionsanlage produktionsanlage;
        public Mitarbeiter mitarbeiter;
        public MitarbeiterInSchicht mitarbeiterInSchicht;
        public SchichtInfo schichtInfo;
        */
    }
}

