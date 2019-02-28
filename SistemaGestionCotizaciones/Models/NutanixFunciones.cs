using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Security.Claims;
using System.Web;
using Atech.DataAccessLayer;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace SistemaGestionCotizaciones.Models
{
    public class NutanixFunciones
    {
        //Variables Generales
        public string dbConn;
        public string dtProyectosNutanixJson { get; set; }
        public string dtCotizacionesNutanixJson { get; set; }
        public DataSet dtnutanixJson { get; set; }
        public DataSet dtGrilla { get; set; }

        public NutanixFunciones()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }

        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }

        //Parametros y Armado de WebRequest
        public string llamadaAPICreaVM(string _jsonFinal)
        {
            string url = "https://ntxprismctral.falabella.cl:9440/api/nutanix/v3/vms";
            string res = "";
            string autorization = "pesilva@falabella.com" + ":" + "Campeon2019";
            byte[] binaryAuthorization = System.Text.Encoding.UTF8.GetBytes(autorization);
            autorization = Convert.ToBase64String(binaryAuthorization);
            autorization = "Basic " + autorization;

            try
            {
                //soporte conexion SSL
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Headers.Add("Authorization", autorization);
                webRequest.Method = "POST";
                webRequest.Accept = "application/json";
                webRequest.ContentType = "application/json";
                webRequest.Headers["x-nutanix-client-type"] = "ui";
                webRequest.Headers["access-control-allow-origin"] = "*";
                webRequest.Headers["access-control-allow-headers"] = "origin, x-requested-with, content-type, accept";

                using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    streamWriter.Write(_jsonFinal);
                }

                HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse;
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    res = reader.ReadToEnd();
                }

                return res;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //Llamada URL de Task para obtener UUID DE VM
        public string llamadaAPI_Task(string _taskUUID)
        {
            string url = "https://ntxprismctral.falabella.cl:9440/api/nutanix/v3/tasks/" + _taskUUID;
            string res = "";
            string autorization = "pesilva@falabella.com" + ":" + "Campeon2019";
            byte[] binaryAuthorization = System.Text.Encoding.UTF8.GetBytes(autorization);
            autorization = Convert.ToBase64String(binaryAuthorization);
            autorization = "Basic " + autorization;

            try
            {
                //soporte conexion SSL
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Headers.Add("Authorization", autorization);
                webRequest.Method = "GET";
                webRequest.Accept = "application/json";
                webRequest.ContentType = "application/json";
                webRequest.Headers["x-nutanix-client-type"] = "ui";
                webRequest.Headers["access-control-allow-origin"] = "*";
                webRequest.Headers["access-control-allow-headers"] = "origin, x-requested-with, content-type, accept";

                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    res = reader.ReadToEnd();
                }

                return res;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //Llamada URL de VM para obtener Nombre de Maquina
        public string llamadaAPI_IP(string _vmUUID)
        {
            string url = "https://ntxprismctral.falabella.cl:9440/api/nutanix/v3/vms/" + _vmUUID;
            string res = "";
            string autorization = "pesilva@falabella.com" + ":" + "Campeon2019";
            byte[] binaryAuthorization = System.Text.Encoding.UTF8.GetBytes(autorization);
            autorization = Convert.ToBase64String(binaryAuthorization);
            autorization = "Basic " + autorization;

            try
            {
                //soporte conexion SSL
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Headers.Add("Authorization", autorization);
                webRequest.Method = "GET";
                webRequest.Accept = "application/json";
                webRequest.ContentType = "application/json";
                webRequest.Headers["x-nutanix-client-type"] = "ui";
                webRequest.Headers["access-control-allow-origin"] = "*";
                webRequest.Headers["access-control-allow-headers"] = "origin, x-requested-with, content-type, accept";

                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    res = reader.ReadToEnd();
                }

                return res;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //Parametrización por Componente
        public string parametrizacionComponente(NutanixEntrada nutanix,int idComponente) {
            string sErros = "", sError = "";
            string ambito = "", salida = "";
            string json = "", jsonFinal = "";
            string script = "", scriptFinal = "", base64 ="";

            NutanixFunciones nfun = new NutanixFunciones();
            NutanixParametros nparam = new NutanixParametros();
            NutanixPlantillas nplan = new NutanixPlantillas();

            nparam = nfun.cargaDetalleComponente(idComponente, out sError);

            string[] dbConfig = nutanix.charSet == null  ? new string[]{"","",""} : nutanix.charSet.Split('-');
            string charSet = dbConfig[0] != "" ? dbConfig[0] : "";
            string natcharSet = dbConfig[1] != "" ? dbConfig[1] : "";
            string blockSize = dbConfig[2] != "" ? dbConfig[2] : "";

            if (nparam.idTipo == 105)
            {
                if (nparam.idAmbiente == 22)
                {
                    ambito = "PRODUCTIVO";
                }
                else
                    ambito= "NO_PRODUCTIVO";
            }
            else
            {
                ambito = "APP";
            }

            nplan = cargaJsonNutanix(out sErros, out json, out script, ambito, nutanix.sistemaOperativo, nparam.idTipo.ToString(), nutanix.rolSet);
            scriptFinal = armarScript(nplan.plantillaScript, nutanix.dbName, charSet,natcharSet,blockSize);
            base64 = EncodeTo64SL(scriptFinal);
            jsonFinal = armarJson(nplan.plantillaJson, nutanix.sistemaOperativo, nparam.ramComp.ToString(), nparam.vcoresComp.ToString(), nutanix.vlanSec, base64);

            salida = llamadaAPICreaVM(jsonFinal);

            insertaNutanixRequest(idComponente, jsonFinal, scriptFinal, base64, out sError);
            insertaNutanixResponse(idComponente, salida, "", "PENDING", out sError);

            return salida;

        }

        //Inserta Nutanix Request
        public bool insertaNutanixRequest(int idComponente, string jsonRequest, string scriptRequest, string base64Request, out string sErrors)
        {

            sErrors = "";
            string usrCreacion = GetUserName();
            SqlAccess nutPeticion = new SqlAccess(dbConn);

            Boolean result = nutPeticion.Ejecutar("USP_INS_NUTANIX_REQUEST", out sErrors, idComponente.ToString(), jsonRequest, scriptRequest, base64Request, usrCreacion);

            if (!result)
            {
                return false;
            }

            return true;
        }

        //Inserta Nutanix Response
        public bool insertaNutanixResponse(int idComponente, string apiResponse, string estadoResponse, string errorResponse, out string sErrors)
        {

            sErrors = "";
            string usrCreacion = GetUserName();
            SqlAccess nutRespuesta = new SqlAccess(dbConn);

            Boolean result = nutRespuesta.Ejecutar("USP_INS_NUTANIX_RESPONSE", out sErrors, idComponente.ToString(), apiResponse, estadoResponse, errorResponse, usrCreacion);

            if (!result)
            {
                return false;
            }

            return true;
        }

        //Llenado de Tabla Proyectos
        public bool cargaProyectoNutanix(out string sErrors)
        {
            sErrors = "";
            bool res;
            List<string> st = new List<string>();
            string msgError = "";
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);

            Dictionary<string, object> parametro = new Dictionary<string, object>();
            parametro.Add("@Usuario", GetUserName());

            res = cDAL.ConsultarJson("[USP_SEL_NUTANIX_LISTA_PROYECTOS]", out st, out codError, out msgError, parametro); // agregar usuario a la consulta 
            if (!res)
            {
                return false;
            }

            this.dtProyectosNutanixJson = st[0];
            return true;
        }

        //Llenado de Tabla Cotizaciones
        public bool cargaCotizacionNutanix(out string sErrors, out string json, string idproyecto)
        {
            sErrors = "";
            json = "";
            Boolean res;
            List<string> st = new List<string>();
            string msgError = "";
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);

            Dictionary<string, object> parametro = new Dictionary<string, object>();
            parametro.Add("@idProyecto", idproyecto);

            res = cDAL.ConsultarJson("[USP_SEL_NUTANIX_LISTA_COTIZACIONES]", out st, out codError, out msgError, parametro);
            if (!res)
            {
                return false;
            }

            json = st[0];

            return true;
        }

        //Llenado de Tabla Componentes
        public bool cargaComponentesNutanix(out string sErrors, out string json, string idcotizacion)
        {
            sErrors = "";
            json = "";
            Boolean res;
            List<string> st = new List<string>();
            string msgError = "";
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);

            Dictionary<string, object> parametro = new Dictionary<string, object>();
            parametro.Add("@idCotizacion", idcotizacion);

            res = cDAL.ConsultarJson("[USP_SEL_NUTANIX_VISTA_COMPONENTES]", out st, out codError, out msgError, parametro);
            if (!res)
            {
                return false;
            }

            json = st[0];

            return true;
        }

        //Llenado de Tabla de Detalle Componentes
        public NutanixParametros cargaDetalleComponente(int idComponente, out string sErrors)
        {
            Boolean res;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            DataSet dtGrilla = new DataSet();
            Dictionary<string, object> parametro = new Dictionary<string, object>();
            parametro.Add("@idcomponente", idComponente);

            res = cDAL.ConsultarDS("USP_SEL_NUTANIX_DETALLE_COMPONENTE", out dtGrilla, out codError, out sErrors, parametro);

            NutanixParametros salida = new NutanixParametros();
            foreach (DataRow row in dtGrilla.Tables[0].Rows)
            {
                salida.idCotizacion = int.Parse(row["idcotizacion"].ToString());
                salida.idProyecto = int.Parse(row["idproyecto"].ToString());
                salida.plantillaComp = row["plantilla"].ToString();
                salida.descripcionComp = row["descripcion"].ToString();
                salida.catCosto = row["categoriacosto"].ToString();
                salida.estadoComp = row["estado"].ToString();
                salida.idComponente = int.Parse(row["idcomponente"].ToString());
                salida.idAmbiente = int.Parse(row["idambiente"].ToString());
                salida.ambienteComp = row["ambiente"].ToString().Trim();
                salida.idTipo = int.Parse(row["idtipo"].ToString());
                salida.esAplicacion = row["aplicacion"].ToString();
                salida.vcoresComp = int.Parse(row["vcores"].ToString());
                salida.ramComp = int.Parse(row["ram"].ToString());
                salida.idNegocio = int.Parse(row["idnegocio"].ToString());
                salida.negocio = row["negocio"].ToString();
            }

            return salida;
        }

        //Llenado de Tabla Panel de Control
        public bool cargaListaPanel(out string sErrors, out string json)
        {
            sErrors = "";
            json = "";
            Boolean res;
            List<string> st = new List<string>();
            string usuario = GetUserName();
            string msgError = "";
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);

            Dictionary<string, object> parametro = new Dictionary<string, object>();
            parametro.Add("@usuario", usuario);

            res = cDAL.ConsultarJson("[USP_SEL_NUTANIX_PANEL_CONTROL]", out st, out codError, out msgError, parametro);
            if (!res)
            {
                return false;
            }

            json = st[0];

            return true;
        }

        //Llenado de Detalles Panel de Control
        public bool cargaPanelDetalles(out string sErrors, out string json, string idcomponente)
        {
            sErrors = "";
            json = "";
            Boolean res;
            List<string> st = new List<string>();
            string usuario = GetUserName();
            string msgError = "";
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);

            Dictionary<string, object> parametro = new Dictionary<string, object>();
            parametro.Add("@usuario", usuario);
            parametro.Add("@componente", idcomponente);

            res = cDAL.ConsultarJson("[USP_SEL_NUTANIX_PANEL_DETALLES]", out st, out codError, out msgError, parametro);
            if (!res)
            {
                return false;
            }

            json = st[0];

            return true;
        }

        //Armado Json y Script
        public NutanixPlantillas cargaJsonNutanix(out string sErrors, out string json, out string script, string ambito, string so, string tipo, string rol)
        {

            sErrors = "";
            json = "";
            script = "";
            Boolean res;
            List<string> st = new List<string>();
            string msgError = "";
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);

            DataSet dtnutanix = new DataSet();
            Dictionary<string, object> parametro = new Dictionary<string, object>();
            parametro.Add("@ambito", ambito);
            parametro.Add("@SO", so);
            parametro.Add("@tipo", tipo);
            parametro.Add("@rol", rol);

            res = cDAL.ConsultarDS("[USP_SEL_NUTANIX_JSON]", out dtnutanix, out codError, out msgError, parametro);

            NutanixPlantillas plan = new NutanixPlantillas();
            foreach (DataRow row in dtnutanix.Tables[0].Rows)
            {
                plan.plantillaJson = row["json"].ToString();
                plan.plantillaScript = row["script"].ToString();
            }

            return plan;
        }

        //Llenado de Combo SO
        public bool cargaComboSO(out string sErrors, out string json, string idTipo)
        {
            sErrors = "";
            json = "";
            Boolean res;
            List<string> st = new List<string>();
            string msgError = "";
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);

            Dictionary<string, object> parametro = new Dictionary<string, object>();
            parametro.Add("@tipo", idTipo);

            res = cDAL.ConsultarJson("[USP_SEL_NUTANIX_COMBO_SO]", out st, out codError, out msgError, parametro);
            if (!res)
            {
                return false;
            }

            json = st[0];

            return true;
        }

        //Llenado de Combo Rol
        public bool cargaComboRol(out string sErrors, out string json, string idTipo, string idSO)
        {
            sErrors = "";
            json = "";
            Boolean res;
            List<string> st = new List<string>();
            string msgError = "";
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);

            Dictionary<string, object> parametro = new Dictionary<string, object>();
            parametro.Add("@tipo", idTipo);
            parametro.Add("@so", idSO);

            res = cDAL.ConsultarJson("[USP_SEL_NUTANIX_COMBO_ROL]", out st, out codError, out msgError, parametro);
            if (!res)
            {
                return false;
            }

            json = st[0];

            return true;
        }

        //Llenado de Tabla Cotizaciones
        public bool cargaComboCharset(out string sErrors, out string json, string idproyecto)
        {
            sErrors = "";
            json = "";
            Boolean res;
            List<string> st = new List<string>();
            string msgError = "";
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);

            Dictionary<string, object> parametro = new Dictionary<string, object>();
            parametro.Add("@idProyecto", idproyecto);

            res = cDAL.ConsultarJson("[USP_SEL_NUTANIX_LISTA_COTIZACIONES]", out st, out codError, out msgError, parametro);
            if (!res)
            {
                return false;
            }

            json = st[0];

            return true;
        }

        //Reemplazo de Variables en Json
        public string armarJson(string json, string so, string ram, string vcpu, string vlan, string base64)
        {
            int ram_total = int.Parse(ram) * 1024;
            var remplazo = json;

            remplazo = remplazo.Replace("BESP_SO_IMG", so);
            remplazo = remplazo.Replace("BESP_PROJECT", "85a703e6-431d-41eb-91d7-36bf3c6dd99e");
            remplazo = remplazo.Replace("BESP_VLAN", vlan);
            remplazo = remplazo.Replace("BESP_MEM", ram_total.ToString());
            remplazo = remplazo.Replace("BESP_CPU", vcpu);
            remplazo = remplazo.Replace("BESP_USER_DATA", base64);

            return remplazo;
        }

        //Reemplazo de Variables en Script
        public string armarScript(string script, string bdName, string characterSet, string nationalCH, string block_size)
        {
            var remplazoScript = script;
            remplazoScript = remplazoScript.Replace("@@CORREO@@", GetUserName());
            remplazoScript = remplazoScript.Replace("@@BD_NAME@@", bdName);
            remplazoScript = remplazoScript.Replace("@@CHARACTER_SET@@", characterSet);
            remplazoScript = remplazoScript.Replace("@@DB_BLOCK_SIZE@@", block_size);
            remplazoScript = remplazoScript.Replace("@@NATIONAL_CH_SET@@", nationalCH);

            return remplazoScript;
        }

        //Convertir Cloud-Ini a Base64
        public static string EncodeTo64SL(string entrada)
        {
            byte[] toEncodeAsBytes = System.Text.UTF8Encoding.UTF8.GetBytes(entrada);
            string encoded64 = System.Convert.ToBase64String(toEncodeAsBytes);

            return encoded64;
        }

    } //fin clase
} //fin namespace