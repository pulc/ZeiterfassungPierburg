using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Data;

namespace ZeiterfassungPierburg.Models
{
    public class MEBAViewModel 
    {
        public Fertigungsteil fertigungsteil;
        public Produktionsanlage produktionsanlage;
        public Mitarbeiter mitarbeiter;
        public MitarbeiterInSchicht mitarbeiterInSchicht;
        public SchichtInfo schichtInfo;
    }
}

