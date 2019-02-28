using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Xml;
using System.Xml.Serialization;

namespace SistemaGestionCotizaciones.Models
{
    public class PruebaNutanix
    {
        public String dbConn { get; set; }

        public List<Dictionary<object, string>> pruebaNutanixJson { get; set; }

       public S()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }

    }

}