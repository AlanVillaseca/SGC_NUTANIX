using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web.UI.HtmlControls;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    public class Proyecto
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string fecha { get; set; }
        public HttpPostedFileBase rutaFEP{get;set;}
        public string nuevoarchivo { get; set; }
        public string ruta { get; set; }
        public string descripcion { get; set; }
        public string dbConn { get; set; }
        public string usuario { get; set; }
        public string negocio { get; set; }
        public string servicio { get; set; }
        public string pais { get; set; }
        public string hddProrrata { get; set; }
        public string cliente { get; set; }
        public string extension { get; set; }

        public DataTable dtUsuario;

        public DataTable dtNegocios;

        public DataTable dtServicios;
        public DataTable dtCentroCostos;

        public Proyecto()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }
        public bool Cargar(out string sError)
        {
            sError = "";
            DataTable dt = null;
            Boolean res;
            string codError = "";

            UsersAD oUserAD = new UsersAD();

            oUserAD.getFullNameAD();

            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Consultar("USP_SEL_GERENCIAS_00", out dt,out codError, out sError);
            if (!res)
                return false;
            this.dtUsuario = dt;
/*
            var result = from t in dt.AsEnumerable()
                         join x in oUserAD.dtUsers.AsEnumerable() on t.ItemArray.GetValue(0) equals x.Field<string>("userId")
                         select new
                         {
                             idperasignada = t.Field<string>("USERID"),
                             fullname = x.Field<string>("fullName")
                         };

            this.dtUsuario = new DataTable();
            this.dtUsuario.Columns.Add("USERID");
            this.dtUsuario.Columns.Add("fullName");

            foreach (var item in result)
            {
                this.dtUsuario.Rows.Add(item.idperasignada, item.fullname);
            }
            */
            //////////////////

            res = cDAL.Consultar("USP_SEL_LISTA_PROYECTOS_04", out dt,out codError, out sError);
            if (!res)
                return false;

            this.dtServicios = dt;

            res = cDAL.Consultar("USP_SEL_LISTA_PROYECTOS_02", out dt,out codError, out sError);
            if (!res)
                return false;

            this.dtNegocios = dt;
            res = cDAL.Consultar("USP_SEL_LISTA_CENTRO_COSTOS_00", out dt,out codError, out sError);
            if (!res)
                return false;
            this.dtCentroCostos = dt;


            return true;
        }

        public bool Guardar(out string sError)
        {
            /*             
            @param1  VARCHAR(255), --Idserviciodenegoci
            @param2  VARCHAR(255), --Nombre 			
            @param3  VARCHAR(255), --Descripcion 		
            @param4  VARCHAR(255), --Fecha				
            @param5  VARCHAR(255), --Idestado 			
            @param6  VARCHAR(255), --Documentofep		
            @param7  VARCHAR(255), --Idpercreador 		
            @param8  VARCHAR(255), --Idperasignada 	
            @param9  VARCHAR(255), --Idpersolicitante 
            @param10 VARCHAR(255), --NEGOCIOS ASOCIADOS	
            @param11 VARCHAR(255), --USUARIO CREACION             
             */
            this.hddProrrata = this.hddProrrata.Replace("!**", "<");
            this.hddProrrata = this.hddProrrata.Replace("**!", ">");
            string Name = GetUserName();
            Boolean res;
          
            sError = "";

            SqlAccess cDAL = new SqlAccess(dbConn);
            HttpPostedFileBase a = rutaFEP;
            res = cDAL.Ejecutar("USP_INS_CREA_PROYECTOS_00", out sError, servicio, nombre, descripcion, fecha, "1", nuevoarchivo, Name, "-1", usuario, negocio, Name, codigo, pais, hddProrrata, usuario, cliente);

            return true;
        }
        public bool AsignarJDP(string idproyecto, string idjefedeproyecto, out string sError)
        {
            string Name = GetUserName();
            Boolean res;
            sError = "";
            SqlAccess cDAL = new SqlAccess(dbConn);
            res = cDAL.Ejecutar("USP_UPD_PROYECTO_00", out sError, idproyecto, idjefedeproyecto, Name);
            Task t = Task.Factory.StartNew(() => {
                                                    DataTable dtt = null;
                                                    String sErrort = "";
                                                    string codError = "";
                                                    cDAL = new SqlAccess(dbConn);
                                                    res = cDAL.Consultar("USP_SEL_ALERTAS_EMAIL_00", out dtt,out codError, out sErrort, idproyecto, "PROY");
                                                    string email, asunto, cuerpo;
                                                    EnviarMail oMail = new EnviarMail();
                                                    foreach (DataRow row in dtt.Rows)
                                                    {
                                                        email = row.ItemArray[0].ToString();
                                                        asunto = row.ItemArray[1].ToString();
                                                        cuerpo = row.ItemArray[2].ToString();
                                                        oMail.enviar(email, asunto, cuerpo);
                                                    }
                                                    });
            return true;
        }
    }
}