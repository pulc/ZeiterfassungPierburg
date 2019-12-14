using System;

namespace ZeiterfassungPierburg.Data
{
    public struct PropertyMappingInfo
    {
        public string PropertyName { get; }
        public string TableName { get; }
        public string ColumnName { get; }
        public Type PropertyType { get; }
        public Func<object, string> ToStringFunction { get; internal set; }
        public PropertyMappingInfo(string propertyName, string tableName, string columnName, Type propertyType)
        {
            PropertyName = propertyName;
            TableName = tableName;
            ColumnName = columnName;
            PropertyType = propertyType;
            ToStringFunction = DefaultToStringFunction;
        }
        public PropertyMappingInfo(string propertyName, string tableName, string columnName, Type propertyType, Func<object, string> toStringFunction)
        {
            PropertyName = propertyName;
            TableName = tableName;
            ColumnName = columnName;
            PropertyType = propertyType;
            ToStringFunction = toStringFunction;
        }
        public PropertyMappingInfo ChangeToStringFunction(Func<object, string> function)
        {
            PropertyMappingInfo result = (PropertyMappingInfo)this.MemberwiseClone();
            result.ToStringFunction = function;
            return result;
        }
        public static Func<object, string> DefaultToStringFunction => (object o) => { return o.ToString(); };
        public static Func<object, string> QuotedStringFunction => (object o) => { return String.Format(DefaultToStringFunction.Invoke(o)); };
    }
}