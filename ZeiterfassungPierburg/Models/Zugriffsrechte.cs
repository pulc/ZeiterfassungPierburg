using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeiterfassungPierburg.Data;
using Dapper.Contrib.Extensions;

namespace ZeiterfassungPierburg.Models
{
    [Table("Zugriffsrechte")]
    public class Zugriffsrechte : BasicModelObject
    {
        [Required(ErrorMessage = "Du musst noch den Benutzernamen eingeben.")]
        public string Benutzername
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Du musst noch die Zugriffsebene auswählen.")]
        public int Zugriffsebene
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Du musst noch das Password auswählen.")]
        public string Password
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Computed]
        [Display(Name = "Zugriffsebene")]
        public IEnumerable<SelectListItem> ZugriffsebeneList { get; set; } = new List<SelectListItem>()
        {
        new SelectListItem{ Text="Admin", Value="1"},
        new SelectListItem{ Text="Verwalter", Value="2"},
        };
    }
}