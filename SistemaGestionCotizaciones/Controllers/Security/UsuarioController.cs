using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using SistemaGestionCotizaciones.Models.Security;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.Web.Configuration;
using System.Data;
using SistemaGestionCotizaciones.Models;
using System.Text;
using System.Web.Script.Serialization;

namespace SistemaGestionCotizaciones.Controllers.Security
{
    public class UsuarioController : BaseController
    {
        // GET: Usuario
        public ActionResult Index()
        {
            //var viewModel =
            //    new UsuariosViewModel()
            //    {
            //        Usuarios = _usuarios.RetrieveAll()
            //    };


            string sError = "";
            Usuario ctrlUsusario = new Usuario();


            ctrlUsusario.CargaDatosUsuariosIndex(out sError);



            return View(ctrlUsusario);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            var viewModel = 
                new UsuarioEditViewModel()
                {
                    Usuario = new SecUsuario(),
                    Roles = _roles.RetrieveAll(),
                    NegocioPaises = _usuarios.GetNegocioPaisAll(),
                    CentroCosto = _usuarios.GetCentroCostoAll()
                };

            return View(viewModel);
        }

        // POST: Usuario/Create
        [HttpPost]
        public ActionResult Create(string displayname, string email, string username, string negociopais, string roles, string activo, string centrocosto)
        {
            string sError = "";
            Usuario ctrlUsuario = new Usuario();

            ctrlUsuario.GuardarUsuario(displayname, email, username, negociopais, roles, activo, centrocosto, out sError);

            //_usuarios.Insert(ParseSelectedRoles(model));

            return RedirectToAction("Index");
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(string id)
        {
            string serrors = "";
            Usuario ctrlUsuario = new Usuario();

            ctrlUsuario.CargaNegociosUsuario(id, out serrors);
            ctrlUsuario.CargaRolesUsuario(id, out serrors);
            ctrlUsuario.CargaNegociosPais(id, out serrors);
            ctrlUsuario.CargaRoles(out serrors);
            ctrlUsuario.CargaCentroCostro(out serrors);
            ctrlUsuario.CargaDatosUsuarios(id, out serrors);
            ctrlUsuario.CargaCentroCostoUsuario(id, out serrors);

            //var viewModel = new UsuarioEditViewModel()
            //{
            //    Usuario = _usuarios.RetrieveById(id),
            //    Roles = _roles.RetrieveAll(),
            //    NegocioPaises = _usuarios.GetNegocioPaisAll()
            //};

            ViewBag.idusuario = id;

            return View(ctrlUsuario);
        }
        public string AgregarNegocioPais(string idnegocio, string idpais, string idusuario)
        {

            string sError = "";

            Usuario ctrlUsuario = new Usuario();

            ctrlUsuario.GuardaNegocioPais(idnegocio, idpais, idusuario, out sError);

            return sError;
        }
        public string EliminaNegocioPais(string idnegocio, string idpais, string idusuario)
        {

            string sError = "";

            Usuario ctrlUsuario = new Usuario();

            ctrlUsuario.EliminaNegocioPais(idnegocio, idpais, idusuario, out sError);

            return sError;
        }
        public string ModificarRoles(string idroles, string idusuario, string activo, string centrocosto)
        {

            string sError = "";

            Usuario ctrlUsuario = new Usuario();

            ctrlUsuario.ModificarRoles(idroles, idusuario, activo, centrocosto, out sError);

            return sError;
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        public ActionResult Edit(UsuarioEditViewModel model)
        {
            _usuarios.Update(ParseSelectedRoles(model));
            return RedirectToAction("Index");
        }


        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            var usuario = _usuarios.RetrieveById(id);

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            string sError = "";
            Usuario ctrlUsuario = new Usuario();

            ctrlUsuario.EmilinarUsuario(id, out sError);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public String GetUsuarioFromAD(string username)
        {
            //get user properties from AD

           // var rootQuery = ConfigurationManager.ConnectionStrings["ADConnectionString"].ConnectionString;
            string[] LDAP = WebConfigurationManager.AppSettings["LDAP"].Split('|');
            int contCatch = 0;
            string glosaError = "";
            foreach(var ldap in LDAP)
            {  
            try
            {
                    PrincipalContext context = new PrincipalContext(ContextType.Domain, ldap);
                UserPrincipal qbeUser = new UserPrincipal(context);
                qbeUser.UserPrincipalName = username;

                PrincipalSearcher search = new PrincipalSearcher(qbeUser);
                Principal result = search.FindOne();
                //var dirSearch = new DirectorySearcher(new DirectoryEntry(rootQuery));
                //dirSearch.Filter = String.Format("(sAMAccountName={0})", username);
                //var result = dirSearch.FindOne();

            if (result != null)
            {
                var user = _usuarios.RetrieveByUsername(username);
                if (user != null)
                {
                    return string.Format("Usuario {0} ({1}) ya existe en el sistema.", username, user.Displayname);
                }
                    var Email = result.UserPrincipalName;//result.GetDirectoryEntry().Properties["userPrincipalName"].Value.ToString();
                    var DisplayName = result.DisplayName;//result.GetDirectoryEntry().Properties["displayName"].Value.ToString();

                return DisplayName + "|" + Email;
            }

                glosaError = string.Format("Usuario {0} no existe en Active Directory", username);
            }
            catch(Exception ex)
            {
                    contCatch++;
                    if (contCatch == LDAP.Length)
                    {
                        glosaError = string.Format("Error: {0} ", ex.Message);
                    }
                }
            }

            return glosaError;
        }
        private Boolean ParseUserName(string path, out string accountName, out string domainName)
        {
            bool retVal = false;
            accountName = string.Empty;
            domainName = string.Empty;
            string[] userPath = path.Split(new char[] { '\\' });
            if (userPath.Length > 0)
            {
                retVal = true;
                accountName = userPath[userPath.Length - 1];
                if (userPath.Length > 1)
                {
                    domainName = userPath[userPath.Length - 2];
                }
            }
            return retVal;
        }
        private SecUsuario ParseSelectedRoles(UsuarioEditViewModel model)
        {
            if (model.Roles != null)
            {
                foreach (var rol in model.Roles)
                {
                    if (rol.Id == 0) 
                        continue;
                    model.Usuario.Roles.Add(rol);
                }
            }
            if (model.SelectedNegocioPais != null)
            {
            var arr = model.SelectedNegocioPais.Split('-');
            if (arr.Length == 2)
            {
                model.Usuario.Idpais = int.Parse(arr[0]);
                model.Usuario.Idnegocio = int.Parse(arr[1]);
                }
            }
            else
            {
                model.Usuario.Idpais = null;
                model.Usuario.Idnegocio = null;
            }
            return model.Usuario;
        }
        public String SecUsuarioFileXls()
        {
            string serror = "";

            DataTable tabla = new DataTable();


            SecUsuarioDB ctrlSecRol = new SecUsuarioDB();


            tabla = ctrlSecRol.TablaSecUsuario(out serror);


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
