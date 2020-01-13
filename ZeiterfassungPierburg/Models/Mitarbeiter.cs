using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Data;

namespace ZeiterfassungPierburg.Models
{
    public class Mitarbeiter: BasicModelObject
    {
        public Mitarbeiter()
        {
            FillValuesDictionaryWithDefaultValues();
        }

        [Required(ErrorMessage = "Du musst noch die Kostenstelle eingeben.")]
        public int Kostenstelle
        { 
            get => GetValue<int>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Du musst noch die Personalnummber eingeben.")]
        public int Personalnummer
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Du musst noch den Nachnamen eingeben.")]
        public string Nachname
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Du musst noch den Vornamen eingeben.")]
        public string Vorname
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Du musst noch den Abrechnungskreis eingeben.")]
        public int Abrechnungskreis
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Du musst noch den Mitarbeiterkreis eingeben.")]
        public int Mitarbeiterkreis
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Du musst noch die Beschäftigungsart eingeben.")]
        public string Beschäftigungsart
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}
 
