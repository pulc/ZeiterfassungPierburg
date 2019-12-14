using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ZeiterfassungPierburg.Data
{
    public class BasicModelObject
    {
        public BasicModelObject()
        {
            // on creation of a new object, fill values dictionary with
            // default values
            FillValuesDictionaryWithDefaultValues();
        }

        protected void FillValuesDictionaryWithDefaultValues()
        {
            foreach (PropertyInfo pi in this.GetType().GetProperties())
            {
                if (!values.ContainsKey(pi.Name))
                {
                    // try to invoke parameterless constructor of respective
                    // property Type
                    Type t = pi.PropertyType;
                    object value;
                    if (t == typeof(string))
                    {
                        value = String.Empty;
                    }
                    else

                    if (t.IsPrimitive)
                    {
                        value = Activator.CreateInstance(t);
                    }
                    else
                    if (t.IsEnum)
                    {
                        value = (int)0;
                    }
                    else
                    if (t == typeof(DateTime))
                    {
                        value = DateTime.Now;
                    }
                    else
                    {
                        value = t.GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
                    }
                    values[pi.Name.ToLower()] = value;
                }
            }
        }

        protected Dictionary<string, object> values = new Dictionary<string, object>();
        internal object GetValue([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (String.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("PropertyName is <null>");

            if (!values.TryGetValue(propertyName.ToLower(), out object val))
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
        public int ID
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
        public override string ToString()
        {
            return String.Join(", ", values.Select(v => v.Key + "=" + v.Value));
        }
    }
}