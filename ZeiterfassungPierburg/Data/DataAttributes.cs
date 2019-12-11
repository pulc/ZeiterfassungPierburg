using System;
using System.Collections.Generic;
using System.Globalization;
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

    public interface IPropertyStringFunctionAttribute
    {
        Func<object, string> ToStringFunction { get; }
    }

    public class DateTimeAttribute: Attribute, IPropertyStringFunctionAttribute
    {
        public enum DateTimeUsage { DateTime, Date, Time };
        public DateTimeUsage Usage { get; set; }
        public Func<object,string> ToStringFunction { get => GetStringFunction(); }

        private Func<object, string> GetStringFunction()
        {
            if (Usage == DateTimeUsage.Date)
                return (object o) =>
                { return ((DateTime)o).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); };
            if (Usage == DateTimeUsage.Time)
                return (object o) =>
                { return ((DateTime)o).ToString("HH:mm:ss", CultureInfo.InvariantCulture); };
            return
                (object o) =>
                {
                   return ((DateTime)o).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                };
        }

        public DateTimeAttribute(DateTimeUsage usage)
        {
            Usage = usage;
        }
    }
}