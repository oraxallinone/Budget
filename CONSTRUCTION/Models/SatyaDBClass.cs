using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
namespace HotelBill.Models
{
    public class SatyaDBClass
    {
        public static DateTime ISTZone(DateTime dateTimeNow)
        {
            DateTime utcTime = dateTimeNow.ToUniversalTime();
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime istTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
            return istTime;
        }
        public static DateTime? ISTZoneNull(DateTime? dateTimeNow)
        {
            DateTime? utcTime = dateTimeNow?.ToUniversalTime();
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime? istTime = utcTime.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(utcTime.Value, timeZoneInfo) : (DateTime?)null;
            return istTime;
        }


        public static SqlConnection conn()
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            return con;
        }
        public static void execute(string query)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            com.ExecuteNonQuery();
            con.Close();
        }
        public static DataTable returndatatable(string query)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter ada = new SqlDataAdapter(query, con);
            ada.Fill(dt);
            con.Close();
            return dt;
        }
        public static DataSet returndataset(string query)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());
            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(query, con);
            ada.Fill(ds);
            con.Close();
            return ds;
        }

        public static void FillGrid(GridView grd, string query)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());
            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(query, con);
            ada.Fill(ds);
            con.Close();

            grd.DataSource = ds;
            grd.DataBind();
        }
        public static void FillDrop(DropDownList Drp, string query, string name, string code)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());
            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(query, con);
            ada.Fill(ds);
            con.Close();

            Drp.DataSource = ds;
            Drp.DataTextField = name;
            Drp.DataValueField = code;
            Drp.DataBind();
        }
        public static void msg_upd_box(UpdatePanel upd, string script)
        {
            Page pg = new Page();
            ScriptManager.RegisterClientScriptBlock(upd, pg.GetType(), "strscript", script, true);
        }

        public static void insertprocedurestockcoma(string procedurename, string[] param, string[] paramobj)
        {
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());
                con.Open();
                //string[] paramarr = param.Split(',');
                //string[] paramobjarr = paramobj.Split(',');
                SqlCommand com = new SqlCommand(procedurename, con);
                com.CommandType = CommandType.StoredProcedure;
                //for (int i = 0; i < paramarr.Length; i++)
                //{
                //    com.Parameters.AddWithValue(paramarr[i], paramobjarr[i]);
                //}
                for (int i = 0; i < param.Length; i++)
                {
                    com.Parameters.AddWithValue(param[i], paramobj[i]);
                }
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                string str = ex.ToString();
                // throw;
            }

        }

        public static DataTable SPReturnDataTable1(string procedurename, string[] param, string[] paramobj)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());
            con.Open();
            // string[] paramarr = param.Split(',');
            //string[] paramobjarr = paramobj.Split(',');


            DataTable dt = new DataTable();
            SqlDataAdapter ada = new SqlDataAdapter(procedurename, con);
            ada.SelectCommand.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < param.Length; i++)
            {
                ada.SelectCommand.Parameters.AddWithValue(param[i], paramobj[i].Trim());
            }
            try
            {
                ada.Fill(dt);
            }
            catch (Exception)
            {

                // throw;
            }

            con.Close();
            return dt;
        }


        public static void insertprocedure(string procedurename, string param, string paramobj)
        {
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());
                con.Open();
                string[] paramarr = param.Split(',');
                string[] paramobjarr = paramobj.Split(',');
                SqlCommand com = new SqlCommand(procedurename, con);
                com.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < paramarr.Length; i++)
                {
                    com.Parameters.AddWithValue(paramarr[i], paramobjarr[i]);
                }
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                string str = ex.ToString();
                // throw;
            }

        }

        public static DataSet SPReturnDataSet(string procedurename, string param, string paramobj)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());
            con.Open();
            string[] paramarr = param.Split(',');
            string[] paramobjarr = paramobj.Split(',');
            DataSet ds = new DataSet();
            SqlDataAdapter ada = new SqlDataAdapter(procedurename, con);
            ada.SelectCommand.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < paramarr.Length; i++)
            {
                ada.SelectCommand.Parameters.AddWithValue(paramarr[i], paramobjarr[i]);
            }
            ada.Fill(ds);
            con.Close();
            return ds;
        }

        //used
        public static DataTable SPReturnDataTable(string procedurename, string param, string paramobj)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString());
            con.Open();
            string[] paramarr = param.Split(',');
            string[] paramobjarr = paramobj.Split(',');
            DataTable dt = new DataTable();
            SqlDataAdapter ada = new SqlDataAdapter(procedurename, con);
            ada.SelectCommand.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < paramarr.Length; i++)
            {
                ada.SelectCommand.Parameters.AddWithValue(paramarr[i], paramobjarr[i].Trim());
            }
            try
            {
                ada.Fill(dt);
            }
            catch (Exception)
            {

                // throw;
            }

            con.Close();
            return dt;
        }



        public static string ContDataTabloJSON(DataTable dt)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return serializer.Serialize(rows);
        }
    }
}