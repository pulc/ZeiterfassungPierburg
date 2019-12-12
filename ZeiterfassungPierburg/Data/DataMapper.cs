using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ZeiterfassungPierburg.Data
{
    public interface IDataMapper
    {
        string GetItemsSQLString();
    }
    public class DataMapper<T> : IDataMapper where T: BasicModelObject
    {
        private readonly string defaultTableName;
        internal readonly Dictionary<string, string> propertyColumnMappings;
        internal readonly Dictionary<string, Func<object, string>> propertyToStringFunctions;
        public DataMapper(string tableName)
        {
            if (String.IsNullOrEmpty(tableName)) throw new ArgumentNullException("tableName");

            defaultTableName = tableName;
            propertyColumnMappings = new Dictionary<string, string>();
            propertyToStringFunctions = new Dictionary<string, Func<object, string>>();
            RegisterStandardMappings();
            RegisterToStringFunctions();
        }

        protected void RegisterToStringFunctions()
        {
            Type attType = typeof(IPropertyStringFunctionAttribute);
            // find all properties with an attribute that defines
            // a custom ToString function for its value
            foreach (var property in typeof(T).GetProperties())
            {
                IPropertyStringFunctionAttribute attribute = null;
                var found =
                property.GetCustomAttributes()
                        .Where(a => a is IPropertyStringFunctionAttribute);
                // always select only the first attribute, as only one should be set
                // (multiple attributes with this interface is not valid)
                if (found.Count() > 0)
                    attribute = (IPropertyStringFunctionAttribute)found.First();

                // add to dictionary
                if (attribute != null)
                    propertyToStringFunctions.Add(property.Name,
                        attribute.ToStringFunction);
            }
        }

        /*
* Checks all properties for DataAttributes and applies standard mapping
* to column names, if not prohibited by NoStandardMapping attribute
* */
        protected void RegisterStandardMappings()
        {
            Type t = typeof(T);
            // if NoStandardMapping set on class, disable for all properties
            bool registerStandardMapping = !t.IsDefined(typeof(NoStandardMappingAttribute));

            foreach (PropertyInfo pi in t.GetProperties())
            {
                if (registerStandardMapping)
                {
                    // check if NoStandardMapping is set for individual property
                    if (!t.IsDefined(typeof(NoStandardMappingAttribute)))
                    {
                        propertyColumnMappings[pi.Name] = pi.Name.ToLower();
                    }
                }
            }
        }
        public void RegisterPropertyMapping(string propertyName, string columnName, string tableName = "")
        {
            propertyColumnMappings[propertyName.ToLower()] = tableName == "" ? tableName + "|" + columnName : columnName;
        }

        /* mapping functions */
        protected Dictionary<string,string> GetColumnValuePairs(T model)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (string property in propertyColumnMappings.Keys)
            {
                Console.WriteLine(model.ToString());
                Console.WriteLine("GetValue : " + property + " , " + model.GetValue(property));
                dic.Add(propertyColumnMappings[property],
                        MakeSQLStringValue(model.GetValue(property)));
            }
            return dic;
        }
        public string InsertCommand(T model)
        {
            string sql = "INSERT INTO {0} ({1}) VALUES ({2})";

            Dictionary<string, string> columnValuePairs = GetColumnValuePairs(model);

            string columnsString = String.Join(", ", columnValuePairs.Keys);
            string valuesString = String.Join(", ", columnValuePairs.Values);

            return String.Format(sql, defaultTableName, columnsString, valuesString);
        }

        public static string MakeSQLStringValue(object value)
        {
            string s = value.ToString();

            if (value is String)
                return String.Format("'{0}'", (string)value);

            if (value is DateTime)
                return ((DateTime)value).ToString("yyyy-MM-dd ");
            return s;
        }

        // IDataMapper interface
        public string GetItemsSQLString()
        {
            string sql = "SELECT {0} FROM {1}";
            return String.Format(sql, String.Join(", ", propertyColumnMappings.Values), defaultTableName);
            throw new NotImplementedException();
        }
    }
}