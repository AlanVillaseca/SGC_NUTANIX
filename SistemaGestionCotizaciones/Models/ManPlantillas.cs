using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class ManPlantillas
    {
        public String dbConn { get; set; }
        public List<Contrl> controles { get; set; }
        public List<Dictionary<object, string>> hojasxPadre { get; set; }
        public List<Dictionary<object, string>> parametrosCostoJson { get; set; }
        public List<Dictionary<object, string>> VariablesJson { get; set; }
        public List<Dictionary<object, string>> implementacionesJson { get; set; }
        public List<Dictionary<object, string>> parametrosCostoHojasJson { get; set; }
        public List<Dictionary<object, string>> plantillas { get; set; }
        public List<Dictionary<object, string>> plantillasCostos { get; set; }
        public List<Dictionary<object, string>> categoriasCostos { get; set; }
        public List<Dictionary<object, string>> listaFormulas { get; set; }
        public List<Dictionary<object, string>> listaCostoVariables { get; set; }
        public List<Dictionary<object, string>> listaConceptosCostoNvl1 { get; set; }
        public List<Dictionary<object, string>> listaParCostoNvl1 { get; set; }
        public List<Dictionary<object, string>> listaParCostoNvlN { get; set; }
        public List<Dictionary<object, string>> listaValores { get; set; }
        public List<Dictionary<object, string>> parCostoCatElemento { get; set; }
        public List<Dictionary<object, string>> elementosFaltantesJson { get; set; }
        public List<Dictionary<object, string>> familiasJson { get; set; }
        public List<Dictionary<object, string>> varJson { get; set; }
        public List<Dictionary<object, string>> plantillaPorId { get; set; }
        public List<Dictionary<object, string>> plantillaCostosPorId { get; set; }
        public List<Dictionary<object, string>> plantillaVariablesPorId { get; set; }
        public List<Dictionary<object, string>> plantillaImplementPorId { get; set; }
        public ManPlantillas()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }

        public bool CargaHojasPadre(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_04", out dt,out codError, out sError);
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

            this.hojasxPadre = rows;
            return true;
        }

        public bool CargaParametrosCosto(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_PARAMETRO_COSTO_00", out dt,out codError, out sError);
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

            this.parametrosCostoJson = rows;
            return true;
        }

        public bool CargaVariables(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_PARAMETRO_COSTO_01", out dt,out codError, out sError);
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

            this.VariablesJson = rows;
            return true;
        }
        public bool CargaImplementaciones(out string sError)
        {
            sError = "";
            var Categoria = "4";
            DataTable dt = null;
            string codError = "";

            var cDAL = new SqlAccess(dbConn);
            var res = cDAL.Consultar("USP_SEL_PARAMETRO_COSTO_02", out dt,out codError, out sError, Categoria);
            if (!res) return false;

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var rows = new List<Dictionary<object, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                var row = new Dictionary<object, string>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);
            }

            this.implementacionesJson = rows;
            return true;
        }

        public bool CargaParametrosCostoHojas(string idParam, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_05", out dt,out codError, out sError, idParam);
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

            this.parametrosCostoHojasJson = rows;
            return true;
        }
        public bool CargaVar(string idParam, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_COMPORTAVARIABLE_00", out dt,out codError, out sError, idParam);
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

            this.varJson = rows;
            return true;
        }

        public bool GuardaHojasPadre(List<Plantilla> plantilla, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(plantilla.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlJson = "";

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, plantilla, emptyNamepsaces);
                xmlJson = stream.ToString();
            }


            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_INS_ARBOL_00", out dt,out codError, out sError, xmlJson);
            System.Web.Script.Serialization.JavaScriptSerializer cserializer = new System.Web.Script.Serialization.JavaScriptSerializer();

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

            this.elementosFaltantesJson = rows;

            return true;
        }

        public bool GuardaConceptoCosto(List<ConceptoCosto> concepto, out string sError)
        {
            sError = "";
            Boolean res;

            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(concepto.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlJson = "";

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, concepto, emptyNamepsaces);
                xmlJson = stream.ToString();
            }


            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_CONCEPTO_COSTO_00", out sError, xmlJson);

            if (!res)
            {
                return false;
            }

            return true;
        }

        public bool GuardaListaPlantillas(string id, string nombre, string habilitado, out string sError)
        {
            sError = "";
            Boolean res;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_LISTA_PLANTILLAS_00", out sError, id, nombre, habilitado);

            if (!res)
            {
                return false;
            }

            return true;
        }

        public bool CargaPlantillas(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_FORM_PLANTILLAS_02", out dt,out codError, out sError);
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

            this.plantillas = rows;
            return true;
        }

        public bool CargaPlantillasCostos(string id, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_06", out dt,out codError, out sError, id);
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

            this.plantillasCostos = rows;
            return true;
        }

        public bool CargaCategoriasCostos(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_VALORES_01", out dt,out codError, out sError);
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

            this.categoriasCostos = rows;
            return true;
        }

        public bool CargaListaFormulas(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_FORMULAS_00", out dt,out codError, out sError);
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

            this.listaFormulas = rows;
            return true;
        }

        public bool CargaFamilias(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_FAMILIAS_00", out dt,out codError, out sError, "1");
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

            this.familiasJson = rows;
            return true;
        }

        public bool CargaVariablesCostoV(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_CATALOGO_ELEMENTO_00", out dt,out codError, out sError, "0");
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

            this.listaCostoVariables = rows;
            return true;
        }

        public bool CargaVariablesCostoV2(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_CATALOGO_ELEMENTO_00", out dt,out codError, out sError, "2");
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

            this.listaCostoVariables = rows;
            return true;
        }

        public bool CargaConceptosCostosNvl1(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_CATALOGO_ELEMENTO_00", out dt,out codError, out sError, "3");
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

            this.listaConceptosCostoNvl1 = rows;
            return true;
        }
        public bool CargaParCostoNvl1(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_PARAMETRO_COSTO_00", out dt,out codError, out sError, "1", "");
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

            this.listaParCostoNvl1 = rows;
            return true;
        }

        public bool CargaParCostoNvlN(string idParPadre, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_PARAMETRO_COSTO_00", out dt,out codError, out sError, "n", idParPadre);
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

            this.listaParCostoNvlN = rows;
            return true;
        }

        public bool CargaListaValores(string idCatEle, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_VALORES_02", out dt,out codError, out sError, idCatEle);
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

            this.listaValores = rows;
            return true;
        }

        public bool CargaParCostoCatElemento(string idPlantilla, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_PARCOSTOCATELEMENTO_00", out dt,out codError, out sError, idPlantilla);
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

            this.parCostoCatElemento = rows;
            return true;
        }

        public bool GuardaPlantillaCostos(List<PlantillaCostosJson> plantillacostosJson, out string sError)
        {
            sError = "";
            Boolean res;

            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(plantillacostosJson.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlVar = "";

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, plantillacostosJson, emptyNamepsaces);
                xmlVar = stream.ToString();
            }

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_PARCOSTOCATELEMENTO_00", out sError, xmlVar, "usuario");

            if (!res)
            {
                return false;
            }

            return true;
        }

        /************************************************************************/
        /*                     nvaPlantillasElementosVerMod                     */
        /************************************************************************/

        public bool CargaPlantillaPorId(string idPlantilla, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_FORM_PLANTILLAS_01", out dt,out codError, out sError, idPlantilla);
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

            this.plantillaPorId = rows;
            return true;
        }

        public bool CargaPlantillaCostosPorId(string idPlantilla, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_PLANTILLAS_COSTO_00", out dt,out codError, out sError, idPlantilla);
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

            this.plantillaCostosPorId = rows;
            return true;
        }

        public bool CargaPlantillaVariablesPorId(string idPlantilla, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_PLANTILLAS_VARIABLES_00", out dt,out codError, out sError, idPlantilla);
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

            this.plantillaVariablesPorId = rows;
            return true;
        }

        public bool CargaPlantillaImplementPorId(string idPlantilla, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_PLANTILLAS_IMPLEMENTACION_00", out dt,out codError, out sError, idPlantilla);
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

            this.plantillaImplementPorId = rows;
            return true;
        }

        public bool GuardaPlnatillasEdit(List<Plantilla> plantilla, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(plantilla.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlJson = "";

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, plantilla, emptyNamepsaces);
                xmlJson = stream.ToString();
            }


            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_UPD_PLANTILLAS_00", out dt,out codError, out sError, xmlJson);
            System.Web.Script.Serialization.JavaScriptSerializer cserializer = new System.Web.Script.Serialization.JavaScriptSerializer();

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

            this.elementosFaltantesJson = rows;

            return true;
        }


    }
}