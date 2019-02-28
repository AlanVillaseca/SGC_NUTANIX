using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    
    public class ReporteGenerico
    {
        String dbConn;
        public ReporteGenerico()
        {
            dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }

        public List<Dictionary<object, string>> dtPaisJson { get; set; }
        public List<Dictionary<object, string>> dtNegocioPaisJson { get; set; }
        public List<Dictionary<object, string>> dtSerNegJson { get; set; }
        public List<Dictionary<object, string>> dtEstadosJson { get; set; }
        public List<Dictionary<object, string>> dtElementoJson { get; set; }
        public List<Dictionary<object, string>> dtValoresJson { get; set; }
        public List<Dictionary<object, string>> dtNegociosJson { get; set; }
        public List<Dictionary<object, string>> dtServiciosJson { get; set; }
        public Boolean cargaPaises(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError="";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_PAIS_01", out dt,out codError,  out sErrors);
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
        public Boolean cargaNegociosCascada(string idpais, out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_NEGOCIO_02", out dt,out codError, out sErrors, idpais);
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
            this.dtNegociosJson = rows;
            return true;
        }
        public Boolean cargaServiciosCascada(string idpais, string idnegocio, out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SERVICIO_NEGOCIO_02", out dt,out codError, out sErrors, idnegocio, idpais);
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
            this.dtServiciosJson = rows;
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

        public Boolean cargaEstados(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            String idTabla = "COTIZACION";
            string codError = "";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ESTADO", out dt, out codError, out sErrors, idTabla);
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

            this.dtEstadosJson = rows;
            return true;
        }
        public Boolean cargaElementos(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_CATALOGO_ELEMENTO_00", out dt,out codError, out sErrors);
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
            this.dtElementoJson = rows;
            return true;
        }
        public Boolean cargaValores(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_VALORES_04", out dt,out codError, out sErrors);
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
            this.dtValoresJson = rows;
            return true;
        }
        /*Método que retorna PDF o Excel de reporte generico llamado desde PartialView vpReporteGenerico.cshtml*/
        public byte[] getReportGenerico(string pais, string negocio, string servNegocio, string tieneComponentes,
            string tieneResumen, string tieneDetalle, string tieneCaracteristicas, string tieneHistoria,
            string elemento1, string elemento2, string elemento3, string codElemento1, string codElemento2,
            string codElemento3, string valElemento1, string valElemento2, string valElemento3, string fa_desde,
            string fa_hasta, string ff_desde, string ff_hasta, string estados, string flg_docto)
        {
            //Contenido a mostrar
            //String elemento1            = "Marca Equipo";
            //String elemento2            = "Modelo Equipo";
            //String elemento3            = "Tipo de Servidor";
            /*String tieneComponentes     = "S";
            String tieneResumen         = "S";
            String tieneDetalle         = "S";
            String tieneCaracteristicas = "S";
            String tieneHistoria        = "S";*/
            //Filtross

            String error;
            Boolean res;
            String dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
            DataTable dt_0;
            //String tip_docto = "PDF";
            string codError = "";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_REPORTE_GEN_00", out dt_0,out codError, out error, pais, negocio, servNegocio, estados, fa_desde,fa_hasta,ff_desde,ff_hasta);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = "Content/Reportes/ReportGenerico.rdlc";
            ReportParameter[] param = new ReportParameter[14];
            param[0] = new ReportParameter("elemento1", elemento1, false);
            param[1] = new ReportParameter("elemento2", elemento2, false);
            param[2] = new ReportParameter("elemento3", elemento3, false);
            param[3] = new ReportParameter("tieneComponentes", tieneComponentes, false);
            param[4] = new ReportParameter("tieneResumen", tieneResumen, false);
            param[5] = new ReportParameter("tieneDetalle", tieneDetalle, false);
            param[6] = new ReportParameter("tieneCaracteristicas", tieneCaracteristicas, false);
            param[7] = new ReportParameter("tieneHistoria", tieneHistoria, false);
            param[8] = new ReportParameter("codElemento1", codElemento1, false);
            param[9] = new ReportParameter("codElemento2", codElemento2, false);
            param[10] = new ReportParameter("codElemento3", codElemento3, false);
            param[11] = new ReportParameter("valElemento1", valElemento1, false);
            param[12] = new ReportParameter("valElemento2", valElemento2, false);
            param[13] = new ReportParameter("valElemento3", valElemento3, false);
            reportViewer.LocalReport.SetParameters(param);
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReporteGen_00", dt_0));
            reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReportGenerico00EventHandler);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            String filenameExtension;
            reportViewer.LocalReport.Refresh();

            byte[] pdf = reportViewer.LocalReport.Render(
                flg_docto, null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);

            return pdf;

            //MemoryStream ms = new MemoryStream();
            //ms.Write(pdf, 0, pdf.Length);
            //ms.Position = 0;
            //if (flg_docto.Equals("PDF"))
            //    return new FileStreamResult(ms, "application/pdf");
            //else
            //    return new FileStreamResult(ms, "application/vnd.ms-excel");


        }
        /*Evento gatillado por los sub reportes SubReportGenerico_00.rdlc y SubReportGenerico_01.rdlc*/
        void SubReportGenerico00EventHandler(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath.Equals("SubReportGenerico_00"))
            {
                DataTable dt_1 = new DataTable();
                DataTable dt_2 = new DataTable();
                String idVersion = e.Parameters["VER_IDVERSION"].Values[0];
                String idCotizacion = e.Parameters["COT_IDCOTIZACION"].Values[0];
                String tieneHistoria = e.Parameters["tieneHistoria"].Values[0];
                String elemento1 = e.Parameters["elemento1"].Values[0];
                String elemento2 = e.Parameters["elemento2"].Values[0];
                String elemento3 = e.Parameters["elemento3"].Values[0];
                String codElemento1 = e.Parameters["codElemento1"].Values[0];
                String codElemento2 = e.Parameters["codElemento2"].Values[0];
                String codElemento3 = e.Parameters["codElemento3"].Values[0];
                String valElemento1 = e.Parameters["valElemento1"].Values[0];
                String valElemento2 = e.Parameters["valElemento2"].Values[0];
                String valElemento3 = e.Parameters["valElemento3"].Values[0];

                String dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
                SqlAccess cDAL = new SqlAccess(dbConn);
                string codError = "";
                String error;
                Boolean res;
                res = cDAL.Consultar("USP_SEL_LISTA_COMPONENTES_01",
                                                                out dt_1,
                                                                out codError,
                                                                out error,
                                                                idVersion,
                                                                elemento1,
                                                                elemento2,
                                                                elemento3,
                                                                codElemento1,
                                                                codElemento2,
                                                                codElemento3,
                                                                valElemento1,
                                                                valElemento2,
                                                                valElemento3);
               // if (tieneHistoria.Equals("S"))
                //{
                res = cDAL.Consultar("USP_SEL_REPORTE_COT_02", out dt_2,out codError, out error, idVersion);
                //}
                e.DataSources.Add(new ReportDataSource("ReporteGen_03", dt_1));
                e.DataSources.Add(new ReportDataSource("ReporteDt2", dt_2));
            }
            else //SubReportGenerico_01
            {
                DataTable dt_1 = new DataTable();
                DataTable dt_2 = new DataTable();
                DataTable dt_3 = new DataTable();
                String idcomponente = e.Parameters["idcomponente"].Values[0];
                String tieneDetalle = e.Parameters["tieneDetalle"].Values[0];
                String tieneResumen = e.Parameters["tieneResumen"].Values[0];
                String tieneCaracteristicas = e.Parameters["tieneCaracteristicas"].Values[0];
                String dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
                SqlAccess cDAL = new SqlAccess(dbConn);
                string codError = "";
                String error;
                Boolean res;

                res = cDAL.Consultar("USP_SEL_CALCULAR_COMPONENTE_01", out dt_1,out codError, out error, idcomponente);


                if (tieneCaracteristicas.Equals("S"))
                {
                    //caracteristicas
                    res = cDAL.Consultar("USP_SEL_LISTA_SUBCOMPONENTES_00", out dt_3,out codError, out error, idcomponente);
                }
                if (tieneResumen.Equals("S"))
                {
                    //resumen
                    dt_2 = getResumenes(dt_1);
                }
                e.DataSources.Add(new ReportDataSource("ReporteGen_02", dt_1));
                e.DataSources.Add(new ReportDataSource("ReporteGen_04", dt_2));
                e.DataSources.Add(new ReportDataSource("ReporteGen_01", dt_3));
            }
        }
        void MuestraError(ref object sender,ref SubreportProcessingEventArgs e)
        {
            e.Parameters["ERROR_MSG"].Values[0] = "No Existen Componentes";
        }
        /*Método que agrupa los costos de una componente por categoria, para generar el resumen de costos a partir del detalle.*/
        private DataTable getResumenes(DataTable detalle)
        {
            var result =
                from row in detalle.AsEnumerable()
                group row by row.Field<string>("Categoria") into gpr
                orderby gpr.Key
                select new
                {
                    Categoria = gpr.Key,
                    Costo = gpr.Sum(r => r.Field<double>("Costo"))
                };
            return ToDataTable(result);
        }
        /*Método que transfoma un IEnumerable (obtenido desde linkq) en un datatable (Necesario para Reportes)*/
        private static DataTable ToDataTable<T>(IEnumerable<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

    }
}