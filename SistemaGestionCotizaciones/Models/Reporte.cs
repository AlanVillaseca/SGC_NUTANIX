using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class Reporte
    {
        public String dbConn { get; set; }
        public List<Dictionary<object, string>> cargaVariablesJson { get; set; }
        public List<Dictionary<object, string>> ReporteCabeceraJson { get; set; }
        public List<Dictionary<object, string>> ReporteJson { get; set; }
        public List<Dictionary<object, string>> ReporteJson2 { get; set; }


        /*Datos para reporte rdlc, requiere datos como datatable*/
        public DataTable cargaVariablesDt { get; set; }
        public DataTable ReporteCabeceraDt { get; set; }
        public DataTable ReporteDt { get; set; }
        public DataTable ReporteDt2 { get; set; }
        public DataTable TotalServicios { get; set; }
        public Reporte()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }
        public bool CargaReporteCabeceraDt(string idversion, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_REPORTE_COT_03", out dt,out codError, out sError, idversion);
            if (!res)
            {
                return false;
            }


            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
           
            this.ReporteCabeceraDt = dt;
            return true;
        }
        public bool CargaReporteComponenteDt(string idversion, out string sError)
        {
            sError = "";
            Boolean res;
            string codError = "";
            DataTable dt = null;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_REPORTE_COT_00", out dt,out codError, out sError, idversion);
            if (!res)
            {
                return false;
            }
  
            this.ReporteDt =dt;
            return true;
        }
        public bool CargaReporteServicioDt(string idversion, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_REPORTE_COT_02", out dt,out codError, out sError, idversion);
            if (!res)
            {
                return false;
            }
           
            this.ReporteDt2 = dt;
            return true;
        }
        public bool CargaTotalServicios(string idversion, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError="";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_REPORTE_COT_06", out dt,out codError, out sError, idversion);
            if (!res)
            {
                return false;
            }

            this.TotalServicios = dt;
            return true;
        }
        public Boolean cargaVariablesDataTable(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable cargaVariablesSp = null;
            string codError = "";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_CATALOGO_ELEMENTO_00", out cargaVariablesSp,out codError, out sErrors, "2");
            if (!res)
            {
                return false;
            }

            this.cargaVariablesDt = cargaVariablesSp;
            return true;
        }
        /*Metodos Carga en JSON*/
        public bool CargaReporteCabecera(string idversion, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_REPORTE_COT_03", out dt,out codError, out sError, idversion);
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

            this.ReporteCabeceraJson = rows;
            return true;
        }
        public bool CargaReporteComponente(string idversion, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_REPORTE_COT_00", out dt,out codError, out sError, idversion);
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
        public bool CargaReporteServicio(string idversion, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_REPORTE_COT_01", out dt,out codError, out sError, idversion);
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
            this.ReporteJson2 = rows;
            return true;
        }
        public Boolean cargaVariables(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable cargaVariablesSp = null;
            string codError = "";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_CATALOGO_ELEMENTO_00", out cargaVariablesSp,out codError, out sErrors, "2");
            if (!res)
            {
                return false;
            }
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<object, string>> rows = new List<Dictionary<object, string>>();
            Dictionary<object, string> row;
            foreach (DataRow dr in cargaVariablesSp.Rows)
            {
                row = new Dictionary<object, string>();
                foreach (DataColumn col in cargaVariablesSp.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);
            }
            this.cargaVariablesJson = rows;
            return true;
        }
    }
}