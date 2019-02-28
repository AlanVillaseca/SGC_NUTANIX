using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;

namespace SistemaGestionCotizaciones.Models
{
    [DataContract]
    public class dolar
    {
        [DataMember]
        public string codigo { get; set; }
        [DataMember]
        public string nombre { get; set; }
        [DataMember]
        public string unidad_medida { get; set; }
        [DataMember]
        public string fecha { get; set; }
        [DataMember]
        public string valor { get; set; }
    }
}