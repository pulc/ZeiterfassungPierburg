using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Data;

namespace ZeiterfassungPierburg.Models
{
    public class SchichtInfo : BasicModelObject
    {
        public SchichtInfo()
        {
            FillValuesDictionaryWithDefaultValues();
        }
        public enum Schichtart { Früh = 1, Spät = 2, Nacht = 3 }

        [DateTime(DateTimeAttribute.DateTimeUsage.Date)]
        public DateTime Datum
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }
        public Schichtart Art
        {
            get => GetValue<Schichtart>();
            set => SetValue(value);
        }
    }
}