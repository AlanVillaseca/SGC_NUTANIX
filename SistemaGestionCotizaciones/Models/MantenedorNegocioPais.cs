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
    public class MantenedorNegocioPais
    {
        public string dbConn;
        public List<Dictionary<object,string>> dtPaisJson {get; set;}
        public List<Dictionary<object, string>> dtNegocioJson {get; set;}
        public List<Dictionary<object, string>> dtNegocioPaisJson {get; set;}
        public MantenedorNegocioPais()

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
        public Boolean cargaNegocio(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_NEGOCIO_01", out dt,out codError, out sErrors);
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

            this.dtNegocioJson = rows;
            return true;
        }
        public Boolean cargaNegocioPais(string pagina, out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_NEGOCIO_PAIS_00", out dt,out codError, out sErrors, pagina, "idpais");
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

            this.dtNegocioPaisJson = rows;
            return true;
        }

        public Boolean GuardarNegocioPais(string idpais, string idnegocio, string facturable, string codigo, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_NEGOCIO_PAIS_00", out sError, idpais, idnegocio, facturable, codigo, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());

            if (!res)
            {
                return false;
            }

            return true;
        }
        public Boolean EliminarNegocioPais(string idpais, string idnegocio, out string sError)
        {
            sError = "";
            Boolean res;
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_NEGOCIO_PAIS_00", out sError, idpais, idnegocio, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());

            if (!res)
            {
                return false;
            }

            return true;
        }
        public Boolean ModificarNegocioPais(string idpais, string idnegocio, string codigo, string facturable, out string sError)
        {
            sError = "";
            Boolean res;
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_NEGOCIO_PAIS_01", out sError, idpais, idnegocio, codigo, facturable, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());

            if (!res)
            {
                return false;
            }

            return true;
        }
        public DataTable TablaNegocioPais(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_NEGOCIO_PAIS_03", out dt,out codError, out sError);

            return dt;
        }
    }
}