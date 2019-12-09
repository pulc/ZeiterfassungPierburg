﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeiterfassungPierburg.Models
{
    public class SchichtInfo
    {
        public int ID { get; set; }
        public enum Schichtart { Früh, Spät }
        public Schichtart Art { get; set; }
        public DateTime Datum { get; set; }
    }
}