using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGestionCotizaciones.Models
{
    public class iCostos
    {
        public string Costo { get; set; }
        public string FkIdparametrocosto { get; set; }
        public string Glosa { get; set; }
        public string Nombre { get; set; }
        public string NumVariables { get; set; }
        public string idcatalogoelemento { get; set; }

        public string idformula { get; set; }
        public string idparametrocosto { get; set; }
        public string orden { get; set; }
        public string origen { get; set; }
    }
}