using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaGestionCotizaciones.Models;
using System.Web.Script.Serialization;
using System.Data;
using System.Text;

namespace SistemaGestionCotizaciones.Controllers
{
    [Authorize]
    public class RetiroController : Controller
    {
        public ActionResult FiltroNegocio(string id)
        {

            try
            {
                string sError = "";
                string idnegocio = "";

                RetiroComponentes ctrlRetiroComponente = new RetiroComponentes();

                if (idnegocio != null)
                {

                    idnegocio = ctrlRetiroComponente.CargaNegocio(id, out sError);

                }

                ctrlRetiroComponente.CargaNegocios(out sError);

                ViewBag.Error = sError;
                ViewBag.Idnegocio = idnegocio;

                return View(ctrlRetiroComponente);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }

        }
        public ActionResult vpFiltroNegocio(string pagina, string idnegocio)
        {
            try
            {
                string sError = "";

                if (pagina == null)
                {

                    pagina = "1";

                }

                RetiroComponentes ctrlRetiroComponentes = new RetiroComponentes();

                ctrlRetiroComponentes.CargaServicioNegocios(pagina, idnegocio, out sError);

                ViewBag.Idnegocio = idnegocio;
                ViewBag.PaginaActual = pagina;

                return PartialView(ctrlRetiroComponentes);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return PartialView();
            }
        }
        public ActionResult FiltroAmbiente(string id)
        {
            try
            {
                string sError = "";

                JavaScriptSerializer varjon = new JavaScriptSerializer();
                RetiroComponentes ctrlRetiroComponentes = new RetiroComponentes();

                ctrlRetiroComponentes.CargaInfraestructura(id, out sError);

                ViewBag.Error = sError;
                ViewBag.IdServicioNegocio = id;
                ViewBag.ServicioNegocio = ctrlRetiroComponentes.CargaNombreServicioNegocio(id, out sError);

                return View(ctrlRetiroComponentes);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }
        public ActionResult SolicitudRetiro(string id, string idservicionegocio, string ambiente)
        {
            try
            {
                string sError = "";

                RetiroComponentes ctrlRetiroComponentes = new RetiroComponentes();

                ctrlRetiroComponentes.CargaDetalleSolicitudRetiro(id, idservicionegocio, ambiente, out sError);

                ViewBag.PaginaActual = id;
                ViewBag.ambiente = ambiente;
                ViewBag.Error = sError;
                ViewBag.IdServicioNegocio = idservicionegocio;
                ViewBag.ServicioNegocio = ctrlRetiroComponentes.CargaNombreServicioNegocio(idservicionegocio, out sError);

                return View(ctrlRetiroComponentes);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }
        public String GuardarSolicitudRetiro(string idcomponentes, string motivo)
        {
            try
            {
                string sError = "";

                RetiroComponentes ctrlRetiroComponentes = new RetiroComponentes();

                ctrlRetiroComponentes.GuardarSolicitudRetiro(out sError, idcomponentes, motivo);

                ViewBag.Error = sError;

                return sError;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return "error";
            }
        }
        public ActionResult SolicitudRetiroAprobacion(string id)
        {

            try
            {
                string sError = "";


                if (id == null)
                {
                    id = "1";
                }

                RetiroComponentes ctrlRetiroComponente = new RetiroComponentes();

                ctrlRetiroComponente.CargaSolicitudesRetiro(out sError, id);

                ViewBag.PaginaActual = id;
                ViewBag.Error = sError;

                return View(ctrlRetiroComponente);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }

        }
        public ActionResult SolicitudRetiroAprobacionDetalle(string id)
        {

            try
            {
                string sError = "";



                RetiroComponentes ctrlRetiroComponente = new RetiroComponentes();

                ctrlRetiroComponente.CargaSolicitudesRetiroDetalle(out sError, id);

                ViewBag.Error = sError;
                ViewBag.IdsolicitudRetiro = id;

                return View(ctrlRetiroComponente);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }

        }
        public String ApruebaSolicitudFacturador(string idsolicitudretiro, List<iComponentesRetiro> componentesJson)
        {
            try
            {
                string sError = "";

                RetiroComponentes ctrlRetiroComponente = new RetiroComponentes();

                ctrlRetiroComponente.ApruebaSolicitudFacturador(out sError, idsolicitudretiro, componentesJson);

                ViewBag.Error = sError;

                return sError;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return "error";
            }
        }
        public ActionResult SolicitudRetiroAprobacionCliente(string id)
        {

            try
            {
                string sError = "";


                if (id == null)
                {
                    id = "1";
                }

                RetiroComponentes ctrlRetiroComponente = new RetiroComponentes();

                ctrlRetiroComponente.CargaSolicitudesRetiroCliente(out sError, id);

                ViewBag.PaginaActual = id;
                ViewBag.Error = sError;

                return View(ctrlRetiroComponente);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }

        }
        public ActionResult SolicitudRetiroAprobacionClienteDetalle(string id)
        {

            try
            {
                string sError = "";



                RetiroComponentes ctrlRetiroComponente = new RetiroComponentes();

                ctrlRetiroComponente.CargaSolicitudesRetiroClienteDetalle(out sError, id);

                ViewBag.Error = sError;
                ViewBag.IdsolicitudRetiro = id;

                return View(ctrlRetiroComponente);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }

        }
        public String ReporteSolicitudRetiro(string id)
        {
            RetiroComponentes ctrlRetiro = new RetiroComponentes();
            byte[] filestream = ctrlRetiro.getReporteSolicitudRetiro(id);



            return Convert.ToBase64String(filestream);
        }
        public String ApruebarRechazarSolicitud(string accion, string idsolictudretiro)
        {
            try
            {
                string sError = "";

                RetiroComponentes ctrlRetiroComponente = new RetiroComponentes();

                ctrlRetiroComponente.AprobarRechazarSolicitudRetiro(out sError, idsolictudretiro, accion);

                ViewBag.Error = sError;

                return sError;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return "error";
            }
        }
        public ActionResult SolicitudRetiroInfra(string id)
        {
            try
            {
                string sError = "";

                if (id == null)
                {
                    id = "1";
                }

                RetiroComponentes ctrlRetiroComponentes = new RetiroComponentes();

                ctrlRetiroComponentes.CargaDetalleInfraestructura(id, out sError);

                ViewBag.PaginaActual = id;
                ViewBag.Error = sError;

                return View(ctrlRetiroComponentes);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }
        public ActionResult SolicitudRetiroInfraDetalle(string id)
        {
            try
            {
                string sError = "";


                RetiroComponentes ctrlRetiroComponentes = new RetiroComponentes();

                ctrlRetiroComponentes.CargaInfraestructuraSolicitud(id, out sError);

                ViewBag.IdsolicitudRetiro = id;
                ViewBag.Error = sError;

                return View(ctrlRetiroComponentes);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }

        public ActionResult CrearSolicitud(string id)
        {
            return View();
        }

        public JsonResult BuscarComponente(string id, string cadena, string flag)
        {

            try
            {
                string sError = "";
                string codigo = "0";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                bool bResp = ctrlSolicitudRetiro.Buscar(cadena, flag, out sError);


                if (!bResp)
                codigo = "9999";

                var jsonresult = ctrlSolicitudRetiro.JsonComponentes;


                var result = new { jsondatos = jsonresult, codigo = codigo, mensaje = sError };

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }

        public JsonResult GuardarDatos(string id, string idcomponentes )
        {

            try
            {
                string sError = ""; 
                string codigo = "0";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                bool bResp = ctrlSolicitudRetiro.Guardar(idcomponentes, out sError);

                if (!bResp)
                    codigo="9999";

                var result = new { codigo = codigo, mensaje = sError };

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }


        public ActionResult ListaSolicitudes()
        {

            try
            {
                string sError = "";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                ctrlSolicitudRetiro.ListaSolicitud(out sError);

                ViewBag.Error = sError;
                ViewBag.JsonSolicitudes = ctrlSolicitudRetiro.JsonListaSolicitud;
                return View(ctrlSolicitudRetiro);

            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }
        public ActionResult DetalleCuotaSalida(string id)
        {
            try
            {
                string sError = "";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                ctrlSolicitudRetiro.CuotaSalida(id, out sError);

                ViewBag.Error = sError;
                ViewBag.JsonListaComponentes = ctrlSolicitudRetiro.JsonComponentesLista;
                return View(ctrlSolicitudRetiro);

            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }

        public String GuardarCuotaSolicitud(string id, string cuota, string numcuota)
        {

            try
            {
                string sError = "";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                ctrlSolicitudRetiro.GuardarCuota(id, cuota, numcuota, out sError);

                ViewBag.Error = sError;

                return sError;

            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return "error";
            }
        }


        public ActionResult DetalleSolicitudesFacturadas(string id)
        {
            try
            {
                string sError = "";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                ctrlSolicitudRetiro.CuotaRetiro(id, out sError);

                ViewBag.Error = sError;
                ViewBag.JsonComponentesRetiro = ctrlSolicitudRetiro.JsonComponentesRetiro;
                return View(ctrlSolicitudRetiro);

            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }

        public String GuardarComentarioSolicitud(string idComponente, string idSolicitud, string comentario)
        {

            try
            {
                string sError = "";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                ctrlSolicitudRetiro.GuardarComentario(idComponente, idSolicitud, comentario, out sError);

                ViewBag.Error = sError;

                return sError;

            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return "error";
            }
        }

        public ActionResult DetalleComponentesRetiro(string id)
        {
            try
            {
                string sError = "";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                ctrlSolicitudRetiro.RetiroComponentes(id, out sError);

                ViewBag.Error = sError;
                ViewBag.JsonComponentesRetirados = ctrlSolicitudRetiro.JsonRetiroComponentes;
                return View(ctrlSolicitudRetiro);

            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }

        public String GuardarAprobacionCuota(string id)
        {

            try
            {
                string sError = "";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                ctrlSolicitudRetiro.GuardarAprobacion(id, out sError);

                ViewBag.Error = sError;

                return sError;

            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return "error";
            }
        }



        public JsonResult GuardarCuotaAgregada(string id)
        {

            try
            {
                string sError = "";
                string codigo = "0";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                bool bResp = ctrlSolicitudRetiro.GuardarCuotaAprobacion(id, out sError);

                if (!bResp)
                    codigo = "9999";

                var result = new { codigo = codigo, mensaje = sError };

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }


        public String GuardarAprobacionRetiro(string id)
        {

            try
            {
                string sError = "";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                ctrlSolicitudRetiro.GuardarRetiro(id, out sError);

                ViewBag.Error = sError;

                return sError;

            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return "error";
            }
        }

        public ActionResult ListaEstadoSolicitudes()
        {

            try
            {
                string sError = "";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                ctrlSolicitudRetiro.ListaEstadoSolicitud(out sError);

                ViewBag.Error = sError;
                ViewBag.JsonEstadoSolicitudes = ctrlSolicitudRetiro.JsonListaEstadoSolicitud;
                return View();

            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }

        public ActionResult DetalleEstadoSoloLectura(string id)
        {
            try
            {
                string sError = "";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                ctrlSolicitudRetiro.DetalleSolicitudSoloLectura(id, out sError);

                ViewBag.Error = sError;
                ViewBag.JsonListaComponentesEstadoSolicitudRetiro = ctrlSolicitudRetiro.JsonDetalleEstadoSolicitudRetiro;
                return View(ctrlSolicitudRetiro);

            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }
        

        public ActionResult RouteSolicitud  (string id, int estado)
        {
            
            if (estado == 1 && (User.IsInRole("ADMIN") || User.IsInRole("FACTURADOR")))
                {
                    return RedirectToAction("DetalleCuotaSalida", "Retiro", new {@id=id});
                    
                }
            else if (estado == 2 && (User.IsInRole("ADMIN") || User.IsInRole("CLIENTE") || User.IsInRole("CLIENTE+")))
                {
                    return RedirectToAction("DetalleSolicitudesFacturadas", "Retiro", new { @id = id });
                }
            else if (estado == 3 && (User.IsInRole("ADMIN") || User.IsInRole("FACTURADOR")))
                {
                    return RedirectToAction("DetalleComponentesRetiro", "Retiro", new { @id = id });
                }
            else if (estado == 5 && (User.IsInRole("ADMIN") || User.IsInRole("FACTURADOR")))
            {
                return RedirectToAction("DetalleCuotaSalida", "Retiro", new { @id = id });
            }
            else
            {
                return RedirectToAction("DetalleEstadoSoloLectura", "Retiro", new { @id = id }); 
            }
            
        }

        


        public ActionResult DetalleEstadoSolicitudRetiro(string id)
        {
            try
            {
                string sError = "";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                ctrlSolicitudRetiro.DetalleSolicitudSoloLectura(id, out sError);

                ViewBag.Error = sError;
                ViewBag.JsonListaComponentesEstadoSolicitudRetiro = ctrlSolicitudRetiro.JsonDetalleEstadoSolicitudRetiro;
                return View(ctrlSolicitudRetiro);

            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }

        public ActionResult DetalleHistorialSolicitud(string id)
        {
            try
            {
                string sError = "";
                bool Resp = false;
                string codigo = "0";

                SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();

                Resp = ctrlSolicitudRetiro.ListaHistorialSolicitud(id, out sError);
                
                if (!Resp)
                    codigo = "9999";

                var jsonresult = ctrlSolicitudRetiro.JsonHistorialSolicitud; ;


                var result = new { jsondatos = jsonresult, codigo = codigo, mensaje = sError };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }

        public ActionResult ReporteRetiro(string id)
        {
            return View();
        }

        public string SolicitudesRetiroReporte(string id, string hasta, string f_desde, string f_hasta)
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
            SolicitudRetiro ctrlSolicitudRetiro = new SolicitudRetiro();
            string serror = "";

            tabla = ctrlSolicitudRetiro.ListaReporteSolicitudesRetiro(out serror, f_desde, f_hasta );

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

        public String RetiroSolicitud(string idsolictudretiro)
        {
            try
            {
                string sError = "";

                RetiroComponentes ctrlRetiroComponente = new RetiroComponentes();

                ctrlRetiroComponente.RetirarSolicitudRetiro(out sError, idsolictudretiro);

                ViewBag.Error = sError;

                return sError;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return "error";
            }
        }


    }
}