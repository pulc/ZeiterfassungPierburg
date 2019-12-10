using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ZeiterfassungPierburg.Models
{
    public class Mitarbeiter
    {
       // [Display(Name = "Id")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Du musst noch die Kostenstelle eingeben.")]
        public int Kostenstelle { get; set; }

        [Required(ErrorMessage = "Du musst noch die Personalnummber eingeben.")]
        public int Personalnummer { get; set; }

        [Required(ErrorMessage = "Du musst noch den Nachnamen eingeben.")]
        public string Nachname { get; set; }

        [Required(ErrorMessage = "Du musst noch den Vornamen eingeben.")]
        public string Vorname { get; set; }

        [Required(ErrorMessage = "Du musst noch den Abrechnungskreis eingeben.")]
        public int Abrechnungskreis { get; set; }

        [Required(ErrorMessage = "Du musst noch den Mitarbeiterkreis eingeben.")]
        public int Mitarbeiterkreis { get; set; }

        [Required(ErrorMessage = "Du musst noch die Beschäftigungsart eingeben.")]
        public string Beschäftigungsart { get; set; }
    }
}
 
