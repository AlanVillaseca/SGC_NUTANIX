using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class SolicitudRetiro
    {
        public string JsonComponentes;
        public string dbConn;
        public string JsonListaSolicitud;
        public string JsonComponentesLista;
        public string JsonVerificarSolicitud;
        public string JsonComponentesRetiro;
        public string JsonSolicitudesRetirar;
        public string JsonRetiroComponentes;
        public string JsonListaEstadoSolicitud;
        public string JsonEstadoSolicitudes;
        public string JsonDetalleEstadoSolicitudRetiro;
        public string JsonPerfilUsuarioSolicitud;
        public string JsonHistorialSolicitud;
        public string JsonReporteRetiro;
        public string JsonListaReporteRetiro;
        
        public List<Dictionary<object, string>> detalleretirosolicitudJson { get; set; }
        public SolicitudRetiro()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }
        public bool Buscar(string cadena, string flag, out string sError)
        {
            //sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_COMPONENTES_09", out dt,out codError, out sError, cadena, flag, GetUserName());
            if (!res)
            {
                return false;
            }

            JsonComponentes = JsonConvert.SerializeObject(dt);  

            return true;
        }//lista de componentes para iniciar la solicitud de retiro

        public bool Guardar(string componentes, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_INS_SOLICITUD_RETIRO", out dt,out codError, out sError, componentes, GetUserName());
            if (!res)
            {
                return false;
            }

            JsonComponentes = JsonConvert.SerializeObject(dt);

            
            string IdSolicitud = dt.Rows[0]["IdSolicitud"].ToString(); 
            
            Task t = Task.Factory.StartNew(() =>
            {

                try
                {
                    Boolean resp;
                    DataTable dtt = null;
                    
                    String sErrorst = "";
                    String codErrors = "";
                    string id = IdSolicitud;
                    SqlAccess coDAL = new SqlAccess(dbConn);
                    resp = coDAL.Consultar("USP_SEL_ALERTAS_EMAIL_00", out dtt,out codErrors, out sErrorst, id, "RET ");
                    string email, asunto, cuerpo;
                    EnviarMail oMail = new EnviarMail();
                    foreach (DataRow row in dtt.Rows)
                    {
                        email = row.ItemArray[0].ToString();
                        asunto = row.ItemArray[1].ToString();
                        cuerpo = row.ItemArray[2].ToString();
                        oMail.enviar(email, asunto, cuerpo);
                    }

                }
                catch (Exception) { }
            });
            

            return true;
        }//fin metodo guardar... insertar los regitros seleccionados por el cliente para ser retirados

        public bool ListaSolicitud(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_COMPONENTES", out dt,out codError, out sError);
            if (!res)
            {
                return false;
            }

            JsonListaSolicitud = JsonConvert.SerializeObject(dt);

            return true;
        }//fin metodo ListaSolicitud... muestra la lista de componentes que el facturador debera agregar la cuota de retiro

        public bool CuotaSalida(string id, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_DETALLE_SOLICITUD_RETIRO", out dt,out codError, out sError, id, GetUserName());
            if (!res)
            {
                return false;
            }

            JsonComponentesLista = JsonConvert.SerializeObject(dt);

            return true;
        }//fin metodo buscar... muestra el detalle de la solicitud seleccionada por el facturador para colocar la cuota de retiro


        public bool CuotaRetiro(string id, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_DETALLE_SOLICITUD_RETIRO_02", out dt,out codError, out sError, id, GetUserName());
            if (!res)
            {
                return false;
            }

            JsonComponentesRetiro = JsonConvert.SerializeObject(dt);

            return true;
        }//fin metodo buscar... muestra el detalle de la solicitud seleccionada por el facturador para colocar la cuota de retiro


        public bool GuardarCuota(string id,  string cuota, string numcuota, out string sError)
        {
            sError = "";
            Boolean res;
            

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_DETALLE_SOLICITUD_RETIRO_04", out sError, id, cuota, numcuota, GetUserName());
            if (!res)
            {
                return false;
            }

            return true;
        }//fin metodo GuardarCuota... permite guardar la cuota asiganada al componente que se va a retirar 

        
        public bool GuardarComentario(string idComponente, string idSolicitud, string comentario, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_DETALLE_SOLICITUD_RETIRO_05", out sError, idComponente, idSolicitud,  comentario);
            if (!res)
            {
                return false;
            }

            Task t = Task.Factory.StartNew(() =>
            {
                try
                {
                    Boolean resp;
                    DataTable dtt = null;
                    String sErrorst = "";
                    string codError = "";
                    SqlAccess coDAL = new SqlAccess(dbConn);
                    resp = coDAL.Consultar("USP_SEL_ALERTAS_EMAIL_00", out dtt, out codError, out sErrorst, idSolicitud, "RET ");
                    string email, asunto, cuerpo;
                    EnviarMail oMail = new EnviarMail();
                    foreach (DataRow row in dtt.Rows)
                    {
                        email = row.ItemArray[0].ToString();
                        asunto = row.ItemArray[1].ToString();
                        cuerpo = row.ItemArray[2].ToString();
                        oMail.enviar(email, asunto, cuerpo);
                    }

                }
                catch (Exception) { }
            });



            return true;
        }//fin Motivo Rechazo... permite insertar el motivo de rechazo


        public bool RetiroComponentes(string id, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_DETALLE_SOLICITUD_RETIRO_04", out dt, out codError, out sError, id, GetUserName());
            if (!res)
            {
                return false;
            }

            JsonRetiroComponentes = JsonConvert.SerializeObject(dt);

            return true;
        }//permite visualizar el detalle da la solicitud al finalizar la solicitud de retiro



        public bool GuardarAprobacion(string id, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_DETALLE_SOLICITUD_RETIRO_02", out sError, id, GetUserName());
            if (!res)
            {
                return false;
            }

            Task t = Task.Factory.StartNew(() =>
            {
                try
                {
                    Boolean resp;
                    DataTable dtt = null;
                    String sErrorst = "";
                    string codError = "";
                    SqlAccess coDAL = new SqlAccess(dbConn);
                    resp = coDAL.Consultar("USP_SEL_ALERTAS_EMAIL_00", out dtt,out codError, out sErrorst, id, "RET ");
                    string email, asunto, cuerpo;
                    EnviarMail oMail = new EnviarMail();
                    foreach (DataRow row in dtt.Rows)
                    {
                        email = row.ItemArray[0].ToString();
                        asunto = row.ItemArray[1].ToString();
                        cuerpo = row.ItemArray[2].ToString();
                        oMail.enviar(email, asunto, cuerpo);
                    }

                }
                catch (Exception) { }
            });



            return true;
        }//... permite insertar la aprobacion de la cuota por parte del cliente, cambia estado a 3


        public bool GuardarCuotaAprobacion(string id, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_DETALLE_SOLICITUD_RETIRO_03", out sError, id, GetUserName());
            if (!res)
            {
                return false;
            }

            JsonComponentes = JsonConvert.SerializeObject(dt);

            Task t = Task.Factory.StartNew(() =>
            {
                try
                {
                    Boolean resp;
                    DataTable dtt = null;
                    String sErrorst = "";
                    string codError = "";
                    SqlAccess coDAL = new SqlAccess(dbConn);
                    resp = coDAL.Consultar("USP_SEL_ALERTAS_EMAIL_00", out dtt,out codError, out sErrorst, id, "RET ");
                    string email, asunto, cuerpo;
                    EnviarMail oMail = new EnviarMail();
                    foreach (DataRow row in dtt.Rows)
                    {
                        email = row.ItemArray[0].ToString();
                        asunto = row.ItemArray[1].ToString();
                        cuerpo = row.ItemArray[2].ToString();
                        oMail.enviar(email, asunto, cuerpo);
                    }

                }
                catch (Exception) { }
            });



            return true;
        }//SP permite cambiar el estatus de la solicitud indicando que se agrego la cuota


        public bool GuardarRetiro(string id, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_DETALLE_SOLICITUD_RETIRO_01", out sError, id, GetUserName());
            if (!res)
            {
                return false;
            }

            Task t = Task.Factory.StartNew(() =>
            {
                try
                {
                    Boolean resp;
                    DataTable dtt = null;
                    String sErrorst = "";
                    string codError = "";
                    SqlAccess coDAL = new SqlAccess(dbConn);
                    resp = coDAL.Consultar("USP_SEL_ALERTAS_EMAIL_00", out dtt,out codError, out sErrorst, id, "RET ");
                    string email, asunto, cuerpo; 
                    EnviarMail oMail = new EnviarMail();
                    foreach (DataRow row in dtt.Rows)
                    {
                        email = row.ItemArray[0].ToString();
                        asunto = row.ItemArray[1].ToString();
                        cuerpo = row.ItemArray[2].ToString();
                        oMail.enviar(email, asunto, cuerpo);
                    }

                }
                catch (Exception) { }
            });


            return true;
        }//SP que realiza las actualizaciones de las tablas que intervienen en el proceso de retiro. Solicitud retirada

        public bool ListaEstadoSolicitud(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_COMPONENTES_002", out dt,out codError, out sError, GetUserName());
            if (!res)
            {
                return false;
            }
                   
            JsonListaEstadoSolicitud = JsonConvert.SerializeObject(dt);

            return true;
        }//SP trae la lista de solicitudes mostrando el estado

        public bool ListaHistorialSolicitud(string id, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_COMPONENTES_003", out dt,out codError, out sError, id);
            if (!res)
            {
                return false;
            }

            JsonHistorialSolicitud = JsonConvert.SerializeObject(dt);

            return true;
        }//fin metodo ListaSolicitud... muestra la lista de componentes que el facturador debera agregar la cuota de retiro


        public bool DetalleSolicitudSoloLectura(string id, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_DETALLE_SOLICITUD_RETIRO_11", out dt,out codError, out sError, id, GetUserName());
            if (!res)
            {
                return false;
            }

            JsonDetalleEstadoSolicitudRetiro = JsonConvert.SerializeObject(dt);

            return true;
        }//SP que muestra el detalle de la solicitud, muestra los datos solo lectura


        public DataTable ListaReporteSolicitudesRetiro(out string msgError, string f_desde, string f_hasta)
        {

            DataTable table = new DataTable();
            string codError = "";
            SqlAccess cdal = new SqlAccess(dbConn);

            cdal.Consultar("USP_SEL_LISTA_COMPONENTES_11", out table,out codError, out msgError, f_desde, f_hasta);

            return table;
        }//SP trae la lista de componentes para visualizar el reporte de solicitudes de retiro

    }
}