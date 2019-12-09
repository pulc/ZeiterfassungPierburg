using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZeiterfassungPierburg.Models
{
    public class Mitarbeiter
    {
        public int ID { get; set; }
        public int Kostenstelle { get; set; }
        public int Personalnummer { get; set; }
        public string Nachname { get; set; }
        public string Vorname { get; set; }
        public int Abrechnungskreis { get; set; }
        public int Mitarbeiterkreis { get; set; }
        public string Beschäftigungsart { get; set; }
    }
}