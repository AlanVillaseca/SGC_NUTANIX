using SistemaGestionCotizaciones.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SistemaGestionCotizaciones.Controllers
{
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult nvaPlantillasElementos()
        {

            String sError = "";

            ManPlantillas oManP = new ManPlantillas();
            oManP.CargaHojasPadre(out sError);
            oManP.CargaParametrosCosto(out sError);
            oManP.CargaVariables(out sError);
            oManP.CargaImplementaciones(out sError);
            oManP.CargaFamilias(out sError);

            ViewBag.Error = sError;

            return View(oManP);

        }

        public ActionResult nvaPlantillasElementosVerMod(string id)
        {
            String sError = "";
            ManPlantillas oManP = new ManPlantillas();
            oManP.CargaHojasPadre(out sError);
            oManP.CargaParametrosCosto(out sError);
            oManP.CargaVariables(out sError);
            oManP.CargaImplementaciones(out sError);
            oManP.CargaFamilias(out sError);
            oManP.CargaPlantillaPorId(id, out sError);
            oManP.CargaPlantillaCostosPorId(id, out sError);
            oManP.CargaPlantillaVariablesPorId(id, out sError);
            oManP.CargaPlantillaImplementPorId(id, out sError);
            ViewBag.Error = sError;
            return View(oManP);
        }
        public JsonResult jsonPlantillas(string id)
        {
            string sError = "";

            ManPlantillas oManP = new ManPlantillas();

            oManP.CargaParametrosCostoHojas(id, out sError);

            ViewBag.Error = sError;
            return Json(oManP.parametrosCostoHojasJson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult jsonVariable(string id)
        {
            string sError = "";

            ManPlantillas oManP = new ManPlantillas();

            oManP.CargaVar(id, out sError);

            ViewBag.Error = sError;
            return Json(oManP.varJson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string jsonReceiver(List<Plantilla> plantilla)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string sError = "";

            ManPlantillas oManP = new ManPlantillas();

            oManP.GuardaHojasPadre(plantilla, out sError);
            return jss.Serialize(oManP.elementosFaltantesJson);
        }
        [HttpPost]
        public string jsonReceiverPlantillaEdit(List<Plantilla> plantilla)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string sError = "";
            ManPlantillas oManP = new ManPlantillas();
            oManP.GuardaPlnatillasEdit(plantilla, out sError);
            return jss.Serialize(oManP.elementosFaltantesJson);
        }
        [HttpPost]
        public ActionResult jsonReceiverConcepto(List<ConceptoCosto> concepto)
        {
            string sError = "";

            ManPlantillas oManP = new ManPlantillas();

            oManP.GuardaConceptoCosto(concepto, out sError);
            return null;
        }
        [HttpPost]
        public string actualizarListaPlantilla(string id, string nombre, string habilitado)
        {
            string sError = "";

            ManPlantillas oManP = new ManPlantillas();

            oManP.GuardaListaPlantillas(id, nombre, habilitado, out sError);
            ViewBag.Error = sError;

            return sError;
        }
        public ActionResult Testeador()
        {
            return View();
        }
        public ActionResult nvoElemento()
        {
            return View();
        }
        public ActionResult manPlantillas()
        {
            String sError = "";

            ManPlantillas oManP = new ManPlantillas();
            oManP.CargaPlantillas(out sError);

            ViewBag.Error = sError;

            return View(oManP);
        }

        public ActionResult nvaPlantillasCostos(string id)
        {
            String sError = "";

            ViewBag.idPlantilla = id;
            ManPlantillas oManP = new ManPlantillas();
            oManP.CargaPlantillasCostos(id, out sError);
            oManP.CargaCategoriasCostos(out sError);
            oManP.CargaListaFormulas(out sError);
            oManP.CargaVariablesCostoV(out sError);
            oManP.CargaParCostoCatElemento(id, out sError);

            ViewBag.Error = sError;

            return View(oManP);
        }

        public ActionResult nvaConceptoCosto()
        {
            String sError = "";

            ManPlantillas oManP = new ManPlantillas();
            oManP.CargaConceptosCostosNvl1(out sError);
            oManP.CargaCategoriasCostos(out sError);
            oManP.CargaParCostoNvl1(out sError);
            oManP.CargaListaFormulas(out sError);
            oManP.CargaVariablesCostoV2(out sError);

            ViewBag.Error = sError;

            return View(oManP);
        }

        public JsonResult jsonParametroCostoNvlN(string id)
        {
            string sError = "";

            ManPlantillas oManP = new ManPlantillas();

            oManP.CargaParCostoNvlN(id, out sError);

            ViewBag.Error = sError;
            return Json(oManP.listaParCostoNvlN, JsonRequestBehavior.AllowGet);
        }

        public JsonResult jsonListaValores(string id)
        {
            string sError = "";

            ManPlantillas oManP = new ManPlantillas();

            oManP.CargaListaValores(id, out sError);

            ViewBag.Error = sError;
            return Json(oManP.listaValores, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public ActionResult jsonReceiverGuardar(List<PlantillaCostosJson> plantillaJson)
        {
            string sError = "";

            ManPlantillas oManPlantillas = new ManPlantillas();

            oManPlantillas.GuardaPlantillaCostos(plantillaJson, out sError);
            return null;
        }
        public ActionResult PaisDetalle()
        {

            return View();
        }
        [HttpGet]
        public ActionResult vpGrillaDetallePais(string id, string ordenarPor)
        {
            string sError = "";
            DetalleMantenedor ctrlGrillaPais = new DetalleMantenedor();
            ctrlGrillaPais.cargaModGrillaPais(out sError, id, ordenarPor);
            ViewBag.Error = sError;

            return PartialView(ctrlGrillaPais);
        }
        public ActionResult vpAgregarPais()
        {
            string sError = "";
            DetalleMantenedor oPais = new DetalleMantenedor();
            ViewBag.Error = sError;
            return PartialView();
        }
        [HttpPost]
        public string vpAgregarPais(string id)
        {
            string sError = "";
            DetalleMantenedor oPais = new DetalleMantenedor();
            oPais.GuardarPais(out sError,id);
            ViewBag.Error = sError;
            return sError;
        }
        public string actualizarPais(string id, string cadena)
        {
            string sError = "";

            DetalleMantenedor oPais = new DetalleMantenedor();
            oPais.ActualizarPais(id, cadena, out sError);
            ViewBag.Error = sError;
            return sError;
        }

        /// <summary>
        /// vistas pantalla servicio
        /// </summary>
        /// <returns></returns>
        public ActionResult CatalogoServicioDetalle()
        {

            return View();
        }

        [HttpGet]
        public ActionResult vpGrillaDetalleCatalogoServicio(string id, string ordenarPor, string ascDesc)
        {
            string sError = "";
            string idHeadAct = "";
            string ascDescAct = "";
            idHeadAct = ordenarPor;
            ascDescAct = ascDesc;
            DetalleMantenedor ctrlGrillaServicio = new DetalleMantenedor();
            ctrlGrillaServicio.cargaModGrillaCatalogoServicio(out sError, id, ordenarPor + ascDesc);
            ctrlGrillaServicio.GetCompElemntList(out sError);
            ctrlGrillaServicio.GetNegocioList(out sError);
            ctrlGrillaServicio.GetPaisList(out sError);
            ViewBag.Error = sError;
            ViewBag.idHeadAct = idHeadAct;
            ViewBag.ascDescAct = ascDescAct;
            return PartialView(ctrlGrillaServicio);
        }

        [HttpGet]
        public ActionResult vpAgregarCentroCostoParamCosto(string idparamcosto)
        {
            DetalleMantenedor ctrlCentroCostoParamCosto = new DetalleMantenedor();
            string sError = "";
            ctrlCentroCostoParamCosto.CargaCentroCosto(out sError);
            ctrlCentroCostoParamCosto.cargarDetalleParamCostoCentroCosto(idparamcosto, out sError);
            return PartialView(ctrlCentroCostoParamCosto);
        }

        [HttpGet]
        public ActionResult vpAgregarCentroCostroServicio(string idservicio)
        {
            DetalleMantenedor ctrlCentroCostoServicio = new DetalleMantenedor();
            string sError = "";
            ctrlCentroCostoServicio.CargaCentroCosto(out sError);

            ctrlCentroCostoServicio.cargarDetalleServicioCentroCosto(idservicio, out sError);
            
            return PartialView(ctrlCentroCostoServicio);
        }
        public ActionResult vpDominioCatalogoServicio(string id, string pais, string negocio, string subgerencia)
        {
            string sError = "";
            DetalleMantenedor ctrlGrillaServicio = new DetalleMantenedor();
            ctrlGrillaServicio.GetNegocioList(out sError);
            ctrlGrillaServicio.GetPaisList(out sError);
            ViewBag.Error = sError;
            ViewBag.id = id;
            ViewBag.pais = pais;
            ViewBag.negocio = negocio;
            ViewBag.subgerencia = subgerencia;

            return PartialView(ctrlGrillaServicio);
        }
        public string actualizarDominioCatalogoServicio(string id, string pais, string negocio, string subgerencia)
        {
            string sError = "";

            DetalleMantenedor oCatServicio = new DetalleMantenedor();
            oCatServicio.ActualizarDominioCatalogoServ(id, pais, negocio, subgerencia, out sError);
            ViewBag.Error = sError;
            return sError;
        }
        public ActionResult vpFechaAplicacionCServicio()
        {
            string sError = "";

            DetalleMantenedor oCatServicio = new DetalleMantenedor();
            // oCatServicio.ActualizarFechaAplicacionCostoCatalogoServ(id, fecha, CUInicial,CUMensual,aplicacion, out sError);
            ViewBag.Error = sError;
            return PartialView();
        }
        [HttpPost]
        public string vpFechaAplicacionCServicio(string id, string fecha, string CUInicial, string CUMensual, string aplicacion)
        {
            string sError = "";

            DetalleMantenedor oCatServicio = new DetalleMantenedor();
            oCatServicio.ActualizarFechaAplicacionCostoCatalogoServ(id, fecha, CUInicial, CUMensual, aplicacion, out sError);
            ViewBag.Error = sError;
            return sError;
        }
        public string actualizarCatalogoServicio(string id, string cadena)
        {
            string sError = "";
            string[] word = cadena.Split(';');
            string servicio, descripcion, tipoCobroInicial, tipoCobroMensual,
                   tipoUnidad, tipoUnidadMensual;


            servicio = word[0];
            descripcion = word[1];
            tipoCobroInicial = word[2];
            tipoCobroMensual = word[3];
            tipoUnidad = word[4];
            tipoUnidadMensual = word[5];

            DetalleMantenedor oCatServicio = new DetalleMantenedor();
            oCatServicio.ActualizarCatalogoServ(id, servicio, descripcion,
                                            tipoCobroInicial, tipoCobroMensual, tipoUnidad, tipoUnidadMensual, out sError);
            ViewBag.Error = sError;
            return sError;

        }
        [HttpGet]
        public ActionResult vpAgregarCatalogoServicio()
        {
            string sError = "";
            DetalleMantenedor oCatServicio = new DetalleMantenedor();

            oCatServicio.GetVariablesList(out sError);
            oCatServicio.GetSubGerenciaList(out sError);

            oCatServicio.CargaNegocios(out sError);
            oCatServicio.CargaPaises(out sError);
            oCatServicio.CargaTipoServicio(out sError);
            oCatServicio.CargaCentroCosto(out sError);
            ViewBag.Error = sError;
            return PartialView(oCatServicio);
        }
        [HttpPost]
        public string eliminarCatalogoServicio(string id)
        {
            string sError = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            DetalleMantenedor oCatServicio = new DetalleMantenedor();
            oCatServicio.eliminarCatalogoServicio(id, out sError);
            ViewBag.Error = sError;

            return jss.Serialize(oCatServicio.dtServicios);
        }
        public string ServicioFileXls()
        {
            string serror = "";

            DataTable tabla = new DataTable();

            DetalleMantenedor ctrlServicios = new DetalleMantenedor();


            tabla = ctrlServicios.TablaServicios(out serror);


            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);

        }
        public JsonResult jsonListaUnidadesCS()
        {
            string sError = "";

            DetalleMantenedor oHeader = new DetalleMantenedor();

            oHeader.cargaListaUnidadesCatServicios(out sError);

            ViewBag.Error = sError;
            return Json(oHeader.jsListaUnidadesCatServicio, JsonRequestBehavior.AllowGet);
        }

        public JsonResult jsonListaParametroCosto()
        {
            string sError = "";
            DetalleMantenedor jsonParamCosto = new DetalleMantenedor();
            jsonParamCosto.CargaCentroCosto(out sError);
            ViewBag.Error = sError;
            return Json(jsonParamCosto.jsListaConceptoCosto, JsonRequestBehavior.AllowGet);
        }

        public JsonResult jsonListaCatalagoDeServicio()
        {
            string sError = "";

            DetalleMantenedor oHeader = new DetalleMantenedor();

            oHeader.cargaListaCatServicios(out sError);
            oHeader.CargaCentroCosto(out sError);

            ViewBag.Error = sError;
            return Json(oHeader.jsListaCatalogoDeServicio, JsonRequestBehavior.AllowGet);
        }
        public JsonResult jsonListafunciones()
        {
            string sError = "";

            DetalleMantenedor oHeader = new DetalleMantenedor();

            oHeader.cargaListafunciones(out sError);

            ViewBag.Error = sError;
            return Json(oHeader.jsListaFunciones, JsonRequestBehavior.AllowGet);
        }
        public JsonResult jsonListaElementos()
        {
            string sError = "";

            DetalleMantenedor oHeader = new DetalleMantenedor();

            oHeader.cargaListaElementos(out sError);

            ViewBag.Error = sError;
            return Json(oHeader.jsListaElementos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult jsonListaConceptoCosto()
        {
            string sError = "";

            DetalleMantenedor oHeader = new DetalleMantenedor();

            oHeader.cargaListaConceptoCosto(out sError);
            oHeader.CargaCentroCosto(out sError);

            ViewBag.Error = sError;
            return Json(oHeader.jsListaConceptoCosto, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult jsonListaNegocio()
        {
            string sError = "";

            DetalleMantenedor oHeader = new DetalleMantenedor();

            oHeader.cargaListaNegocio(out sError);

            ViewBag.Error = sError;
            return Json(oHeader.jsListaNegocio, JsonRequestBehavior.AllowGet);
        }
         [HttpPost]
        public JsonResult jsonListaPais()
        {
            string sError = "";

            DetalleMantenedor oHeader = new DetalleMantenedor();

            oHeader.cargaListaPais(out sError);

            ViewBag.Error = sError;
            return Json(oHeader.jsListaPais, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult jsonGuardarServicio(List<CatalogoServicio> Servicio)
        {
            string sError = "";
            DetalleMantenedor oManDetalle = new DetalleMantenedor();
            oManDetalle.GuardarCatalogoServicio(Servicio, out sError);
            return null;
        }

        /// <summary>
        /// vistas pantalla negocio
        /// </summary>
        /// <returns></returns>
        public ActionResult NegocioDetalle()
        {

            return View();
        }
        [HttpGet]
        public ActionResult vpGrillaDetalleNegocio(string id, string ordenarPor)
        {
            string sError = "";
            DetalleMantenedor ctrlGrillaNegocio = new DetalleMantenedor();
            ctrlGrillaNegocio.cargaModGrillaNegocio(out sError, id, ordenarPor);
            ViewBag.Error = sError;

            return PartialView(ctrlGrillaNegocio);
        }
        public ActionResult vpAgregarNegocio()
        {
            string sError = "";
            DetalleMantenedor oNegocio = new DetalleMantenedor();
            ViewBag.Error = sError;
            return PartialView();
        }

        [HttpPost]
        public string vpAgregarCentroCostoParametroCostoGuardar(string idparamcosto, string decentrocosto)
        {
            string sError = "";
            DetalleMantenedor oPCosto = new DetalleMantenedor();
            oPCosto.guardarDetalleParamCostoCC(idparamcosto, decentrocosto, out sError);
            
            ViewBag.Error = sError;
            return sError;
        }

        [HttpPost]
        public string vpAgregarCentroCostoServicioGuardar(string idservicio, string dtcentrocosto)
        {
            string sError = "";
            DetalleMantenedor oSCosto = new DetalleMantenedor();
            oSCosto.guardarDetalleServicioCentroCosto(idservicio, dtcentrocosto, out sError);
            ViewBag.Error = sError;
            return sError;
        }
        [HttpPost]
        public String vpAgregarNegocio(string id)
        {
            string sError = ""; 
            DetalleMantenedor oNegocio = new DetalleMantenedor();
            oNegocio.GuardarNegocio(out sError, id);
            ViewBag.Error = sError;
            return sError;
        }
        public String actualizarNegocio(string id, string cadena)
        {
            string sError = "";

            DetalleMantenedor oNegocio = new DetalleMantenedor();
            oNegocio.ActualizarNegocio(id, cadena, out sError);
            ViewBag.Error = sError;
            return sError;
        }
        /// <summary>
        /// vistas pantalla formula
        /// </summary>
        /// <returns></returns>
        public ActionResult FormulaDetalle()
        {

            return View();
        }
        [HttpGet]
        public ActionResult vpGrillaDetalleFormula(string id, string ordenarPor)
        {
            string sError = "";
            DetalleMantenedor ctrlGrillaFormula = new DetalleMantenedor();
            ctrlGrillaFormula.cargaModGrillaFormula(out sError, id, ordenarPor);
            ctrlGrillaFormula.cargaFunciones(out sError);
            ViewBag.Error = sError;

            return PartialView(ctrlGrillaFormula);
        }
        public ActionResult vpAgregarFormula()
        {
            string sError = "";
            DetalleMantenedor oFormula = new DetalleMantenedor();
            oFormula.cargaFunciones(out sError);
            ViewBag.Error = sError;
            return PartialView(oFormula);
        }
        [HttpPost]
        public ActionResult vpAgregarFormula(string id, string Descripcion, string Funcion, string NumVariables)
        {
            string sError = "";
            DetalleMantenedor oFormula = new DetalleMantenedor();
            oFormula.GuardarFormula(id, Descripcion, Funcion, NumVariables, out sError);

            ViewBag.Error = sError;
            return RedirectToAction("FormulaDetalle", "Mantenedor");
        }
        public string actualizarFormula(string id, string cadena)
        {
            string sError = "";
            string[] word = cadena.Split(';');
            string nombre, descripcion, funcion, variables, estado;
            nombre = word[0];
            descripcion = word[1];
            funcion = word[2];
            variables = word[3];
            estado = word[4];
            DetalleMantenedor oFormula = new DetalleMantenedor();
            oFormula.ActualizarFormula(id, nombre, descripcion, funcion, variables, estado, out sError);
            ViewBag.Error = sError;
            return sError;
        }

        /// <summary>
        /// vistas pantalla Lista
        /// </summary>
        /// <returns></returns>
        public ActionResult ListaValoresDetalle()
        {
            string sError = "";

            DetalleMantenedor ctrlGrillaLValores = new DetalleMantenedor();
            ctrlGrillaLValores.cargaModGrillaDetalleListaValor(out sError, "1", "idlistavalores", "1");

            ViewBag.Error = sError;
            ViewBag.OrdenarPor = "idlistavalores";
            ViewBag.Orden = "1";
            ViewBag.PaginaActual = "1";
            ViewBag.TotalPaginas = ctrlGrillaLValores.totalPaginas;
            ViewBag.TotalRegistros = ctrlGrillaLValores.totalRegistros;

            return View(ctrlGrillaLValores);
        }
        [HttpPost]
        public String vpGrillaDetalleListaValores(string pagina, string ordenarPor, string orden)
        {
            try{
                string sError = "";
                
                JavaScriptSerializer varjon = new JavaScriptSerializer();

                if (pagina == null || pagina == "")
                {
                    pagina = "1";
                }
                if (ordenarPor == null || ordenarPor == "")
                {
                    ordenarPor = "idlistavalores";
                }
                if (orden == null || orden == "")
                {
                    orden = "1";
                }

                DetalleMantenedor ctrlGrillaLValores = new DetalleMantenedor();
                ctrlGrillaLValores.cargaModGrillaDetalleListaValor(out sError, pagina, ordenarPor, orden);

                ViewBag.Error = sError;
                ViewBag.OrdenarPor = ordenarPor;
                ViewBag.Orden = orden;
                ViewBag.PaginaActual = pagina;
                ViewBag.TotalPaginas = ctrlGrillaLValores.totalPaginas;
                ViewBag.TotalRegistros = ctrlGrillaLValores.totalRegistros;
                
                return varjon.Serialize(ctrlGrillaLValores.dtListaValoresJson);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return e.Message;
            }
        }
        [HttpPost]
        public string actualizarListaValores(string id, string cadena)
        {
            string sError = "";
            string[] words = cadena.Split(',');
            string nombre, contrl;
            nombre = words[0];
            contrl = words[1];
            DetalleMantenedor oHeader = new DetalleMantenedor();
            oHeader.ActualizarTpoDato(id, nombre, contrl, out sError);
            ViewBag.Error = sError;
            return sError;
        }
        [HttpPost]
        public String actualizarListaValor(string id, string valor, string descripcion)
        {
            try
            {
                string sError = "";

                DetalleMantenedor oListaValor = new DetalleMantenedor();

                oListaValor.ActualizarListaValor(id, valor, descripcion, out sError);

                ViewBag.Error = sError;

                return sError;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return e.Message;
            }
        }
        public String eliminarListaValor(string idlista)
        {
            try
            {
                string sError = "";

                DetalleMantenedor oListaValor = new DetalleMantenedor();

                oListaValor.eliminarListaValores(idlista, out sError);

                ViewBag.Error = sError;

                return sError;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return e.Message;
            }
        }
        public String ListaValoresFileXls()
        {
            string serror = "";

            DataTable tabla = new DataTable();


            DetalleMantenedor ctrlListaValores = new DetalleMantenedor();


            tabla = ctrlListaValores.TablaListaValores(out serror);


            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);

        }
        public ActionResult TipoDatoDetalle()
        {

            return View();
        }
        [HttpGet]
        public ActionResult vpGrillaDetalleTipoDato(string id, string ordenarPor)
        {
            string sError = "";
            DetalleMantenedor ctrlGrillaTdato = new DetalleMantenedor();
            ctrlGrillaTdato.cargaModGrillaDetalleTipoDato(out sError, id, ordenarPor);
            ViewBag.Error = sError;

            return PartialView(ctrlGrillaTdato);
        }
        [HttpPost]
        public string actualizarTipoDato(string id, string cadena)
        {
            string sError = "";
            string[] words = cadena.Split(',');
            string nombre, contrl;
            nombre = words[0];
            contrl = words[1];
            DetalleMantenedor oHeader = new DetalleMantenedor();
            oHeader.ActualizarTpoDato(id, nombre, contrl, out sError);
            ViewBag.Error = sError;
            return sError;
        }

        /// <summary>
        /// vistas pantalla catalogo elemento
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public ActionResult CatalogoElementoDetalle()
        {

            return View();
        }
        public ActionResult vpGrillaDetalleCatalogoElemento(string id, string ordenarPor,string ascDesc,
            string idtipodato, string categoria)
        {
            string sError = "";
            string idHeadAct = "";
            string ascDescAct = "";

            if (idtipodato == null)
            {
                idtipodato = "-1";
            }
            if (categoria == null)
            {
                categoria = "-1";
            }

            idHeadAct = ordenarPor;
            ascDescAct = ascDesc;

            DetalleMantenedor ctrlGrillaHeader = new DetalleMantenedor();

            ctrlGrillaHeader.cargaModGrillaDetalle(out sError, id, ordenarPor, ascDesc, idtipodato, categoria);
            ctrlGrillaHeader.GetTipoDatoList(out sError);
            ctrlGrillaHeader.GetCompElemntList(out sError);
            ViewBag.Error = sError;
            ViewBag.idHeadAct = idHeadAct;
            ViewBag.ascDescAct = ascDescAct;
            return PartialView(ctrlGrillaHeader);
        }

        public string ElementoFileXls()
        {
            string serror = "";

            DataTable tabla = new DataTable();

            DetalleMantenedor ctrlElemento = new DetalleMantenedor();


            tabla = ctrlElemento.TablaElementos(out serror);


            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);

        }
    


        [HttpPost]
        public string actualizarComponenteElemento(string id, string cadena)
        {
            string sError = "";
            string[] words = cadena.Split(';');
            string idTipoDato, nombre, descripcion, categoria;

            idTipoDato = words[0];
            nombre = words[1];
            descripcion = words[2];
            categoria = words[3];

            DetalleMantenedor oHeader = new DetalleMantenedor();
            oHeader.ActualizarComponenteElem(id, idTipoDato, nombre, descripcion, categoria, out sError);
            ViewBag.Error = sError;

            return sError;
        }
        [HttpPost]
        public string eliminarComponenteElemento(string id)
        {
            string sError = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();

            DetalleMantenedor oHeader = new DetalleMantenedor();

            oHeader.eliminarComponenteElem(id, out sError);

            ViewBag.Error = sError;
            ViewBag.Plantillas = oHeader.dtPlantillas;

            return jss.Serialize(oHeader.dtPlantillas);
        }
        [HttpPost]
        public string dependenciaComponenteElemento(string id)
        {
            string sError = "";
            DetalleMantenedor oHeader = new DetalleMantenedor();
            oHeader.dependenciasComponenteElem(id, out sError);
            ViewBag.Error = sError;
            return sError;
        }

        public ActionResult vpAgregarCatalogoElemento()
        {
            string sError = "";
            DetalleMantenedor oTipoDato = new DetalleMantenedor();
            oTipoDato.GetTipoDatoList(out sError);
            ViewBag.Error = sError;
            return PartialView(oTipoDato);
        }

        [HttpPost]
        public ActionResult vpAgregarCatalogoElemento(string id, string descripcion, string idTipoDato, string categoria)
        {
            string sError = "";
            DetalleMantenedor oHeader = new DetalleMantenedor();
            oHeader.GuardarHeader(id, descripcion, idTipoDato, categoria, out sError);
            ViewBag.Error = sError;
            return RedirectToAction("CatalogoElementoDetalle", "Mantenedor");
        }
        [HttpPost]
        public ActionResult vpAgregarCatalogoElemento1(string id, string descripcion, string idTipoDato, string categoria, string cadena)
        {
            string sError = "";

            DetalleMantenedor oHeader = new DetalleMantenedor();
            oHeader.GuardarHeader1(id, descripcion, idTipoDato, categoria, cadena, out sError);
            ViewBag.Error = sError;
            return RedirectToAction("CatalogoElementoDetalle", "Mantenedor");
        }
        /// <summary>
        /// vistas pantalla catalogo Concepto de costo
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public ActionResult CatalogoConceptoCostoDetalle()
        {

            return View();
        }
        public ActionResult vpGrillaDetalleConceptoCosto(string id, string ordenarPor, string ascDesc)
        {
            string sError = "";
            string idHeadAct = "";
            string ascDescAct = "";
            idHeadAct = ordenarPor;
            ascDescAct = ascDesc;
            DetalleMantenedor ctrlGrillaConceptoCosto = new DetalleMantenedor();

            ctrlGrillaConceptoCosto.cargaModGrillaConceptoCostoDetalle(out sError, id, ordenarPor + ascDesc);
            ctrlGrillaConceptoCosto.GetTipoDatoList(out sError);
            ctrlGrillaConceptoCosto.GetCompElemntList(out sError);
            ctrlGrillaConceptoCosto.cargaListaValorPorCE(out sError, "35");

            ViewBag.Error = sError;
            ViewBag.idHeadAct = idHeadAct;
            ViewBag.ascDescAct = ascDescAct;

            return PartialView(ctrlGrillaConceptoCosto);
        }
        public string ConceptoCostoFileXls()
        {
            string serror = "";

            DataTable tabla = new DataTable();

            DetalleMantenedor ctrlConceptoCosto = new DetalleMantenedor();


            tabla = ctrlConceptoCosto.TablaConceptoCosto(out serror);


            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);
        }

        [HttpPost]
        public string actualizarConceptoCosto(string id, string cadena)
        {
            string sError = "";
            string[] words = cadena.Split(';');
            string grupo, nombre, descripcion;

            nombre = words[0];
            descripcion = words[1];
            grupo = words[2];

            DetalleMantenedor oConceptoCosto = new DetalleMantenedor();
            oConceptoCosto.ActualizarConceptoCosto(id, nombre, descripcion, grupo, out sError);
            ViewBag.Error = sError;

            return sError;
        }
        [HttpPost]
        public string eliminarConceptoCosto(string id)
        {
            string sError = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();

            DetalleMantenedor oHeader = new DetalleMantenedor();

            oHeader.eliminarConceptoCosto(id, out sError);

            ViewBag.Error = sError;

            return jss.Serialize(oHeader.dtElementos);
        }
        //[HttpPost]
        //public string dependenciaComponenteElemento(string id)
        //{
        //    string sError = "";
        //    DetalleMantenedor oHeader = new DetalleMantenedor();
        //    oHeader.dependenciasComponenteElem(id, out sError);
        //    ViewBag.Error = sError;
        //    return sError;
        //}

        public ActionResult vpAgregarConceptoCosto()
        {
            string sError = "";
            DetalleMantenedor ctrlConceptoCosto = new DetalleMantenedor();
            ctrlConceptoCosto.cargaListaValorPorCE(out sError, "35");
            ctrlConceptoCosto.cargarCentroCosto(out sError);
            ViewBag.Error = sError;
            return PartialView(ctrlConceptoCosto);
        }

        [HttpPost]
        public ActionResult vpAgregarConceptoCosto(string id, string descripcion, string grupo, string centrocosto)
        {
            string sError = "";
            DetalleMantenedor oHeader = new DetalleMantenedor();
            oHeader.GuardarConceptoCosto(id, descripcion, grupo, centrocosto, out sError);
            ViewBag.Error = sError;
            return RedirectToAction("CatalogoConceptoCostoDetalle", "Mantenedor");
        }

        /// <summary>
        /// ////// tipo dato 
        /// </summary>
        /// <returns></returns>
        public ActionResult vpAgregarTipoDato()
        {
            string sError = "";
            ViewBag.Error = sError;
            return PartialView();
        }

        [HttpPost]
        public ActionResult vpAgregarTipoDato(DetalleMantenedor oTipoDato)
        {
            string sError = "";
            oTipoDato.GuardarTipoDato(out sError);
            ViewBag.Error = sError;
            return RedirectToAction("Detalle", "Mantenedor");
        }
        public ActionResult vpAgregarListaValores()
        {
            string sError = "";
            DetalleMantenedor oHeader = new DetalleMantenedor();
            oHeader.GetHeaderList(out sError);
            return PartialView(oHeader);
        }
        public JsonResult jsonListaValoresLV(string id)
        {
            string sError = "";

            DetalleMantenedor oHeader = new DetalleMantenedor();

            oHeader.cargaListaValorPorCE(out sError, id);

            ViewBag.Error = sError;
            return Json(oHeader.jsListaValores, JsonRequestBehavior.AllowGet);
        }
         public JsonResult jsonListaValoresGrilla()
                {
                    string sError = "";

                    DetalleMantenedor oHeader = new DetalleMantenedor();

                    oHeader.cargaListaValorGrilla(out sError);

                    ViewBag.Error = sError;
                    return Json(oHeader.jsListaValoresGrilla, JsonRequestBehavior.AllowGet);
                }
        [HttpPost]
        public string vpAgregarListaValores(string id, string cadena)
        {
            string sError = "";
            DetalleMantenedor oHeader = new DetalleMantenedor();
            oHeader.guardarListaValor(id, cadena, out sError);
            ViewBag.Error = sError;
            return sError;

        }
        /// <summary>
        /// ////// ServicioNegocio
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult ServicioNegocioDetalle()
        {

            return View();
        }

        public ActionResult vpGrillaDetalleServicioNegocio(string id, string ordenarPor)
        {
            string sError = "";
            DetalleMantenedor oSNegocio = new DetalleMantenedor();

            oSNegocio.cargarGrillaServicioNegocio(out sError, id, ordenarPor);
            ViewBag.Error = sError;

            return PartialView(oSNegocio);
        }
        public ActionResult vpAgregarServicioNegocio()
        {
            string sError = "";
            DetalleMantenedor oSNegocio = new DetalleMantenedor();
            return PartialView(oSNegocio);
        }
        [HttpPost]
        public string vpAgregarServicioNegocio(string id, string idnegocio, string nombre, string descripcion)
        {
            string sError = "";
            DetalleMantenedor oSNegocio = new DetalleMantenedor();
            oSNegocio.guardarServicioNegocio(id, idnegocio, nombre, descripcion, out sError);
            ViewBag.Error = sError;
            return sError;

        }
        public JsonResult jsonSimulacionCambioCostoServicio(string id, string CMEnsual, string CInicial)
        {
            string sError = "";

            DetalleMantenedor oServicio = new DetalleMantenedor();

            oServicio.cargaSimulacionCambioCosto(id, CMEnsual, CInicial, out sError);

            ViewBag.Error = sError;
            return Json(oServicio.jsListaSimulacionCambioCosto, JsonRequestBehavior.AllowGet);
        }

        public string actualizarServicioDeNegocio(string id, string cadena)
        {
            string sError = "";
            string[] word = cadena.Split(';');
            string nombre, descripcion;


            nombre = word[0];
            descripcion = word[1];


            DetalleMantenedor oCatServicio = new DetalleMantenedor();
            oCatServicio.actualizarServicioDeNegocio(id, nombre, descripcion, out sError);
            ViewBag.Error = sError;
            return sError;

        }
        public ActionResult Costos(string id, string nuevocosto, string idformula, string idcategoria)
        {
            try
            {

                string sError = "";
                MantenedorCostos ctrlCostos = new MantenedorCostos();

                ctrlCostos.cargaParametrosPadres(out sError);
                ctrlCostos.cargaFormulas(out sError);
                ctrlCostos.cargaCategorias(out sError);
                ctrlCostos.cargaVariables(out sError);
                ctrlCostos.cargaVariablesFormulas(out sError);

                //if (id != null && nuevocosto != null)
                //{
                //    ctrlCostos.guardarCostos(id, nuevocosto, idformula, out sError);
                //}

                if (id != null && idcategoria != null)
                {
                    ctrlCostos.guardarCategoria(id, idcategoria, out sError);
                }

                return View(ctrlCostos);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public String actualizarCosto(List<iCostos> updCostoJson, List<iGlosa> updGlosaJson)
        {
            try
            {
                string sError = "";

                MantenedorCostos ctrlCostos = new MantenedorCostos();

                ctrlCostos.actualizarCosto(out sError, updCostoJson, updGlosaJson);

                ViewBag.Error = sError;

                return sError;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return "error";
            }
        }
        public ActionResult vpGrillaMantenedorCostos(string id)
        {
            try
            {

                if (id == null) { id = "0"; }

                string sError = "";
                MantenedorCostos ctrlGrillaMantenedorCostos = new MantenedorCostos();

                ctrlGrillaMantenedorCostos.cargaGrillaMantenedorCostos(id, out sError);
                ctrlGrillaMantenedorCostos.cargaVariables(out sError);

                ViewBag.Error = sError;
                ViewBag.Pagina = id;

                return PartialView(ctrlGrillaMantenedorCostos);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return PartialView();
            }
        }
        public String SimularCosto(List<iSimulacionCosto> elementoCostoJson)
        {
            string sError = "";
            
            ViewBag.Error = sError;

            DataTable tabla = new DataTable();

            MantenedorCostos ctrlSimularCosto = new MantenedorCostos();


            tabla = ctrlSimularCosto.simularCosto(elementoCostoJson, out sError);

            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);

        }
        [HttpPost]
        public Boolean modificarSimular(List<iSimulacionCosto> elementoCostoJson, string fecha)
        {
            try
            {
                string sError = "";
                JavaScriptSerializer varjon = new JavaScriptSerializer();
                MantenedorCostos ctrlSimularCosto = new MantenedorCostos();
                ctrlSimularCosto.guardaSimulacion(elementoCostoJson, fecha, out sError);
                ViewBag.Error = sError;
                return true;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return false;
            }
        }
        public ActionResult ParametrosGenerales()
        {
            try
            {
                string sError = "";

                ViewBag.Error = sError;

                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult vpGrillaParametrosGenerales(string id, string valor)
        {
            try
            {
                string sError = "";
                MantenedorParametrosGenerales ctrlGrillaParGenerales = new MantenedorParametrosGenerales();

                if (id != null && valor != null && valor != "")
                {
                    ctrlGrillaParGenerales.modificarParametroGeneral(id, valor, out sError);
                }

                ctrlGrillaParGenerales.cargaModGrillaParametrosGenerales(out sError);
                ViewBag.Error = sError;

                return PartialView(ctrlGrillaParGenerales);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return PartialView();
            }
        }
        public String ParametrosGeneralesFileXls()
        {
            string serror = "";

            DataTable tabla = new DataTable();

            MantenedorParametrosGenerales ctrlElemento = new MantenedorParametrosGenerales();


            tabla = ctrlElemento.TablaParametrosGenerales(out serror);


            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);

        }
        public ActionResult NegocioPais()
        {
            try
            {
                string sError = "";

                ViewBag.Error = sError;

                return View();
            }
            catch (Exception e)
                {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult vpGrillaNegocioPais(string id)
        {
            try
            {
                string sError = "";
                if(id == null) {
                    id = "1";
                }

                MantenedorNegocioPais ctrlNegocioPais = new MantenedorNegocioPais();
                ctrlNegocioPais.cargaPaises(out sError);
                ctrlNegocioPais.cargaNegocio(out sError);
                ctrlNegocioPais.cargaNegocioPais(id, out sError);

                ViewBag.Error = sError;
                ViewBag.pagina = id;

                return View(ctrlNegocioPais);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult guardarNegocioPais(string id, string idnegocio, string facturable, string codigo)
        {
            try
            {
                string sError = "";
                MantenedorNegocioPais ctrlNegocioPais = new MantenedorNegocioPais();
                ctrlNegocioPais.GuardarNegocioPais(id, idnegocio, facturable, codigo, out sError);
                ViewBag.Error = sError;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult eliminarNegocioPais(string id, string idnegocio)
        {
            try
            {
                string sError = "";
                MantenedorNegocioPais ctrlNegocioPais = new MantenedorNegocioPais();
                ctrlNegocioPais.EliminarNegocioPais(id, idnegocio, out sError);
                ViewBag.Error = sError;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult modificaNegocioPais(string id, string idnegocio, string codigo, string facturable)
        {
            try
            {
                string sError = "";
                MantenedorNegocioPais ctrlNegocioPais = new MantenedorNegocioPais();
                ctrlNegocioPais.ModificarNegocioPais(id, idnegocio, codigo, facturable, out sError);
                ViewBag.Error = sError;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public String NegocioPaisFileXls()
        {
            string serror = "";

            DataTable tabla = new DataTable();

            MantenedorNegocioPais ctrlElemento = new MantenedorNegocioPais();


            tabla = ctrlElemento.TablaNegocioPais(out serror);


            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);

        }
        public ActionResult CentroCosto()
        {
            try
            {
                string sError = "";
                ViewBag.Error = sError;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult vpGrillaCentroCosto(string id)
        {
            try
            {
                string sError = "";
                if (id == null)
                {
                    id = "1";
                }
                MantenedorCentroCosto ctrlCentroCosto = new MantenedorCentroCosto();
                ctrlCentroCosto.cargaPaises(out sError);
                ctrlCentroCosto.cargaCentroCosto(id, out sError);
                ViewBag.Error = sError;
                ViewBag.pagina = id;
                return PartialView(ctrlCentroCosto);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return PartialView();
            }
        }
        public String CentroCostoFileXls()
        {
            string serror = "";

            DataTable tabla = new DataTable();

            MantenedorCentroCosto ctrlElemento = new MantenedorCentroCosto();


            tabla = ctrlElemento.TablaCentroCosto(out serror);


            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);
        }
        public ActionResult guardarCentroCosto(string id, string gerencia, string facturable, string codigo)
        {
            try
            {
                string sError = "";
                MantenedorCentroCosto ctrlCentroCosto = new MantenedorCentroCosto();
                ctrlCentroCosto.GuardarCentroCosto(id, gerencia, facturable, codigo, out sError);
                ViewBag.Error = sError;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult eliminarCentroCosto(string id)
        {
            try
            {
                string sError = "";
                MantenedorCentroCosto ctrlCentroCosto = new MantenedorCentroCosto();
                ctrlCentroCosto.EliminarCentroCosto(id, out sError);
                ViewBag.Error = sError;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult modificarCentroCosto(string id, string codigo, string tipo, string firma)
        {
            try
            {
                string sError = "";
                MantenedorCentroCosto ctrlCentroCosto = new MantenedorCentroCosto();
                ctrlCentroCosto.ModificaCentroCosto(id, codigo, tipo, firma, out sError);
                ViewBag.Error = sError;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult ServicioNegocio(string id, string ordenarPor, string orden, string ascDesc)
        {
            
            string sError = "";
            string idHeadAct  ;
            string ascDescAct ;
            idHeadAct = ordenarPor;
            ascDescAct = ascDesc;

            if (id == null)
            {

                id = "1";

            }
            if (ordenarPor == null) {

                ordenarPor = "Idserviciodenegocio";
                
            }
            if (orden == null)
            {

                orden = "1";

            }

            ServicioNegocioMantenedor ctrlServicioNegocio = new ServicioNegocioMantenedor();


            ctrlServicioNegocio.cargarServicioNegocio(id, ordenarPor, orden, out sError);
                ctrlServicioNegocio.cargarNegocioPais(out sError);
                
                

            ViewBag.Error = sError;
            ViewBag.idHeadAct = idHeadAct;
            ViewBag.ascDescAct = ascDescAct;
            ViewBag.OrdenarPor = ordenarPor;
            ViewBag.Orden = orden;
            return View(ctrlServicioNegocio);
           
        }
        public Boolean eliminarServicioNegocio(string id)
        {
            try
            {
                string sError = "";
                ServicioNegocioMantenedor ctrlServicioNegocio = new ServicioNegocioMantenedor();
                ctrlServicioNegocio.EliminarServicioDeNegocio(id, out sError);
                ViewBag.Error = sError;
                return true;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return false;
            }
        }
        [HttpPost]
        public ActionResult jsonActualizarServicioDeNegocio(List<ServicioDeNegocio> ServicioDeNegocio)
        {
            string sError = "";
            ServicioNegocioMantenedor oManDetalle = new ServicioNegocioMantenedor();
            oManDetalle.ActualizarServicioDeNegocio(ServicioDeNegocio, out sError);
            return null;
        }
        public String jsonActualizarServicioNegocio(List<iSimulacionCosto> elementoCostoJson)
        {
            string sError = "";
            JavaScriptSerializer varjon = new JavaScriptSerializer();
            MantenedorCostos ctrlSimularCosto = new MantenedorCostos();

            ctrlSimularCosto.simularCosto(elementoCostoJson, out sError);

            ViewBag.Error = sError;

            return varjon.Serialize(ctrlSimularCosto.dtSimulacionJson);
        }

        public ActionResult Familia(List<iNegocioPais> familiasubfamiliaJson)
        {
            try
            {
                string sError = "";

                MantenedorFamilia ctrlFamilia = new MantenedorFamilia();

                if (familiasubfamiliaJson != null)
                {
                    ctrlFamilia.ModificarSubfamilia(familiasubfamiliaJson, out sError);
                }

                ctrlFamilia.cargaFamilia(out sError);
                ctrlFamilia.cargaSubfamilia(out sError);
                ctrlFamilia.cargaFamiliaSubfamilia(out sError);

                ViewBag.Error = sError;

                return View(ctrlFamilia);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        /// <summary>
        /// vistas pantalla Familia
        /// </summary>
        /// <returns></returns>
        public ActionResult FamiliaDetalle()
        {

            return View();
        }
        [HttpGet]
        public ActionResult vpGrillaDetalleFamilia(string id, string ordenarPor)
        {
            string sError = "";
            MantenedorFamilia ctrlGrillaFamilia = new MantenedorFamilia();
            ctrlGrillaFamilia.cargaModGrillaDetalleFamilia(id, ordenarPor, out sError);
            //ctrlGrillaFamilia.cargaListaFamilia(out sError);
            ViewBag.Error = sError;
            return PartialView(ctrlGrillaFamilia);
        }
        [HttpPost]
        public string actualizarFamilia(string id, string nombre)
        {
            string sError = "";
            MantenedorFamilia oHeader = new MantenedorFamilia();
            oHeader.ActualizarFamilia(id, nombre, out sError);
            ViewBag.Error = sError;
            return sError;
        }
        public string vpAgregarFamilia(string id)
        {
            string sError = "";
            MantenedorFamilia oHeader = new MantenedorFamilia();
            oHeader.agregarFamilia(id, out sError);
            ViewBag.Error = sError;
            return sError;
        }

        public String FamiliaFileXls()
        {
            string serror = "";

            DataTable tabla = new DataTable();

            MantenedorFamilia ctrlElemento = new MantenedorFamilia();


            tabla = ctrlElemento.TablaFamilias(out serror);


            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);

        }

        public ActionResult SubFamiliaDetalle()
        {
            return View();
        }
        [HttpGet]
        public ActionResult vpGrillaDetalleSubFamilia(string id, string ordenarPor)
        {
            string sError = "";
            MantenedorFamilia ctrlGrillaSubFamilia = new MantenedorFamilia();
            ctrlGrillaSubFamilia.cargaModGrillaDetalleSubFamilia(id, ordenarPor, out sError);
            ctrlGrillaSubFamilia.cargaListaFamilia(out sError);
            ViewBag.Error = sError;
            return PartialView(ctrlGrillaSubFamilia);
        }
        [HttpPost]
        public string actualizarSubfamila(string id, string nombre, string Familia)
        {
            string sError = "";
            MantenedorFamilia oHeader = new MantenedorFamilia();
            oHeader.ActualizarSubFamilia(id, nombre, Familia, out sError);
            ViewBag.Error = sError;
            return sError;
        }
        public string vpAgregarSubFamilia(string id, string cadena)
        {
            string sError = "";
            MantenedorFamilia oHeader = new MantenedorFamilia();
            oHeader.agregarSubFamilia(id, cadena, out sError);
            ViewBag.Error = sError;
            return sError;
        }
        public string SubFamiliaFileXls()
        {
            string serror = "";

            DataTable tabla = new DataTable();

            MantenedorFamilia ctrlElemento = new MantenedorFamilia();


            tabla = ctrlElemento.TablaSubFamilias(out serror);


            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);

        }
        public ActionResult Feriados(string id, string descripcion)
        {
            try
            {
                string sError = "";
                Feriados ctrlFeriados = new Feriados();
                if(id != null && descripcion != null) {
                    ctrlFeriados.AgregarFeriado(id, descripcion, out sError);
                }
                ctrlFeriados.CargaFeriados(out sError);
                ViewBag.Error = sError;
                return View(ctrlFeriados);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public String FeriadosFileXls()
        {
            string serror = "";

            DataTable tabla = new DataTable();

            Feriados ctrlElemento = new Feriados();


            tabla = ctrlElemento.TablaFeriados(out serror);


            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);

        }
        [HttpPost]
        public ActionResult eliminarFeriados(string id)
        {
            try
            {
                string sError = "";
                Feriados ctrlFeriados = new Feriados();
                ctrlFeriados.EliminarFeriado(id, out sError);
                ViewBag.Error = sError;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult PlantillaSubservicio()
        {
            try
            {
                string sError = "";

                MantenedorPlantillaSubservicio ctrlPlantillaSubservicio = new MantenedorPlantillaSubservicio();

                ctrlPlantillaSubservicio.cargaServicio(out sError);

                return View(ctrlPlantillaSubservicio);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult vpGrillaPlantillaSubservicio(string id)
        {
            try
            {
                string sError = "";

                if (id == null)
                {
                    id = "1";
                }

                MantenedorPlantillaSubservicio ctrlPlantillaSubservicio = new MantenedorPlantillaSubservicio();

                ctrlPlantillaSubservicio.cargaPlantillaSubservicio(id, out sError);

                ViewBag.Error = sError;

                return PartialView(ctrlPlantillaSubservicio);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return PartialView();
            }
        }
        public ActionResult vpGrillaPlantillaSubservicio_Subservicios(string pagina, string idplantillasubservicio)
        {
            try
            {
                string sError = "";

                if (pagina == null)
                {
                    pagina = "1";
                }
                if (idplantillasubservicio == null)
                {
                    idplantillasubservicio = "1";
                }

                MantenedorPlantillaSubservicio ctrlPlantillaSubservicio = new MantenedorPlantillaSubservicio();

                ctrlPlantillaSubservicio.cargaSubservicio(pagina, idplantillasubservicio, out sError);
                ctrlPlantillaSubservicio.cargaSubservicioComprobar(idplantillasubservicio, out sError);

                ViewBag.Error = sError;
                ViewBag.idplantillasubservicio = idplantillasubservicio;

                return PartialView(ctrlPlantillaSubservicio);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return PartialView();
            }
        }
        public ActionResult GuardarSubservicio(string idplantillasubservicio,string nombre, string horas)
        {
            try
            {
                string sError = "";

                MantenedorPlantillaSubservicio ctrlPlantillaSubservicio = new MantenedorPlantillaSubservicio();

                ctrlPlantillaSubservicio.GuardarSubservicio(idplantillasubservicio, nombre, horas, out sError);

                ViewBag.Error = sError;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult GuardarPlantillaSubservicio(string nombre, string idcatalogoservicio)
        {
            try
            {
                string sError = "";

                MantenedorPlantillaSubservicio ctrlPlantillaSubservicio = new MantenedorPlantillaSubservicio();

                ctrlPlantillaSubservicio.GuardarPlantillaSubservicio(nombre, idcatalogoservicio, out sError);

                ViewBag.Error = sError;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult EliminarSubservicio(string idsubservicio)
        {
            try
            {
                string sError = "";

                MantenedorPlantillaSubservicio ctrlPlantillaSubservicio = new MantenedorPlantillaSubservicio();

                ctrlPlantillaSubservicio.EliminarSubservicio(idsubservicio, out sError);

                ViewBag.Error = sError;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult EliminarPlantillaSubservicio(string idplantillasubservicio, string idservicio)
        {
            try
            {
                string sError = "";

                MantenedorPlantillaSubservicio ctrlPlantillaSubservicio = new MantenedorPlantillaSubservicio();

                ctrlPlantillaSubservicio.EliminarPlantillaSubservicio(idplantillasubservicio, idservicio, out sError);

                ViewBag.Error = sError;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public ActionResult ModificarSubservicio(string idsubservicio, string nombre, string horas)
        {
            try
            {
                string sError = "";

                MantenedorPlantillaSubservicio ctrlPlantillaSubservicio = new MantenedorPlantillaSubservicio();

                ctrlPlantillaSubservicio.ModificarSubservicio(idsubservicio, nombre, horas, out sError);

                ViewBag.Error = sError;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        public String PlantillaSubservicioFileXls()
        {
            string serror = "";

            DataTable tabla = new DataTable();

            MantenedorPlantillaSubservicio ctrlElemento = new MantenedorPlantillaSubservicio();


            tabla = ctrlElemento.TablaPlantillaSubservicio(out serror);


            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = tabla.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in tabla.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Trim());

                sb.AppendLine(string.Join(";", fields));
            }

            byte[] plainTextBytes = System.Text.Encoding.UTF32.GetBytes(sb.ToString());

            return System.Convert.ToBase64String(plainTextBytes);

        }
    }
}