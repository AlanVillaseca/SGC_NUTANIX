using SistemaGestionCotizaciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SistemaGestionCotizaciones.Controllers
{
    public class ServicioController : Controller
    {
        public ActionResult FiltroServicio(string id)
        {
            try
            {
                string sError = "";
                string idnegocio = "";

                DetalleServicio ctrlDetalleServicio = new DetalleServicio();

                if (idnegocio != null)
                {
                
                    idnegocio = ctrlDetalleServicio.CargaNegocio(id, out sError);
                
                }

                ctrlDetalleServicio.CargaNegocios(out sError);

                ViewBag.Error = sError;
                ViewBag.Idnegocio = idnegocio;

                return View(ctrlDetalleServicio);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }
        public ActionResult Ver(string id)
        {
            try
            {
                string sError = "";

                JavaScriptSerializer varjon = new JavaScriptSerializer();
                DetalleServicio ctrlDetalleServicio = new DetalleServicio();

                ctrlDetalleServicio.CargaInfraestructura(id, out sError);

                ViewBag.Error = sError;
                ViewBag.IdServicioNegocio = id;
                ViewBag.ServicioNegocio = ctrlDetalleServicio.CargaNombreServicioNegocio(id, out sError);

                return View(ctrlDetalleServicio);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }
        public ActionResult DetalleServicio(string id, string idservicionegocio, string ambiente)
        {
            try
            {
                string sError = "";

                DetalleServicio ctrlDetalleServicio = new DetalleServicio();

                ctrlDetalleServicio.CargaDetalleInfraestructura(id, idservicionegocio, ambiente, out sError);

                ViewBag.PaginaActual = id;
                ViewBag.ambiente = ambiente;
                ViewBag.Error = sError;
                ViewBag.IdServicioNegocio = idservicionegocio;
                ViewBag.ServicioNegocio = ctrlDetalleServicio.CargaNombreServicioNegocio(idservicionegocio, out sError);

                return View(ctrlDetalleServicio);
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
                return View(ViewBag.Error);
            }
        }
        public ActionResult vpFiltroServicio(string pagina,string idnegocio)
        {
            try
            {
                string sError = "";

                if (pagina == null) {

                    pagina = "1";
                
                }

                DetalleServicio ctrlDetalleServicio = new DetalleServicio();

                ctrlDetalleServicio.CargaServicioNegocios(pagina, idnegocio, out sError);

                ViewBag.Idnegocio = idnegocio;
                ViewBag.PaginaActual = pagina;

                return PartialView(ctrlDetalleServicio);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return PartialView();
            }
        }
        public String DetalleInfraestructura(string idservicionegocio)
        {
            string sError = "";

            JavaScriptSerializer varjon = new JavaScriptSerializer();
            DetalleServicio ctrlDetalleServicio = new DetalleServicio();

            ctrlDetalleServicio.CargaInfraestructura(idservicionegocio, out sError);

            ViewBag.Error = sError;

            return varjon.Serialize(ctrlDetalleServicio.infraestructuraJson);
        }
        public String DetalleCotizacion(string idcomponente)
        {
            string sError = "";
            JavaScriptSerializer varjon = new JavaScriptSerializer();
            DetalleServicio ctrlElemento = new DetalleServicio();

            ctrlElemento.CargaReporteComponente(idcomponente, out sError);

            ViewBag.Error = sError;

            return varjon.Serialize(ctrlElemento.ReporteJson);
        }
        public String DetalleCabecera(string idcomponente)
        {
            string sError = "";
            JavaScriptSerializer varjon = new JavaScriptSerializer();
            DetalleServicio ctrlElemento = new DetalleServicio();

            ctrlElemento.CargaCabeceraReporteComponente(idcomponente, out sError);

            ViewBag.Error = sError;

            return varjon.Serialize(ctrlElemento.CabeceraReporteJson);
        }
    }
}