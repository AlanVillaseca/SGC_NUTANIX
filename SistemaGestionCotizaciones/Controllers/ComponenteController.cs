using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaGestionCotizaciones.Models;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Data;

namespace SistemaGestionCotizaciones.Controllers
{
    public class ComponenteController : Controller
    {
        private log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index(string id)
        {
            String sError = "";
            Componente oComp = new Componente();
            api obj = new api();
            DataTable b = obj.verificar(out sError);
            try{
                if (b.Rows[0].ItemArray[0].ToString() == "1")
                {
                System.Net.WebClient wc = new System.Net.WebClient();
                    string webData = wc.DownloadString("http://www.mindicador.cl/api");
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(api));
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(webData));
                obj = (api)ser.ReadObject(stream);
                obj.updValor(obj.dolar.valor, obj.uf.valor, out sError);
                }
            }catch(Exception e){
                logger.Error(e.Message);
                sError = "No se pudo actualizar los valores de la UF y del Dolar al valor de hoy. Por favor contáctese con el administrador";
                ViewBag.Error = sError;
                return View();
            }
            oComp.cargaListaPlantillas(out sError);
            oComp.CargaBreadcrumbs(id, "1", out sError);
            oComp.CargaFamilias(out sError);

            oComp.idCotizacion = id;

            ViewBag.Error = sError;

            return View(oComp);
        }

        public ActionResult leerComponente(string id, string estado)
        {
            String sError = "";
            Componente oComp = new Componente();
            oComp.cargaListaPlantillas(out sError);
            oComp.CargaBreadcrumbs(id,"2", out sError);
            oComp.idComponente = id;

            ViewBag.Estado = estado;
            ViewBag.Error = sError;

            return View(oComp);
        }

        public ActionResult vpArbolTabs(string id)
        {
            String sError = "";
            Componente oComp = new Componente();
            oComp.CargaArbol(id, out sError);
            oComp.cargaFormPlantillas(id, out sError);

            ViewBag.Error = sError;

            return PartialView(oComp);
        }
        public ActionResult modCamposComponentes(string id, string idelemento, string valor)
        {
            string sError = "";

            Componente datosComponentes = new Componente();

            if (idelemento != null)
            {
                datosComponentes.agregarCampoElemento(id, idelemento, valor, out sError);
            }

            datosComponentes.cargaCampos(id, out sError);
            datosComponentes.cargaCamposFaltantes(id, out sError);

            ViewBag.idcomponente = id;
            ViewBag.error = sError;

            return PartialView(datosComponentes);
        }
        public string modCamposComponentes2(string valoresCampos)
        {
            string sError = "";

            Componente datosComponentes = new Componente();

            datosComponentes.guardarCamposElementos(valoresCampos, out sError);

            ViewBag.error = sError;

            return sError;
        }

        public ActionResult vpCotizador(string id)
        {
            String sError = "";
            Componente oComp = new Componente();
            oComp.CargaArbusto(id, out sError);
            oComp.CargaParametros(id, out sError);
            oComp.CargaCostosOpcionales(id, out sError);
            oComp.CargaHeader(id, "1", out sError);
            oComp.CargaVariablesJson(id, out sError);           
            oComp.CargaCatSW(out sError);

            ViewBag.Error = sError;

            return PartialView(oComp);
        }

        public ActionResult vpComponente(string id, string estado)
        {
            String sError = "";
            Componente oComp = new Componente();
            oComp.CargaArbustoComponente(id, out sError);
            oComp.CargaParametrosComponente(id, out sError);
            oComp.CargaCostosOpcionalesComponente(id, out sError);
            oComp.CargaHeader(id, "2", out sError);
            oComp.CargaSW(id, out sError);
            oComp.CargaVariablesJsonComp(id, out sError);
            oComp.CargaCatSW(out sError);

            ViewBag.Origen = oComp.VerOrigen(id, out sError);
            ViewBag.Estado = estado;
            ViewBag.Error = sError;

            return PartialView(oComp);
        }

        [HttpPost]
        public String jsonReceiver(List<ComponenteJson> componenteJson)
        {
            string sError = "";

            Componente oComponente = new Componente();

            oComponente.GuardaComponente(componenteJson, out sError);
            return sError;
        }


        [HttpPost]
        public string jsonReceiverImplementar(List<ComponenteJson> componenteJson)
        {
            string sError = "";

            api obj = new api();
            Componente oComponente = new Componente();

            try
            {

                
                 oComponente.EditaComponente(componenteJson, out sError);
                
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                sError = ex.Message;
                ViewBag.Error = sError;

            }


            return sError;
        }

        [HttpPost]
        public string jsonReceiverEdit(List<ComponenteJson> componenteJson)
        {
            string sError = "";

            api obj = new api();
            Componente oComponente = new Componente();
            
                try
                {

                    DataTable b = obj.verificar(out sError);
                    if (b.Rows[0].ItemArray[0].ToString() == "1")
                    {
                        System.Net.WebClient wc = new System.Net.WebClient();
                        string webData = wc.DownloadString("http://www.mindicador.cl/api");
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(api));
                        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(webData));
                        obj = (api)ser.ReadObject(stream);
                        obj.updValor(obj.dolar.valor, obj.uf.valor, out sError);
                    }
                    else
                    {
                        oComponente.EditaComponente(componenteJson, out sError);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    sError = "No se pudo actualizar los valores de la UF y del Dolar al valor de hoy. Por favor contáctese con el administrador";
                    ViewBag.Error = sError;

                }
            
            
            return sError;
        }

        

        [HttpPost]
        public string vpValidarServicioComponente(string idcotizacion, string id, string tipo)
        {
            string sError = "";
            DetalleCotizacion dcot = new DetalleCotizacion();
            DataTable validate = new DataTable();
            validate = dcot.validarServicioComponenteCC(idcotizacion, id, tipo, out sError);
            return validate.Rows[0][0].ToString();
        }

        [HttpPost]
        public string jsonReceiverCalcular(List<ComponenteJson> componenteJson)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string sError = "";

            Componente oComponente = new Componente();

            oComponente.CalcularComponente(componenteJson, out sError);
            return jss.Serialize(oComponente.calculaMontosJson);
        }

        [HttpPost]
        public string jsonreceivercalcularcomp(List<ComponenteJson> componenteJson)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string serror = "";

            Componente ocomponente = new Componente();

            ocomponente.CalcularComponenteComp(componenteJson, out serror);
            return jss.Serialize(ocomponente.calculaMontosJson);
        }

    }
}