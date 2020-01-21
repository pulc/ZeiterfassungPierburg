using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Data;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace ZeiterfassungPierburg.Models
{
    [Table("Fertigungsteil")]
    public class Fertigungsteil : BasicModelObject
    {
        [Required(ErrorMessage = "Du musst noch die ZeichenNr eingeben.")]
        public string ZeichenNr
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = "Du musst noch die Bezeichnung eingeben.")]
        public string Bezeichnung
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        [Required(ErrorMessage = "Du musst noch die Bezeichnung eingeben.")]
        public float teZEIT
        {
            get => GetValue<float>();
            set => SetValue(value);
        }
    }
}