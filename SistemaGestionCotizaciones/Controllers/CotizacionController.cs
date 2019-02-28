using Microsoft.Reporting.WebForms;
using SistemaGestionCotizaciones.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SistemaGestionCotizaciones.Controllers
{
    public class CotizacionController : Controller
    {
        // GET: Cotizacion
        public ActionResult Listar()
        {
            String sError = "";
            ListarCotizaciones oCot = new ListarCotizaciones();
            oCot.Cargar(out sError);

            ViewBag.Error = sError;

            return View(oCot);
        }

        [HttpPost]
        public string vpValidarCotizacion(string idcotizacion, string tipo)
        {
            string sError = "";
            DetalleCotizacion dcot = new DetalleCotizacion();
            DataTable validate = new DataTable();
            validate = dcot.validarCotizacion(idcotizacion, tipo, out sError);
            return validate.Rows[0][0].ToString();
        }

        public ActionResult vpReporte(string id)
        {
            String sError = "";
            Reporte oRep = new Reporte();
            oRep.CargaReporteCabecera(id, out sError);
            oRep.CargaReporteComponente(id, out sError);
            oRep.CargaReporteServicio(id, out sError);
            oRep.cargaVariables(out sError);

            ViewBag.Error = sError;

            return View(oRep);
        }

        public ActionResult vpGrillaListar(string id, string cantidad, string campoOrden, string nombre, string jefe, string fechaINI, string fechaFIN, string estado)
        {
            String sError = "";
            ListarCotizaciones oCot = new ListarCotizaciones();
            oCot.CargarGrilla(id, cantidad, campoOrden, nombre, jefe, fechaINI, fechaFIN, estado, out sError);

            ViewBag.Error = sError;
            return PartialView(oCot);
        }

        public ActionResult Detalle(string id)
        {
            try
            {
                string sError = "";
                string cotVersion = "0";
                DetalleCotizacion ctrlFiltrosCotizacion = new DetalleCotizacion();

                ctrlFiltrosCotizacion.cargaDetalleCotizacion(id, cotVersion, out sError);
                ctrlFiltrosCotizacion.cargaVersionCotizacion(id, out sError);
                ctrlFiltrosCotizacion.idCotizacion = id;
                ctrlFiltrosCotizacion.CargaServicios(id, out sError);
                ctrlFiltrosCotizacion.cargaImplementadores(out sError);
                ctrlFiltrosCotizacion.cargaMensajes(id, out sError);
                ctrlFiltrosCotizacion.cargaProyectosCopiar(out sError);
                ctrlFiltrosCotizacion.cargaPlantillasCopiar(out sError);
                ctrlFiltrosCotizacion.CargaPlantillaSubservicios2(out sError);

                ctrlFiltrosCotizacion.cargaPermisoCotizacion(id, out sError);

                
                ViewBag.Error = sError;
                ViewBag.IdCotizacion = id;
                ViewBag.idEstado = ctrlFiltrosCotizacion.dtDetalleCotizacion.Rows[0].ItemArray.GetValue(7);


                if (ctrlFiltrosCotizacion.dtPermiso.Rows[0].ItemArray.GetValue(0).ToString() == "0")
                {
                    ViewBag.Error = "El usuario no posee el servicio de negocio asociado al proyecto";
                }


                return View(ctrlFiltrosCotizacion);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }
        [HttpPost]
        public Boolean EliminarCotizacion(string id)
        {
            try
            {
                string sError = "";
                DetalleCotizacion ctrlEliminarCotizacion = new DetalleCotizacion();
                ctrlEliminarCotizacion.BorrarCotizacion(id, out sError);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public ActionResult vpGrillaDetalle(string id, string pagina, string cotVersion)
        {
            try
            {
                string campo = "idcotizacion";

                if (pagina == null)
                {
                    pagina = "1";
                }
                string sError = "";

                if (cotVersion == null)
                {
                    cotVersion = "0";
                }

                DetalleCotizacion oCotGrilla = new DetalleCotizacion();
                oCotGrilla.cargarGrilla(pagina, campo, id, cotVersion, out sError);
                oCotGrilla.cargaDetalleCotizacion(id, cotVersion, out sError);
                oCotGrilla.cargaVersionCotizacion(id, out sError);
                oCotGrilla.CargaServicios(id, out sError);

                ViewBag.Error = sError;
                ViewBag.Pagina = pagina;
                ViewBag.IdCotizacion = id;
                ViewBag.idEstado = oCotGrilla.dtDetalleCotizacion.Rows[0].ItemArray.GetValue(7);
                if (oCotGrilla.dtVersionCotizacion.Rows[0].ItemArray.GetValue(0).ToString().Equals(cotVersion))
                {
                    ViewBag.MaxVersion = true;
                }
                else
                {
                    ViewBag.MaxVersion = false;
                }

                return PartialView(oCotGrilla);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }

        [HttpPost]
        public string editarCotizacion(string id, string descripcion, string fecha)
        {
            string sError = "";
            DetalleCotizacion oCotGrilla = new DetalleCotizacion();
            oCotGrilla.UpdateCotizacion(id, descripcion, fecha, out sError);

            return sError;
        }

        public JsonResult jsonLeerServicios(string id, string idCotizacion)
        {
            string sError = "";

            Servicio oServicio = new Servicio();

            oServicio.leerServicio(id, idCotizacion, out sError);

            ViewBag.Error = sError;
            return Json(oServicio.servicioJson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Detalle(DetalleCotizacion cotizacion)
        {
            try
            {
                string sError = "";
                string id;
                string cotVersion;
                DetalleCotizacion ctrlFiltrosCotizacion = cotizacion;
                id = cotizacion.idCotizacion;
                cotVersion = cotizacion.cotVersion;

                ctrlFiltrosCotizacion.cargaDetalleCotizacion(id, cotVersion, out sError);
                ctrlFiltrosCotizacion.cargaVersionCotizacion(id, out sError);
                ctrlFiltrosCotizacion.idCotizacion = id;
                ctrlFiltrosCotizacion.CargaServicios(id, out sError);
                ctrlFiltrosCotizacion.cargaImplementadores(out sError);
                    //ctrlFiltrosCotizacion.cargarGrilla("0", "10", "idcotizacion", cotizacion.idCotizacion, cotVersion, out sError);
                ctrlFiltrosCotizacion.cargaMensajes(id, out sError);
                    ctrlFiltrosCotizacion.cargaProyectosCopiar(out sError);
                    ctrlFiltrosCotizacion.cargaPlantillasCopiar(out sError);
                ViewBag.Error = sError;
                ViewBag.IdCotizacion = id;

                ViewBag.Error = sError;
                return View(ctrlFiltrosCotizacion);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }

        public string vpValidarServicioComponente(string idcotizacion, string id, string tipo)
        {
            string sError = "";
            DetalleCotizacion dcot = new DetalleCotizacion();
            DataTable validate = new DataTable();
            validate = dcot.validarServicioComponenteCC(idcotizacion, id, tipo, out sError);
            
            
            sError = validate.Rows[0][0].ToString();
            
            
            return sError;
            
        }

        public String vpAgregarServicioCotizacion(string id, string idCotizacion, string cantidad, string cantidadMensual,
            string descripcion, string tipomoneda)
        {
            string sError = "";

            if (cantidadMensual == null)
            {

                cantidadMensual = "";

            }

            if (cantidad == null)
            {

                cantidad = "";

            }

            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();

            ctrlCotizacion.AgregarServicioCotizacion(id, idCotizacion, cantidad, cantidadMensual, out sError, descripcion, tipomoneda);

            return sError;
        }

        [HttpPost]
        public string vpValidarCentroCostoServicio(string idusuario, string idcotizacion)
        {
            string sError = "";
            DetalleCotizacion dcot = new DetalleCotizacion();
            DataTable validate = new DataTable();
            validate = dcot.validarCentroCostoServicio(idusuario, idcotizacion, out sError);
            return validate.Rows[0][0].ToString();
        }

        [HttpPost]
        public string vpValidarCentroCostoComponente(string idusuario, string idcotizacion)
        {
            string sError = "";
            DetalleCotizacion dcot = new DetalleCotizacion();
            DataTable validate = new DataTable();
            validate = dcot.validarCentroCostoComponente(idusuario, idcotizacion, out sError);
            return validate.Rows[0][0].ToString();
        }
        [HttpPost]
        public String vpAgregarServicioActividadesCotizacion(List<iActividad> actividadesJson, string idcotizacion,
            string idservicio, string cantidad)
        {
            try
            {
                string sError = "";

                if (cantidad == null) {

                    cantidad = "";

                }

                DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();

                ctrlCotizacion.AgregarServicioActividadCotizacion(actividadesJson, idcotizacion, idservicio, cantidad, out sError);

                ViewBag.Error = sError;

                return sError;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return "error";
            }
        }
        public String vpModificarServicioCotizacion(string id, string idVersion, string cantidad, string cantidadMensual,
            string tipomoneda)
        {
            string sError = "";
            api obj = new api();
            DataTable b = obj.verificar(out sError);

            if (tipomoneda == null)
            {
                tipomoneda = "";
            }


            try
            {
                if (b.Rows[0].ItemArray[0].ToString() == "1")
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    string webData = wc.DownloadString("http://www.mindicadorr.cl/api");
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(api));
                    MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(webData));
                    obj = (api)ser.ReadObject(stream);
                    obj.updValor(obj.dolar.valor, obj.uf.valor, out sError);
                }
                else
                {
                    DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();
                    ctrlCotizacion.ActualizarServicioCotizacion(id, idVersion, cantidad, cantidadMensual, tipomoneda, out sError);
                }
            }
            catch (Exception e)
            {
                
                sError = "No se pudo actualizar los valores de la UF y del Dolar al valor de hoy. Por favor contáctese con el administrador";
                ViewBag.Error = sError;
                
            }

            


           

            return sError;
        }
        [HttpPost]
        public string CopiarComponente(string id)
        {
            string sError = "";
            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();
            ctrlCotizacion.CopiaComponente(id, out sError);

            return sError;
        }
        [HttpPost]
        public string EliminarComponente(string id)
        {
            string sError = "";
            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();
            ctrlCotizacion.EliminarComponente(id, out sError);
            return sError;
        }
        public string EliminarServicio(string id, string idVersion)
        {
            string sError = "";
            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();
            ctrlCotizacion.EliminarServicio(id, idVersion, out sError);
            return sError;
        }
        public string CambiarEstadoCotizacion(string id, string idAplicacion, string idAccion, string motivo)
        {
            string sError = "";
            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();
            ctrlCotizacion.CambiarEstado(id, idAplicacion, idAccion, motivo, out sError);
            return sError;
        }
        public string AgregarCuotas(string id, string idAplicacion, string idAccion, string motivo, string cuotas)
        {
            string sError = "";
            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();
            ctrlCotizacion.CambiarEstado(id, idAplicacion, idAccion, motivo, out sError);
            ctrlCotizacion.AgregarCuotas(id, cuotas, out sError);
            return sError;
        }
        public string VersionarCotizacion(string id, string comentarios)
        {
            string sError = "";
            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();
            ctrlCotizacion.Versionar(id, comentarios, out sError);
            return sError;
        }
        
        public string CopiarCotizacion(string id, string idProyecto)
        {
            string sError = "";
            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();
            ctrlCotizacion.Copiar(id, idProyecto, out sError);
            return sError;
        }
        public string CopiarCotizacionMigrada(string id, string idProyecto, string idPlantilla)
        {
            string sError = "";
            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();
            ctrlCotizacion.CopiarMigrada(id, idProyecto,idPlantilla, out sError);
            return sError;
        }
        public ActionResult vpReporteCotizacion(string id, String flg_docto)
        {
            var URL = Url.Action("getReportCotizacion", "Cotizacion", new { id = "idR", flg_docto = "v_flg_docto" });
            URL = URL.Replace("idR", id);
            URL = URL.Replace("v_flg_docto", flg_docto);
            ViewBag.url = URL;
            return PartialView();
        }
        public FileStreamResult getReportCotizacion(String id ,String flg_docto)
        {
            Reporte rpt = new Reporte();
            String error;
            String idVersion =id;
            String tip_docto = flg_docto;
            rpt.CargaReporteCabeceraDt(idVersion, out error);//ReporteCabeceraDt
            rpt.CargaReporteComponenteDt(idVersion, out error);//reporte dt
            rpt.CargaReporteServicioDt(idVersion, out error);
            rpt.CargaTotalServicios(idVersion, out error);
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = "Content/Reportes/CotizaRpt.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReporteDt", rpt.ReporteDt));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReporteDt2", rpt.ReporteDt2));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReporteCabeceraDt", rpt.ReporteCabeceraDt));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReporteTabServicios", rpt.ReporteDt2));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TotalServicios", rpt.TotalServicios));
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            String filenameExtension;
            reportViewer.LocalReport.Refresh();
            byte[] pdf = reportViewer.LocalReport.Render(
                tip_docto, null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);
            MemoryStream ms = new MemoryStream();
            ms.Write(pdf, 0, pdf.Length);
            ms.Position = 0;
            if (tip_docto.Equals("PDF"))
                return new FileStreamResult(ms, "application/pdf");
            else
                return new FileStreamResult(ms, "application/vnd.ms-excel");
        }
        public string SolicitarDescuento(string id, string comentarios)
        {
            string sError = "";
            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();
            ctrlCotizacion.SolicitarDescuento(id, comentarios, out sError);
            return sError;
        }
        public string Descuento(string id, string comentarios, string descuento)
        {
            string sError = "";
            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();
            ctrlCotizacion.Descuento(id, comentarios, descuento, out sError);
            return sError;
        }
        public string CargaServicio(string idservicio, string idversion)
        {

            string sError = "";


            JavaScriptSerializer jss = new JavaScriptSerializer();

            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();

            ctrlCotizacion.CargaServicios(idservicio, idversion, out sError);

            ViewBag.Error = sError;

            return jss.Serialize(ctrlCotizacion.dtSubservicios);
        }
        public string CargaSubservicio(string pagina, string idservicio, string idversion)
        {

            string sError = "";

            if (pagina == null)
            {
                pagina = "1";
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();

            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();

            ctrlCotizacion.CargaSubservicios(pagina, idservicio, idversion, out sError);

            ViewBag.Error = sError;

            return jss.Serialize(ctrlCotizacion.dtSubservicios);
        }
        public string CargaPlantillaSubservicio(string idservicio)
        {

            string sError = "";


            JavaScriptSerializer jss = new JavaScriptSerializer();

            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();

            ctrlCotizacion.CargaPlantillaSubservicios(idservicio, out sError);

            ViewBag.Error = sError;

            return jss.Serialize(ctrlCotizacion.plantillasubserviciosJson);
        }
        public string GuardarSubservicio(string idservicio, string idversion, string nombre, string horas)
        {
            string sError = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();

            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();

            ctrlCotizacion.AgregarSubservicios(idservicio, idversion, nombre, horas, out sError);
            ctrlCotizacion.CargaSubservicios("1", idservicio, idversion, out sError);

            ViewBag.Error = sError;

            return jss.Serialize(ctrlCotizacion.dtSubservicios);
        }
        public string GuardarSubservicioPlantilla(string idservicio, string idplantillasubservicio, string idversion)
        {
            string sError = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();

            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();

            ctrlCotizacion.AgregarSubserviciosPlantilla(idservicio, idplantillasubservicio, idversion, out sError);
            ctrlCotizacion.CargaSubservicios("1", idservicio, idversion, out sError);

            ViewBag.Error = sError;

            return jss.Serialize(ctrlCotizacion.dtSubservicios);
        }
        public string EliminarSubservicio(string idservicio, string idsubservicio, string idversion)
        {
            string sError = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();

            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();

            ctrlCotizacion.EliminarSubservicio(idsubservicio, idversion, out sError);
            ctrlCotizacion.CargaSubservicios("1", idservicio, idversion, out sError);

            ViewBag.Error = sError;

            return jss.Serialize(ctrlCotizacion.dtSubservicios);
        }
        public string ModificarSubservicio(string idservicio, string idsubservicio, string idversion, string nombre, string horas)
        {
            string sError = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();

            DetalleCotizacion ctrlCotizacion = new DetalleCotizacion();

            ctrlCotizacion.ModificarSubservicio(idsubservicio, nombre, horas, out sError);
            ctrlCotizacion.CargaSubservicios("1", idservicio, idversion, out sError);

            ViewBag.Error = sError;

            return jss.Serialize(ctrlCotizacion.dtSubservicios);
        }
    }
}