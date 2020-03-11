using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZeiterfassungPierburg.Models.ViewModels
{
    public class TeileInProduktionsanlageViewModel
    {
        public int ID;

        public string Produktionsanlage;

        public string Fertigungsteil;
        [Display(Name="Zeich. Nr")]
        public string ZeichenNr;

    }
}