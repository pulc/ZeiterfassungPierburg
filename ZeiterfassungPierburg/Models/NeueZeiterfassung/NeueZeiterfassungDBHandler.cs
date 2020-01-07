using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ZeiterfassungPierburg.Models.NeueZeiterfassung
{
    public class NeueZeiterfassungDBHandler
    {
        private SqlConnection con;
        private void connection()
        {
            con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
        public bool AddZeiterfassung(NeueZeiterfassung smodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewZeiterfassung", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Datum", smodel.Datum);
            cmd.Parameters.AddWithValue("@Schicht", smodel.Schicht);
            cmd.Parameters.AddWithValue("@Name", smodel.Name);
            cmd.Parameters.AddWithValue("@Produktionsanlage", smodel.Produktionsanlage);
            cmd.Parameters.AddWithValue("@Fertigungsteil", smodel.Fertigungsteil);
            cmd.Parameters.AddWithValue("@Stückzahl", smodel.Stückzahl);
            cmd.Parameters.AddWithValue("@DirZeit", smodel.DirZeit);
            cmd.Parameters.AddWithValue("@InDirZeit", smodel.InDirZeit);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }
    }
}