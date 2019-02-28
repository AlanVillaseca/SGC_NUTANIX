using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class MantenedorFamilia
    {
        public string dbConn;
        public List<Dictionary<object, string>> dtFamiliaJson { get; set; }
        public List<Dictionary<object, string>> dtSubfamiliaJson { get; set; }
        public List<Dictionary<object, string>> dtFamiliaSubfamiliaJson { get; set; }

        public DataTable dtFamilia { get; set; }
        public DataTable dtListaFamilia { get; set; }
        public DataTable dtSubFamilia { get; set; }

        public string idFamilia { get; set; }
        public string diSubFamilia { get; set; }
        public string nombre { get; set; }

        public MantenedorFamilia()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        public Boolean cargaFamilia(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_FAMILIA_00", out dt,out codError, out sErrors);
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

            this.dtFamiliaJson = rows;

            return true;
        }
        public Boolean cargaListaFamilia(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_FAMILIA_00", out dt,out codError, out sErrors);
            if (!res)
            {
                return false;
            }

            this.dtListaFamilia = dt;
            return true;
        }
        public Boolean cargaSubfamilia(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SUBFAMILIA_00", out dt,out codError, out sErrors);
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

            this.dtSubfamiliaJson = rows;
            return true;
        }
        public Boolean cargaFamiliaSubfamilia(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_FAMILIA_SUBFAMILIA_00", out dt,out codError, out sErrors);
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

            this.dtFamiliaSubfamiliaJson = rows;
            return true;
        }
        public Boolean ModificarSubfamilia(List<iNegocioPais> familiasubfamiliaJson, out string sError)
        {
            sError = "";
            Boolean res;

            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(familiasubfamiliaJson.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlVar = "";

            using (var stream = new System.IO.StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, familiasubfamiliaJson, emptyNamepsaces);
                xmlVar = stream.ToString();
            }

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_SUBFAMILIA_00", out sError, xmlVar, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());

            if (!res)
            {
                return false;
            }

            return true;
        }

        public Boolean cargaModGrillaDetalleFamilia(string pagina, string ordenadoPor, out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_FAMILIA_00", out dt,out codError, out sErrors, pagina, ordenadoPor);
            if (!res)
            {
                return false;
            }

            this.dtFamilia = dt;
            return true;
        }
        public Boolean cargaModGrillaDetalleSubFamilia(string pagina, string ordenadoPor, out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_SUBFAMILIA_00", out dt,out codError, out sErrors, pagina, ordenadoPor);
            if (!res)
            {
                return false;
            }

            this.dtSubFamilia = dt;
            return true;
        }

        public Boolean ActualizarFamilia(string id, string nombre, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_FAMILIA_00", out sError, id, nombre);

            if (!res)
            {
                return false;
            }

            return true;
        }
        public Boolean ActualizarSubFamilia(string id, string nombre, string idfamilia, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_SUBFAMILIA_01", out sError, id, nombre, idfamilia);

            if (!res)
            {
                return false;
            }

            return true;
        }
        public Boolean agregarSubFamilia(string id, string cadena, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_SUBFAMILIA_00", out sError, id, cadena);

            if (!res)
            {
                return false;
            }

            return true;
        }
        public Boolean agregarFamilia(string id, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_FAMILIA_01", out sError, id);

            if (!res)
            {
                return false;
            }

            return true;
        }
        public DataTable TablaFamilias(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_FAMILIA_01", out dt,out codError, out sError);

            return dt;
        }
        public DataTable TablaSubFamilias(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SUBFAMILIA_01", out dt,out codError, out sError);

            return dt;
        }
    }
}