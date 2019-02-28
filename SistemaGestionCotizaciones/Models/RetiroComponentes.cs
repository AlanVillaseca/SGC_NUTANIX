using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class RetiroComponentes
    {
        public string dbConn;
        public List<Dictionary<object, string>> negociosJson { get; set; }
        public List<Dictionary<object, string>> serviciosnegociosJson { get; set; }
        public List<Dictionary<object, string>> infraestructuraJson { get; set; }
        public List<Dictionary<object, string>> detalleinfraestructuraJson { get; set; }
        public List<Dictionary<object, string>> ReporteJson { get; set; }
        public List<Dictionary<object, string>> CabeceraReporteJson { get; set; }
        public List<Dictionary<object, string>> cargaVariablesJson { get; set; }
        public List<Dictionary<object, string>> solicitudretiroJson { get; set; }
        public List<Dictionary<object, string>> solicitudretiroclienteJson { get; set; }
        public List<Dictionary<object, string>> solicitudretirodetalleJson { get; set; }
        public List<Dictionary<object, string>> solicitudretiroclientedetalleJson { get; set; }
        public List<Dictionary<object, string>> solicitudesinfraestructuraJson { get; set; }
        public List<Dictionary<object, string>> solicitudesjefeproyectoJson { get; set; }
        public List<Dictionary<object, string>> ReporteSolicitudRetiroJson { get; set; }
        public RetiroComponentes()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }

        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }

        public String CargaNegocio(string idservicionegocio, out string sError)
        {
            sError = "";

            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_NEGOCIO_06", out dt,out codError, out sError, idservicionegocio);
            if (!res)
            {
                return "";
            }

            return dt.Rows[0].ItemArray[0].ToString();
        }

        public bool CargaNegocios(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_NEGOCIO_05", out dt,out codError, out sError, GetUserName());
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

            this.negociosJson = rows;
            return true;
        }

        public bool CargaServicioNegocios(string pagina, string idnegocio, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SERVICIO_NEGOCIO_05", out dt,out codError, out sError, pagina, GetUserName(), idnegocio);
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

            this.serviciosnegociosJson = rows;
            return true;
        }
        public bool CargaInfraestructura(string idservicionegocio, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_COMPONENTES_07", out dt,out codError, out sError, idservicionegocio);
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

            this.infraestructuraJson = rows;
            return true;
        }

        public String CargaNombreServicioNegocio(string idservicionegocio, out string sError)
        {
            sError = "";

            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SERVICIO_NEGOCIO_06", out dt,out codError, out sError, idservicionegocio);
            if (!res)
            {
                return "";
            }

            return dt.Rows[0].ItemArray[0].ToString();
        }

        public bool CargaDetalleSolicitudRetiro(string pagina, string idservicionegocio, string ambiente, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_COMPONENTES_08", out dt,out codError, out sError, pagina, idservicionegocio, ambiente);
            if (!res)
            {
                return false;
            }


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

            this.detalleinfraestructuraJson = rows;
            return true;
        }
        public Boolean GuardarSolicitudRetiro(out string sError, string idcomponentes, string motivo)
        {

            sError = "";
            bool res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_SOLICITUD_RETIRO_00", out sError, idcomponentes, motivo, GetUserName());

            if (!res)
            {
                return false;
            }

            return true;
        }
        public bool CargaSolicitudesRetiro(out string sError, string pagina)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SOLICITUD_RETIRO_00", out dt,out codError, out sError, pagina);
            if (!res)
            {
                return false;
            }


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

            this.solicitudretiroJson = rows;
            return true;
        }
        public bool CargaSolicitudesRetiroDetalle(out string sError, string pagina)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SOLICITUD_RETIRO_01", out dt,out codError, out sError, pagina);
            if (!res)
            {
                return false;
            }


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

            this.solicitudretirodetalleJson = rows;
            return true;
        }
        public Boolean ApruebaSolicitudFacturador(out string sError, string idsolictudretiro, List<iComponentesRetiro> componentesJson)
        {

            sError = "";
            Boolean res;
            string xmlVar = "";
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(componentesJson.GetType());
            var settings = new XmlWriterSettings();

            if (componentesJson != null)
            {

                settings.Indent = false;
                settings.OmitXmlDeclaration = true;

                using (var stream = new System.IO.StringWriter())
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, componentesJson, emptyNamepsaces);
                    xmlVar = stream.ToString();
                }
            }

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_SOLICITUD_RETIRO_00", out sError, idsolictudretiro, xmlVar, GetUserName());

            if (!res)
            {
                return false;
            }

            return true;
        }
        public bool CargaSolicitudesRetiroCliente(out string sError, string pagina)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SOLICITUD_RETIRO_02", out dt,out codError, out sError, pagina, GetUserName());
            if (!res)
            {
                return false;
            }


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

            this.solicitudretiroclienteJson = rows;
            return true;
        }
        public bool CargaSolicitudesRetiroClienteDetalle(out string sError, string pagina)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SOLICITUD_RETIRO_03", out dt,out codError, out sError, pagina);
            if (!res)
            {
                return false;
            }


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

            this.solicitudretiroclientedetalleJson = rows;
            return true;
        }
        public Boolean AprobarRechazarSolicitudRetiro(out string sError, string idsolicitudretiro, string accion)
        {

            sError = "";
            string codError = "";
            bool res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_SOLICITUD_RETIRO_01", out sError, idsolicitudretiro, accion, GetUserName());

            if (!res)
            {
                return false;
            }

            if (accion.ToString() == "0")
            {
                try
                {
                    Task t = Task.Factory.StartNew(() =>
                    {
                        DataTable dtt = null;
                        String sErrorst = "";
                        res = cDAL.Consultar("USP_SEL_ALERTAS_EMAIL_01", out dtt,out codError, out sErrorst, idsolicitudretiro);

                        string email, asunto, cuerpo;
                        EnviarMail oMail = new EnviarMail();
                        foreach (DataRow row in dtt.Rows)
                        {
                            email = row.ItemArray[0].ToString();
                            asunto = row.ItemArray[1].ToString();
                            cuerpo = row.ItemArray[2].ToString();
                            oMail.enviar(email, asunto, cuerpo);
                        }
                    });
                }
                catch (Exception e) { }
            }


            return true;
        }
        public bool CargaDetalleInfraestructura(string pagina, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SOLICITUD_RETIRO_04", out dt,out codError, out sError, pagina, GetUserName());
            if (!res)
            {
                return false;
            }


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

            this.solicitudesinfraestructuraJson = rows;
            return true;
        }
        public bool CargaInfraestructuraSolicitud(string idsolicitudretiro, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError="";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SOLICITUD_RETIRO_05", out dt,out codError, out sError, idsolicitudretiro);
            if (!res)
            {
                return false;
            }


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

            this.solicitudesjefeproyectoJson = rows;
            return true;
        }
        public Boolean RetirarSolicitudRetiro(out string sError, string idsolicitudretiro)
        {

            sError = "";
            bool res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_SOLICITUD_RETIRO_02", out sError, idsolicitudretiro, GetUserName());

            if (!res)
            {
                return false;
            }

            return true;
        }
        public byte[] getReporteSolicitudRetiro(string idsolicitud)
        {
            

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.LocalReport.ReportPath = "Content/Reportes/ReporteApruebaSolicitud.rdlc";
            reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

            SqlAccess comCotiz = new SqlAccess(dbConn);
            System.Data.DataTable dt = new DataTable();
            String msgError;
            string codError;

            comCotiz.Consultar("USP_SEL_REPORTE_APRUEBA_SOLICITUD_00", out dt,out codError, out msgError, idsolicitud, GetUserName());


            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DatosSolicitudRetiro", dt));


            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            String filenameExtension;
            reportViewer.LocalReport.Refresh();


            byte[] pdf = reportViewer.LocalReport.Render("pdf", null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);
            


            return pdf;

        }
    }
}