using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Data;
using Dapper.Contrib.Extensions;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ZeiterfassungPierburg.Models.ViewModels
{
    public class CreateTeileInProduktionsanlageViewModel
    {
        [Display(Name = "Produktionsanlage")]
        [Required(ErrorMessage = "Du musst noch die Produktionsanlage auswählen.")]
        public int Produktionsanlage;

        [Display(Name = "Fertigungsteil")]
        [Required(ErrorMessage = "Du musst noch den Fertigungsteil auswählen.")]
        public int Fertigungsteil;


        [Display(Name = "Produktionsanlagen und Maschinen")]
        public IEnumerable<SelectListItem> ProduktionsanlageList
        {
            get => SQLServer.Instance.GetDictionary("Produktionsanlage", "Bezeichner", null)
                    .Select(s => new SelectListItem()
                    {
                        Text = s.Value,
                        Value = s.Key.ToString()
                    });
        }

        [Display(Name = "Fertigungsteile")]
        public IEnumerable<SelectListItem> FertigungteilList
        {
            get => SQLServer.Instance.GetDictionary("Fertigungsteil", "Bezeichnung", null)
                .Select(s => new SelectListItem()
                {
                    Text = s.Value,
                    Value = s.Key.ToString()
                });
        }
    }
}