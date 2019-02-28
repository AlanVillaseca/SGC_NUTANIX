using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using SistemaGestionCotizaciones.Models;
using System.Collections.Generic;

namespace SistemaGestionCotizaciones.Controllers
{
    public class NutanixController : Controller
    {
        // GET: Nutanix
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProyectosNutanix()
        {
            String sError = "";

            NutanixFunciones sv = new NutanixFunciones();
            sv.cargaProyectoNutanix(out sError);
            ViewBag.Error = sError;

            return View(sv);
          
        }


        public ActionResult CotizacionesNutanix()
        {
          
            return View();
        }

        public ActionResult PanelControlNutanix()
        {

            return View();
        }

        public ActionResult FacturacionNutanix()
        {

            return View();
        }

        public string getCotizacionesNutanix(string id)
        {
            string sError = "";
            string json="";
            NutanixFunciones sv = new NutanixFunciones();
            sv.cargaCotizacionNutanix(out sError,out json,id);

            return json ;
        }

        public string getListaPanelNutanix()
        {
            string sError = "";
            string json = "";
            NutanixFunciones sv = new NutanixFunciones();
            sv.cargaListaPanel(out sError, out json);

            return json;
        }

        public string getDetallesPanelNutanix(string id)
        {
            string sError = "";
            string json = "";
            NutanixFunciones sv = new NutanixFunciones();
            sv.cargaPanelDetalles(out sError, out json, id);

            return json;
        }

        public ActionResult ComponentesNutanix(string id)
        {
            ViewBag.Message = id;
            return View();
        }

        public string getComponentesNutanix(string id)
        {
            string sError = "";
            string json = "";
            NutanixFunciones sv = new NutanixFunciones();
            sv.cargaComponentesNutanix(out sError, out json, id);

            return json;

        }

        public string getComponentesCotiNutanix(string id)
        {
            string sError = "";
            string json = "";
            NutanixFunciones sv = new NutanixFunciones();
            sv.cargaComponentesNutanix(out sError, out json, id);

            return json;

        }

        public string getComboSO(string id)
        {
            string sError = "";
            string json = "";
            NutanixFunciones sv = new NutanixFunciones();
            sv.cargaComboSO(out sError, out json, id);

            return json;

        }

        public string getComboRol(string id_tipo, string id_so)
        {
            string sError = "";
            string json = "";
            NutanixFunciones sv = new NutanixFunciones();
            sv.cargaComboRol(out sError, out json, id_tipo, id_so);

            return json;

        }

        public string getComboCharset(string id)
        {
            string sError = "";
            string json = "";
            NutanixFunciones sv = new NutanixFunciones();
            sv.cargaComboCharset(out sError, out json, id);

            return json;

        }

        public string creaMaquinaNutanix(string componenteJson)
        {

            WebServiceNutanix ws = new WebServiceNutanix();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<NutanixEntrada> listComponentes = serializer.Deserialize<List<NutanixEntrada>>(componenteJson);


            string resultado = "";
            List<object> jsonArray = new List<object> { };
            foreach (var listaJson in listComponentes)
            {
                resultado = ws.creaNutanixVM(
                    listaJson.idComponente,
                    listaJson.sistemaOperativo,
                    listaJson.siteSec,
                    listaJson.vlanSec,
                    listaJson.rolSet,
                    listaJson.charSet,
                    listaJson.dbName);
                jsonArray.Add(JsonConvert.SerializeObject(resultado));      
            }
            var json = JsonConvert.SerializeObject(jsonArray);

            return json;

        }//fin de procesa vm

        public string datosMaquinaNutanix(string taskUUID)
        {
            WebServiceNutanix ws = new WebServiceNutanix();
            string resultado = ws.infoNutanixVM(taskUUID);

            return resultado;

        }//fin de actualiza vm


    }//fin clase
}//fin namespace