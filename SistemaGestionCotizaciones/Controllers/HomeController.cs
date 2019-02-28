using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaGestionCotizaciones.Models;

namespace SistemaGestionCotizaciones.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index(string id, string fecha, string nombreProyecto, string idcotizacion,
            string nombreCotizacion, string estado)
        {

            string sError = "";

            IndexTareas ctrlTareasPorUsuario = new IndexTareas();

            if (id == null) {

                id = "0";

            }
            if (fecha == null)
            {

                fecha = "";

            }
            if (nombreProyecto == null)
            {

                nombreProyecto = "";

            }
            if (idcotizacion == null)
            {

                idcotizacion = "";

            }
            if (nombreCotizacion == null)
            {

                nombreCotizacion = "";

            }
            if (estado == null)
            {

                estado = "0";

            }

            ctrlTareasPorUsuario.cargarTareasUsuario(id, fecha, nombreProyecto, idcotizacion, nombreCotizacion, estado, out sError);
            ctrlTareasPorUsuario.cargarUsuario(out sError);
            ctrlTareasPorUsuario.cargarEstados(out sError);



            ViewBag.fecha               = fecha;
            ViewBag.nombreproyecto      = nombreProyecto;
            ViewBag.idcotizacion        = idcotizacion;
            ViewBag.nombrecotizacion    = nombreCotizacion;
            ViewBag.estado              = estado;
            ViewBag.Error               = sError;

            return View(ctrlTareasPorUsuario);

        }
        public ActionResult PendientesRetiros(string id)
        {

            string sError = "";

            IndexTareas ctrlTareasPorUsuario = new IndexTareas();

            if (id == null)
            {
                id = "1";
            }

            ctrlTareasPorUsuario.cargarTareasUsuarioRetiro(id, out sError);
            ctrlTareasPorUsuario.cargarUsuario(out sError);
            ctrlTareasPorUsuario.cargarEstados(out sError);


            
            ViewBag.Error = sError;

            return View(ctrlTareasPorUsuario);

        }
    }
}