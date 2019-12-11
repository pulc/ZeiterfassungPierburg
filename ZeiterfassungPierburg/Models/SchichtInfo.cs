﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeiterfassungPierburg.Data;

namespace ZeiterfassungPierburg.Models
{
    public class SchichtInfo : BasicModelObject
    {
        public enum Schichtart { Früh, Spät, Nacht }

        public int MitarbeiterSchichtID
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
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