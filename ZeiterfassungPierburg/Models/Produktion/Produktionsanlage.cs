using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Data;
using Dapper.Contrib.Extensions;


namespace ZeiterfassungPierburg.Models
{
    [Table("Produktionsanlage")]
    public class Produktionsanlage : BasicModelObject
    {
        // Arbeitsplatznummer
        public int APNr
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
        // Kostenstelle
        public int Kostenstelle
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
        // SAP-Arbeitsplatz

        public int SAPAPNr
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
        public string Bezeichner
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        // Ist die Anlage ein Band (true) oder eine Maschine (false)?
        [Display(Name = "Ist eine Maschine")]
        public bool IstEineMaschine
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        [Display(Name = "ist Aktiv")]
        public bool IstAktiv
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }
    }   
}