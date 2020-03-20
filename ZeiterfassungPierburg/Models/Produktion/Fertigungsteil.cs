using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Data;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace ZeiterfassungPierburg.Models
{
    [Table("Fertigungsteil")]
    public class Fertigungsteil : BasicModelObject
    {
        [Display(Name = "ZeichnungsNr")]
        [Required(ErrorMessage = "Du musst noch die Zeichnungsnummer eingeben.")]
        public string ZeichenNr
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Du musst noch die Bezeichnung eingeben.")]
        public string Bezeichnung
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        [Range(0.01, 10000)]
        [Required(ErrorMessage = "Du musst noch die teZEIT eingeben.")]
        public float teZEIT
        {
            get => GetValue<float>();
            set => SetValue(value);
        }
        [Display(Name = "ist Aktiv")]
        public bool IstAktiv
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Du musst noch die Anzahl der Mitarbeiter eingeben.")]
        [Display(Name = "Anzhal MA")]
        public int AnzahlMA
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
    }
}