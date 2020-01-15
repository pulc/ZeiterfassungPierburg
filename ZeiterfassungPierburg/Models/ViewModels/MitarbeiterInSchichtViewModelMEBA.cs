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
        public DateTime Datum { get; set; }

        public int Art{ get; set; }

        public string Anlage { get; set; }

        public string Name { get; set; }

        public int Personalnummer { get; set; }

        public int Kostenstelle { get; set; }

        public int SAPAPNr { get; set; }

        public string ZeichenNr { get; set; }

        public int Stück { get; set; }

        public float DirStunden { get; set; }

        public float InDirStunden { get; set; }

        public string Bemerkung { get; set; }
    }
}

