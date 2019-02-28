using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class ElementoComponentes
    {
        public string dbConn;
        public List<Dictionary<object, string>> dtComponentesJson { get; set; }
        public List<Dictionary<object, string>> dtPaisJson { get; set; }
        public List<Dictionary<object, string>> dtNegocioJson { get; set; }
        public List<Dictionary<object, string>> dtServicioNegocioJson { get; set; }
        public List<Dictionary<object, string>> ReporteJson { get; set; }
        public List<Dictionary<object, string>> CabeceraReporteJson { get; set; }
        public ElementoComponentes()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }
        public bool CargaComponentes(out string sError, string pagina, string ordenamiento, string id, string alias, string nombre,
            string fechadesde, string fechahasta, string pais, string negocio, string servicio)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ELEMENTO_COMPONENTE_00", out dt,out codError, out sError, pagina, ordenamiento, alias, nombre,
                fechadesde, fechahasta, pais, negocio, servicio, GetUserName(), id);
            
            if (!res)
            {
                return false;
            }



            //StringBuilder sb = new StringBuilder();

            //IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
            //                                  Select(column => column.ColumnName);
            //sb.AppendLine(string.Join(";", columnNames));

            //foreach (DataRow row2 in dt.Rows)
            //{
            //    IEnumerable<string> fields = row2.ItemArray.Select(field => field.ToString().Trim());

            //    sb.AppendLine(string.Join(";", fields));
            //}
            //string ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);


            //try
            //{
            //    System.IO.File.Delete(ruta + "\\ListaCotizacionFacturada.csv");
            //    System.IO.File.WriteAllText(ruta + "\\ListaCotizacionFacturada.csv", sb.ToString());
            //}
            //catch {}



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

            this.dtComponentesJson = rows;
            return true;
        }
        public bool CargaPais(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_PAIS_01", out dt,out codError, out sError, GetUserName());

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
        public bool CargaNegocio(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_NEGOCIO_01", out dt,out codError, out sError);

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
        public bool CargaServicioNegocio(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SERVICIO_NEGOCIO_00", out dt,out codError, out sError);

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

            this.dtServicioNegocioJson = rows;
            return true;
        }
        public bool CargaReporteComponente(string idversion, string idcomponente, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_REPORTE_COT_04", out dt,out codError, out sError, idversion, idcomponente);
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
        public DataTable TablaComponentes(string idcomponente, string nombrecomponente, string fechamin,
            string fechamax, string pais, string negocio, string servicio, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ELEMENTO_COMPONENTE_00", out dt,out codError, out sError, "-1", "1", nombrecomponente,
                fechamin, fechamax, pais, negocio, servicio, GetUserName(), idcomponente);

            return dt;
        }

        public DataTable TablaComponentes2( out string sError)
        {
            sError = "";
            Boolean res;
            string codError = "";
            DataTable dt = null;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ELEMENTO_COMPONENTE_01", out dt,out codError, out sError, GetUserName());

            return dt;
        }
        
    }
}