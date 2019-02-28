using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class ListarCotizaciones
    {
        public string nomProyecto { get; set; }
        public DataTable dtJefe { get; set; }
        public DataTable dtEstado { get; set; }

        private string dbConn;

        public DataTable dtGrilla { get; set; }

        public ListarCotizaciones()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }

        public bool Cargar(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            UsersAD oUserAD = new UsersAD();

            oUserAD.getFullNameAD();

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_COTIZACIONES_02", out dt,out codError, out sError);
            if (!res)            
                return false;

            var result = from t in dt.AsEnumerable()
                         join x in oUserAD.dtUsers.AsEnumerable() on t.ItemArray.GetValue(0) equals x.Field<string>("userId")
                         select new
                         {
                             idperasignada = t.Field<string>("Idperasignada"),
                             fullname = x.Field<string>("fullName")
                         };

            this.dtJefe = new DataTable();
            this.dtJefe.Columns.Add("Idperasignada");
            this.dtJefe.Columns.Add("fullName");

            foreach(var item in result)
            {
                this.dtJefe.Rows.Add(item.idperasignada, item.fullname);
            }


            res = cDAL.Consultar("USP_SEL_LISTA_COTIZACIONES_01", out dt,out codError, out sError);
            if (!res)
                return false;
            
            this.dtEstado = dt;

            return true;
        }

        public bool CargarGrilla(string numPag, string cant, string campOrden,string nombre, string jefe, string fechai, string fechaf, string estado, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_COTIZACIONES_00", out dt,out codError, out sError,numPag, cant, campOrden, nombre, jefe, fechai, fechaf, estado);

            this.dtGrilla = dt;
            return true;
        }
    }
}