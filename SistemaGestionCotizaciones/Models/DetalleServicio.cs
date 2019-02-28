using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class DetalleServicio
    {
        public string dbConn;
        public List<Dictionary<object, string>> negociosJson { get; set; }
        public List<Dictionary<object, string>> serviciosnegociosJson { get; set; }
        public List<Dictionary<object, string>> infraestructuraJson { get; set; }
        public List<Dictionary<object, string>> detalleinfraestructuraJson { get; set; }
        public List<Dictionary<object, string>> ReporteJson { get; set; }
        public List<Dictionary<object, string>> CabeceraReporteJson { get; set; }
        public List<Dictionary<object, string>> cargaVariablesJson { get; set; }

        public DetalleServicio()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }

        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }

        public bool CargaNegocios(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_NEGOCIO_03", out dt,out codError, out sError, GetUserName());
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

            this.negociosJson = rows;
            return true;
        }

        public bool CargaServicioNegocios(string pagina, string idnegocio,out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SERVICIO_NEGOCIO_03", out dt,out codError, out sError, pagina, GetUserName(), idnegocio);
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

            this.serviciosnegociosJson = rows;
            return true;
        }

        public bool CargaInfraestructura(string idservicionegocio, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_COMPONENTES_05", out dt,out codError, out sError, idservicionegocio);
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

            this.infraestructuraJson = rows;
            return true;
        }

        public bool CargaDetalleInfraestructura(string pagina, string idservicionegocio, string ambiente, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_COMPONENTES_06", out dt,out codError, out sError, pagina, idservicionegocio, ambiente);
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

            this.detalleinfraestructuraJson = rows;
            return true;
        }

        public bool CargaReporteComponente(string idcomponente, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_REPORTE_COT_05", out dt,out codError, out sError, idcomponente);
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

            this.ReporteJson = rows;
            return true;
        }

        public bool CargaCabeceraReporteComponente(string idcomponente, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_COMPONENTE_00", out dt,out codError, out sError, idcomponente);
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

            this.CabeceraReporteJson = rows;
            return true;
        }
        public String CargaNombreServicioNegocio(string idservicionegocio, out string sError)
        {
            sError = "";

            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SERVICIO_NEGOCIO_04", out dt,out codError, out sError, idservicionegocio);
            if (!res)
            {
                return "";
            }

            return dt.Rows[0].ItemArray[0].ToString();
        }
        public String CargaNegocio(string idservicionegocio, out string sError)
        {
            sError = "";

            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_NEGOCIO_04", out dt,out codError, out sError, idservicionegocio);
            if (!res)
            {
                return "";
            }

            return dt.Rows[0].ItemArray[0].ToString();
        }
    }
}