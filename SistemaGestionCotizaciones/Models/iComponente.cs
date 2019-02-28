using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaGestionCotizaciones.Models
{
    public class iComponente
    {
        public string Categoria { get; set; }
        public decimal costo { get; set; }
        public string glosa { get; set; }
        public string Descripcion { get; set; }
    }
}