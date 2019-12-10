using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ZeiterfassungPierburg.Data
{
    public class DataMapper<T> where T: BasicModelObject
    {
        private string defaultTableName;
        private Dictionary<string, string> propertyFieldMappings;

        public DataMapper(string tableName)
        {
            if (String.IsNullOrEmpty(tableName)) throw new ArgumentNullException("tableName");

            defaultTableName = tableName;
            propertyFieldMappings = new Dictionary<string, string>();
            RegisterStandardMappings();
        }

        protected void RegisterStandardMappings()
        {
            Type t = typeof(T);
            if (t.IsDefined(typeof(NoStandardMappingAttribute))) return;

            foreach (PropertyInfo pi in t.GetProperties())
            {
                if (!t.IsDefined(typeof(NoStandardMappingAttribute)))
                {
                    propertyFieldMappings[pi.Name] = pi.Name.ToLower();
                }
            }
        }
        public void RegisterPropertyMapping(string propertyName, string columnName, string tableName = "")
        {
            propertyFieldMappings[propertyName.ToLower()] = tableName == "" ? tableName + "|" + columnName : columnName;
        }

        /* mapping functions */
        protected Dictionary<string,string> GetColumnValuePairs(T model)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (string property in propertyFieldMappings.Keys)
            {
                dic.Add(propertyFieldMappings[property],
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
            return value.ToString();
        }
    }
}