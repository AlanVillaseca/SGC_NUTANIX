using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class FacturacionDepreciacion : Controller
    {
        public string dbConn;
        public List<Dictionary<object, string>> dtDepreciacionJson { get; set; }
        public FacturacionDepreciacion()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        public Boolean cargaDepreciacion(string pagina, out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_DEPRECIACION", out dt,out codError, out sErrors, pagina);
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

            this.dtDepreciacionJson = rows;
            return true;
        }
        public Boolean ModificarDepreciacion(List<iComponentes> idcomponenteJson, out string sError)
        {
            sError = "";
            Boolean res;

            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(idcomponenteJson.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlVar = "";

            using (var stream = new System.IO.StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, idcomponenteJson, emptyNamepsaces);
                xmlVar = stream.ToString();
            }

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_DEPRECIACION_00", out sError, xmlVar, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());

            if (!res)
            {
                return false;
            }

            return true;
        }
    }
}