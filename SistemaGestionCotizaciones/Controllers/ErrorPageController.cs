using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaGestionCotizaciones.Controllers
{
    public class ErrorPageController : Controller
    {
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult Forbidden()
        {
            return View();
        }
        public ActionResult Expired()
        {
            return View();
        }
    }
}