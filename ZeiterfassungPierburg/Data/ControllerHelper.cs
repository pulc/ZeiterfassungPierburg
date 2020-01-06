using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZeiterfassungPierburg.Data
{
    public static class ControllerHelper
    {

        public static IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            // Create an empty list to hold result of the operation
            var selectList = new List<SelectListItem>();
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }

            return selectList;
        }
        public static IEnumerable<string> SelectColumn(String table, String column, String orderby)
        {

            List<String> values = new List<String>();
            String cmmd = @"SELECT " + column + @"

  FROM [" + table + @"]
order by " + orderby;

            SqlConnection cnn;
            cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            cnn.Open();
            SqlCommand command;
            SqlDataReader dataReader;

            command = new SqlCommand(cmmd, cnn);
            dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                values.Add(SafeGetString(dataReader, 0));
            }
            dataReader.Close();
            cnn.Close();
            return values;
        }

        public static string SafeGetString(this SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }
    }
}