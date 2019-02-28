using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaGestionCotizaciones.Models;

namespace SistemaGestionCotizaciones.Controllers
{
    public class FacturacionController : Controller
    {
        public ActionResult Depreciacion(string id)
        {
            try
            {
                string sError = "";

                if (id == null) {

                    id = "1";

                }

                FacturacionDepreciacion ctrlDepreciacion = new FacturacionDepreciacion();

                ctrlDepreciacion.cargaDepreciacion(id, out sError);

                ViewBag.Error = sError;

                return View(ctrlDepreciacion);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        [HttpPost]
        public Boolean modificaDepreciacion(List<iComponentes> idcomponente)
        {

            try
            {
                string sError = "";

                FacturacionDepreciacion ctrlDepreciacion = new FacturacionDepreciacion();

                ctrlDepreciacion.ModificarDepreciacion(idcomponente, out sError);

                ViewBag.Error = sError;

                return true;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return false;
            }

        }
    }
}