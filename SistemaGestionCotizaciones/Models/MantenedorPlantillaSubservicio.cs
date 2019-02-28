using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class MantenedorPlantillaSubservicio
    {
        public string dbConn;
        public List<Dictionary<object, string>> dtSubservicio { get; set; }
        public List<Dictionary<object, string>> dtSubservicioComprobar { get; set; }
        public List<Dictionary<object, string>> dtPlantillaSubservicio { get; set; }
        public List<Dictionary<object, string>> dtServicio { get; set; }
        public MantenedorPlantillaSubservicio()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        public Boolean cargaPlantillaSubservicio(string pagina, out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_PLANTILLA_SUBSERVICIO_00", out dt,out codError, out sErrors, pagina);
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

            this.dtPlantillaSubservicio = rows;
            return true;
        }
        public Boolean cargaServicio(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SERVICIO_01", out dt,out codError, out sErrors);
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

            this.dtServicio = rows;
            return true;
        }
        public Boolean cargaSubservicio(string pagina, string idplantillasubservicio, out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SUBSERVICIO_00", out dt,out codError, out sErrors, pagina, idplantillasubservicio);
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

            this.dtSubservicio = rows;
            return true;
        }
        public Boolean cargaSubservicioComprobar(string idplantillasubservicio, out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SUBSERVICIO_01", out dt,out codError, out sErrors, idplantillasubservicio);
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

            this.dtSubservicioComprobar = rows;
            return true;
        }
        public Boolean GuardarPlantillaSubservicio(string nombre, string idcatalogoservicio, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);

            res = cDAL.Ejecutar("USP_INS_PLANTILLA_SUBSERVICIO_00", out sError, nombre, idcatalogoservicio);

            if (!res)
            {
                return false;
            }

            return true;
        }
        public Boolean GuardarSubservicio(string idplantillasubservicio, string nombre, string horas, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_SUBSERVICIO_00", out sError, idplantillasubservicio, nombre, horas);

            if (!res)
            {
                return false;
            }

            return true;
        }
        public Boolean EliminarSubservicio(string idsubservicio, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_DEL_SUBSERVICIO_00", out sError, idsubservicio);

            if (!res)
            {
                return false;
            }

            return true;
        }
        public Boolean EliminarPlantillaSubservicio(string idplantillasubservicio, string idservicio, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_DEL_PLANTILLA_SUBSERVICIO_00", out sError, idplantillasubservicio, idservicio);

            if (!res)
            {
                return false;
            }

            return true;
        }
        public Boolean ModificarSubservicio(string idsubservicio, string nombre, string horas, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_SUBSERVICIO_00", out sError, idsubservicio, nombre, horas);

            if (!res)
            {
                return false;
            }

            return true;
        }
        public DataTable TablaPlantillaSubservicio(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_PLANTILLA_SUBSERVICIO_01", out dt, out codError, out sError);

            return dt;
        }
    }
}