using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Data;

namespace ZeiterfassungPierburg.Models
{
    [Table("Schichtinfo")]
    public class SchichtInfo : BasicModelObject
    {
        public SchichtInfo()
        {
            FillValuesDictionaryWithDefaultValues();
        }
        
        public int Art  //1 = Früh, 2 = Spät, 3 = Nacht
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        [DataType(DataType.Date)]
        public DateTime Datum
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }
        /*
        public enum Schichtart { Früh = 1, Spät = 2, Nacht = 3 }

        public Schichtart Art
        {
            get => GetValue<Schichtart>();
            set => SetValue(value);
        }
        */
        
    }
}