using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class ComponentesImplementado
    {
        // GET: ComponentesImplementado
        public string dbConn;
        public string id;
        public DataTable dtCotizacion { get; set; }
        public DataTable dtComponetes { get; set; }
        public ComponentesImplementado()

        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        public Boolean cargaCotizacion(string id, out string sErrors)
        {
            DataTable dtCotizacionSp = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            string codError = "";

            //Reutilizo sp que llama detalle de una cotizacion sin filtro ni restriccion

            Boolean result = comCotiz.Consultar("USP_SEL_RETIRO_COMPONENTES_00", out dtCotizacionSp,out codError, out sErrors, id);

            if (!result)
            {
                return false;
            }

            this.dtCotizacion  = dtCotizacionSp;

            return true;
        }
        public Boolean cargaModGrillaComponentes(string id, string campo, string pagina, out string sErrors)
        {

            DataTable dtComponentesSp = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_IMPLEMENTADO_COMPONENTES_00", out dtComponentesSp,out codError, out sErrors,
                                                pagina, campo, id);


            if (!result)
            {
                return false;
            }

            this.id = id;
            this.dtComponetes = dtComponentesSp;

            if (dtComponetes.Rows.Count == 0)
            {
                dtComponetes.Columns.Add("0");
                dtComponetes.Columns.Add("1");
                dtComponetes.Columns.Add("2");
                dtComponetes.Columns.Add("3");
                dtComponetes.Columns.Add("4");
                dtComponetes.Columns.Add("5");
                dtComponetes.Columns.Add("6");
                dtComponetes.Columns.Add("7");
                dtComponetes.Columns.Add("8");
                DataRow fila = dtComponetes.NewRow();
                fila["8"] = 0;
                dtComponetes.Rows.Add(fila);
            }

            return true;
        }
        public Boolean gestionarComponente(string idcotizacion, string accion, string motivo, out string sErrors)
        {

            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Ejecutar("USP_UPD_ESTADO_COT", out sErrors, idcotizacion, "3", System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last(), accion, "7", motivo);

            if (!result)
            {
                return false;
            }

            return true;
        }
    }
}