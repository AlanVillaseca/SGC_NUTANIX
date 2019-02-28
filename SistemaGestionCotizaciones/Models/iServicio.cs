using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaGestionCotizaciones.Models
{
    public class iServicio
    {
        public string Descripcion { get; set; }
        public decimal CostoMensual { get; set; }
        public decimal CostoInicial { get; set; }
        public string NombreSubservicio { get; set; }
        public string HorasSubservicio { get; set; }


    }
}