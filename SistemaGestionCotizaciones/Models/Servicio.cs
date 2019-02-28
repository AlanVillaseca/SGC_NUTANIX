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
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class Servicio
    {
        public string nombre {get;set;}
        public string descripcion { get; set; }
        public string cantidad { get; set; }
        public string TipoUnidad { get; set; }
        public string TipoUnidadMensual { get; set; }
        public string idServicio { get; set; }
        public string idCotizacion { get; set; }
         public String  dbConn { get; set; }
         public List<Dictionary<object, string>> servicioJson { get; set; }
        public DataTable dtServicio { get; set; }
        public Servicio()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        public bool leerServicio(string idServicio ,string idCotizacion, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SERVICIO_POR_ID", out dt,out codError, out sError,idServicio, idCotizacion);
            if (!res)
            {
                return false;
            }


            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<object, string>> rows = new List<Dictionary<object, string>>();
            Dictionary<object, string> row;

            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<object, string>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);
            }

            this.servicioJson = rows;
            return true;
        }
    }
}