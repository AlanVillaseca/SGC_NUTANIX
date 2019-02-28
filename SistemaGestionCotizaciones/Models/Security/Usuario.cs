using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models.Security
{
    public class Usuario
    {

        public string dbConn { get; set; }
        public List<Dictionary<object, string>> negociosUsuarioJson { get; set; }

        public List<Dictionary<object, string>> centroCostoUsuarioJson { get; set; }

        public List<Dictionary<object, string>> rolesUsuarioJson { get; set; }
        public List<Dictionary<object, string>> negocioPaisJson { get; set; }

        public List<Dictionary<object, string>> centroCostoJson { get; set; }

        public List<Dictionary<object, string>> rolesJson { get; set; }
        public List<Dictionary<object, string>> datosUsuarioJson { get; set; }
        public List<Dictionary<object, string>> datosUsuarioIndexJson { get; set; }

        public Usuario()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }

        public bool CargaCentroCostoUsuario(string idusuario, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDal = new SqlAccess(dbConn);
            res = cDal.Consultar("USP_SEL_CENTRO_COSTO_USUARIO_00", out dt,out codError, out sError, idusuario);

            if (!res)
            {
                return false;
            }

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
            this.centroCostoUsuarioJson = rows;
            return true;
        }

        public bool CargaNegociosUsuario(string idusuario, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_NEGOCIO_PAIS_04", out dt,out codError, out sError, idusuario);

            if (!res)
            {
                return false;
            }


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

            this.negociosUsuarioJson = rows;
            return true;
        }
        public bool CargaRolesUsuario(string idusuario, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SECROL_01", out dt,out codError, out sError, idusuario);

            if (!res)
            {
                return false;
            }


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

            this.rolesUsuarioJson = rows;
            return true;
        }

        public bool CargaCentroCostro(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError="";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_LISTA_CENTRO_COSTOS_01", out dt,out codError, out sError);

            if (!res)
            {
                return false;
            }

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

            this.centroCostoJson = rows;
            return true;
        }

        public bool CargaNegociosPais(string idusuario, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_NEGOCIO_PAIS_05", out dt,out codError, out sError, idusuario);

            if (!res)
            {
                return false;
            }


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

            this.negocioPaisJson = rows;
            return true;
        }
        public bool CargaRoles(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SECROL_02", out dt,out codError, out sError);

            if (!res)
            {
                return false;
            }


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

            this.rolesJson = rows;
            return true;
        }
        public Boolean CargaDatosUsuarios(string idusuario, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SECUSUARIO_01", out dt,out codError, out sError, idusuario);

            if (!res)
            {
                return false;
            }


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

            this.datosUsuarioJson = rows;
            return true;
        }
        public Boolean CargaDatosUsuariosIndex(out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_USUARIO_00", out dt,out codError, out sError);

            if (!res)
            {
                return false;
            }


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

            this.datosUsuarioIndexJson = rows;
            return true;
        }
        public Boolean GuardaNegocioPais(string idnegocio, string idpais, string idusuario, out string sErrors)
        {

            sErrors = "";


            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_INS_NEGOCIO_PAIS_SECUSUARIO_00", out sErrors, idnegocio, idpais, idusuario);


            if (!result)
            {
                return false;
            }

            return true;
        }
        public Boolean GuardarUsuario(string displayname, string email, string username, string negociopais, string roles,
            string activo, string centrocosto, out string sErrors)
        {

            sErrors = "";


            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_INS_SEC_USUARIO", out sErrors, displayname, email, username, negociopais, roles, activo, centrocosto);


            if (!result)
            {
                return false;
            }

            return true;
        }
        public Boolean EliminaNegocioPais(string idnegocio, string idpais, string idusuario, out string sErrors)
        {

            sErrors = "";


            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_DEL_NEGOCIO_PAIS_SECUSUARIO_00", out sErrors, idnegocio, idpais, idusuario);


            if (!result)
            {
                return false;
            }

            return true;
        }
        public Boolean ModificarRoles(string idroles, string idusuario, string activo, string centrocosto, out string sErrors)
        {

            sErrors = "";


            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_UPD_SECUSUARIO_00", out sErrors, idroles, idusuario, activo, centrocosto);


            if (!result)
            {
                return false;
            }

            return true;
        }
        public Boolean EmilinarUsuario(string idusuario, out string sErrors)
        {

            sErrors = "";


            SqlAccess comCotiz = new SqlAccess(dbConn);
            Boolean result = comCotiz.Ejecutar("USP_DEL_SECUSUARIO_00", out sErrors, idusuario);


            if (!result)
            {
                return false;
            }

            return true;
        }

    }
}