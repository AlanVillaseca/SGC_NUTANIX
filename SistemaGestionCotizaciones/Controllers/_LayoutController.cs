using SistemaGestionCotizaciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SistemaGestionCotizaciones.Controllers
{
    public class _LayoutController : Controller
    {
        // GET: _Layout
        [ChildActionOnly]
        public string UserBarFullName()
        {
            string fullName = "";
            string usrName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();

            UsersAD usr = new UsersAD();

            fullName = usr.getUserFullName(usrName);

            return fullName+"10000";
        }

        public string cantPendientes(string id)
        {
            String sError = "";

            IndexTareas oInd = new IndexTareas();

            oInd.CargaNumeroPendientes(id, out sError);

            ViewBag.Error = sError;

            return oInd.numPendientes;
        }

    }
}