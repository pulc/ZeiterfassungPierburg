    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeiterfassungPierburg.Models
{
    public class MitarbeiterInSchicht
    {
        public int ID { get; set; }
        public int SchichtInfoID { get; set; }
        public int MitarbeiterID { get; set; }
        public int Produktionsanlage { get; set; }
        public int FertigungsteilID { get; set; }
        public int Stück { get; set; }
        // todo: Direkt / Indirekt-Stunden trennen? Was ist SZ?
        public float Stunden { get; set; }
        /* Administrativ */
        public bool IstInSAPEintragen { get; set; }
    }
}