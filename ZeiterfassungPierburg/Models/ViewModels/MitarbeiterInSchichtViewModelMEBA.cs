using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;

namespace ZeiterfassungPierburg.Models.ViewModel.MitarbeiterInschichtViewModel
{
    public class MitarbeiterInschichtViewModel
    {
        public int ID { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Datum { get; set; }

        [Display(Name="Schicht")]
        public int Art{ get; set; }

        public string Anlage { get; set; }

        public string Name { get; set; }

        [Display(Name = "PNr")]
        public int Personalnummer { get; set; }

        [Display(Name = "KST")]
        public int Kostenstelle { get; set; }

        public int SAPAPNr { get; set; }

        public string ZeichenNr { get; set; }

        public int Stück { get; set; }

        public float DirStunden { get; set; }

        [Display(Name = "SZ")]
        public float InDirStunden { get; set; }

        public string Bemerkung { get; set; }

        [Display(Name = "Akkord")]
        public float Auswertung { get; set; }

    }
}

