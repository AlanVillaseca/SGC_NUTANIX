using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Security.Claims;
using System.Data;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class ReporteCotizacionesEstado
    {
        public string dbConn;
        public string cadena { get; set; }
        public List<Dictionary<object, string>> dtEstadosJson { get; set; }
        public List<Dictionary<object, string>> dtImplementadorJson { get; set; }
        public List<Dictionary<object, string>> dtJefeProyectoJson { get; set; }



        public ReporteCotizacionesEstado()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }

        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }
        public Boolean cargaEstados(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ESTADO_02", out dt,out codError, out sErrors);
            if (!res)
            {
                return false;
            }

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

            this.dtEstadosJson = rows;
            return true;
        }
        public Boolean cargaImplementadores(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_IMPLEMENTADORES_01", out dt,out codError, out sErrors, GetUserName());
            if (!res)
            {
                return false;
            }

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

            this.dtImplementadorJson = rows;
            return true;
        }
        public Boolean cargaJefesProyectos(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_JEFEPROYECTOS_01", out dt, out codError, out sErrors, GetUserName());
            if (!res)
            {
                return false;
            }

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

            this.dtJefeProyectoJson = rows;
            return true;
        }

    }
}