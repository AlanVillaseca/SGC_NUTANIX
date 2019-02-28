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
    public class Descarga
    {
        public string dbConn;

        public Descarga()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }

        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }

       public DataTable DescargaListaComponente(out string serror, string elemento1, string elemento2, string elemento3, string idpais, string idnegocio, string idservicio)
        {
            DataTable table = new DataTable();
            SqlAccess cdal = new SqlAccess(dbConn);
            string codError = "";
            Boolean res;
            res = cdal.Consultar("USP_SEL_REPORTE_COMPONENTE", out table,out codError, out serror, elemento1, elemento2, elemento3, idpais, idnegocio, idservicio);
            return table;
        }
        public DataTable DescargaCotizacionFacturada(out string serror, string idestado, string implementador, string jefeproyecto)
        {
            DataTable table = new DataTable();
            string codError = "";
            SqlAccess cdal = new SqlAccess(dbConn);
            Boolean res;
            res = cdal.Consultar("USP_SEL_DESCARGA_COTIZACION_FACTURADA", out table,out codError, out serror, idestado, implementador, jefeproyecto, GetUserName());
            return table;
        }
    }
}