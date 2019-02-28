using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGestionCotizaciones.Models
{
    public class ComponenteJson
    {
        public string id { get; set; }
        public string tipo { get; set; }
        public string padre { get; set; }
        public string nombre { get; set; }
        public string licencias { get; set; }
        public string mantencion { get; set; }
        public string cotizacion { get; set; }
        public string plantilla { get; set; }
    }
}