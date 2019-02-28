using Microsoft.Reporting.WebForms;
using SistemaGestionCotizaciones.Content.Reportes;
using SistemaGestionCotizaciones.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SistemaGestionCotizaciones.Controllers

{
    public class ReportesController : Controller
    {
        // GET: Reportes
        /*******************************************************************
         * Métodos necesario para reporte genérico de cotizaciones
         * *****************************************************************/

        /*Método controlador de formulario de impresión de reporte genérico*/
        public ActionResult reporteGenerico()
        {
            String errors;
            ReporteGenerico modelReporteGenerico = new ReporteGenerico();
            bool res = modelReporteGenerico.cargaPaises(out errors);
            res = modelReporteGenerico.cargaNegocioPais(out errors);
            res = modelReporteGenerico.cargaServicioNegocio(out errors);
            res = modelReporteGenerico.cargaEstados(out errors);
            res = modelReporteGenerico.cargaElementos(out errors);
            res = modelReporteGenerico.cargaValores(out errors);
            return View(modelReporteGenerico);
        }

        /**************************************************************************************
         * Métodos para reporte de Facturación
         * ************************************************************************************/
        /*Método que retorna PDF o Excel para reporte de facturación*/
		public ActionResult reporteFacturacion()
        {
            String sError = "";

            GeneraReporte fc = new GeneraReporte();
            fc.cargaPaises(out sError);
            fc.cargaNegocioPais(out sError);
            fc.cargaServicioNegocio(out sError);

            ViewBag.Error = sError;

            return View(fc);
        }
        //public String getReport(string id, string desde, string hasta, string pais, string negocio, string serneg,
        //    string flg_docto)
        //{

        //    GeneraReporte fc = new GeneraReporte();
        //    byte[] filestream = fc.getReport(desde, hasta, pais, negocio, serneg, flg_docto);
        //    return Convert.ToBase64String(filestream);

        //}
        public String getReport(string id, string desde, string hasta, string pais, string negocio, string serneg,
            string flg_docto)
        {
            DataTable tabla = new DataTable();
            Models.GeneraReporte ctrlFacturacion = new Models.GeneraReporte();
            string serror = "";

            tabla = ctrlFacturacion.getReport(out serror, desde, hasta, pais, negocio, serneg, flg_docto);



            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);
        }
        public String getReportGenerico(string id, string pais, string negocio, string servNegocio,
            string tieneComponentes , string tieneResumen , string tieneDetalle , string tieneCaracteristicas,
            string tieneHistoria , string elemento1 , string elemento2 , string elemento3 , string codElemento1,
            string codElemento2 , string codElemento3 , string valElemento1 , string valElemento2 , string valElemento3,
            string fa_desde , string fa_hasta , string ff_desde , string ff_hasta , string estados , string flg_docto)
        {
            ReporteGenerico rpg = new ReporteGenerico();
            byte[] filestream = rpg.getReportGenerico(pais, negocio, servNegocio, tieneComponentes,
                tieneResumen, tieneDetalle, tieneCaracteristicas, tieneHistoria, elemento1, elemento2, elemento3,
                codElemento1, codElemento2, codElemento3, valElemento1, valElemento2, valElemento3, fa_desde,
                fa_hasta, ff_desde, ff_hasta, estados, flg_docto);



            return Convert.ToBase64String(filestream);
        }

        /*Controlador de vista parcial*/
        public ActionResult vpReporteFacturacion(String id, String desde, String hasta, String pais, String negocio, String serneg,String flg_docto)
        {
            ViewBag.url = Url.Action("getReport", "reportes", new { id = id, desde = desde, hasta = hasta, pais = pais, negocio = negocio, serneg = serneg ,flg_docto= flg_docto});
            return PartialView();
        }

        public ActionResult vpReporteFacturacionGenerico(String id 
                                                        , String pais
                                                        , String negocio
                                                        , String servNegocio
                                                        , String tieneComponentes    
                                                        , String tieneResumen        
                                                        , String tieneDetalle        
                                                        , String tieneCaracteristicas
                                                        , String tieneHistoria
                                                        , String elemento1
                                                        , String elemento2
                                                        , String elemento3
                                                        , String codElemento1
                                                        , String codElemento2
                                                        , String codElemento3
                                                        , String valElemento1
                                                        , String valElemento2
                                                        , String valElemento3
                                                        , String fa_desde
                                                        , String fa_hasta
                                                        , String ff_desde
                                                        , String ff_hasta
                                                        , String estados
                                                        , String flg_docto)
        {
            ViewBag.url = Url.Action("getReportGenerico", "reportes", new { 
                                                                    id                   = id, 
                                                                    pais = pais, 
                                                                    negocio = negocio, 
                                                                    servNegocio = servNegocio,
                                                                    tieneComponentes = tieneComponentes,
                                                                    tieneResumen         = tieneResumen,        
                                                                    tieneDetalle         = tieneDetalle,        
                                                                    tieneCaracteristicas = tieneCaracteristicas,
                                                                    tieneHistoria        = tieneHistoria,
                                                                    elemento1            = elemento1,
                                                                    elemento2            = elemento2,
                                                                    elemento3            = elemento3,
                                                                    codElemento1         = codElemento1,
                                                                    codElemento2         = codElemento2,
                                                                    codElemento3         = codElemento3,
                                                                    valElemento1         = valElemento1,
                                                                    valElemento2         = valElemento2,
                                                                    valElemento3         = valElemento3,
                                                                    fa_desde             = fa_desde,
                                                                    fa_hasta             = fa_hasta,
                                                                    ff_desde             = ff_desde,
                                                                    ff_hasta             = ff_hasta,
                                                                    estados        = estados,
                                                                    flg_docto = flg_docto });
            return PartialView();
        }
        public ActionResult ReporteComponentes()
        {
            ReporteComponenteSimple rptsimple = new ReporteComponenteSimple();
            String error;
            rptsimple.cargaNegocioPais(out error);
            rptsimple.cargaPaises(out error);
            rptsimple.cargaServicioNegocio(out error);
            rptsimple.CargaFiltro(out error);
            rptsimple.cargaListaFiltros(out error);
            rptsimple.cargaElementos(out error);
            return View(rptsimple);
        }
        public ActionResult vpReporteComponentes(String id, String filtro,String pais,String negocio,String serneg,String elemento1,String elemento2,String elemento3,String orden1,String orden2,String orden3 ,String flg_docto)
        {
            ViewBag.url = Url.Action("getReportComponentes", "reportes", new {          
                                                                                id          = id, 
                                                                                filtro      = filtro, 
                                                                                pais        = pais,
                                                                                negocio     = negocio,
                                                                                serneg      = serneg,
                                                                                elemento1   = elemento1,
                                                                                elemento2   = elemento2,
                                                                                elemento3   = elemento3,
                                                                                orden1      = orden1,
                                                                                orden2      = orden2,
                                                                                orden3      = orden3,
                                                                                flg_docto   = flg_docto
                                                                            });
            return PartialView();
        }
      
        public string getReportComponentes(String id, String filtro, String pais, String negocio, String serneg, String elemento1, String elemento2, String elemento3, String orden1, String orden2, String orden3, String flg_docto)
        {
            byte[] filestream = new ReporteComponenteSimple().getReportComponentes(id, filtro, pais, negocio, serneg,elemento1,elemento2,elemento3,orden1,orden2,orden3,flg_docto);
            return Convert.ToBase64String(filestream);
        }
/*Métodos Reporte ReportCostos*/
        public ActionResult ReportCostos()
        {
            ReportCostos rptsimple = new ReportCostos();
            String error;
            rptsimple.cargaElementos(out error);
            return View(rptsimple);
        }
        public ActionResult vpReportCostos(String id, String elemento1, String flg_docto)
        {
            ViewBag.url = Url.Action("getReportCostos", "reportes", new
            {
                id = id,
                elemento1 = elemento1,
                flg_docto = flg_docto
            });
            return PartialView();
        }

        public string getReportCostosDesglosados()
        {
            DataTable tabla = new DataTable();
            Models.GeneraReporte ctrlFacturacion = new Models.GeneraReporte();
            string serror = "";

            tabla = ctrlFacturacion.getReportCostoDesglose(out serror);



            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);
        }

        public string getReportCostos(String id, String elemento1, String flg_docto)
        {
            byte[] filestream = new ReportCostos().getReportCostos(id, elemento1, flg_docto);
            return Convert.ToBase64String(filestream);
        }

        /*Métodos Reporte ReportRetiros*/
        public ActionResult ReportRetiros()
        {
            ReportRetiros rptsimple = new ReportRetiros();
            String error;
            rptsimple.cargaNegocioPais(out error);
            rptsimple.cargaPaises(out error);
            rptsimple.cargaServicioNegocio(out error);
            return View(rptsimple);
        }
        public ActionResult vpReportRetiros(String id, String pais, String negocio, String serneg, String fec_desde, String fec_hasta, String flg_docto)
        {
            ViewBag.url = Url.Action("getReportRetiros", "reportes", new
            {
                id = id,
                pais = pais,
                negocio = negocio,
                serneg = serneg,
                fec_desde = fec_desde,
                fec_hasta = fec_hasta,
                flg_docto = flg_docto
            });
            return PartialView();
        }

        public string getReportRetiros(String id, String filtro, String pais, String negocio, String serneg, String fec_desde, String fec_hasta, String flg_docto)
        {
            byte[] filestream = new ReportRetiros().getReporteRetiros(id, pais, negocio, serneg, fec_desde, fec_hasta, flg_docto);
            return Convert.ToBase64String(filestream);
        }

        /*Métodos Reporte ReportServiciosDeNegocio*/
        public ActionResult ReportServiciosDeNegocio()
        {
            ReportServiciosDeNegocio rptsimple = new ReportServiciosDeNegocio();
            String error;
            rptsimple.cargaNegocioPais(out error);
            rptsimple.cargaPaises(out error);
            return View(rptsimple);
        }

        public ActionResult vpReportServiciosDeNegocio(String id, String pais, String negocio, String serneg, String flg_docto)
        {
            ViewBag.url = Url.Action("getReportServiciosDeNegocio", "reportes", new
            {
                id = id,
                pais = pais,
                negocio = negocio,
                flg_docto = flg_docto
            });
            return PartialView();
        }

       /************************************************************************************************************************/
        public string getReportServiciosDeNegocio(string id, string pais, string negocio, string flg_docto)
        {

            if (id == null)
            {
                id = "";
            }
            if (flg_docto == null)
            {
                flg_docto = "";
            }

            DataTable tabla = new DataTable();
            ReportServiciosDeNegocio ctrlReportServiciosDeNegocio = new ReportServiciosDeNegocio();
            string serror = "";

            tabla = ctrlReportServiciosDeNegocio.getReportServiciosDeNegocio(out serror, negocio, pais);

            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);
        }
        /***********************************************************************************************************************/



        public ActionResult ReporteCotizacionesPorFacturar()
        {
            String sError = "";


            ViewBag.Error = sError;

            return View();
        }

        public string RepotePorFacturar(string id, string hasta)
        {

            if (id == null)
            {
                id = "";
            }
            if (hasta == null)
            {
                hasta = "";
            }

            DataTable tabla = new DataTable();
            Models.ReporteCotizacionPorFacturar ctrlFacturacion = new Models.ReporteCotizacionPorFacturar();
            string serror = "";

            tabla = ctrlFacturacion.ReportePorFacturar(out serror, id, hasta);

            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);
        }

        public ActionResult ReportesCostosDesglosados()
        {
            return View();
        }

        public ActionResult ReporteServiciosFacturados()
        {
            String sError = "";


            ViewBag.Error = sError;

            return View();
        }

        public string ReporteServicios(string id, string hasta)
        {

            if (id == null)
            {
                id = "";
            }
            if (hasta == null)
            {
                hasta = "";
            }

            DataTable tabla = new DataTable();
            Models.ReporteServiciosFacturados ctrlFacturacion = new Models.ReporteServiciosFacturados();
            string serror = "";

            tabla = ctrlFacturacion.ReporteServicios(out serror, id, hasta);

            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
