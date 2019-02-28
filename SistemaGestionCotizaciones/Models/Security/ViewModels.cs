using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SistemaGestionCotizaciones.Models.Security
{


    public class UsuariosViewModel
    {
        public List<SecUsuario> Usuarios { get; set; }
    }

    public class UsuarioEditViewModel
    {
        public SecUsuario Usuario { get; set; }
        public List<SecRol> Roles { get; set; }
        public List<vwNegocioPais> NegocioPaises { get; set; }
        public string SelectedNegocioPais { get; set; }

        public List<vwCentroCosto> CentroCosto { get; set; }

        public string SelectedCentroCosto { get; set; }
    }

    public class RolPermisoEditViewModel
    {
        public SecRol Rol { get; set; }
        public List<SecAplicacion> Aplicaciones { get; set; } //all 
        public List<SecAccion> Acciones { get; set; } //all acciones grupo 1
        public List<SecAccion> Acciones2 { get; set; } //all acciones grupo 2
    }
}