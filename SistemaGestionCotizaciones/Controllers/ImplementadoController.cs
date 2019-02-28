using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaGestionCotizaciones.Models;

namespace SistemaGestionCotizaciones.Controllers
{
    public class ImplementadoController : Controller
    {
        // GET: Implementado
        public ActionResult Proyectos()
        {
            try
            {

                string sError = "";
                ProyectosImplementado Proyectos = new ProyectosImplementado();

                Proyectos.proyectos(out sError);
                ViewBag.Error = sError;

                return View(Proyectos);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult vpGrillaProyectos(string id, string negocio, string usrSolicitante, string fchIni, string fchFin,
            string usrAsignada, string pagina)
        {
            //variable id es el nombre del proyecto
            try
            {
                string campo = "idproyecto";

                if (id == null) { id = ""; }
                if (negocio == null) { negocio = ""; }
                if (usrSolicitante == null) { usrSolicitante = ""; }
                if (fchIni == null) { fchIni = ""; }
                if (fchFin == null) { fchFin = ""; }
                if (usrAsignada == null) { usrAsignada = ""; }
                if (pagina == null) { pagina = "1"; }

                string sError = "";
                ProyectosImplementado ctrlGrillaProyectos = new ProyectosImplementado();

                ctrlGrillaProyectos.cargaModGrillaProyectos(id, campo, negocio, usrSolicitante, fchIni, fchFin, usrAsignada, pagina, out sError);
                ViewBag.Error = sError;
                ViewBag.Pagina = pagina;
                ViewBag.IdCotizacion = id;

                return PartialView(ctrlGrillaProyectos);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return PartialView();
            }
        }
        public ActionResult Cotizaciones(string id)
        {
            try
            {
                string sError = "";
                CotizacionesImplementado datosProyectos = new CotizacionesImplementado();

                datosProyectos.cargaProyecto(id, out sError);
                ViewBag.IdCotizacion = id;
                ViewBag.Error = sError;
                return View(datosProyectos);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult vpGrillaCotizaciones(string id, string pagina)
        {
            try
            {
                string sError = "";

                string campo = "idcotizacion";

                if (pagina == null)
                {
                    pagina = "1";
                }

                CotizacionesImplementado datosProyectos = new CotizacionesImplementado();

                datosProyectos.cargaModGrillaCotizaciones(id, campo, pagina, out sError);
                ViewBag.Error = sError;
                ViewBag.Pagina = pagina;
                ViewBag.IdCotizacion = id;
                return PartialView(datosProyectos);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return PartialView();
            }
        }
        public ActionResult Componentes(string id)
        {
            try
            {
                string sError = "";

                ComponentesImplementado datosCotizacion = new ComponentesImplementado();

                datosCotizacion.cargaCotizacion(id, out sError);
                ViewBag.IdCotizacion = id;
                ViewBag.Error = sError;

                return View(datosCotizacion);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult vpGrillaComponentes(string id, string pagina)
        {
            try
            {
                string sError = "";

                string campo = "idcomponente";

                if (pagina == null)
                {
                    pagina = "1";
                }

                ComponentesImplementado datosProyectos = new ComponentesImplementado();

                datosProyectos.cargaModGrillaComponentes(id, campo, pagina, out sError);
                ViewBag.Error = sError;
                ViewBag.Pagina = pagina;
                ViewBag.IdCotizacion = id;
                return PartialView(datosProyectos);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return PartialView();
            }
        }
        public string modGestionCotizacion(string id, string accion, string motivo)
        {
            string sError = "";

            ComponentesImplementado datosComponentes = new ComponentesImplementado();

            datosComponentes.gestionarComponente(id, accion, motivo, out sError);

            ViewBag.error = sError;

            return sError;
        }
    }
}