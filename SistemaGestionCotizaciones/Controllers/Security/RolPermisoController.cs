using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaGestionCotizaciones.Models.Security;

namespace SistemaGestionCotizaciones.Controllers.Security
{
    public class RolPermisoController : BaseController
    {

        //
        // GET: /rol/rolPemisos/idrol
        public ActionResult Edit(int id)
        {
            var model = new RolPermisoEditViewModel()
            {
                Rol = _roles.RetrieveById(id),
                Aplicaciones = _aplicaciones.RetrieveAll(),
                Acciones = _rolPermisos.RetrieveAccionByGrupo(1),
                Acciones2 = _rolPermisos.RetrieveAccionByGrupo(2)
            };
            model.Rol.Permisos = _rolPermisos.RetrieveByRolId(id);

            return View(model);
        }

        //
        // POST: /rol/RolPermisos/idrol
        [HttpPost]
        public ActionResult Edit(RolPermisoEditViewModel model)
        {
            _rolPermisos.UpdateRolPermiso(ParseSelectedRolPermisos(model.Rol));

            return RedirectToAction("Index", "Rol");
        }
        private SecRol ParseSelectedRolPermisos(SecRol rol)
        {
            if (rol != null && rol.Permisos != null)
            {
                var Permisos = rol.Permisos;
                rol.Permisos = new List<SecRolPermiso>();
                foreach (var rolpermiso in Permisos)
                {
                    if (rolpermiso.Idsecaplicacion != 0)
                        rol.Permisos.Add(rolpermiso);
                }
            }

            return rol;
        }
    }
}
