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

        [Display(Name = "Produktionsanlage")]
        public int ProduktionsanlageID
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
        [Display(Name = "Produkt")]
        public int FertigungsteilID
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
    }
}