using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaGestionCotizaciones.Models.Security;

namespace SistemaGestionCotizaciones.Controllers.Security
{
    public class BaseController : Controller
    {
        protected UsuarioRepository _usuarios;
        protected RolRepository _roles;
        protected PermisoRepository _rolPermisos;
        protected AplicacionRepository _aplicaciones;

        protected override RedirectToRouteResult RedirectToAction(string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues)
        {
            return base.RedirectToAction(actionName, controllerName, routeValues);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var db = new PetaPoco.Database("falabellaDesarrollo");

            _roles = new RolRepository(db);
            _usuarios = new UsuarioRepository(_roles, db);
            _rolPermisos = new PermisoRepository(db);
            _aplicaciones = new AplicacionRepository(db);
        }

    }
}