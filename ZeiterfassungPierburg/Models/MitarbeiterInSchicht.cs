using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Data;

namespace ZeiterfassungPierburg.Models
{
    [Table("MitarbeiterInSchicht")]
    public class MitarbeiterInSchicht : BasicModelObject
    {
        public int SchichtInfoID
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int MitarbeiterID
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int Stück
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int FertigungsteilID
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int ProduktionsanlageID
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public float DirStunden
        {
            get => GetValue<float>();
            set => SetValue(value);
        }

        public float InDirStunden
        {
            get => GetValue<float>();
            set => SetValue(value);
        }

        public string Bemerkung
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        /* Administrativ */
        public bool IstInSAPEingetragen
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public DateTime ErstelltAm
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }


    }
}