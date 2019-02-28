using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class CotizacionesRetiro
    {
        public string dbConn;
        public string id;
        public DataTable dtProyecto { get; set; }
        public DataTable dtCotizaciones { get; set; }
        public CotizacionesRetiro()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        public Boolean cargaProyecto(string id, out string sErrors)
        {
            DataTable dtSolitante = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_DETALLE_PROYECTO_00", out dtSolitante,out codError, out sErrors, id);

            if (!result)
            {
                return false;
            }

            this.dtProyecto = dtSolitante;

            return true;
        }
        public Boolean cargaModGrillaCotizaciones(string id, string campo, string pagina, out string sErrors)
        {

            DataTable dtCotizacionesSp = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_RETIRO_COTIZACIONES_00", out dtCotizacionesSp,out codError, out sErrors,
                                                pagina, campo, id);


            if (!result)
            {
                return false;
            }

            this.id = id;
            this.dtCotizaciones = dtCotizacionesSp;

            if (dtCotizaciones.Rows.Count == 0)
            {
                dtCotizaciones.Columns.Add("0");
                dtCotizaciones.Columns.Add("1");
                dtCotizaciones.Columns.Add("2");
                dtCotizaciones.Columns.Add("3");
                dtCotizaciones.Columns.Add("4");
                dtCotizaciones.Columns.Add("5");
                dtCotizaciones.Columns.Add("6");
                dtCotizaciones.Columns.Add("7");
                dtCotizaciones.Columns.Add("8");
                DataRow fila = dtCotizaciones.NewRow();
                fila["8"] = 0;
                dtCotizaciones.Rows.Add(fila);
            }

            return true;
        }
    }
}