using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class ProyectosImplementado
    {
        public string dbConn;
        public DataTable dtImplementado { get; set; }
        public DataTable dtPSolicitante { get; set; }
        public DataTable dtSNegocio { get; set; }
        public DataTable dtUAsignado { get; set; }
        public string id { get; set; }
        public ProyectosImplementado()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        public Boolean proyectos(out string sErrors){
            
            DataTable dtImplementadoSp = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_IMPLEMENTADO_PROYECTOS_01", out dtImplementadoSp,out codError, out sErrors);

            this.dtPSolicitante = dtImplementadoSp;

            Boolean result2 = comCotiz.Consultar("USP_SEL_IMPLEMENTADO_PROYECTOS_02", out dtImplementadoSp,out codError, out sErrors);

            this.dtSNegocio = dtImplementadoSp;

            Boolean result3 = comCotiz.Consultar("USP_SEL_IMPLEMENTADO_PROYECTOS_03", out dtImplementadoSp,out codError, out sErrors);

            this.dtUAsignado = dtImplementadoSp;

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
            DataTable dtImplementadosSp = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_IMPLEMENTADO_PROYECTOS_00", out dtImplementadosSp,out codError, out sErrors, pagina, campo,
                id, fchIni, fchFin, "", usrSolicitante, negocio, usrAsignada);


            if (!result)
            {
                return false;
            }

            this.id = id;
            this.dtImplementado = dtImplementadosSp;

            if (dtImplementado.Rows.Count == 0)
            {
                dtImplementado.Columns.Add("0");
                dtImplementado.Columns.Add("1");
                dtImplementado.Columns.Add("2");
                dtImplementado.Columns.Add("3");
                dtImplementado.Columns.Add("4");
                dtImplementado.Columns.Add("5");
                dtImplementado.Columns.Add("6");
                dtImplementado.Columns.Add("7");
                dtImplementado.Columns.Add("8");
                DataRow fila = dtImplementado.NewRow();
                fila["8"] = 0;
                dtImplementado.Rows.Add(fila);
            }

            return true;
        }
    }
}