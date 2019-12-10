using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ZeiterfassungPierburg.Models.Produktion
{
    public class FertigungsteilDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["ZeiterfassungConn"].ToString();
            con = new SqlConnection(constring);
        }

        // **************** ADD NEW STUDENT *********************
        public bool AddFertigungsteil(Fertigungsteil smodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewFertigungsteil", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ZeichenNr", smodel.ZeichenNr);
            cmd.Parameters.AddWithValue("@Bezeichnung", smodel.Bezeichnung);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        // ********** VIEW FERTIGUNGSTEIL DETAILS ********************
        public List<Fertigungsteil> GetFertigungsteil()
        {
            connection();
            List<Fertigungsteil> fertigungsteillist = new List<Fertigungsteil>();

            SqlCommand cmd = new SqlCommand("GetFertigungsteilDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                fertigungsteillist.Add(
                    new Fertigungsteil
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        ZeichenNr = Convert.ToString(dr["ZeichenNr"]),
                        Bezeichnung = Convert.ToString(dr["Bezeichnung"]),
                    });
            }
            return fertigungsteillist;
        }

        // ***************** UPDATE STUDENT DETAILS *********************
        public bool UpdateDetails(Fertigungsteil smodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateFertigungsteilDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TeilID", smodel.ID);
            cmd.Parameters.AddWithValue("@ZeichenNr", smodel.ZeichenNr);
            cmd.Parameters.AddWithValue("@Bezeichnung", smodel.Bezeichnung);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        // ********************** DELETE STUDENT DETAILS *******************
        public bool DeleteFertigungsteil(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteFertigungsteil", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TeilID", id);

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