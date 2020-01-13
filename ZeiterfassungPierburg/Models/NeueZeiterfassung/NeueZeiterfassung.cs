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
        public int Name { get; set; }

        public int Schicht { get; set; }

        [Display(Name = "Produktionsanlage")]
        public IEnumerable<SelectListItem> ProduktionsanlageList
        {
            get => SQLServer.Instance.GetDictionary("Produktionsanlage", "Bezeichner")
                    .Select(s=>new SelectListItem()
                    {
                        Text = s.Value,
                        Value = s.Key.ToString()
                    });
        }
        [Display(Name = "Schicht")]
        public IEnumerable<SelectListItem> SchichtList { get; set; } = new List<SelectListItem>()
        {
        new SelectListItem{ Text="Früh", Value="1"},
        new SelectListItem{ Text="Spät", Value="2"},
        new SelectListItem{ Text="Nacht", Value="3"},
        };

        [Display(Name = "Name")]
        public IEnumerable<SelectListItem> NameList
        {
            get => SQLServer.Instance.GetDictionary("Mitarbeiter", @"Nachname + ', '+ Vorname")
                .Select(s => new SelectListItem()
                {
                    Text = s.Value,
                    Value = s.Key.ToString()
                });
        }

        [Display(Name = "Teil")]
        public IEnumerable<SelectListItem> FertigungteilList
        {
            get => SQLServer.Instance.GetDictionary("Fertigungsteil", "Bezeichnung")
                .Select(s => new SelectListItem()
                {
                    Text = s.Value,
                    Value = s.Key.ToString()
                });
        }

        [Required(ErrorMessage = "Du musst noch den Fertigungsteil auswählen.")]
        public int Fertigungsteil { get; set; }

        [Range(0, 9999)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Stückzahl muss eine ganze Zahl sein.")]
        [Required(ErrorMessage = "Du musst noch die Stückzahl auswählen.")]
        public int Stückzahl { get; set; }

        [Display(Name = "Dir. Stunden")]
        [Range(0, 9.99)]
        [Required(ErrorMessage = "Du musst noch die direkte Zeit eingeben.")]
        public float DirZeit { get; set; }

        [Display(Name = "Indir. Stunden")]
        [Range(0, 9.99)]
        [Required(ErrorMessage = "Du musst noch die indirekte Zeit eingeben.")]
        public float InDirZeit { get; set; }

        [Required(ErrorMessage = "Du musst noch die Produktionsanlage auswählen.")]
        public int Produktionsanlage { get; set; }

    }
}

