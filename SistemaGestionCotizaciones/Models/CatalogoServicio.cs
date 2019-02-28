using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGestionCotizaciones.Models
{
    public class CatalogoServicio
    {
        public string Nombreservicio { get; set; }
        public string Descripcion { get; set; }
        public string SubGerencia { get; set; }
        public string Tipocobroinicial { get; set; }
        public string Tipocobromensual { get; set; }
        public string Tipounidad { get; set; }
        public string TipounidadMensual { get; set; }
        public string Costounidadinicial { get; set; }
        public string Costounidadmensual { get; set; }
        public string DominioPais { get; set; }
        public string DominioNegocio { get; set; }
        public string TipoServicio { get; set; }
        public string CentroCosto { get; set; }
             
    }

}