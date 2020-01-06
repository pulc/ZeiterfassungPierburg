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
        [Required]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }

        [Required(ErrorMessage = "Du musst noch den Namen auswählen.")]
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

        [Display(Name = "Teil")]
        public IEnumerable<SelectListItem> FertigungteilList { get; set; }

        [Required(ErrorMessage = "Du musst noch den Fertigungsteil auswählen.")]
        public string Fertigungsteil { get; set; }

        [Required(ErrorMessage = "Du musst noch die Stückzahl auswählen.")]
        public int Stückzahl { get; set; }

        [RegularExpression(@"^\d+\,\d{0,2}$", ErrorMessage = "Die musst statt einer Komma einen Punkt eingeben und darfst maximal 2 Nachkommanummer eingeben")]
        [Range(0, 9.99)]
        [Required(ErrorMessage = "Du musst noch die Zeit eingeben.")]
        public decimal Zeit { get; set; }

        [Required(ErrorMessage = "Du musst noch die Produktionsanlage auswählen.")]
        public string Produktionsanlage { get; set; }
    }
}

