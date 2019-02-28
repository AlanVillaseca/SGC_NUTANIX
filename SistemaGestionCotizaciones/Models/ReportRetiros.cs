using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class ReportRetiros
    {

        public string dbConn;
        public List<Dictionary<object, string>> dtPaisJson { get; set; }
        public List<Dictionary<object, string>> dtNegocioPaisJson { get; set; }
        public List<Dictionary<object, string>> dtSerNegJson { get; set; }
        public ReportRetiros()
        {
        
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        public byte[] getReporteRetiros(
                                                       String id        ,
                                                       String pais      ,
                                                       String negocio   ,
                                                       String serneg    ,
                                                       String fec_desde ,
                                                       String fec_hasta ,
                                                       String flg_docto 
            
                                                        )
        {
            String tip_docto = flg_docto;
            
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.LocalReport.ReportPath = "Content/Reportes/ReportRetiros.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);
            System.Data.DataTable dt = new DataTable();
            String msgError;
            comCotiz.Consultar("USP_SEL_REPORTE_RETIRO", out dt,out codError, out msgError, fec_desde, fec_hasta, pais, negocio, serneg);
            
            /*
            ReportParameter[] param = new ReportParameter[6];
            param[0] = new ReportParameter("elemento1", dt2.Rows[0].ItemArray.GetValue(2).ToString(), false);
            param[1] = new ReportParameter("elemento2", dt2.Rows[1].ItemArray.GetValue(2).ToString(), false);
            param[2] = new ReportParameter("elemento3", dt2.Rows[2].ItemArray.GetValue(2).ToString(), false);
            param[3] = new ReportParameter("elemento4", dt2.Rows[3].ItemArray.GetValue(2).ToString(), false);
            param[4] = new ReportParameter("elemento5", dt2.Rows[4].ItemArray.GetValue(2).ToString(), false);
            param[5] = new ReportParameter("elemento6", dt2.Rows[5].ItemArray.GetValue(2).ToString(), false);
            reportViewer.LocalReport.SetParameters(param);
            */
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SEL_REPORTE_RETIRO", dt));
            //cargaElementos
             
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            String filenameExtension;
            reportViewer.LocalReport.Refresh();

            byte[] pdf = reportViewer.LocalReport.Render(
                tip_docto, null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);
            /*
            MemoryStream ms = new MemoryStream();
            ms.Write(pdf, 0, pdf.Length);
            ms.Position = 0;
            if (tip_docto.Equals("PDF"))
                return new FileStreamResult(ms, "application/pdf");
            else
                return new FileStreamResult(ms, "application/vnd.ms-excel");
            */
            return pdf;

        }
        private DataTable SortTable(DataTable dt,   String elemento1 ,String elemento2 ,String elemento3 ,String orden1 ,String orden2 ,String orden3  )
        {
            DataView dv = dt.DefaultView;
            String columName1 = dt.Columns[6 + int.Parse(elemento1)].ColumnName;
            String columName2 = dt.Columns[6 + int.Parse(elemento2)].ColumnName;
            String columName3 = dt.Columns[6 + int.Parse(elemento3)].ColumnName;
            String orderString = "";

            if(orden1.Equals("1"))
            {
                orderString = columName1 + " asc";
            }else if(orden1.Equals("2")){
                orderString = columName1 + " desc";
            }

            if (orden2.Equals("1"))
            {
                if (!orderString.Equals("")) orderString = orderString + ", ";
                orderString =orderString+" "+ columName2 + " asc";
            }
            else if (orden2.Equals("2"))
            {
                if (!orderString.Equals("")) orderString = orderString + ", ";
                orderString = orderString + " " + columName2 + " desc";
            }

            if (orden3.Equals("1"))
            {
                if (!orderString.Equals("")) orderString = orderString + ", ";
                orderString = orderString + " " + columName2 + " asc";
            }
            else if (orden3.Equals("2"))
            {
                if (!orderString.Equals("")) orderString = orderString + ", ";
                orderString = orderString + " " + columName3 + " desc";
            }

            dv.Sort = orderString;

            return dv.ToTable();
        }
        public Boolean cargaPaises(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_PAIS_01", out dt,out codError, out sErrors);
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
        public Boolean cargaNegocioPais(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_NEGOCIO_PAIS_01", out dt,out codError, out sErrors);
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

        public Boolean cargaServicioNegocio(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError="";

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