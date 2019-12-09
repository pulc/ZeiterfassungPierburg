using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeiterfassungPierburg.Models
{
    public class Produktionsanlage
    {
        public int ID { get; set; }
        public string Bezeichner { get; set; }
        // Arbeitsplatznummer
        public int APNr { get; set; }
        // Kostenstelle
        public int Kostenstelle { get; set; }
        // SAP-Arbeitsplatz
        public int SAPAPNr { get; set; }
        // Ist die Anlage ein Band (true) oder eine Maschine (false)?
        public bool IstEineMaschine { get; set; }
    }
}