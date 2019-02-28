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
    public class ReportCostos
    {

        public string dbConn;
        public List<Dictionary<object, string>> dtElementosJson { get; set; }
        public ReportCostos()
        {
        
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }

        public byte[] getReportCostosDesglosados()
        {

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.LocalReport.ReportPath = "Content/Reportes/ReportCostos.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

            SqlAccess comCotiz = new SqlAccess(dbConn);
            System.Data.DataTable dt = new DataTable();
            String msgError;
            string codError = "";
            comCotiz.Consultar("USP_SEL_REPORTE_COSTOS_DESGLOSADOS_00", out dt,out codError, out msgError);
            /*
            ReportParameter[] param = new ReportParameter[1];
            param[0] = new ReportParameter("elemento1", dt2.Rows[0].ItemArray.GetValue(2).ToString(), false);
             
            reportViewer.LocalReport.SetParameters(param);
            */
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SEL_REPORTE_COSTOS_DESGLOSADOS_00", dt));
            //cargaElementos

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            String filenameExtension;
            reportViewer.LocalReport.Refresh();

            byte[] pdf = reportViewer.LocalReport.Render(
                "EXCEL", null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);

            return pdf;
            /*
            MemoryStream ms = new MemoryStream();
            ms.Write(pdf, 0, pdf.Length);
            ms.Position = 0;
            if (tip_docto.Equals("PDF"))
                return new FileStreamResult(ms, "application/pdf");
            else
                return new FileStreamResult(ms, "application/vnd.ms-excel");
            */

        }


        public byte[] getReportCostos(
                                                       String id        ,
                                                       String elemento1 ,
                                                       String tip_docto
            
                                                        )
        {
            
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.LocalReport.ReportPath = "Content/Reportes/ReportCostos.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

            SqlAccess comCotiz = new SqlAccess(dbConn);
            System.Data.DataTable dt = new DataTable();
            String msgError;
            string codError = "";
            comCotiz.Consultar("USP_SEL_REPORTE_COSTOS_00", out dt, out codError, out msgError, elemento1);
            /*
            ReportParameter[] param = new ReportParameter[1];
            param[0] = new ReportParameter("elemento1", dt2.Rows[0].ItemArray.GetValue(2).ToString(), false);
             
            reportViewer.LocalReport.SetParameters(param);
            */
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SEL_REPORTE_COSTOS_00", dt));
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

            return pdf;
            /*
            MemoryStream ms = new MemoryStream();
            ms.Write(pdf, 0, pdf.Length);
            ms.Position = 0;
            if (tip_docto.Equals("PDF"))
                return new FileStreamResult(ms, "application/pdf");
            else
                return new FileStreamResult(ms, "application/vnd.ms-excel");
            */

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
       
        public Boolean cargaElementos(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SUBCOMPONENTE_PADRES_00", out dt,out codError, out sErrors);
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

            this.dtElementosJson = rows;
            return true;
        }
    }
}