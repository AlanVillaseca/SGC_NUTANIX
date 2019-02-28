using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Claims;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class IndexTareas
    {
        public string dbConn;
        public string numPendientes { get; set; }
        public List<Dictionary<object,string>> dtPaisJson {get; set;}
        public List<Dictionary<object, string>> dtNegocioJson {get; set;}
        public List<Dictionary<object, string>> TareasporUsuarioJson { get; set; }
        public List<Dictionary<object, string>> TareasporUsuarioRetiroJson { get; set; }
        public List<Dictionary<object, string>> UsuarioJson { get; set; }
        public List<Dictionary<object, string>> EstadosJson { get; set; }
        public IndexTareas()

        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }


        public Boolean cargarTareasUsuario(string id, string fecha, string nombreProyecto, string idcotizacion,
            string nombreCotizacion, string estado, out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_TO_DO_00", out dt,out codError, out sErrors, id, fecha, nombreProyecto, idcotizacion,
                nombreCotizacion, estado,GetUserName());

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

            this.TareasporUsuarioJson = rows;
            return true;
        }
        public Boolean cargarTareasUsuarioRetiro(string id, out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_TO_DO_02", out dt,out codError, out sErrors, id, GetUserName());

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

            this.TareasporUsuarioRetiroJson = rows;
            return true;
        }
        public Boolean cargarUsuario(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_TO_DO_01", out dt,out codError, out sErrors, GetUserName());
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

            this.UsuarioJson = rows;
            return true;
        }
        public Boolean cargarEstados(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ESTADO_01", out dt,out codError, out sErrors);
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

            this.EstadosJson = rows;
            return true;
        }
        
        public Boolean ModificarNegocioPais(List<iNegocioPais> negociopaisJson, out string sError)
        {
            sError = "";
            Boolean res;

            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(negociopaisJson.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlVar = "";

            using (var stream = new System.IO.StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, negociopaisJson, emptyNamepsaces);
                xmlVar = stream.ToString();
            }

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_NEGOCIO_PAIS_00", out sError, xmlVar, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());

            if (!res)
            {
                return false;
            }

            return true;
        }

        /************************************************************************/
        /*                     LAYOUT CANTIDAD DE PENDIENTES                    */
        /************************************************************************/

        public bool CargaNumeroPendientes(string idUser, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dtResult = new DataTable();

            SqlAccess comCotiz = new SqlAccess(dbConn);
            string codError = "";

            res = comCotiz.Consultar("USP_SEL_LISTA_TO_DO_00", out dtResult,out codError, out sError, "0", "", "", "", "", "0", idUser);
            if (!res)
            {
                return false;
            }

            if (dtResult.Rows.Count > 0)
            {
                this.numPendientes = dtResult.Rows[0]["totalRegistros"].ToString();
            }

            return true;
        }
    }
}