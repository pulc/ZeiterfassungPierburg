using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeiterfassungPierburg.Data
{
    public class BasicModelObject
    {
        protected Dictionary<string, object> values = new Dictionary<string, object>();
        internal object GetValue([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            object val = null;
            if (String.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("PropertyName is <null>");
            if (!values.TryGetValue(propertyName, out val))
                throw new ArgumentOutOfRangeException("Property not found");
            return val;
        }
        internal T GetValue<T>([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            return (T)GetValue(propertyName);
        }
        internal void SetValue(object value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (String.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("PropertyName is <null>");
            values[propertyName.ToLower()] = value;
        }
        public int Identifier
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
    }
}