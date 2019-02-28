using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Security.Claims;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class ProyectosRetiro
    {
        public string dbConn;
        public DataTable dtRetiro { get; set; }
        public DataTable dtPSolicitante { get; set; }
        public DataTable dtSNegocio { get; set; }
        public DataTable dtUAsignado { get; set; }
        public string id { get; set; }
        public ProyectosRetiro()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }
        public Boolean proyectos(out string sErrors){
            
            DataTable dtRetirosSp = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_RETIRO_PROYECTOS_01", out dtRetirosSp,out codError, out sErrors);

            this.dtPSolicitante = dtRetirosSp;

            Boolean result2 = comCotiz.Consultar("USP_SEL_RETIRO_PROYECTOS_02", out dtRetirosSp,out codError, out sErrors);

            this.dtSNegocio = dtRetirosSp;

            Boolean result3 = comCotiz.Consultar("USP_SEL_RETIRO_PROYECTOS_03", out dtRetirosSp,out codError, out sErrors);

            this.dtUAsignado = dtRetirosSp;

            if (!result && !result2)
            {
                return false;
            }

            return true;
        }
        public Boolean cargaModGrillaProyectos(string id, string campo, string negocio, string usrSolicitante, string fchIni,
            string fchFin, string usrAsignada, string pagina, out string sErrors)
        {
            string codError = "";
            DataTable dtRetirosSp = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_RETIRO_PROYECTOS_00", out dtRetirosSp,out codError, out sErrors, pagina, campo,
                id, fchIni, fchFin, "", usrSolicitante, negocio, usrAsignada, GetUserName());


            if (!result)
            {
                return false;
            }

            this.id = id;
            this.dtRetiro = dtRetirosSp;

            if (dtRetiro.Rows.Count == 0)
            {
                dtRetiro.Columns.Add("0");
                dtRetiro.Columns.Add("1");
                dtRetiro.Columns.Add("2");
                dtRetiro.Columns.Add("3");
                dtRetiro.Columns.Add("4");
                dtRetiro.Columns.Add("5");
                dtRetiro.Columns.Add("6");
                dtRetiro.Columns.Add("7");
                dtRetiro.Columns.Add("8");
                DataRow fila = dtRetiro.NewRow();
                fila["8"] = 0;
                dtRetiro.Rows.Add(fila);
            }

            return true;
        }
    }
}