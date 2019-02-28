using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Web.Helpers;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Claims;
using Atech.DataAccessLayer;


namespace SistemaGestionCotizaciones.Models
{


    public class DetalleMantenedor
    {
        public string dbConn, dbConnPerfil;
        public string totalPaginas{ get; set; }
        public string totalRegistros{ get; set; }
        //header
        public string idTipoDato { get; set; }
        public string nombreAtributo { get; set; }
        public string descripcion { get; set; }
        public string usrCreacion { get; set; }
        public string usrModificador { get; set; }
        //tipoDato
        public string nombre { get; set; }
        public string largo { get; set; }
        public string contrl { get; set; }
        //listaValores
        public string idListaValores { get; set; }
        public string idHeader { get; set; }
        public string Valor { get; set; }
        //CATALOGO servicio
        public string Nombreservicio { get; set; }
        public string idComponente { get; set; }
        public string SubGerencia { get; set; }
        public string TipoCobroInicial { get; set; }
        public string TipoCobroMensual { get; set; }
        public string Cantidad { get; set; }
        public string TipoUnidad { get; set; }
        public string CostoUnidadinicial { get; set; }
        public string CostoUnidadMensual { get; set; }
        public string Dominiopais { get; set; }
        public string Dominionegocio { get; set; }
        public string Fecha { get; set; }
        // BOOL
        public bool AplicacionInmediata { get; set; }
        public bool habilitado { get; set; }
        //Formula
        public string Funcion { get; set; }
        public string numVariables { get; set; }
        //dropdown
        public string selectedHeaderList { get; set; }
        public SelectList HeaderList { get; set; }
        public SelectList RolList { get; set; }
        public DataTable dtListHeader { get; set; }
        public DataTable dtListNegocio { get; set; }
        public DataTable dtListVariables { get; set; }

        public List<Dictionary<object, string>> dtPlantillas { get; set; }
        public List<Dictionary<object, string>> dtElementos { get; set; }
        public List<Dictionary<object, string>> dtServicios { get; set; }
        //TABLAS

        public DataTable dtHeaders { get; set; }
        public DataTable dtConceptoCosto { get; set; }
        public DataTable dtCentroCosto { get; set; }
        public DataTable dtListaSubGerencia { get; set; }
        public DataTable dtListPais { get; set; }
        public DataTable dtTipodato { get; set; }
        public DataTable dtListaValoresCE { get; set; }
        public DataTable dtServicio { get; set; }
        public DataTable dtNegocio { get; set; }
        public DataTable dtPais { get; set; }
        public DataTable dtTransicionEstado { get; set; }
        public DataTable dtGroupProfiler { get; set; }
        public DataTable dtAplicacion { get; set; }
        public DataTable dtRol { get; set; }
        public DataTable dtAccion { get; set; }
        public DataTable dtUsuarios { get; set; }
        public DataTable dtServicioNegocio { get; set; }
        public DataTable dtFormulas { get; set; }
        public DataTable dtFunciones { get; set; }
        /// <summary>
        /// JSON
        /// </summary>
        ///       
        public List<Dictionary<object, string>> jsListaValoresGrilla { get; set; }
        public List<Dictionary<object, string>> jsListaValores { get; set; }
        public List<Dictionary<object, string>> jsListaSimulacionCambioCosto { get; set; }
        public List<Dictionary<object, string>> jsListaUnidadesCatServicio { get; set; }
        public List<Dictionary<object, string>> jsListaCatalogoDeServicio { get; set; }
        public List<Dictionary<object, string>> jsListaFunciones{ get; set; }
        public List<Dictionary<object, string>> jsListaElementos { get; set; }
        public List<Dictionary<object, string>> jsListaConceptoCosto { get; set; }
        public List<Dictionary<object, string>> jsListaNegocio { get; set; }
        public List<Dictionary<object, string>> jsListaPais { get; set; }
        public List<Dictionary<object, string>> dtListaValoresJson { get; set; }
        public List<Dictionary<object, string>> dtTipoServicioJson { get; set; }

