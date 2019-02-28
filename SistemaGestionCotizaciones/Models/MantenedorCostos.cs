using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class MantenedorCostos
    {
        public string dbConn;
        public string id;
        public DataTable dtMantenedorCostos { get; set; }
        public DataTable dtCostosSubcomponentes { get; set; }
        public List<Dictionary<object, string>> dtCategoriasJson { get; set; }
        public List<Dictionary<object, string>> dtParametrosCategoriasJson { get; set; }
        public List<Dictionary<object, string>> dtParCostosJson { get; set; }
        public List<Dictionary<object, string>> dtParPadreJson { get; set; }
        public List<Dictionary<object, string>> cargaFormulasJson { get; set; }
        public List<Dictionary<object, string>> cargaVariablesJson { get; set; }
        public List<Dictionary<object, string>> dtSimulacionJson { get; set; }
        public List<Dictionary<object, string>> cargaVariablesFormulasJson { get; set; }
        public MantenedorCostos()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        public Boolean cargaGrillaMantenedorCostos(string idadan, out string sErrors)
        {

            sErrors = "";
            Boolean res;
            Boolean res2;
            DataTable dtParametrosCategoriasSp = null;
            DataTable dtParCostosSp = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SUBCOMPONENTE_COSTO_00", out dtParametrosCategoriasSp,out codError, out sErrors, idadan);
            res2 = cDAL.Consultar("USP_SEL_SUBCOMPONENTE_COSTO_01", out dtParCostosSp,out codError, out sErrors);

            if (!res)
            {
                return false;
            }

            if (!res2)
            {
                return false;
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<object, string>> rows = new List<Dictionary<object, string>>();
            List<Dictionary<object, string>> rows2 = new List<Dictionary<object, string>>();
            Dictionary<object, string> row;

            foreach (DataRow dr in dtParametrosCategoriasSp.Rows)
            {
                row = new Dictionary<object, string>();
                foreach (DataColumn col in dtParametrosCategoriasSp.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);
            }
            this.dtParametrosCategoriasJson = rows;

            foreach (DataRow dr in dtParCostosSp.Rows)
            {
                row = new Dictionary<object, string>();
                foreach (DataColumn col in dtParCostosSp.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows2.Add(row);
            }

            this.dtParCostosJson = rows2;

            return true;
        }
        public Boolean cargaCategorias(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable dtCategoriasSp = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_PARAMETROCOSTO_CATEGORIA_00", out dtCategoriasSp,out codError, out sErrors);
            if (!res)
            {
                return false;
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<object, string>> rows = new List<Dictionary<object, string>>();
            Dictionary<object, string> row;

            foreach (DataRow dr in dtCategoriasSp.Rows)
            {
                row = new Dictionary<object, string>();
                foreach (DataColumn col in dtCategoriasSp.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);
            }

            this.dtCategoriasJson = rows;
            return true;
        }
        public Boolean cargaParametrosPadres(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable cargaParametrosPadresSp = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SUBCOMPONENTE_PADRES_00", out cargaParametrosPadresSp,out codError, out sErrors);
            if (!res)
            {
                return false;
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<object, string>> rows = new List<Dictionary<object, string>>();
            Dictionary<object, string> row;

            foreach (DataRow dr in cargaParametrosPadresSp.Rows)
            {
                row = new Dictionary<object, string>();
                foreach (DataColumn col in cargaParametrosPadresSp.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);
            }

            this.dtParPadreJson = rows;
            return true;
        }
        public Boolean cargaFormulas(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable cargaFormulasSp = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_FORMULAS_01", out cargaFormulasSp,out codError, out sErrors);
            if (!res)
            {
                return false;
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<object, string>> rows = new List<Dictionary<object, string>>();
            Dictionary<object, string> row;

            foreach (DataRow dr in cargaFormulasSp.Rows)
            {
                row = new Dictionary<object, string>();
                foreach (DataColumn col in cargaFormulasSp.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);
            }

            this.cargaFormulasJson = rows;
            return true;
        }
        public Boolean cargaVariables(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            DataTable cargaVariablesSp = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_CATALOGO_ELEMENTO_00", out cargaVariablesSp,out codError, out sErrors, "2");
            if (!res)
            {
                return false;
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<object, string>> rows = new List<Dictionary<object, string>>();
            Dictionary<object, string> row;

            foreach (DataRow dr in cargaVariablesSp.Rows)
            {
                row = new Dictionary<object, string>();
                foreach (DataColumn col in cargaVariablesSp.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);
            }

            this.cargaVariablesJson = rows;
            return true;
        }
        public Boolean cargaModMantenedorCostos(string datos, out string sErrors)
        {
            DataTable dtCostosSubcomponentesSp = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_COTIZACION_PRECIO_00", out dtCostosSubcomponentesSp,out codError, out sErrors, datos);

            if (!result)
            {
                return false;
            }

            this.dtCostosSubcomponentes = dtCostosSubcomponentesSp;

            return true;
        }
        public Boolean cargaVariablesFormulas(out string sErrors)
        {
            sErrors = "";
            Boolean res;
            string codError = "";
            DataTable cargaVariablesFormulasSp = null;

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_PARAMETRO_COSTO_03", out cargaVariablesFormulasSp,out codError, out sErrors);

            if (!res)
            {
                return false;
            }

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<object, string>> rows = new List<Dictionary<object, string>>();
            Dictionary<object, string> row;

            foreach (DataRow dr in cargaVariablesFormulasSp.Rows)
            {
                row = new Dictionary<object, string>();

                foreach (DataColumn col in cargaVariablesFormulasSp.Columns)
                {
                    row.Add(col.ColumnName, dr[col].ToString());
                }
                rows.Add(row);
            }

            this.cargaVariablesFormulasJson = rows;

            return true;
        }
        public Boolean actualizarCosto(out string sError, List<iCostos> updCostoJson, List<iGlosa> updGlosaJson)
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

            if (updGlosaJson != null)
            {
                emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                serializer = new XmlSerializer(updGlosaJson.GetType());
                settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                xmlVar2 = "";
                using (var stream = new System.IO.StringWriter())
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, updGlosaJson, emptyNamepsaces);
                    xmlVar2 = stream.ToString();
                }
            }
            else {
            }
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_PARAMETRO_COSTO_02", out sError, xmlVar, xmlVar2, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());

            if (!res)
            {
                return false;
            }

            return true;
        }
        public Boolean guardarCategoria(string idparametrocosto, string idcategoria, out string sErrors)
        {

            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Ejecutar("USP_UPD_PARAMETRO_COSTO_01", out sErrors, idparametrocosto, idcategoria, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());

            if (!result)
            {
                return false;
            }

            return true;
        }
        //public Boolean guardarFormula(out string sError, List<iCostos> updFormulaJson)
        //{

        //    sError = "";
        //    Boolean res;

        //    var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        //    var serializer = new XmlSerializer(updFormulaJson.GetType());
        //    var settings = new XmlWriterSettings();
        //    settings.Indent = false;
        //    settings.OmitXmlDeclaration = true;
        //    string xmlVar = "";

        //    using (var stream = new System.IO.StringWriter())
        //    using (var writer = XmlWriter.Create(stream, settings))
        //    {
        //        serializer.Serialize(writer, updFormulaJson, emptyNamepsaces);
        //        xmlVar = stream.ToString();
        //    }

        //    CotizadorDAL.CotizadorDAL cDAL = new CotizadorDAL.CotizadorDAL(dbConn);
        //    res = cDAL.Ejecutar("USP_UPD_PARAMETRO_COSTO_02", out sError, xmlVar, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());

        //    if (!res)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        public Boolean guardarCosto(out string sError, List<iCostos> updCostoJson)
        {

            sError = "";
            Boolean res;

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
            res = cDAL.Ejecutar("", out sError, xmlVar, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());

            if (!res)
            {
                return false;
            }

            return true;
        }

        public DataTable simularCosto(List<iSimulacionCosto> elementos, out string sError)
        {
            sError = "";
            string codError = "";
            Boolean res;
            DataTable cargaSimulacionSp = null;

            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(elementos.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlVar = "";

            using (var stream = new System.IO.StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, elementos, emptyNamepsaces);
                xmlVar = stream.ToString();
            }

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_COTIZACION_PRECIO_00", out cargaSimulacionSp,out codError, out sError, xmlVar);

            return cargaSimulacionSp;

        }
        public Boolean guardaSimulacion(List<iSimulacionCosto> elementos, string fecha, out string sError)
        {
            sError = "";
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(elementos.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlVar = "";
            using (var stream = new System.IO.StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, elementos, emptyNamepsaces);
                xmlVar = stream.ToString();
            }
            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_UPD_SIMULACION_00", out sError, xmlVar, fecha, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());
            if (!result)
            {
                return false;
            }
            return true;
        }
    }
}