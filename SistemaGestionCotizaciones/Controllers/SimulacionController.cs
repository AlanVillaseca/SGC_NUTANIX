using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaGestionCotizaciones.Models;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using Microsoft.Reporting.WebForms;
using System.Data;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Security.Claims;

namespace SistemaGestionCotizaciones.Controllers
{
    public class SimulacionController : Controller
    {
        // GET: Simulacion
        public ActionResult Cotizacion()
        {

            string sError = "";

            SimulacionCotizacion ctrlSimulacion = new SimulacionCotizacion();

            ctrlSimulacion.cargaInfoSimulacion(out sError);
            ctrlSimulacion.CargaServicios(out sError);
            ctrlSimulacion.CargaPlantillaSubservicios2(out sError);
            ctrlSimulacion.CargaParametrosGenerales(out sError);
            ctrlSimulacion.CargaFamilias(out sError);

            ViewBag.usuario = ctrlSimulacion.GetUserName();

            return View(ctrlSimulacion);
        }
        public ActionResult vpCotizador(string id)
        {
            string sError = "";
            SimulacionCotizacion ctrlSimulacion = new SimulacionCotizacion();

            ctrlSimulacion.CargaArbusto(id, out sError);
            ctrlSimulacion.CargaParametros(id, out sError);
            ctrlSimulacion.CargaCostosOpcionales(id, out sError);
            ctrlSimulacion.CargaHeader(id, "1", out sError);
            ctrlSimulacion.CargaVariablesJson(id, out sError);
            ctrlSimulacion.CargaCatSW(out sError);

            ViewBag.Error = sError;

            return PartialView(ctrlSimulacion);
        }
        [HttpPost]
        public string jsonReceiverCalcular(List<ComponenteJson> componenteJson)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string sError = "";

            SimulacionCotizacion ctrlSimulacion = new SimulacionCotizacion();

            ctrlSimulacion.CalcularComponente(componenteJson, out sError);

