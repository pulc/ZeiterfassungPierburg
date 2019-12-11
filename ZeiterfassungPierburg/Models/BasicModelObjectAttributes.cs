using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeiterfassungPierburg.Models
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class BasicModelObjectAttributes : Attribute
    {
    }
}