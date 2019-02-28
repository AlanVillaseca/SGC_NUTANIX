using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using SistemaGestionCotizaciones.Models;

namespace SistemaGestionCotizaciones.Controllers
{
    public class DescargaController : Controller
    {
        // GET: Descarga
        public ActionResult Index()
        {
            string errors;
            Models.ReporteCotizacionesEstado ctrlReporteEstados = new Models.ReporteCotizacionesEstado();

            ctrlReporteEstados.cargaEstados(out errors);
            ctrlReporteEstados.cargaImplementadores(out errors);
            ctrlReporteEstados.cargaJefesProyectos(out errors);


            return View(ctrlReporteEstados);
        }

        public ActionResult Reportelistacomponente()
        {
            string errors;

            Models.ReporteGenerico modelReporteGenerico = new Models.ReporteGenerico();
            modelReporteGenerico.cargaElementos(out errors);
            modelReporteGenerico.cargaPaises(out errors);
            modelReporteGenerico.cargaNegociosCascada("-1", out errors);
            modelReporteGenerico.cargaServiciosCascada("-1", "-1", out errors);

            return View(modelReporteGenerico);
        }
        public String CascadaNegocio(string idpais)
        {
            string sError = "";

            JavaScriptSerializer varjon = new JavaScriptSerializer();
            ReporteGenerico ctrlReporte = new ReporteGenerico();

            ctrlReporte.cargaNegociosCascada(idpais, out sError);

            ViewBag.Error = sError;

            return varjon.Serialize(ctrlReporte.dtNegociosJson);
        }
        public String CascadaServicio(string idpais, string idnegocio)
        {
            string sError = "";

            JavaScriptSerializer varjon = new JavaScriptSerializer();
            ReporteGenerico ctrlReporte = new ReporteGenerico();

            ctrlReporte.cargaServiciosCascada(idpais, idnegocio, out sError);

            ViewBag.Error = sError;

            return varjon.Serialize(ctrlReporte.dtServiciosJson);
        }
        [HttpPost]
        public string DescargaListaComponente(string primero, string segundo, string tercero, string idpais, string idnegocio, string idservicio)
        {
            DataTable tabla = new DataTable();
            Models.Descarga descarga = new Models.Descarga();
            string serror = "";

            tabla = descarga.DescargaListaComponente(out serror, primero, segundo, tercero, idpais, idnegocio, idservicio);

            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().Select(column => column.ColumnName.ToLower());
            sb.AppendLine(string.Join(";", columnNames));

            foreach(DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());
                sb.AppendLine(string.Join(";", fields));
            }

            byte[] buffer = Encoding.UTF32.GetBytes(sb.ToString());

            return Convert.ToBase64String(buffer);
        }

        public String FileXls(string idestado, string implementador, string jefeproyecto)
        {
            DataTable tabla = new DataTable();
            Models.Descarga descarga = new Models.Descarga();
            string serror = "";

            tabla = descarga.DescargaCotizacionFacturada(out serror, idestado, implementador, jefeproyecto);

     
           
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