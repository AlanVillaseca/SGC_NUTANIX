using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class MantenedorParametrosGenerales
    {
        public string dbConn;
        public DataTable dtParametrosGenerales;
        public MantenedorParametrosGenerales()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        public Boolean cargaModGrillaParametrosGenerales(out string sErrors)
        {

            DataTable dtParametrosGeneralesSp = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            string codError = "";

            Boolean result = comCotiz.Consultar("USP_SEL_PARAMETROS_GENERALES_00", out dtParametrosGeneralesSp,out codError, out sErrors);


            if (!result)
            {
                return false;
            }

            this.dtParametrosGenerales = dtParametrosGeneralesSp;

            return true;
        }
        public Boolean modificarParametroGeneral(string nombre,string valor ,out string sErrors)
        {
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Ejecutar("USP_UPD_PARAMETROS_GENERALES_00", out sErrors, nombre, valor);


            if (!result)
            {
                return false;
            }

            return true;
        }
        public DataTable TablaParametrosGenerales(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_PARAMETROS_GENERALES_00", out dt,out codError, out sError);

            return dt;
        }
    }
}