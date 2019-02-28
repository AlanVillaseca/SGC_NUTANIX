using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Security.Claims;
using System.Data;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class Carga
    {
        public string dbConn;
        public string cadena { get; set; }

       
        
        public Carga()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }

        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }

        public bool BuscarElementoCarga(string elemento, out string serror)
        {
            Boolean res;
            SqlAccess cdal = new SqlAccess(dbConn);
            res = cdal.Ejecutar("USP_SEL_BUSCAR_ELEMENTO", out serror, elemento);
            if(res == false)
            {
                return false;
            }
            return true;
        }
        public bool CargaArchivo(string cadena, string head, out string serror)
        {
            Boolean res;
            SqlAccess cdal = new SqlAccess(dbConn);
            res = cdal.Ejecutar("USP_SEL_CARGA", out serror, cadena, head, GetUserName());
            if(res == false)
            {
                return false;
            }
            return true;
        }

        public DataTable BuscarValorElemento(string alias, string elemento, string serror)
        {
            DataTable table = new DataTable();
            string codError = "";
            SqlAccess cdal = new SqlAccess(dbConn);
            Boolean res;
            res = cdal.Consultar("USP_SEL_BUSCAR_VALOR_ELEMENTO", out table,out codError, out serror, alias, elemento);
            
            return table;
        }
        public DataTable ReporteConsumoVariable(string datos, string serror)
        {
            DataTable table = new DataTable();
            string codError = "";
            SqlAccess cdal = new SqlAccess(dbConn);
            Boolean res;
            res = cdal.Consultar("USP_SEL_CONSUMO_VARIABLE_00", out table,out codError, out serror, datos);

            return table;
        }

    }
}