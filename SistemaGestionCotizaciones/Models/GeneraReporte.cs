using Microsoft.Reporting.WebForms;
using SistemaGestionCotizaciones.Content.Reportes;
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
    public class GeneraReporte
    {
        public string dbConn;

        public List<Dictionary<object, string>> dtPaisJson { get; set; }

        public List<Dictionary<object, string>> dtNegocioJson { get; set; }

        public List<Dictionary<object, string>> dtSerNegJson { get; set; }
        public GeneraReporte()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }
        //public byte[] getReport(String desde, String hasta, String pais, String negocio, String serneg, String tip_docto)
        //{
        //    CotizadorDAL.CotizadorDAL comCotiz = new CotizadorDAL.CotizadorDAL(dbConn);
        //    System.Data.DataTable dt = new DataTable();
        //    String msgError;
        //    comCotiz.Consultar("USP_SEL_FACTURACION", out dt, out msgError, desde, hasta, pais, negocio, serneg);
        //    ReportViewer reportViewer = new ReportViewer();
        //    reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        //    reportViewer.LocalReport.ReportPath = "Content/Reportes/ReportFacturacion.rdlc";
        //    if (pais.Equals("-1")) pais = "Todos";
        //    if (negocio.Equals("-1")) negocio = "Todos";
        //    if (serneg.Equals("-1")) serneg = "Todos";
        //    ReportParameter[] param = new ReportParameter[5];
        //    param[0] = new ReportParameter("pais", "Todos", false);
        //    param[1] = new ReportParameter("negocio", negocio, false);
        //    param[2] = new ReportParameter("serneg", serneg, false);
        //    param[3] = new ReportParameter("desde", desde, false);
        //    param[4] = new ReportParameter("hasta", hasta, false);

        //    reportViewer.LocalReport.SetParameters(param);
        //    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataFacturacion2", dt));
        //    Warning[] warnings;
        //    string[] streamids;
        //    string mimeType;
        //    string encoding;
        //    String filenameExtension;
        //    reportViewer.LocalReport.Refresh();

        //    byte[] pdf = reportViewer.LocalReport.Render(
        //        tip_docto, null, out mimeType, out encoding, out filenameExtension,
        //        out streamids, out warnings);

        //    return pdf;
        //    /*
        //    MemoryStream ms = new MemoryStream();
        //    ms.Write(pdf, 0, pdf.Length);
        //    ms.Position = 0;
        //    if (tip_docto.Equals("PDF"))
        //        return new FileStreamResult(ms, "application/pdf");
        //    else
        //        return new FileStreamResult(ms, "application/vnd.ms-excel");
        //    */
        //}
        public DataTable getReport(out string msgError ,string desde, string hasta, string pais, string negocio, string serneg, string tip_docto)
        {
            DataTable table = new DataTable();
            string codError = "";
            SqlAccess cdal = new SqlAccess(dbConn);

            cdal.Consultar("USP_SEL_FACTURACION", out table,out codError, out msgError, desde, hasta, pais, negocio, serneg);

            return table;
        }

        public DataTable getReportCostoDesglose(out string msgError)
        {
            DataTable table = new DataTable();
            string codError = "";
            SqlAccess cdal = new SqlAccess(dbConn);

            cdal.Consultar("USP_SEL_REPORTE_COSTOS_DESGLOSADOS_00", out table,out codError, out msgError);

            return table;
        }
        public Boolean cargaPaises(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_PAIS_02", out dt,out codError, out sErrors, GetUserName());
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
        public Boolean cargaNegocioPais(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_NEGOCIO_PAIS_02", out dt,out codError, out sErrors, GetUserName());
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

        public Boolean cargaServicioNegocio(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SERVICIO_NEGOCIO_00", out dt,out codError, out sErrors);
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

            this.dtSerNegJson = rows;
            return true;
        }
    }
}