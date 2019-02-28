using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class DetalleProyecto
    {
        public string dbConn;
        public string usrCreacion { get; set; }
        public DataTable dtImplementadores { get; set; }
        public DataTable dtDetalleProyecto { get; set; }
        public DataTable dtCotizaciones { get; set; }
        public DataTable dtJefeDeProyectos { get; set; }
        public DataTable dtNegocios { get; set; }
        public DataTable dtCentroCostos { get; set; }
        public DataTable dtProrrata { get; set; }
        public DataTable dtGerencias { get; set; }
        public DataTable dtFacturacion { get; set; }
        public string id { get; set; }
        public string descCotizacion { get; set; }
        public string usuarioCreador { get; set; }
        public List<Dictionary<object, string>> solicitanteCombo { get; set; }
        public List<Dictionary<object, string>> negocioCombo { get; set; }
        public List<Dictionary<object, string>> servicioNegocioCombo { get; set; }
        public string fileAdjunto { get; set; }
        
        public DetalleProyecto()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;

        }
        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }
        public Boolean cargaDetalleProyecto(string idproyecto, out string sErrors)
        {

            UsersAD usersAd = new UsersAD();
            DataTable dtSolitante = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_DETALLE_PROYECTO_00", out dtSolitante,out codError, out sErrors, idproyecto);

            if (!result)
            {
                return false;
            }

            /*
            foreach (DataRow dr in dtSolitante.Rows)
            {
                dr[4] = usersAd.getUserFullName(dr.ItemArray.GetValue(4).ToString());
                dr[6] = usersAd.getUserFullName(dr.ItemArray.GetValue(6).ToString());
            }
            */
            this.id = idproyecto;
            if (dtSolitante == null)
            {
                this.dtDetalleProyecto = new DataTable();
            }
            else
            {
            this.dtDetalleProyecto = dtSolitante;
            }

            return true;
        }

        public Boolean cargaFileAdjunto(string idproyecto, out string sErrors)
        {

            UsersAD usersAd = new UsersAD();
            DataTable dtSolitante = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_DETALLE_PROYECTO_00", out dtSolitante,out codError, out sErrors, idproyecto);

            if (!result)
            {
                return false;
            }

            /*
            foreach (DataRow dr in dtSolitante.Rows)
            {
                dr[4] = usersAd.getUserFullName(dr.ItemArray.GetValue(4).ToString());
                dr[6] = usersAd.getUserFullName(dr.ItemArray.GetValue(6).ToString());
            }
            */
            this.id = idproyecto;
            if (dtSolitante == null)
            {
                this.dtDetalleProyecto = new DataTable();
            }
            else
            {
                this.fileAdjunto = dtSolitante.Rows[0].ItemArray.GetValue(20).ToString();
            }

            return true;
        }
        public Boolean cargaModGrillaDetalle(string id, string campo, string pagina, out string sErrors)
        {
            Boolean res;
            UsersAD usersAd = new UsersAD();
            DataTable dtCotizacionesSp = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_LISTA_COTIZACIONES_03", out dtCotizacionesSp,out codError, out sErrors,
                                                pagina, campo, id);

            if (!result)
            {
                return false;
            }

            this.id = id;
            this.dtCotizaciones = dtCotizacionesSp;
            res = comCotiz.Consultar("USP_SEL_LISTA_PROYECTOS_02", out dtCotizacionesSp,out codError, out sErrors);
            if (!res)
                return false;
            this.dtNegocios = dtCotizacionesSp;
            res = comCotiz.Consultar("USP_SEL_LISTA_CENTRO_COSTOS_00", out dtCotizacionesSp,out codError, out sErrors);
            if (!res)
                return false;
            this.dtCentroCostos = dtCotizacionesSp;
            res = comCotiz.Consultar("USP_SEL_LISTA_PRORRATEO_00", out dtCotizacionesSp,out codError, out sErrors, id);
            if (!res)
                return false;
            this.dtProrrata = dtCotizacionesSp;
            res = comCotiz.Consultar("USP_SEL_GERENCIAS_00", out dtCotizacionesSp, out codError, out sErrors);
            if (!res)
                return false;
            this.dtGerencias = dtCotizacionesSp;

            return true;
        }
        public Boolean guardarNuevaCotizacion(out string sErrors)
        {
            Boolean res;
            SqlAccess comCotiz = new SqlAccess(dbConn);
            string usrCre = GetUserName();

            sErrors = "";

            res = comCotiz.Ejecutar("USP_INS_DETALLE_PROYECTOS_00", out sErrors, id, descCotizacion, usrCre);

            return true;
        }
        public Boolean cargaJDP(out string sErrors)
        {
            Boolean res;
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);
            string usrCre = GetUserName();
            sErrors = "";
            res = comCotiz.Consultar("USP_SEL_JEFEDEPROYECTOS_00", out dt,out codError, out sErrors);
            if (!res)
            {
                return false;
            }
            this.dtJefeDeProyectos = dt;
            return true;
        }
		
		public Boolean bloquearProyecto(string id, out string sErrors)
        {
            Boolean res;
            SqlAccess comCotiz = new SqlAccess(dbConn);
            string usrCre = GetUserName();

            sErrors = "";

            res = comCotiz.Ejecutar("USP_UPD_ESTADO_PROYECTTO_00", out sErrors, id);

            return true;
        }
        public Boolean desbloquearProyecto(string id, out string sErrors)
        {
            Boolean res;
            SqlAccess comCotiz = new SqlAccess(dbConn);
            string usrCre = GetUserName();

            sErrors = "";

            res = comCotiz.Ejecutar("USP_UPD_ESTADO_PROYECTTO_01", out sErrors, id);

            return true;
        }

        /************************************************************************/
        /*                           vpGrillaDetalle                            */
        /************************************************************************/

        public bool CargaComboModalNegocio(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_PROYECTOS_02", out dt,out codError, out sError);
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

            this.negocioCombo = rows;
            return true;
        }

        public bool CargaComboModalServicioNegocio(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_PROYECTOS_04", out dt, out codError, out sError);
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

            this.servicioNegocioCombo = rows;
            return true;
        }

        public bool UpdateProyecto(string id, string codigo, string nombre, string descripcion, string idSolicitante, string idNegocio, string idPais, string idServicioNegocio, string xmlProrrateo, string cliente, string ruta, out string sError)
        {
            Boolean res;
            SqlAccess comCotiz = new SqlAccess(dbConn);
            string usrCre = GetUserName();

            xmlProrrateo = xmlProrrateo.Replace("!**", "<");
            xmlProrrateo = xmlProrrateo.Replace("**!", ">");
            sError = "";

            res = comCotiz.Ejecutar("USP_UPD_PROYECTO_01", out sError, id, codigo, nombre, descripcion, idSolicitante, idNegocio, idPais, idServicioNegocio, usrCre, xmlProrrateo, cliente, ruta);

            return true;
        }
        public String VerFacturacion(string idproyecto, out string sError)
        {
            string value;

            UsersAD usersAd = new UsersAD();
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_FACTURACION_PROYECTO", out dt,out codError, out sError, idproyecto);

            if (!result)
            {
                return "0";
            }

            this.dtFacturacion = dt;

            value = dtFacturacion.Rows[0].ItemArray.GetValue(0).ToString();

            return value;
        }
        public Boolean EliminarProyecto(string idproyecto, out string sError)
        {
            UsersAD usersAd = new UsersAD();
            DataTable dt = new DataTable();
           
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Ejecutar("USP_DEL_MIGRA_COTZCNS_00", out sError, idproyecto);

            if (!result)
            {
                return false;
            }

            return true;
        }
        public Boolean AsignarImplementadorCotizacion(string id, string idImp, out string sErrors)
        {
            sErrors = "";
            usrCreacion = GetUserName();
            DataTable dtSolitante = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_UPD_COTIZACION_00", out sErrors, id, idImp, usrCreacion);
            if (!result)
            {
                return false;
            }

            Task t = Task.Factory.StartNew(() =>
            {
                Boolean res;
                DataTable dtt = null;
                String sErrorst = "";
                string codError = "";
                SqlAccess cDAL = new SqlAccess(dbConn);
                res = cDAL.Consultar("USP_SEL_ALERTAS_EMAIL_00", out dtt,out codError, out sErrorst, id);

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


            return true;
        }
        public Boolean cargaImplementadores(out string sErrors)
        {
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Consultar("USP_SEL_IMPLEMENTADORES_00", out dt,out codError, out sErrors);

            if (!result)
            {
                return false;
            }
            this.dtImplementadores = dt;

            return true;
        }
    }
}