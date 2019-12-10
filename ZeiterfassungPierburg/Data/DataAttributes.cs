using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeiterfassungPierburg.Data
{
    [AttributeUsage(AttributeTargets.Class | 
                    AttributeTargets.Field | 
                    AttributeTargets.Property, 
                    AllowMultiple = false)]
    public class NoStandardMappingAttribute : Attribute
    {
    }


}