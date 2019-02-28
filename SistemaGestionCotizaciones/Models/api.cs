using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.Configuration;
using System.Data;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    [DataContract]
    public class api
    {
        [DataMember]
        public dolar dolar { get; set; }
        [DataMember]
        public uf uf { get; set; }

        public String dbConn { get; set; }
        public Boolean updValor(string dolar, string uf, out string sErrors)
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;

            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Ejecutar("USP_UPD_VALOR", out sErrors, dolar, uf);

            if (!result)
            {
                return false;
            }

            return true;
        }

        public DataTable verificar(out string sErrors)
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;

            DataTable dtverificar = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_FECHA_VALOR", out dtverificar,out codError, out sErrors);

            return dtverificar;
        }

    }
}