using SistemaGestionCotizaciones.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SistemaGestionCotizaciones.Controllers
{
    public class ElementoController : Controller
    {
        public ActionResult Componentes()
        {
            try {

                string sError = "";
                
                ElementoComponentes ctrlElemento = new ElementoComponentes();

                ctrlElemento.CargaPais(out sError);
                ctrlElemento.CargaNegocio(out sError);
                ctrlElemento.CargaServicioNegocio(out sError);

                return View(ctrlElemento);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }
        public String DetalleCotizacion(string idversion, string idcomponente)
        {
            string sError = "";
            JavaScriptSerializer varjon = new JavaScriptSerializer();
            ElementoComponentes ctrlElemento = new ElementoComponentes();

            ctrlElemento.CargaReporteComponente(idversion, idcomponente,out sError);

            ViewBag.Error = sError;

            return varjon.Serialize(ctrlElemento.ReporteJson);
        }
        public String DetalleCabecera(string idcomponente)
        {
            string sError = "";
            JavaScriptSerializer varjon = new JavaScriptSerializer();
            ElementoComponentes ctrlElemento = new ElementoComponentes();

            ctrlElemento.CargaCabeceraReporteComponente(idcomponente, out sError);

            ViewBag.Error = sError;

            return varjon.Serialize(ctrlElemento.CabeceraReporteJson);
        }
        [HttpGet]
        public ActionResult vpGrillaComponentes()
        {
            try
            {
                string sError = "";
                string pagina = "1";
                string ordenamiento = "1";
                string id = "-1";
                string alias = "";
                string nombre = "-1";
                string fechadesde = "-1";
                string fechahasta = "-1";
                string pais = "-1";
                string negocio = "-1";
                string servicio = "-1";

                ElementoComponentes ctrlElemento = new ElementoComponentes();

                ctrlElemento.CargaComponentes(out sError, pagina, ordenamiento, id, alias, nombre, fechadesde, fechahasta,
                    pais, negocio, servicio);

                ViewBag.Error = sError;

                return View(ctrlElemento);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }
        [HttpPost]
        public ActionResult vpGrillaComponentes(string pagina, string ordenamiento, string id, string alias, string nombre,
            string fechadesde, string fechahasta, string pais, string negocio, string servicio)
        {
            try
            {

                string sError = "";
                
                MantenedorCostos ctrlSimularCosto = new MantenedorCostos();

                if (pagina == null)
                {
                    pagina = "1";
                }
                if (ordenamiento == null)
                {
                    ordenamiento = "1";
                }
                if (id == null)
                {
                    id = "-1";
                }
                if (alias == null)
                {
                    alias = "";
                }
                if (nombre == null)
                {
                    nombre = "-1";
                }
                if (fechadesde == null)
                {
                    fechadesde = "-1";
                }
                if (fechahasta == null)
                {
                    fechahasta = "-1";
                }
                if (pais == null)
                {
                    pais = "-1";
                }
                if (negocio == null)
                {
                    negocio = "-1";
                }
                if (servicio == null)
                {
                    servicio = "-1";
                }

                ElementoComponentes ctrlElemento = new ElementoComponentes();

                ctrlElemento.CargaComponentes(out sError, pagina, ordenamiento, id, alias, nombre, fechadesde, fechahasta,
                    pais, negocio, servicio);

                ViewBag.Error = sError;

                return View(ctrlElemento);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }
        [HttpPost]
        public String FileXls(string idcomponente, string nombrecomponente, string fechamin, string fechamax,
            string pais, string negocio, string servicio)
        {
            string serror = "";

            if (idcomponente == "")
            {
                idcomponente = "-1";
            }
            if (nombrecomponente == "")
            {
                nombrecomponente = "-1";
            }
            if (fechamin == "")
            {
                fechamin = "-1";
            }
            if (fechamax == "")
            {
                fechamax = "-1";
            }

            DataTable tabla = new DataTable();

            ElementoComponentes ctrlElemento = new ElementoComponentes();


            tabla = ctrlElemento.TablaComponentes(idcomponente, nombrecomponente, fechamin, fechamax,
            pais, negocio, servicio, out serror);


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
        [HttpPost]
        public String FileXls2()
        {
            string serror = "";

            DataTable tabla = new DataTable();

            ElementoComponentes ctrlElemento = new ElementoComponentes();


            tabla = ctrlElemento.TablaComponentes2(out serror);


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