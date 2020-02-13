using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Data;

namespace ZeiterfassungPierburg.Models
{
    [Table("AnlagenInSchicht")]
    public class AnlageInSchicht : BasicModelObject
    {
        public AnlageInSchicht()
        {
            FillValuesDictionaryWithDefaultValues();
        }
        public int AnlageInSchichtID  
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
    }
}