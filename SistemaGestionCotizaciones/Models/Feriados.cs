using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class Feriados
    {
        public string dbConn { get; set; }
        public List<Dictionary<object, string>> dtFeriadosJson { get; set; }
        public Feriados()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        public bool CargaFeriados(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_FERIADOS_00", out dt,out codError, out sError);
            if (!res)
            {
                return false;
            }


            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<object, string>> rows = new List<Dictionary<object, string>>();
            Dictionary<object, string> row;

            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<object, string>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);
            }

            this.dtFeriadosJson = rows;
            return true;
        }
        public bool AgregarFeriado(string fecha, string descripcion, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_FERIADOS_00", out sError, fecha, descripcion, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());

            if (!res)
            {
                return false;
            }

            return true;
        }
        public bool EliminarFeriado(string fecha, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_DEL_FERIADO_00", out sError, fecha);

            if (!res)
            {
                return false;
            }

            return true;
        }
        public DataTable TablaFeriados(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_FERIADOS_01", out dt,out codError, out sError);

            return dt;
        }
    }
}