using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Data;
using Dapper.Contrib.Extensions;

namespace ZeiterfassungPierburg.Models.Produktion
{
    [Table("TeileInProduktionsanlage")]
    public class TeileInProduktionsanlage : BasicModelObject
    {
        public int ProduktionsanlageID
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int FertigungsteilID
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
    }
}