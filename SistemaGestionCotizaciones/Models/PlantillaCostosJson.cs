using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGestionCotizaciones.Models
{
    public class PlantillaCostosJson
    {
        public string idParametroCosto { get; set; }
        public string idCatalogoElemento { get; set; }
        public string fGlosa { get; set; }
        public string idPlantilla { get; set; }
    }
}