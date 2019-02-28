using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Security.Claims;
using System.Threading.Tasks;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class ListarProyecto
    {
        public string dbConn;
        public DataTable dtProyectos { get; set; }
        public DataTable dtUsuariosolicitante { get; set; }
        public DataTable dtJPAsignado { get; set; }
        public DataTable dtNegocio { get; set; }
        public DataTable dtServicioNegocio { get; set; }
        public DataTable dtCreadores { get; set; }
        public List<Dictionary<object, string>> dtServicioJson { get; set; }

        public List<Dictionary<object, string>> dtCotizacionesJson { get; set; }

        public List<string> Errors = new List<string>();
        
        private ClaimsIdentity claimId;

        public ListarProyecto(ClaimsIdentity claimid)
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
            this.claimId = claimid;

        }

        private string GetUserName()
        {
            //var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            var claim = this.claimId.FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }

        public void SetClaim(ClaimsIdentity claimid)
        {
            this.claimId = claimid;
        }

        public Boolean cargaModGrillaProyectos(string id, string paisNegocio, string negocio, string usrSolicitante,
                                                string srvNegocio, string fchIni, string fchFin, string usrAsignada, 
                                                string pagina, string idHead, string ascDesc, string idMostrar, 
                                                string codCot, string usrCreador, out string sErrors, out string idHeadAct, out string ascDescAct)
        {

            UsersAD usersAd = new UsersAD();
            string codError = "";

            usersAd.getFullNameAD();

            DataTable dtProyectosSp = new DataTable();
            SqlAccess comCotiz = new SqlAccess(dbConn);


            Boolean result = comCotiz.Consultar("USP_SEL_LISTA_PROYECTOS_00", out dtProyectosSp,out codError, out sErrors,
                                                pagina, idHead + ascDesc, id, fchIni, fchFin, negocio, usrSolicitante, srvNegocio, paisNegocio, usrAsignada,
                                                GetUserName(), idMostrar, codCot, usrCreador);


            idHeadAct = idHead;
            ascDescAct = ascDesc;
            this.dtProyectos = dtProyectosSp;

            return true;
        }

        /***************************************************************************/
        /***************************************************************************/
        public async Task<bool> cargaListarCotizaciones()
        { 
                 
                 Task task1 = cargaModServicioNegocio();
                 Task task2 = cargaModNegocio();
                 Task task3 = cargaModCreadores();
                 Task task4 = cargaModUsuariosolicitante();
                 Task task5 = cargaModJPAsignado();
                 Task task6 = CargaCotizaciones();

                 await Task.WhenAll(task1, task2, task3, task4, task5, task6); 
                 
                 return true;
             }
        
        /***/
        public async Task<bool> cargaModServicioNegocio()
        {
            bool resp = false;

            await Task.Run(() =>  {
                    string sErrors = "";
                    string codError = "";
                    SqlAccess comCotiz = new SqlAccess(dbConn);
                    DataTable dtServicioNegocioTemp = new DataTable();

                    Boolean result = comCotiz.Consultar("USP_SEL_LISTA_PROYECTOS_04", out dtServicioNegocioTemp,out codError, out sErrors);
                    this.dtServicioNegocio = dtServicioNegocioTemp;

                    if(!result){
                        this.Errors.Add(sErrors);
                }
                    resp = true;
        });
            return resp;
            }

        /***/
        public async Task<bool> cargaModNegocio()
            {
                bool resp = false;
                string codError = "";

                await Task.Run(() =>  {
                    string sErrors = "";
                    SqlAccess comCotiz = new SqlAccess(dbConn);
                    DataTable dtNegocioTemp = new DataTable();

                    Boolean result = comCotiz.Consultar("USP_SEL_LISTA_PROYECTOS_02", out dtNegocioTemp,out codError, out sErrors);
                    this.dtNegocio = dtNegocioTemp;
                    if(!result){
                        this.Errors.Add(sErrors);
                    }
                        
                    return true;
                });
                return resp;
        }

        /***/
        public async Task<bool> cargaModCreadores()
        {
            bool resp = false;

            await Task.Run(() =>  {
                string sErrors = "";
                string codError = "";

                SqlAccess comCotiz = new SqlAccess(dbConn);
                DataTable dtCreadoresTemp = new DataTable();

                Boolean result = comCotiz.Consultar("USP_SEL_LISTA_PROYECTOS_09", out dtCreadoresTemp,out codError, out sErrors);
                this.dtCreadores = dtCreadoresTemp;

                if(!result){
                    this.Errors.Add(sErrors);
                }

                resp = true;
               });
            return resp;
        }
  

         /***/
        public async Task<bool> cargaModUsuariosolicitante()
        {
            bool resp = false;

            await Task.Run(() =>  {
                    string sErrors = "";
                    string codError = "";

                    UsersAD usersAd = new UsersAD();

                    usersAd.getFullNameAD();

                    DataTable dtSolitante = new DataTable();
                    SqlAccess comCotiz = new SqlAccess(dbConn);

                    Boolean result = comCotiz.Consultar("USP_SEL_LISTA_PROYECTOS_03", out dtSolitante,out codError, out sErrors);

                    if(!result){
                        this.Errors.Add(sErrors);
                    }
                   
                    this.dtUsuariosolicitante = dtSolitante;
                    resp = true;
                });

            return resp;
        }

        /***/
        public async Task<bool> cargaModJPAsignado()
        {
            bool resp = false;

            await Task.Run(() =>  {
                    string sErrors= "";
                    string codError = "";

                    UsersAD usersAd = new UsersAD();

                    usersAd.getFullNameAD();

                    DataTable dtAsignado = new DataTable();
                    SqlAccess comCotiz = new SqlAccess(dbConn);

                    Boolean result = comCotiz.Consultar("USP_SEL_LISTA_PROYECTOS_07", out dtAsignado,out codError, out sErrors);

                    if(!result){
                        this.Errors.Add(sErrors);
                    }
                    
                    this.dtJPAsignado = dtAsignado;

                    resp = true;
                });
            
            return resp;
        }
        
        /***/
        public async Task<bool> CargaCotizaciones()
        {
            
            await Task.Run(() => {
                    Boolean res;
                    string sErrors = "";
                    DataTable dt = null;
                    string codError = "";

                    SqlAccess cDAL = new SqlAccess(dbConn);
                    res = cDAL.Consultar("USP_SEL_COTIZACIONES_00", out dt,out codError, out sErrors, GetUserName());

                    if (!res)
                    {
                        this.Errors.Add(sErrors);
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

                    this.dtCotizacionesJson = rows;

            });
            return true;
        }
        /***************************************************************************/
        /***************************************************************************/
       
        public Boolean CargaServicioNegocio(string idnegocio, string idpais, out string sError)
        {
            sError = "";
            Boolean res;
            DataTable dt = null;
            string codError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_SERVICIO_NEGOCIO_02", out dt,out codError, out sError, idnegocio, idpais);
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
            this.dtServicioJson = rows;
            return true;
        }

        

        public object sErrors { get; set; }
    }
}