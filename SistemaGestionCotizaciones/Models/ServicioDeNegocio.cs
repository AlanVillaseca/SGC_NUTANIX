using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGestionCotizaciones.Models
{
    public class ServicioDeNegocio
    {
        public string      Idserviciodenegocio { get; set; }
        public string idPais { get; set; }
        public string idNegocio { get; set; }
        public string Nombrepais { get; set; }
        public string Nombrenegocio { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string totalPaginas { get; set; }
        public string totalRegistros { get; set; }
        public string paginaActual { get; set; }
     }
}

