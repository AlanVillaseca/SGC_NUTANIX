using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Xml;
using System.Xml.Serialization;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class ServicioNegocioMantenedor
    {
        public String dbConn { get; set; }
        public List<Dictionary<object, string>> dtServicioNegocioJson { get; set; }
        public List<Dictionary<object, string>> dtNegocioPaisJson { get; set; }
        public ServicioNegocioMantenedor()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        public Boolean cargarServicioNegocio(string id,string ordenarPor, string orden,out string sErrors){
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_SERVICIO_NEGOCIO", out dt,out codError, out sErrors, id, ordenarPor, orden);
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

            this.dtServicioNegocioJson = rows;
            return true;
        }
        public Boolean cargarNegocioPais(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_NEGOCIO_PAIS_00", out dt,out codError, out sErrors);
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

            this.dtNegocioPaisJson = rows;
            return true;
        }
        public Boolean agregarServicioNegocio(string idpais, string idnegocio, string nombre, string descripcion, out string sErrors)
        {
            sErrors = "";
            Boolean res;


            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_SERVICIO_NEGOCIO_00", out sErrors, idpais, idnegocio, nombre, descripcion, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());
            
            if (!res)
            {
                return false;
            }

            return true;
        }
        public Boolean ActualizarServicioDeNegocio(List<ServicioDeNegocio> Servicio, out string sError)
        {
            sError = "";
            Boolean res;
            var usrCreacion = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(Servicio.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlJson = "";
            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, Servicio, emptyNamepsaces);
                xmlJson = stream.ToString();
            }
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_SERVICIO_NEGOCIO_02", out sError, xmlJson, usrCreacion);
            if (!res)
            {
                return false;
            }
            return true;
        }
        public Boolean EliminarServicioDeNegocio(string idservicionegocio, out string sError)
        {
            sError = "";
            Boolean res;
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_DEL_SERVICIO_NEGOCIO_00", out sError, idservicionegocio);
            if (!res)
            {
                return false;
            }
            return true;
        }
    }
}