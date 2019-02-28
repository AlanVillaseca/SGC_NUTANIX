using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class CargaSimulacion
    {

        public string dbConn;
        public DataTable dtPrecio { get; set; }
        public CargaSimulacion()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        public Boolean precioCotizaciones(string id, out string sErrors)
        {
            DataTable dtPreciosSp = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_COTIZACION_PRECIO_00", out dtPreciosSp,out codError, out sErrors, id);

            if (!result)
            {
                return false;
            }

            this.dtPrecio = dtPreciosSp;

            return true;
        }
    }
}