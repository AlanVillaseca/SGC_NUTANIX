using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGestionCotizaciones.Models
{
    public class ConceptoCosto
    {
        public string id { get; set; }
        public string fk { get; set; }
        public string nombre { get; set; }
        public string glosa { get; set; }
        public string tipo { get; set; }
        public string tipoFuncion { get; set; }
        public string valor { get; set; }
        public string idBd { get; set; }
        public string fkBd { get; set; }
        public string used { get; set; }
        public string acc { get; set; }
        public string idLista { get; set; }
        public string optLista { get; set; }

    }

}