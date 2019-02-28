using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace SistemaGestionCotizaciones.Models
{
    public class Contrl
    {
        public string label { get; set; }
        public List<ValorLista> valoresLista { get; set; }
        public int control { get; set; }
        public bool requerido { get; set; }
        public bool editable { get; set; }
        public string predefinido { get; set; }

        public Contrl()
        {
                
        }

    }
}