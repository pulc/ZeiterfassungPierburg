using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ZeiterfassungPierburg.Models;

namespace Zeiterfassung.Models
{
    public class MitarbeiterDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["mitarbeiterconn"].ToString();
            con = new SqlConnection(constring);
        }
        // **************** ADD NEW MITARBEITER *********************
        public bool AddMitarbeiter(Mitarbeiter mitarbeitermodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewMitarbeiter", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Kostenstelle", mitarbeitermodel.Kostenstelle);
            cmd.Parameters.AddWithValue("@Personalnummer", mitarbeitermodel.Personalnummer);
            cmd.Parameters.AddWithValue("@Nachname", mitarbeitermodel.Nachname);
            cmd.Parameters.AddWithValue("@Vorname", mitarbeitermodel.Vorname);
            cmd.Parameters.AddWithValue("@Abrechnungkreis", mitarbeitermodel.Abrechnungskreis);
            cmd.Parameters.AddWithValue("@Mitarbeiterkreis", mitarbeitermodel.Mitarbeiterkreis);
            cmd.Parameters.AddWithValue("@Beschäftigungsart", mitarbeitermodel.Beschäftigungsart);


            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        // ********** VIEW MITARBEITER DETAILS ********************
        public List<Mitarbeiter> GetMitarbeiter()
        {
            connection();
            List<Mitarbeiter> mitarbeiterlist = new List<Mitarbeiter>();

            SqlCommand cmd = new SqlCommand("GetMitarbeiterDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                mitarbeiterlist.Add(
                    new Mitarbeiter
                    {
                        ID = Convert.ToInt32(dr["Id"]),
                        Kostenstelle = Convert.ToInt32(dr["Kostenstelle"]),
                        Personalnummer = Convert.ToInt32(dr["Personalnummer"]),
                        Nachname = Convert.ToString(dr["Nachname"]),
                        Vorname = Convert.ToString(dr["Vorname"]),
                        Abrechnungskreis = Convert.ToInt32(dr["Abrechnungskreis"]),
                        Mitarbeiterkreis = Convert.ToInt32(dr["Mitarbeiterkreis"]),
                        Beschäftigungsart = Convert.ToString(dr["Beschäftigungsart"]),
                    });
            }
            return mitarbeiterlist;
        }

        // ***************** UPDATE MITARBEITER DETAILS *********************
        public bool UpdateDetails(Mitarbeiter mitarbeitermodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateMitarbeiterDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Kostenstelle", mitarbeitermodel.Kostenstelle);
            cmd.Parameters.AddWithValue("@Personalnummer", mitarbeitermodel.Personalnummer);
            cmd.Parameters.AddWithValue("@Nachname", mitarbeitermodel.Nachname);
            cmd.Parameters.AddWithValue("@Vorname", mitarbeitermodel.Vorname);
            cmd.Parameters.AddWithValue("@Abrechnungkreis", mitarbeitermodel.Abrechnungskreis);
            cmd.Parameters.AddWithValue("@Mitarbeiterkreis", mitarbeitermodel.Mitarbeiterkreis);
            cmd.Parameters.AddWithValue("@Beschäftigungsart", mitarbeitermodel.Beschäftigungsart);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        // ********************** DELETE MITARBEITER DETAILS *******************
        public bool DeleteMitarbeiter(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteMitarbeiter", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MitarbeiterId", id);

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