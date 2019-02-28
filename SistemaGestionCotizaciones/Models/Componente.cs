using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Xml;
using System.Xml.Serialization;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class Componente
    {
        public string idCotizacion { get; set; }
        public DataTable dtArbol { get; set; }
        public DataTable dtArbolOpt { get; set; }
        public String dbConn { get; set; }
        public DataTable dtListaPlantilla { get; set; }
        public List<Contrl> controles { get; set; }
        public string idComponente { get; set; }

        public List<Dictionary<object, string>> arbolJson { get; set; }
        public List<Dictionary<object, string>> parametrosJson { get; set; }
        public List<Dictionary<object, string>> costosJson { get; set; }
        public List<Dictionary<object, string>> headerJson { get; set; }
        public List<Dictionary<object, string>> SWJson { get; set; }
        public List<Dictionary<object, string>> montosJson { get; set; }
        public List<Dictionary<object, string>> calculaMontosJson { get; set; }
        public List<Dictionary<object, string>> variablesJson { get; set; }
        public List<Dictionary<object, string>> CatSWJson { get; set; }
        public List<Dictionary<object, string>> familiasJson { get; set; }
        public DataTable dtBreadcrumbs { get; set; }
        public DataTable dtCampos { get; set; }
        public DataTable dtListaCampos { get; set; }
        public DataTable dtCamposFaltantes { get; set; }
        public DataTable dtListaCamposFaltantes { get; set; }
        public DataTable dtOrigen { get; set; }
        public Componente()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        public bool CargaArbol(string idPlantilla, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_00", out dt,out codError, out sError, idPlantilla);
            if (!res)
            {
                return false;
            }

            this.dtArbolOpt = dt;

            res = cDAL.Consultar("USP_SEL_ARBOL_01", out dt,out codError, out sError, idPlantilla);
            if (!res)
            {
                return false;
            }

            this.dtArbol = dt;

            return true;
        }
        public bool cargaFormPlantillas(string Idplantilla, out string sErrors)
        {
            SqlAccess comCotiz = new SqlAccess(dbConn);
            string codError = "";
            DataTable dtFormPlantillaTemp = new DataTable();
            Contrl ctrl;
            ValorLista valLista;
            List<ValorLista> valoresLista = new List<ValorLista>();
            Boolean res;

            res = comCotiz.Consultar("USP_SEL_LISTA_FORM_COMPONENTE_00", out dtFormPlantillaTemp,out codError, out sErrors, Idplantilla);

            if (!res)
            {
                return false;
            }

            var result = from t in dtFormPlantillaTemp.AsEnumerable()
                         select new
                         {
                             Idplantilla = t.Field<int>("Idplantilla"),
                             Idheader = t.Field<int>("idCatalogoElemento"),
                             Idtipodato = t.Field<int>("Contrl"),
                             Nombreatributo = t.Field<string>("Nombreatributo"),
                             Predefindo = t.Field<string>("Predefindo"),
                             Requerido = t.Field<bool>("Requerido"),
                             Editable = t.Field<bool>("Editable")
                         };

            controles = new List<Contrl>();

            foreach (var item in result)
            {
                if (item.Idtipodato == 3)
                {
                    res = comCotiz.Consultar("USP_SEL_LISTA_FORM_COMPONENTE_01", out dtFormPlantillaTemp,out codError, out sErrors, item.Idheader.ToString());
                    if (!res)
                    {
                        return false;
                    }

                    var resultValores = from t in dtFormPlantillaTemp.AsEnumerable()
                                        select new
                                        {
                                            Idlistavalores = t.Field<int>("Idlistavalores"),
                                            Valor = t.Field<string>("Valor")
                                        };

                    foreach (var itemVLista in resultValores)
                    {
                        valLista = new ValorLista();
                        valLista.id = itemVLista.Idlistavalores;
                        valLista.valor = itemVLista.Valor;
                        valoresLista.Add(valLista);
                    }

                }
                ctrl = new Contrl();
                ctrl.label = item.Nombreatributo;
                ctrl.control = item.Idtipodato;
                ctrl.valoresLista = valoresLista;
                ctrl.predefinido = item.Predefindo;
                ctrl.requerido = item.Requerido;
                ctrl.editable = item.Editable;

                controles.Add(ctrl);
            }

            return true;
        }
        public bool CargaFamilias(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_FAMILIAS_00", out dt,out codError, out sError, "2");
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
        public bool cargaListaPlantillas(out string sErrors)
        {
            SqlAccess comCotiz = new SqlAccess(dbConn);
            string codError = "";
            DataTable dtListaPlantillaTemp = new DataTable();

            Boolean result = comCotiz.Consultar("USP_SEL_LISTA_FORM_PLANTILLAS_00", out dtListaPlantillaTemp,out codError, out sErrors);
            this.dtListaPlantilla = dtListaPlantillaTemp;

            return true;
        }


        public Boolean cargaCampos(string idcomponente, out string sErrors)
        {

            DataTable dtSolitante = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            string codError = "";

            Boolean result = comCotiz.Consultar("USP_SEL_CATALOGO_COMPONENTE_00", out dtSolitante,out codError, out sErrors, idcomponente);

            this.dtCampos = dtSolitante;

            Boolean result1 = comCotiz.Consultar("USP_SEL_CATALOGO_COMPONENTE_01", out dtSolitante,out codError, out sErrors, idcomponente);

            this.dtListaCampos = dtSolitante;

            if (!result && !result1)
            {
                return false;
            }

            return true;
        }
        public Boolean guardarCamposElementos(string elementos, out string sErrors)
        {
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Ejecutar("USP_SEL_CATALOGO_COMPONENTE_02", out sErrors, elementos);

            if (!result)
            {
                return false;
            }

            return true;
        }
        public Boolean cargaCamposFaltantes(string idcomponente, out string sErrors)
        {
            DataTable dtSolitante = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);
            string codError = "";

            Boolean result = comCotiz.Consultar("USP_SEL_CATALOGO_COMPONENTE_03", out dtSolitante,out codError, out sErrors, idcomponente);

            this.dtCamposFaltantes = dtSolitante;

            Boolean result1 = comCotiz.Consultar("USP_SEL_CATALOGO_COMPONENTE_01", out dtSolitante,out codError, out sErrors, idcomponente);

            this.dtListaCamposFaltantes = dtSolitante;

            if (!result && !result)
            {
                return false;
            }

            return true;
        }
        public Boolean agregarCampoElemento(string idcomponente, string idcatalogo, string valor, out string sErrors)
        {
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Ejecutar("USP_SEL_CATALOGO_COMPONENTE_04", out sErrors, idcomponente, idcatalogo, valor, System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last());

            if (!result)
            {
                return false;
            }

            return true;
        }
        public bool CargaArbusto(string idPlantilla, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_02", out dt,out codError, out sError, idPlantilla);
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

            this.arbolJson = rows;
            return true;
        }
        public bool CargaArbustoComponente(string idComponente, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_08", out dt,out codError, out sError, idComponente);
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

            this.arbolJson = rows;
            return true;
        }

        public bool CargaParametros(string idPlantilla, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_03", out dt,out codError, out sError, idPlantilla);
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

            this.parametrosJson = rows;
            return true;
        }
        public bool CargaParametrosComponente(string idComponente, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_09", out dt,out codError, out sError, idComponente);
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

            this.parametrosJson = rows;
            return true;
        }

        public bool CargaCostosOpcionales(string idPlantilla, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_07", out dt,out codError, out sError, idPlantilla);
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

            this.costosJson = rows;
            return true;
        }
        public bool CargaVariablesJson(string idPlantilla, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_15", out dt,out codError, out sError, idPlantilla);
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

            this.variablesJson = rows;
            return true;
        }
        public bool CargaVariablesJsonComp(string idComponente, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_16", out dt,out codError, out sError, idComponente);
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

            this.variablesJson = rows;
            return true;
        }
        public bool CargaCostosOpcionalesComponente(string idComponente, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_10", out dt,out codError, out sError, idComponente);
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

            this.costosJson = rows;
            return true;
        }

        public bool GuardaComponente(List<ComponenteJson> componenteJson, out string sError)
        {
            sError = "";
            Boolean res;

            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(componenteJson.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlVar = "";

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, componenteJson, emptyNamepsaces);
                xmlVar = stream.ToString();
            }

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_INS_COMPONENTE_02", out sError, xmlVar);

            if (!res)
            {
                return false;
            }

            return true;
        }
        public bool EditaComponente(List<ComponenteJson> componenteJson, out string sError)
        {
            sError = "";
            Boolean res;

            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(componenteJson.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlVar = "";

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, componenteJson, emptyNamepsaces);
                xmlVar = stream.ToString();
            }

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_COMPONENTE_00", out sError, xmlVar);

            if (!res)
            {
                return false;
            }

            return true;
        }
        public bool CalcularComponente(List<ComponenteJson> componenteJson, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(componenteJson.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlVar = "";

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, componenteJson, emptyNamepsaces);
                xmlVar = stream.ToString();
            }

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_CALCULAR_COMPONENTE_00", out dt,out codError, out sError, xmlVar);
            if (!res)
            {
                return false;
            }

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

            this.calculaMontosJson = rows;
            return true;
        }

        public bool CalcularComponenteComp(List<ComponenteJson> componenteJson, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(componenteJson.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            string xmlVar = "";

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, componenteJson, emptyNamepsaces);
                xmlVar = stream.ToString();
            }

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_CALCULAR_COMPONENTE_02", out dt,out codError, out sError, xmlVar);
            if (!res)
            {
                return false;
            }

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

            this.calculaMontosJson = rows;
            return true;
        }



        public bool CargaHeader(string identificador, string tipo, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_11", out dt,out codError, out sError, identificador, tipo);
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

            this.headerJson = rows;
            return true;
        }

        public bool CargaMontos(string idComponente, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_12", out dt,out codError, out sError, idComponente);
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

            this.montosJson = rows;
            return true;
        }
        public bool CargaSW(string idComponente, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_13", out dt,out codError, out sError, idComponente);
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

            this.SWJson = rows;
            return true;
        }

        public bool CargaCatSW(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_ARBOL_17", out dt,out codError, out sError);
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

            this.CatSWJson = rows;
            return true;
        }
        public Boolean CargaBreadcrumbs(string identificador, string selector, out string sError)
        {
            UsersAD usersAd = new UsersAD();
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);

            Boolean result = comCotiz.Consultar("USP_SEL_BREADCRUMBS_00", out dt,out codError, out sError, identificador, selector);

            if (!result)
            {
                return false;
            }

            this.dtBreadcrumbs = dt;

            return true;
        }
        public String VerOrigen(string idcomponente, out string sError)
        {
            string value;

            UsersAD usersAd = new UsersAD();
            DataTable dt = new DataTable();
            string codError = "";
            SqlAccess comCotiz = new SqlAccess(dbConn);
            
            Boolean result = comCotiz.Consultar("USP_SEL_ORIGEN_COMPONENTE_00", out dt,out codError, out sError, idcomponente);

            if (!result)
            {
                return "0";
            }

            this.dtOrigen = dt;

            value = dtOrigen.Rows[0].ItemArray.GetValue(0).ToString();

            return value;
        }
    }
}