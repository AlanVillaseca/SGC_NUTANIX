using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class DetalleCotizacion
    {
        public string dbConn;
        public string usrCreacion { get; set; }
        public string cotVersion { get; set; }
        public string gestDesc { get; set; }
        public string gest { get; set; }
        public string idCotizacion { get; set; }
        public DataTable dtDetalleCotizacion { get; set; }
        public DataTable dtVersionCotizacion { get; set; }
        public DataTable dtImplementadores { get; set; }
        public DataTable dtProyectos { get; set; }
        public DataTable dtMotivos { get; set; }
        public DataTable dtGrilla { get; set; }
        public DataTable dtPlantillas { get; set; }
        public DataTable dtPermiso { get; set; }
        public DataTable dtValidarCentroCostoComp { get; set; }
        public DataTable dtValidarCentroCostoServ { get; set; }
        public DataTable dtValidarServicioComponenteCC { get; set; }

        public DataTable dtValidarCotizacion { get; set; }
        public List<Dictionary<object, string>> serviciosJson { get; set; }
        public List<Dictionary<object, string>> plantillasubserviciosJson { get; set; }
        public List<Dictionary<object, string>> plantillasubservicios2Json { get; set; }
        public List<Dictionary<object, string>> dtSubservicios { get; set; }

        public DetalleCotizacion()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }

        public DataTable validarCotizacion(string idcotizacion, string tipo, out string sErrors)
        {
            DataTable dtValidarCotizacionsp = new DataTable();
            string codError="";
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Consultar("USP_SEL_VALIDAR_COTIZACION", out dtValidarCotizacionsp,out codError, out sErrors, idcotizacion, tipo);
            if (!result)
            {
                return this.dtValidarCotizacion;
            }
            this.dtValidarCotizacion = dtValidarCotizacionsp;
            return this.dtValidarCotizacion;
        }

        public DataTable validarServicioComponenteCC(string idcotizacion, string id, string tipo, out string sErrors)
        {
            DataTable dtValidarServicioComponenteCCsp = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Consultar("USP_SEL_VALIDAR_CENTRO_COSTO", out dtValidarServicioComponenteCCsp,out codError, out sErrors, idcotizacion, id, tipo);
            if (!result)
            {
                return this.dtValidarServicioComponenteCC;
            }
            this.dtValidarServicioComponenteCC = dtValidarServicioComponenteCCsp;
            return this.dtValidarServicioComponenteCC;
        }

        public DataTable validarCentroCostoServicio(string idusuario, string idcotizacion, out string sErrors)
        {
            DataTable dtValidarCentroCostoServicio = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Consultar("USP_SEL_VALIDAR_CENTRO_COSTO_SER", out dtValidarCentroCostoServicio,out codError, out sErrors, idusuario, idcotizacion);
            if (!result)
            {
                return this.dtValidarCentroCostoServ;
            }
            this.dtValidarCentroCostoServ = dtValidarCentroCostoServicio;
            return this.dtValidarCentroCostoServ;
        }

        public DataTable validarCentroCostoComponente(string idusuario, string idcotizacion, out string sErrors)
        {
            DataTable dtValidarCentroCostoComponente = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Consultar("USP_SEL_VALIDAR_CENTRO_COSTO_COM", out dtValidarCentroCostoComponente,out codError, out sErrors, idusuario, idcotizacion);
            if (!result)
            {
                return this.dtValidarCentroCostoComp;
            }
            this.dtValidarCentroCostoComp = dtValidarCentroCostoComponente;
            return this.dtValidarCentroCostoComp;
        }

        public Boolean cargaDetalleCotizacion(string idcotizacion, string idversion, out string sErrors)
        {
            UsersAD usersAd = new UsersAD();
            DataTable dtSolitante = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_DETALLE_COTIZACIONES_00", out dtSolitante,out codError, out sErrors, idcotizacion, idversion);

            if (!result)
            {
                return false;
            }

            this.dtDetalleCotizacion = dtSolitante;
            return true;
        }
        public Boolean BorrarCotizacion(string idcotizacion,  out string sErrors)
        {
            UsersAD usersAd = new UsersAD();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_DEL_COTIZACION_00", out sErrors, idcotizacion);
            if (!result)
            {
                return false;
            }
            return true;
        }
        public Boolean cargaProyectosCopiar(out string sErrors)
        {
            UsersAD usersAd = new UsersAD();
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Consultar("USP_SEL_LISTA_PROYECTOS_08", out dt,out codError, out sErrors, GetUserName());
            if (!result)
            {
                return false;
            }
            this.dtProyectos = dt;
            return true;
        }
        public Boolean cargaPlantillasCopiar(out string sErrors)
        {
            UsersAD usersAd = new UsersAD();
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Consultar("USP_SEL_LISTA_PLANTILLAS_00", out dt,out codError, out sErrors);
            if (!result)
            {
                return false;
            }
            this.dtPlantillas = dt;

            return true;
        }

        public Boolean cargaVersionCotizacion(string idcotizacion, out string sErrors)
        {
            UsersAD usersAd = new UsersAD();
            DataTable dtSolitante = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_DETALLE_COTIZACIONES_01", out dtSolitante,out codError, out sErrors, idcotizacion);

            if (!result)
            {
                return false;
            }

            this.dtVersionCotizacion = dtSolitante;

            return true;
        }
        public Boolean cargarGrilla(string nropagina, string campoordenamiento, string id, string idversion, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            UsersAD usersAd = new UsersAD();
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_DETALLE_COTIZACIONES_02", out dt,out codError, out sError, nropagina, campoordenamiento, id, idversion);
            if (!res)
            {
                return false;
            }

            /*foreach (DataRow dr in dt.Rows)
            {
                dr[3] = usersAd.getUserFullName(dr.ItemArray.GetValue(3).ToString());
            }*/

            this.dtGrilla = dt;

            return true;
        }

        public Boolean gestionarCotizacion(string idcotizacion, string motivo, string gestion, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_UPD_DETALLE_COTIZACION_00", out dt,out codError, out sError, GetUserName(), idcotizacion, motivo, gestion);
            if (!res)
            {
                return false;
            }
            this.dtGrilla = dt;
            return true;
        }

       public Boolean CargaServicios(string idCotizacion, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SERVICIO_00", out dt,out codError, out sError, idCotizacion);
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

            this.serviciosJson = rows;
            return true;
        }


        public Boolean AgregarServicioCotizacion(string id, string idcotizacion, string cantidad, string cantidadMensual,
            out string sErrors, string descripcion, string tipomoneda)
        {
            sErrors = "";
            usrCreacion = GetUserName();
            DataTable dtSolitante = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Ejecutar("USP_INS_SERVICIO_01", out sErrors, id, idcotizacion, cantidad, cantidadMensual,
                usrCreacion, descripcion, tipomoneda);

            if (!result)
            {
                return false;
            }

            return true;
        }
        public Boolean AgregarServicioActividadCotizacion(List<iActividad> updCostoJson, string idcotizacion,
            string idservicio, string cantidad, out string sError)
        {

            sError = "";
            Boolean res;
            string xmlVar2 = "";
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(updCostoJson.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlVar = "";

            using (var stream = new System.IO.StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, updCostoJson, emptyNamepsaces);
                xmlVar = stream.ToString();
            }

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_SERVICIO_02", out sError, xmlVar, idcotizacion, idservicio, cantidad, GetUserName());

            if (!res)
            {
                return false;
            }

            return true;
        }
        public Boolean ActualizarServicioCotizacion(string id, string idVersion, string cantidad, string cantidadMensual,
            string tipomoneda, out string sErrors)
        {
            sErrors = "";
            usrCreacion = GetUserName();
            DataTable dtSolitante = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Ejecutar("USP_UPD_SERVICIO_00", out sErrors, id, idVersion, cantidad, cantidadMensual, usrCreacion, tipomoneda);

            if (!result)
            {
                return false;
            }

            return true;
        }
        public Boolean CopiaComponente(string id, out string sErrors)
        {
            sErrors = "";
            usrCreacion = GetUserName();
            DataTable dtSolitante = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Ejecutar("USP_INS_COPIA_COMPONETE_00", out sErrors, id, usrCreacion);

            if (!result)
            {
                return false;
            }

            return true;
        }
        public Boolean EliminarComponente(string id, out string sErrors)
        {
            sErrors = "";
            usrCreacion = GetUserName();
            DataTable dtSolitante = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_DEL_COMPONENTE_00", out sErrors, id, usrCreacion);
            if (!result)
            {
                return false;
            }
            return true;
        }
        public Boolean EliminarServicio(string id, string idVersion, out string sErrors)
        {
            sErrors = "";
            usrCreacion = GetUserName();
            DataTable dtSolitante = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_DEL_SERVICIO_00", out sErrors, id, idVersion, usrCreacion);
            if (!result)
            {
                return false;
            }
            return true;
        }
        public Boolean CambiarEstado(string id, string idaplicacion, string idaccion, string motivo, out string sErrors)
        {
            sErrors = "";
            usrCreacion = GetUserName(); //System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            DataTable dtSolitante = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_UPD_ESTADO_COT", out sErrors, id, idaplicacion, usrCreacion, idaccion, motivo);
            if (!result)
            {
                return false;
            }

            try
            {
                Task t = Task.Factory.StartNew(() =>
                {
                    Boolean res;
                    DataTable dtt = null;
                    String sErrorst = "";
                    string codError = "";
                    SqlAccess cDAL = new SqlAccess(dbConn);
                    res = cDAL.Consultar("USP_SEL_ALERTAS_EMAIL_00", out dtt,out codError, out sErrorst, id, "VER");

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
            } catch(Exception e) {}

            return true;
        }
        public Boolean AgregarCuotas(string id, string cuotas, out string sErrors)
        {
            sErrors = "";
            DataTable dtSolitante = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_UPD_COTIZACION_CUOTAS_00", out sErrors, id, cuotas, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());
            if (!result)
            {
                return false;
            }
            return true;
        }
        public Boolean Copiar(string id, string idProyecto, out string sErrors)
        {
            sErrors = "";
            usrCreacion = GetUserName(); //System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            DataTable dtSolitante = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_INS_COPIA_COTIZACION_00", out sErrors, id, idProyecto, usrCreacion);
            if (!result)
            {
                return false;
            }
            return true;
        }
        public Boolean CopiarMigrada(string id, string idProyecto,string idPlantilla, out string sErrors)
        {
            sErrors = "";
            usrCreacion = GetUserName(); //System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            DataTable dtSolitante = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_INS_COPIA_COTIZACION_MIG_00", out sErrors, id, idProyecto, idPlantilla, usrCreacion);
            if (!result)
            {
                return false;
            }
            return true;
        }
        public Boolean Versionar(string id, string comentarios, out string sErrors)
        {
            sErrors = "";
            usrCreacion = GetUserName(); //System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            DataTable dtSolitante = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_INS_VERSION_COT", out sErrors, id, comentarios, usrCreacion);
            if (!result)
            {
                return false;
            }
            Task t = Task.Factory.StartNew(() =>
            {
                try
                {
                    Boolean res;
                    DataTable dtt = null;
                    String sErrorst = "";
                    SqlAccess cDAL = new SqlAccess(dbConn);
                    string codError = "";
                    res = cDAL.Consultar("USP_SEL_ALERTAS_EMAIL_00", out dtt,out codError, out sErrorst, id, "VER");
                    string email, asunto, cuerpo;
                    EnviarMail oMail = new EnviarMail();
                    foreach (DataRow row in dtt.Rows)
                    {
                        email = row.ItemArray[0].ToString();
                        asunto = row.ItemArray[1].ToString();
                        cuerpo = row.ItemArray[2].ToString();
                        oMail.enviar(email, asunto, cuerpo);
                    }
                    
                } catch(Exception e) {}
            });
            return true;
        }
        public Boolean cargaImplementadores(out string sErrors)
        {
            DataTable dt = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            string codError = "";
            Boolean result = comCotiz.Consultar("USP_SEL_IMPLEMENTADORES_00", out dt,out codError, out sErrors);
            if (!result)
            {
                return false;
            }
            this.dtImplementadores = dt;
            return true;
        }
        
        public Boolean cargaMensajes(string id, out string sErrors)
        {
            sErrors = "";
            usrCreacion = GetUserName();
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Consultar("USP_SEL_MOTIVOS_00",out dt,out codError, out sErrors, id, usrCreacion);
            if (!result)
            {
                return false;
            }
            this.dtMotivos = dt;
            return true;
        }

        public Boolean UpdateCotizacion(string id, string descripcion, string fecha, out string sError)
        {
            Boolean res;
            SqlAccess comCotiz = new SqlAccess(dbConn);
            string usrCre = GetUserName();

            sError = "";

            res = comCotiz.Ejecutar("USP_UPD_COTIZACION_01", out sError, id, descripcion, fecha, usrCre);

            return true;
        }

        public Boolean SolicitarDescuento(string id, string comentarios, out string sErrors)
        {
            sErrors = "";
            String usrCreacion = GetUserName(); //System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            DataTable dtSolitante = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_UPD_ESTADO_COT_01", out sErrors, id, comentarios, "17", usrCreacion);
            if (!result)
            {
                return false;
            }
            Task t = Task.Factory.StartNew(() => {
                                                    Boolean res;
                                                    DataTable dtt = null;
                                                    string codError = "";
                                                    String sErrorst = "";
                                                    SqlAccess cDAL = new SqlAccess(dbConn);
                                                    res = cDAL.Consultar("USP_SEL_ALERTAS_EMAIL_00", out dtt,out codError, out sErrorst, id, "VER");
                                                 
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
        public Boolean Descuento(string id, string comentarios, string descuento, out string sErrors)
        {
            sErrors = "";
            String usrCreacion = GetUserName(); //System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            DataTable dtSolitante = new DataTable();
           
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_UPD_ESTADO_COT_02", out sErrors, id, comentarios, "4", usrCreacion, descuento);
            if (!result)
            {
                return false;
            }
            return true;
        }
        public Boolean CargaServicios(string idservicio, string idversion, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SERVICIO_02", out dt,out codError, out sError, idservicio, idversion);
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

            this.dtSubservicios = rows;
            return true;
        }
        public Boolean CargaSubservicios(string pagina, string idservicio, string idversion, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SUBSERVICIO_02", out dt,out codError, out sError, pagina, idservicio, idversion);
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

            this.dtSubservicios = rows;
            return true;
        }
        public Boolean CargaPlantillaSubservicios(string idservicio, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_PLANTILLA_SUBSERVICIO_03", out dt,out codError, out sError, idservicio);
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

            this.plantillasubserviciosJson = rows;
            return true;
        }
        public Boolean CargaPlantillaSubservicios2(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;

            SqlAccess cDAL = new SqlAccess(dbConn);
            string codError = "";
            res = cDAL.Consultar("USP_SEL_PLANTILLA_SUBSERVICIO_02", out dt,out codError, out sError);
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

            this.plantillasubservicios2Json = rows;
            return true;
        }
        public Boolean AgregarSubserviciosPlantilla(string idservicio, string idplantillasubservicio, string idversion, out string sError)
        {

            sError = "";
            Boolean res;
            SqlAccess cDAL = new SqlAccess(dbConn);

            res = cDAL.Ejecutar("USP_INS_SUBSERVICIO_01", out sError, idservicio, idplantillasubservicio, idversion);
            if (!res)
            {

                return false;
            }

            return true;
        }
        public Boolean AgregarSubservicios(string idservicio, string idversion, string nombre, string horas, out string sError)
        {

            sError = "";
            Boolean res;
            SqlAccess cDAL = new SqlAccess(dbConn);

            res = cDAL.Ejecutar("USP_INS_SUBSERVICIO_02", out sError, idservicio, idversion, nombre, horas);
            if (!res)
            {

                return false;
            }

            return true;
        }
        public Boolean EliminarSubservicio(string idservicio, string idversion, out string sError)
        {

            sError = "";
            Boolean res;
            SqlAccess cDAL = new SqlAccess(dbConn);

            res = cDAL.Ejecutar("USP_DEL_SUBSERVICIO_01", out sError, idservicio, idversion);
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

            res = cDAL.Ejecutar("USP_UPD_SUBSERVICIO_01", out sError, idsubservicio, nombre, horas);
            if (!res)
            {

                return false;
            }

            return true;
        }
        public Boolean cargaPermisoCotizacion(string idcotizacion, out string sErrors)
        {
            UsersAD usersAd = new UsersAD();
            DataTable dtSolitante = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_VERIFICA_PERMISO_COTIZACION", out dtSolitante, out codError, out sErrors, GetUserName(), idcotizacion);

            if (!result)
            {
                return false;
            }

            this.dtPermiso = dtSolitante;
            return true;
        }

    }

}