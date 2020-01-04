using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ZeiterfassungPierburg.Data
{
    public interface IDataMapper
    {
        string GetSelectSqlString();
        string GetInsertSqlString(object obj);
        string GetUpdateSqlString(object obj);
        string GetDeleteSqlString(int id);
        IEnumerable ReadItems(IDataReader reader);
    }
    public class DataMapper<T> : IDataMapper where T: BasicModelObject, new()
    {
        private readonly string defaultTableName;
        internal readonly Dictionary<string, PropertyMappingInfo> propertyMappingInfos;
        public DataMapper(string tableName)
        {
            if (String.IsNullOrEmpty(tableName)) throw new ArgumentNullException("tableName");

            defaultTableName = tableName;
            propertyMappingInfos = new Dictionary<string, PropertyMappingInfo>();
            RegisterStandardMappings();
        }

        protected Func<object, string> GetPropertyToStringFunction(PropertyInfo property)
        {
            IPropertyStringFunctionAttribute attribute = null;
            var found =
            property.GetCustomAttributes()
                    .Where(a => a is IPropertyStringFunctionAttribute);
            // always select only the first attribute, as only one should be set
            // (multiple attributes with this interface is not valid)
            if (found.Count() > 0)
                attribute = (IPropertyStringFunctionAttribute)found.First();

            // overwrite default ToStringFunction (object.ToString)
            if (attribute != null)
            {
                return attribute.ToStringFunction;
            }
            else
            // represent enum selection as the underlying int value
            if (property.PropertyType.IsEnum)
            {
                return (o) => { return ((int)o).ToString(); };
            }
            else
            // strings need to be quoted
            if (property.PropertyType == typeof(string) || property.PropertyType == typeof(bool))
            {
                return PropertyMappingInfo.QuotedStringFunction;
            }
            else
            {
                return PropertyMappingInfo.DefaultToStringFunction;
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
                // check if NoStandardMapping is set for individual property
                if (!t.IsDefined(typeof(NoStandardMappingAttribute)))
                {
                    propertyMappingInfos[pi.Name] = new PropertyMappingInfo(
                                        pi.Name,
                                        defaultTableName,
                                        pi.Name.ToLower(),
                                        pi.PropertyType,
                                        GetPropertyToStringFunction(pi)
                                        );
                }
            }
        }

        /* mapping functions */
        protected Dictionary<string,string> GetColumnValuePairs(T model)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (var pmi in propertyMappingInfos.Values)
            {
                dic.Add(pmi.ColumnName,
                        pmi.ToStringFunction.Invoke(model.GetValue(pmi.PropertyName)));
            }
            return dic;
        }
        public string GetInsertSqlString(object mdl)
        {
            string sql = "INSERT INTO {0} ({1}) VALUES ({2}); SELECT @@IDENTITY AS 'Identity';";

            if (mdl.GetType() != typeof(T))
                throw new InvalidCastException("Wrong argument type");

            T model = (T)mdl;

            Dictionary<string, string> columnValuePairs = GetColumnValuePairs(model);
            // no id value allowed (setting an identity column would fail
            columnValuePairs.Remove("id");

            string columnsString = String.Join(", ", columnValuePairs.Keys);
            string valuesString = String.Join(", ", columnValuePairs.Values);

            return String.Format(sql, defaultTableName, columnsString, valuesString);
        }

        public string GetUpdateSqlString(object mdl)
        {
            string sql = "UPDATE {0} SET {1} WHERE ID = {2}";

            if (mdl.GetType() != typeof(T))
                throw new InvalidCastException("Wrong argument type");

            T model = (T)mdl;

            Dictionary<string, string> columnValuePairs = GetColumnValuePairs(model);
            // no id value allowed (setting an identity column would fail)
            columnValuePairs.Remove("id");

            string valuesString = "";
            foreach (KeyValuePair<string, string> entry in columnValuePairs)
            {
                valuesString = valuesString + entry.Key + "=" + entry.Value + ",";
            }
            return String.Format(sql, defaultTableName, valuesString.Substring(0, valuesString.Length - 1), model.ID.ToString());
        }
        public string GetDeleteSqlString(int id)
        {
            string sql = "DELETE FROM {0} WHERE ID = {1}";
            return String.Format(sql, defaultTableName, id);
        }

        // IDataMapper interface
        public string GetSelectSqlString()
        {
            string sql = "SELECT {0} FROM {1}";
            return String.Format(sql, String.Join(", ", propertyMappingInfos.Values.Select(v=>v.ColumnName)), defaultTableName);
        }
        public IEnumerable ReadItems(IDataReader reader)
        {
            List<T> results = new List<T>();
            while (reader.Read())
            {
                T result = new T();
                foreach (PropertyMappingInfo pmi in propertyMappingInfos.Values)
                {
                    int index = reader.GetOrdinal(pmi.ColumnName);
                    result.SetValue(reader.GetValue(index), pmi.PropertyName.ToLower());
                }
                results.Add(result);
            }
            return results.ToArray();
        }
    }
}