        public List<Dictionary<object, string>> dtDetalleCentroCostoParamCostoJson { get; set; }
        public List<Dictionary<object, string>> dtCentroCostoJson { get; set; }

        public List<Dictionary<object, string>> dtDetalleCentroCostoServicioJson { get; set; }
        public List<Dictionary<object, string>> dtPaisesJson { get; set; }
        public List<Dictionary<object, string>> dtNegociosJson { get; set; }

        public DetalleMantenedor()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
            //this.dbConnPerfil = ConfigurationManager.ConnectionStrings["FalabellaPerfilamiento"].ConnectionString;

        }
        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }
        public Boolean cargaModGrillaDetalle(out string sErrors, string pagina, string ordenarPor, string ascDesc,
            string idtipodato, string categoria)
        {

            DataTable dtHeadersSp = new DataTable();
            SqlAccess comHeader = new SqlAccess(dbConn);
            string codError = "";


            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_HEADERS", out dtHeadersSp,out codError, out sErrors, pagina,
                ordenarPor + ascDesc, idtipodato, categoria);


            if (!result)
            {
                return false;
            }


            this.dtHeaders = dtHeadersSp;

            return true;
        }
        public bool GuardarHeader(string nombre, string descripcion, string idTipoDato, string categoria, out string sError)
        {
            /*             
            @param1  VARCHAR(10), --Id del tipo dato
            @param2  VARCHAR(10), --Nombre del atributo		
            @param3  VARCHAR(255), --Descripcion
            @param4  VARCHAR(255), --Id persona logueada
                    
             */

            usrCreacion = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_HEADER", out sError, idTipoDato, nombre, descripcion, categoria, usrCreacion);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public bool GuardarHeader1(string nombre, string descripcion, string idTipoDato, string categoria, string cadena, out string sError)
        {
            /*             
            @param1  VARCHAR(10), --Id del tipo dato
            @param2  VARCHAR(10), --Nombre del atributo		
            @param3  VARCHAR(255), --Descripcion
            @param4  VARCHAR(255), --Id persona logueada
                    
             */

            usrCreacion = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_CATALOGO_ELEMENTO_01", out sError, idTipoDato, nombre, descripcion, categoria, cadena, usrCreacion);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public bool ActualizarComponenteElem(string id, string idTipoDato, string nombre, string descripcion, string categoria, out string sError)
        {
            usrCreacion = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_CATALOGO_ELEMENTO", out sError, id, idTipoDato, nombre, descripcion, categoria, usrCreacion);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }

        public Boolean cargarCentroCosto(out string sError)
        {
            DataTable centrocostosp = new DataTable();
            SqlAccess concentrocosto = new SqlAccess(dbConn);
            string codError = "";
            Boolean result = concentrocosto.Consultar("USP_SEL_LISTA_CENTRO_COSTOS_00", out centrocostosp,out codError, out sError);
            if (!result)
            {
                return false;
            }

            this.dtCentroCosto = centrocostosp;
            return true;
        }

        public Boolean cargaModGrillaConceptoCostoDetalle(out string sErrors, string pagina, string ordenarPor)
        {

            DataTable dtHeadersSp = new DataTable();
            SqlAccess comHeader = new SqlAccess(dbConn);
            string codError = "";


            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_CONCEPTO_COSTO", out dtHeadersSp,out codError, out sErrors, pagina, ordenarPor);


            if (!result)
            {
                return false;
            }


            this.dtConceptoCosto = dtHeadersSp;

            return true;
        }
        public bool GuardarConceptoCosto(string nombre, string descripcion, string grupo, string centrocosto, out string sError)
        {

            usrCreacion = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_CONCEPTO_COSTO_01", out sError, nombre, descripcion, grupo, usrCreacion, centrocosto);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }

        public bool ActualizarConceptoCosto(string id, string nombre, string descripcion, string grupo, out string sError)
        {
            usrCreacion = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_CATALOGO_CONCEPTO_COSTO", out sError, id, nombre, descripcion, grupo, usrCreacion);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public bool ActualizarListaValor(string id, string valor, string descripcion, out string sError)
        {
            usrCreacion = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_LISTA_VALOR", out sError, id, valor, descripcion, usrCreacion);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }

        public bool ActualizarDominioCatalogoServ(string id, string pais, string negocio, string SubGerencia, out string sError)
        {
            usrCreacion = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_CATALOGO_SERVICIO_DOMINIO", out sError, id, pais, negocio, SubGerencia, usrCreacion);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public Boolean GetHeaderList(out string sErrors)
        {
            sErrors = "";

            DataTable dtListaHeadersSp = new DataTable();
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por id y nombreatributo para llenar un dropdown
            string codError = "";

            Boolean result = comHeader.Consultar("USP_SEL_LISTA_HEADERS_01", out dtListaHeadersSp,out codError, out sErrors);

            if (!result)
            {
                return false;
            }


            this.dtListHeader = dtListaHeadersSp;

            return true;

        }
        public Boolean GetSubGerenciaList(out string sErrors)
        {
            sErrors = "no hay datos";

            //DataTable dtListaVariablesSp = new DataTable();
            //CotizadorDAL.CotizadorDAL comHeader = new CotizadorDAL.CotizadorDAL(dbConn);
            ////lista los headers por id y nombreatributo para llenar un dropdown

            //Boolean result = comHeader.Consultar("USP_SEL_LISTA_CATALOGO_ELEMENTO_VARIABLE", out dtListaVariablesSp, out sErrors);

            //if (!result)
            //{
            //    return false;
            //}

            return false;
            //this.dtListVariables = dtListaVariablesSp;

            //return true;
        }
        public Boolean GetVariablesList(out string sErrors)
        {
            sErrors = "";

            DataTable dtListaVariablesSp = new DataTable();
            SqlAccess comHeader = new SqlAccess(dbConn);
            string codError = "";
            //lista los headers por id y nombreatributo para llenar un dropdown

            Boolean result = comHeader.Consultar("USP_SEL_LISTA_CATALOGO_ELEMENTO_VARIABLE", out dtListaVariablesSp,out codError, out sErrors);

            if (!result)
            {
                return false;
            }


            this.dtListVariables = dtListaVariablesSp;

            return true;
        }
        public Boolean GetCompElemntList(out string sErrors)
        {
            sErrors = "";
            string codError="";

            DataTable dtListaHeadersSp = new DataTable();
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por id y nombreatributo para llenar un dropdown

            Boolean result = comHeader.Consultar("USP_SEL_LISTA_HEADERS_02", out dtListaHeadersSp,out codError, out sErrors);

            if (!result)
            {
                return false;
            }


            this.dtListHeader = dtListaHeadersSp;

            return true;
        }
        public Boolean GetNegocioList(out string sErrors)
        {
            sErrors = "";
            string codError = "";

            DataTable dtListaNegocioSp = new DataTable();
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por id y nombreatributo para llenar un dropdown

            Boolean result = comHeader.Consultar("USP_SEL_LISTA_NEGOCIO_01", out dtListaNegocioSp,out codError, out sErrors);

            if (!result)
            {
                return false;
            }


            this.dtListNegocio = dtListaNegocioSp;

            return true;
        }
        public Boolean GetPaisList(out string sErrors)
        {
            sErrors = "";
            string codError = "";

            DataTable dtListaPaisSp = new DataTable();
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por id y nombreatributo para llenar un dropdown

            Boolean result = comHeader.Consultar("USP_SEL_LISTA_PAIS_01", out dtListaPaisSp,out codError, out sErrors);

            if (!result)
            {
                return false;
            }


            this.dtListPais = dtListaPaisSp;

            return true;
        }

        

        public Boolean cargarDetalleParamCostoCentroCosto(string idservicio, out string sErrors)
        {
            sErrors = "";
            DataTable dt = new DataTable();
            SqlAccess cargaCCS = new SqlAccess(dbConn);
            string codError = "";
            Boolean res = cargaCCS.Consultar("USP_SEL_LISTA_PARAMCOSTO_CC", out dt,out codError, out sErrors, idservicio);
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

            this.dtDetalleCentroCostoParamCostoJson = rows;
            return true;
        }

        public Boolean cargarDetalleServicioCentroCosto(string idservicio, out string sErrors)
        {
            sErrors = "";
            string codError = "" ;
            DataTable dt = new DataTable();
            SqlAccess cargaCCS = new SqlAccess(dbConn);
            Boolean res = cargaCCS.Consultar("USP_SEL_LISTA_SERVICIO_CC", out dt,out codError, out sErrors, idservicio);
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

            this.dtDetalleCentroCostoServicioJson = rows;
            return true;
        }


        public Boolean CargaNegocios(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_NEGOCIO_01", out dt,out codError, out sError);
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

            this.dtNegociosJson = rows;
            return true;
        }
        public Boolean CargaPaises(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_PAIS_01", out dt,out codError, out sError);
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

            this.dtPaisesJson = rows;
            return true;
        }
        public Boolean eliminarComponenteElem(string id, out string sError)
        {

            sError = "";
            Boolean res;
            DataTable dtPlantillasSp = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);

            res = cDAL.Consultar("USP_DEL_CATALOGO_ELEMENTO", out dtPlantillasSp,out codError, out sError, id);

            if (!res)
            {
                return false;
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<object, string>> rows = new List<Dictionary<object, string>>();
            Dictionary<object, string> row;

            foreach (DataRow dr in dtPlantillasSp.Rows)
            {
                row = new Dictionary<object, string>();
                foreach (DataColumn col in dtPlantillasSp.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);
            }

            this.dtPlantillas = rows;
            return true;
        }
        public Boolean eliminarConceptoCosto(string id, out string sError)
        {

            sError = "";
            Boolean res;
            DataTable dtElementosSp = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);

            res = cDAL.Consultar("USP_DEL_CONCEPTO_COSTO", out dtElementosSp,out codError, out sError, id);

            if (!res)
            {
                return false;
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<object, string>> rows = new List<Dictionary<object, string>>();
            Dictionary<object, string> row;

            foreach (DataRow dr in dtElementosSp.Rows)
            {
                row = new Dictionary<object, string>();
                foreach (DataColumn col in dtElementosSp.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);
            }

            this.dtElementos = rows;
            return true;
        }
        public Boolean eliminarListaValores(string idlista, out string sError)
        {

            sError = "";
            Boolean res;
            DataTable dtServiciosSp = null;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_DEL_LISTA_VALORES_00", out sError, idlista);
            if (!res)
            {

                return false;
            }

            return true;
        }
        public string dependenciasComponenteElem(string id, out string sError)
        {
            Boolean res;
            sError = "";
            DataTable dtDependencias;
            string codError = "";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_MSG_DEL_CATALOGO_ELEMENTO", out dtDependencias,out codError, out sError, id);
            if (res)
            {
                foreach (DataRow col in dtDependencias.Rows)
                {
                    sError = @col.ItemArray.GetValue(0).ToString();
                };
                return sError;
            }
            else
            {
                return sError;
            }
        }
        public Boolean cargaModGrillaDetalleTipoDato(out string sErrors, string pagina, string ordenarPor)
        {


            DataTable dtTipoDatoSP = new DataTable();
            SqlAccess comHeader = new SqlAccess(dbConn);
            string codError = "";

            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_TIPO_DATO_01", out dtTipoDatoSP,out codError, out sErrors, pagina, ordenarPor);


            if (!result)
            {
                return false;
            }


            this.dtTipodato = dtTipoDatoSP;

            return true;
        }
        public bool ActualizarTpoDato(string id, string nombre, string contrl, out string sError)
        {
            usrModificador = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_TIPO_DATO", out sError, id, nombre, contrl, usrModificador);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public bool GuardarTipoDato(out string sError)
        {
            /* @param1  VARCHAR(255), --Nombre del tipo dato
               @param2  VARCHAR(255), --largo		
               @param3  VARCHAR(10), --contrl
               @param4  VARCHAR(255), --Id persona logueada
   */
            usrCreacion = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_TIPO_DATO", out sError, this.nombre, this.largo, this.contrl, usrCreacion);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public Boolean cargaModGrillaDetalleListaValor(out string sErrors, string pagina, string ordenarPor, string orden)
        {


            DataTable dtListaValoresSP = new DataTable();
            SqlAccess comHeader = new SqlAccess(dbConn);
            string codError = "";

            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_VALORES_00", out dtListaValoresSP,out codError, out sErrors, pagina, ordenarPor, orden);


            if (!result)
            {
                return false;
            }

            List<Dictionary<object, string>> rows = new List<Dictionary<object, string>>();
            Dictionary<object, string> row;

            foreach (DataRow dr in dtListaValoresSP.Rows)
            {
                row = new Dictionary<object, string>();
                foreach (DataColumn col in dtListaValoresSP.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);

                totalPaginas = dr.ItemArray.GetValue(4).ToString();
                totalRegistros = dr.ItemArray.GetValue(5).ToString();
            }

            this.dtListaValoresJson = rows;

            return true;
        }
        public Boolean cargaListaValorPorCE(out string sErrors, string idCatalogoElemento)
        {
            DataTable dt = new DataTable();
            SqlAccess comHeader = new SqlAccess(dbConn);
            string codError = "";

            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_VALORES_03", out dt,out codError, out sErrors, idCatalogoElemento);

            if (!result)
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

            this.jsListaValores = rows;
            this.dtListaValoresCE = dt;
            return true;
        } public Boolean cargaListaValorGrilla(out string sErrors)
        {
            DataTable dt = new DataTable();
            SqlAccess comHeader = new SqlAccess(dbConn);
            string codError = "";

            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_VALORES_04", out dt,out codError, out sErrors);

            if (!result)
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

            this.jsListaValoresGrilla = rows;
          
            return true;
        }
        public Boolean cargaListaUnidadesCatServicios(out string sErrors)
        {
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comHeader = new SqlAccess(dbConn);

            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_UNIDADES_CATALOGO_SERVICIO", out dt,out codError, out sErrors);

            if (!result)
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

            this.jsListaUnidadesCatServicio = rows;

            return true;
        }
        public Boolean cargaListaCatServicios(out string sErrors)
        {
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comHeader = new SqlAccess(dbConn);

            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_CATALOGO_SERVICIO_DOMINIO_01", out dt,out codError, out sErrors);

            if (!result)
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

            this.jsListaCatalogoDeServicio = rows;

            return true;
        }

        public Boolean CargaCentroCosto(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_CENTRO_COSTOS_01", out dt,out codError, out sError);
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
            this.dtCentroCostoJson = rows;
            return true;
        }

        public Boolean CargaTipoServicio(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_TIPOSERVICIO", out dt,out codError, out sError);
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

            this.dtTipoServicioJson = rows;
            return true;
        }
        [HttpPost]
        public bool GuardarCatalogoServicio(List<CatalogoServicio> Servicio, out string sError)
        {
            sError = "";
            Boolean res;
            usrCreacion = GetUserName();
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(Servicio.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlJson = "";
            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, Servicio, emptyNamepsaces);
                xmlJson = stream.ToString();
            }
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_CATALOGO_SERVICIO_00", out sError, xmlJson, usrCreacion);
            if (!res)
            {
                return false;
            }
            return true;
        }
        public Boolean GetTipoDatoList(out string sErrors)
        {
            sErrors = "";
            string codError = "";

            DataTable dtListaTipoDatoSp = new DataTable();
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por id y nombreatributo para llenar un dropdown

            Boolean result = comHeader.Consultar("USP_SEL_LISTA_TIPO_DATO", out dtListaTipoDatoSp,out codError, out sErrors);

            if (!result)
            {
                return false;
            }

            if (dtListaTipoDatoSp.Rows.Count > 0)
            {
                this.dtTipodato = dtListaTipoDatoSp;

                return true;

            }
            else
            {
                return false;
            }
        }

        

        public Boolean cargarGrillaServicioNegocio(out string sErrors, string pagina, string ordenarPor)
        {
            sErrors = "";
            string codError = "";

            DataTable dtServicioNegocioSp = new DataTable();
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por id y nombreatributo para llenar un dropdown

            Boolean result = comHeader.Consultar("USP_SEL_LISTA_SERVICIO_NEGOCIO", out dtServicioNegocioSp,out codError, out sErrors, pagina, ordenarPor);

            if (!result)
            {
                return false;
            }

            if (dtServicioNegocioSp.Rows.Count > 0)
            {
                this.dtServicioNegocio = dtServicioNegocioSp;

                return true;

            }
            else
            {
                return false;
            }
        }

        public Boolean guardarDetalleParamCostoCC(string idparamcosto, string dtcentrocosto, out string sErrors)
        {
            SqlAccess guardarDetalleServicioCC = new SqlAccess(dbConn);
            Boolean result = guardarDetalleServicioCC.Ejecutar("USP_INS_DETALLE_PARAM_COSTO_CC", out sErrors, idparamcosto, dtcentrocosto);
            if (!result)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Boolean guardarDetalleServicioCentroCosto(string idservicio, string dtcentrocosto, out string sErrors)
        {
            SqlAccess guardarDetalleServicioCC = new SqlAccess(dbConn);
            Boolean result = guardarDetalleServicioCC.Ejecutar("USP_INS_DETALLE_SERVICIO_COSTO", out sErrors, idservicio, dtcentrocosto);
            if (!result)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Boolean guardarServicioNegocio(string id, string idnegocio, string nombre, string descripcion, out string sErrors)
        {

            usrCreacion = GetUserName();
            SqlAccess comListaValor = new SqlAccess(dbConn);
            Boolean result = comListaValor.Ejecutar("USP_INS_SERVICIO_NEGOCIO_00", out  sErrors, id, idnegocio, nombre, descripcion, usrCreacion);

            if (!result)
            {
                return false;
            }
            else
                return true;
        }
        public Boolean guardarListaValor(string id, string cadena, out string sErrors)
        {


            usrCreacion = GetUserName();
            SqlAccess comListaValor = new SqlAccess(dbConn);
            Boolean result = comListaValor.Ejecutar("USP_INS_LISTA_VALOR", out  sErrors, id, cadena, usrCreacion);

            if (!result)
            {
                return false;
            }
            else
                return true;
        }
        public Boolean cargaModGrillaPais(out string sErrors, string pagina, string ordenarPor)
        {
            DataTable dtPaisSP = new DataTable();
            string codError = "";
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_PAIS", out dtPaisSP,out codError, out sErrors, pagina, ordenarPor);
            if (!result)
            {
                return false;
            }
            this.dtPais = dtPaisSP;
            return true;
        }
        public Boolean cargaModGrillaFormula(out string sErrors, string pagina, string ordenarPor)
        {
            DataTable dtFormulaSP = new DataTable();
            string codError = "";
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_FORMULAS", out dtFormulaSP,out codError, out sErrors, pagina, ordenarPor);
            if (!result)
            {
                return false;
            }
            this.dtFormulas = dtFormulaSP;
            return true;
        }
        public Boolean cargaModGrillaNegocio(out string sErrors, string pagina, string ordenarPor)
        {
            DataTable dtNegocioSP = new DataTable();
            string codError = "";
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_NEGOCIO", out dtNegocioSP,out codError, out sErrors, pagina, ordenarPor);
            if (!result)
            {
                return false;
            }
            this.dtNegocio = dtNegocioSP;
            return true;
        }
        public Boolean cargaModGrillaCatalogoServicio(out string sErrors, string pagina, string ordenarPor)
        {
            DataTable dtServicioSP = new DataTable();
            string codError = "";
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("[USP_SEL_LISTA_CATALOGO_SERVICIO]", out dtServicioSP,out codError, out sErrors, pagina, ordenarPor);
            if (!result)
            {
                return false;
            }
            this.dtServicio = dtServicioSP;
            return true;
        }
        public Boolean ActualizarCatalogoServ(string id, string servicio, string descripcion, string
        tipoCobroInicial, string tipoCobroMensual, string tipoUnidad,string tipoUnidadMensual, out string sError)
        {
            usrCreacion = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_CATALOGO_SERVICIO", out sError, id, servicio,
                                 descripcion, tipoCobroInicial, tipoCobroMensual, tipoUnidad,tipoUnidadMensual,
                                  usrCreacion);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public Boolean ActualizarFechaAplicacionCostoCatalogoServ(string id, string fecha, string CUInicial, string CUMensual, string aplicacion, out string sError)
        {

            usrCreacion = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_CATALOGO_SERVICIO_FECHA_APLICACION_COSTO", out sError, id, fecha,
                                 CUInicial, CUMensual, aplicacion, usrCreacion);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public Boolean eliminarCatalogoServicio(string id, out string sError)
        {

            sError = "";
            Boolean res;
            DataTable dtServiciosSp = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_DEL_CATALOGO_SERVICIO", out dtServiciosSp,out codError, out sError, id);
            if (!res)
            {

                return false;
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<object, string>> rows = new List<Dictionary<object, string>>();
            Dictionary<object, string> row;
            foreach (DataRow dr in dtServiciosSp.Rows)
            {
                row = new Dictionary<object, string>();
                foreach (DataColumn col in dtServiciosSp.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);
            }
            this.dtServicios = rows;
            return true;
        }
        public string dependenciasCatalogoServicio(string id, out string sError)
        {
            Boolean res;
            sError = "";
            DataTable dtDependencias;
            string codError = "";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_MSG_DEL_CATALOGO_SERVICIO", out dtDependencias,out codError, out sError, id);
            if (res)
            {
                foreach (DataRow col in dtDependencias.Rows)
                {
                    sError = @col.ItemArray.GetValue(0).ToString();
                };
                return sError;
            }
            else
            {
                return sError;
            }
        }
        public Boolean cargaSimulacionCambioCosto(string idServicio, string CostoUnidadmensual, string CostoUnidadinicial, out string sErrors)
        {
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comHeader = new SqlAccess(dbConn);

            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("SP_SIMULACION_CAMBIO_COSTO_SERVICIO", out dt,out codError, out sErrors, idServicio, CostoUnidadmensual, CostoUnidadinicial);

            if (!result)
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

            this.jsListaSimulacionCambioCosto = rows;

            return true;
        }
        public Boolean GuardarPais(out string sError, string nombre)
        {

            usrCreacion = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_PAIS", out sError, nombre, usrCreacion);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public bool ActualizarPais(string id, string nombre, out string sError)
        {
            usrModificador = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_PAIS", out sError, id, nombre, usrModificador);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public Boolean GuardarNegocio(out string sError, string nombre)
        {

            usrCreacion = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_NEGOCIO", out sError, nombre, usrCreacion);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public bool ActualizarNegocio(string id, string descripcion, out string sError)
        {
            usrModificador = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_NEGOCIO", out sError, id, descripcion, usrModificador);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public bool ActualizarFormula(string id, string nombre, string descripcion, string funcion, string variables, string estado, out string sError)
        {
            usrModificador = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_FORMULA", out sError, id, nombre, descripcion, funcion, variables, estado, usrModificador);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }


        public Boolean GuardarFormula(string Nombreformula, string Descripcion, string Funcion, string NumVariables, out string sError)
        {

            usrCreacion = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_FORMULA", out sError, Nombreformula, Descripcion, Funcion, NumVariables, usrCreacion);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public Boolean cargaFunciones(out string sErrors)
        {
            DataTable dtFuncionesSP = new DataTable();
            string codError = "";
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_FUNCION_00", out dtFuncionesSP, out codError, out sErrors);
            if (!result)
            {
                return false;
            }
            this.dtFunciones = dtFuncionesSP;
            return true;
        }
        public Boolean cargaListafunciones(out string sErrors)
        {
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_FORMULAS_02", out dt,out codError, out sErrors);
            if (!result)
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

            this.jsListaFunciones = rows;

            return true;
        }
        public Boolean cargaListaElementos(out string sErrors)
        {
            DataTable dt = new DataTable();
            SqlAccess comHeader = new SqlAccess(dbConn);
            string codError = "";
            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_CATALOGO_ELEMENTO_00", out dt,out codError, out sErrors,"2");
            if (!result)
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

            this.jsListaElementos = rows;

            return true;
        }
        public Boolean cargaListaConceptoCosto(out string sErrors)
        {
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_LISTA_CATALOGO_ELEMENTO_00", out dt,out codError, out sErrors, "4");
            if (!result)
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

            this.jsListaConceptoCosto = rows;

            return true;
        }
        public Boolean cargaListaNegocio(out string sErrors)
        {
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_NEGOCIO_01", out dt,out codError, out sErrors);
            if (!result)
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

            this.jsListaNegocio = rows;

            return true;
        }
        public Boolean cargaListaPais(out string sErrors)
        {
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comHeader = new SqlAccess(dbConn);
            //lista los headers por numero de pagina y idheader
            Boolean result = comHeader.Consultar("USP_SEL_PAIS_01", out dt,out codError, out sErrors);
            if (!result)
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

            this.jsListaPais= rows;

            return true;
        }
        public Boolean actualizarServicioDeNegocio(string id, string nombre, string descripcion, out string sError)
        {
            usrModificador = GetUserName();
            Boolean res;
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_SERVICIO_NEGOCIO_00", out sError, id, nombre, descripcion, usrModificador);
            if (res)
            {

                return true;
            }
            else
            {

                return false;
            }
        }
        public Boolean RolLista(out string sErrors)
        {
            sErrors = "";

            DataTable dtListaRolSp = new DataTable();
            string codError = "";
            SqlAccess comHeader = new SqlAccess(dbConnPerfil);
           
            //lista los headers por id y nombreatributo para llenar un dropdown

            Boolean result = comHeader.Consultar("USP_SEL_LISTA_ROL", out dtListaRolSp,out codError, out sErrors);

            if (!result)
            {
                return false;
            }


            this.dtRol = dtListaRolSp;

            return true;
        }
        public Boolean customLista(out string sErrors)
        {
            sErrors = "";

            DataTable dtUsuariosSp = new DataTable();

            DirectoryContext dc = new DirectoryContext(DirectoryContextType.Domain, Environment.UserDomainName);
            Domain domain = Domain.GetDomain(dc);
            DirectoryEntry de = domain.GetDirectoryEntry();

            DirectorySearcher deSearch = new DirectorySearcher(de);

            deSearch.Filter = "(&(objectClass=user)(objectCategory=person))";
            SearchResultCollection results = deSearch.FindAll();


            foreach (SearchResult srUser in results)
            {
                DirectoryEntry deUser = srUser.GetDirectoryEntry();
                deUser.Invoke("Members");

            }


            dtUsuariosSp.Rows.Add(new Object[] { 1, "Smith" });
            dtUsuariosSp =
            this.dtUsuarios = dtUsuariosSp;

            return true;
        }

        public DataTable TablaElementos(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ELEMENTOS_00", out dt,out codError, out sError);

            return dt;
        }
        public DataTable TablaServicios(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_CATALOGO_SERVICIO_00", out dt,out codError, out sError);

            return dt;
        }
        public DataTable TablaListaValores(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_VALORES_05", out dt,out codError, out sError);

            return dt;
        }

        public DataTable TablaConceptoCosto(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_CATALOGO_ELEMENTO_01", out dt,out codError, out sError);

            return dt;
        }

    }
}