            return jss.Serialize(ctrlSimulacion.calculaMontosJson);
        }
        [HttpPost]
        public ActionResult vpReporteSimulacion(string componenteJson, string servicioJson, string montoservicioJson, string infosimulacionJson)
        {

            var URL = Url.Action("getReportSimulacion", "Simulacion", new { componenteJson = componenteJson, servicioJson = servicioJson, montoservicioJson = montoservicioJson, infosimulacionJson = infosimulacionJson });

            ViewBag.url = URL;

            return PartialView();
        }
        public FileStreamResult getReportSimulacion(string componenteJson, string servicioJson, string montoservicioJson, string infosimulacionJson)
        {

            DataTable componente = new DataTable();
            DataTable servicio = new DataTable();
            DataTable monto = new DataTable();
            DataTable info = new DataTable();

            var list = JsonConvert.DeserializeObject<List<iComponente>>(componenteJson);
            var list2 = JsonConvert.DeserializeObject<List<iServicio>>(servicioJson);
            var list3 = JsonConvert.DeserializeObject<List<iMontoServicio>>(montoservicioJson);
            var list4 = JsonConvert.DeserializeObject<List<iInfoSimulacion>>(infosimulacionJson);


            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(iComponente));

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                componente.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];

            foreach (iComponente item in list)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                componente.Rows.Add(values);
            }


            PropertyDescriptorCollection props2 = TypeDescriptor.GetProperties(typeof(iServicio));

            for (int i = 0; i < props2.Count; i++)
            {
                PropertyDescriptor prop2 = props2[i];
                servicio.Columns.Add(prop2.Name, prop2.PropertyType);
            }

            object[] values2 = new object[props2.Count];

            foreach (iServicio item in list2)
            {
                for (int i = 0; i < values2.Length; i++)
                {
                    values2[i] = props2[i].GetValue(item);
                }
                servicio.Rows.Add(values2);
            }


            PropertyDescriptorCollection props3 = TypeDescriptor.GetProperties(typeof(iMontoServicio));

            for (int i = 0; i < props3.Count; i++)
            {
                PropertyDescriptor prop3 = props3[i];
                monto.Columns.Add(prop3.Name, prop3.PropertyType);
            }

            object[] values3 = new object[props3.Count];

            foreach (iMontoServicio item in list3)
            {
                for (int i = 0; i < values3.Length; i++)
                {
                    values3[i] = props3[i].GetValue(item);
                }
                monto.Rows.Add(values3);
            }


            PropertyDescriptorCollection props4 = TypeDescriptor.GetProperties(typeof(iInfoSimulacion));

            for (int i = 0; i < props4.Count; i++)
            {
                PropertyDescriptor prop4 = props4[i];
                info.Columns.Add(prop4.Name, prop4.PropertyType);
            }

            object[] values4 = new object[props4.Count];

            foreach (iInfoSimulacion item in list4)
            {
                for (int i = 0; i < values4.Length; i++)
                {
                    values4[i] = props4[i].GetValue(item);
                }
                info.Rows.Add(values4);
            }


            MemoryStream ms = new MemoryStream();
            ReportViewer reportViewer = new ReportViewer();
            Warning[] warnings;
            String filenameExtension;
            string[] streamids;
            string mimeType;
            string encoding;

            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = "Content/Reportes/SimulacionCotizacion.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DtComponente", componente));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DtServicio", servicio));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DtMonto", monto));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DtInfo", info));
            reportViewer.LocalReport.Refresh();

            byte[] pdf = reportViewer.LocalReport.Render(
                "PDF", null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);

            ms.Write(pdf, 0, pdf.Length);
            ms.Position = 0;

            return new FileStreamResult(ms, "application/pdf");

        }

        public String ReporteSolicitudRetiro(string componenteJson, string servicioJson, string montoservicioJson)
        {
            DataTable componente = new DataTable();
            DataTable servicio = new DataTable();
            DataTable monto = new DataTable();

            var list = JsonConvert.DeserializeObject<List<iComponente>>(componenteJson);
            var list2 = JsonConvert.DeserializeObject<List<iServicio>>(servicioJson);
            var list3 = JsonConvert.DeserializeObject<List<iMontoServicio>>(montoservicioJson);


            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(iComponente));

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                componente.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];

            foreach (iComponente item in list)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                componente.Rows.Add(values);
            }


            PropertyDescriptorCollection props2 = TypeDescriptor.GetProperties(typeof(iServicio));

            for (int i = 0; i < props2.Count; i++)
            {
                PropertyDescriptor prop2 = props2[i];
                servicio.Columns.Add(prop2.Name, prop2.PropertyType);
            }

            object[] values2 = new object[props2.Count];

            foreach (iServicio item in list2)
            {
                for (int i = 0; i < values2.Length; i++)
                {
                    values2[i] = props2[i].GetValue(item);
                }
                servicio.Rows.Add(values2);
            }


            PropertyDescriptorCollection props3 = TypeDescriptor.GetProperties(typeof(iMontoServicio));

            for (int i = 0; i < props3.Count; i++)
            {
                PropertyDescriptor prop3 = props3[i];
                monto.Columns.Add(prop3.Name, prop3.PropertyType);
            }

            object[] values3 = new object[props3.Count];

            foreach (iMontoServicio item in list3)
            {
                for (int i = 0; i < values3.Length; i++)
                {
                    values3[i] = props3[i].GetValue(item);
                }
                monto.Rows.Add(values3);
            }


            MemoryStream ms = new MemoryStream();
            ReportViewer reportViewer = new ReportViewer();
            Warning[] warnings;
            String filenameExtension;
            string[] streamids;
            string mimeType;
            string encoding;

            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = "Content/Reportes/SimulacionCotizacion.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DtComponente", componente));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DtServicio", servicio));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DtMonto", monto));
            reportViewer.LocalReport.Refresh();

            byte[] pdf = reportViewer.LocalReport.Render(
                "PDF", null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);



            return Convert.ToBase64String(pdf);
        }
    }
}