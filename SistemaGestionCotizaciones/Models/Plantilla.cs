using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGestionCotizaciones.Models
{
    public class Plantilla
    {
        public string id_padre { get; set; }
        public string id_costo { get; set; }
        public string tabs { get; set; }
        public string seleccion { get; set; }
        public string nombrePlantilla { get; set; }
        public string softAdicional { get; set; }
        public string Subfamilia { get; set; }
        public string Predefinido { get; set; }
        public string Editable { get; set; }
        public string Requerido { get; set; }
        public string Idplantilla { get; set; }
        public string OrdenTab { get; set; }
        public string Orden { get; set; }
    }
}