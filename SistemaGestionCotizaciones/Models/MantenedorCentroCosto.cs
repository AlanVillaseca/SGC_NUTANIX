using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Claims;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class MantenedorCentroCosto
    {
        public string dbConn;
        public List<Dictionary<object, string>> dtPaisJson { get; set; }
        public List<Dictionary<object, string>> dtCentroCostoJson { get; set; }
        public MantenedorCentroCosto()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }
        public Boolean cargaPaises(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_PAIS_01", out dt,out codError, out sErrors, GetUserName());
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

            this.dtPaisJson = rows;
            return true;
        }
        public Boolean cargaCentroCosto(string pagina, out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError="";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_CENTRO_COSTO_00", out dt,out codError, out sErrors, pagina, "");
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

            this.dtCentroCostoJson = rows;
            return true;
        }
        public Boolean GuardarCentroCosto(string idpais, string gerencia, string facturable, string codigo, out string sError)
        {
            sError = "";
            Boolean res;


            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_CENTRO_COSTO_00", out sError, idpais, gerencia, facturable, codigo, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());

            if (!res)
            {
                return false;
            }

            return true;
        }
        public Boolean EliminarCentroCosto(string idcentrocosto, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_CENTRO_COSTO_00", out sError, idcentrocosto, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());

            if (!res)
            {
                return false;
            }

            return true;
        }
        public Boolean ModificaCentroCosto(string idcentrocosto, string codigo, string tipo, string firma, out string sError)
        {
            sError = "";
            Boolean res;
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_CENTRO_COSTO_01", out sError, idcentrocosto, codigo, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last(), tipo, firma);
            if (!res)
            {
                return false;
            }
            return true;
        }
        public DataTable TablaCentroCosto(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_CENTRO_COSTO_02", out dt,out codError, out sError);

            return dt;
        }
    }
}