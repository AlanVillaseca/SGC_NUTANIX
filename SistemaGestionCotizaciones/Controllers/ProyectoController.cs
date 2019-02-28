using SistemaGestionCotizaciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCEmail.Models;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Security.Permissions;
using System.Web.Configuration;
using System.Globalization;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SistemaGestionCotizaciones.Controllers
{
    [Authorize]
    public class ProyectoController : Controller
    {
        // GET: Proyecto
        public Boolean enviarMensaje() {
            EnviarMail mail = new EnviarMail();
            EmailFormModel mensaje = new EmailFormModel();
            //mail.enviar(mensaje.Message);
            return true;
        }

        /***************************************************************************/
        /***************************************************************************/
        public async Task<ActionResult> Listar()
        {
           string sError = "";
           ListarProyecto ctrlFiltrosProyectos = new ListarProyecto((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity);

           Task<bool> CargaFiltros = ctrlFiltrosProyectos.cargaListarCotizaciones();
           bool VarResult = await CargaFiltros;

           if(ctrlFiltrosProyectos.Errors.Count > 0)
           {
               foreach (string item in ctrlFiltrosProyectos.Errors)

                   sError += item;
           }


            ViewBag.Error = sError;

            return View(ctrlFiltrosProyectos);
        }

        /***************************************************************************/
        /***************************************************************************/


        public ActionResult vpGrillaListar(string id, string paisNegocio, string negocio, string usrSolicitante,
                                                string srvNegocio, string fchIni, string fchFin, string usrAsignada, 
                                                string pagina, string idHead, string ascDesc, string idMostrar,
                                                string codCot, string usrCreador)
        {
            string sError = "";
            string idHeadAct = "";
            string ascDescAct = "";

            ListarProyecto ctrlGrillaProyectos = new ListarProyecto((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity);

            ctrlGrillaProyectos.cargaModGrillaProyectos(id, paisNegocio, negocio, usrSolicitante,
                                                srvNegocio, fchIni, fchFin, usrAsignada, pagina, idHead, ascDesc, idMostrar,
                                                codCot, usrCreador, out sError, out idHeadAct, out ascDescAct);
            ViewBag.Error = sError;
            ViewBag.idHeadAct = idHeadAct;
            ViewBag.ascDescAct = ascDescAct;

            return PartialView(ctrlGrillaProyectos);
        }
        [HttpPost]
        public String CargaServicio(string idnegocio, string idpais)
        {
            string sError = "";

            JavaScriptSerializer varjon = new JavaScriptSerializer();
            ListarProyecto ctrlProyecto = new ListarProyecto((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity);
            ctrlProyecto.CargaServicioNegocio(idnegocio, idpais, out sError);
            ViewBag.Error = sError;
            return varjon.Serialize(ctrlProyecto.dtServicioJson);
        }
        public ActionResult Ingresar()
        {
            try
            {
                string sError = "";
                Proyecto oProy = new Proyecto();
                oProy.Cargar(out sError);


                ViewBag.Error = sError;

                return View(oProy);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Ingresar(Proyecto oProy)
        {
            string sError = "";

            oProy.Guardar(out sError);
            oProy.Cargar(out sError);

            return View(oProy);
        }
        [HttpPost]
        public string vpIngresar(Proyecto oProy)
        {
            string sError = "";
            var filename = "";
            var path = "";
                    
   

            if (oProy.ruta != null)
            {
                
                if(System.IO.Directory.Exists(WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString()))
                {
                    if(System.IO.Directory.Exists(WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"))))
                    {
                        path = WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")));
                        path = WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    }
                }
                else
                {
                    System.IO.Directory.CreateDirectory(WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString());
                    if (System.IO.Directory.Exists(WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"))))
                    {
                        path = WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")));
                        path = WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    }
                }


                string[] extension = oProy.extension.Split('.');
                string ext = extension[1];
                oProy.nuevoarchivo = oProy.extension;// +"." + ext;
                filename = Path.GetFileName(oProy.nuevoarchivo);
                path = Path.Combine(path, filename);
                oProy.nuevoarchivo = path;
                //oProy.rutaFEP.SaveAs(path);
                System.IO.File.WriteAllBytes(path, Convert.FromBase64String(oProy.ruta));
            }
            else
            {
                oProy.nuevoarchivo = "";
            }

            if (oProy.codigo == null)
                oProy.codigo = "";

            if (oProy.descripcion == null)
                oProy.descripcion = "";

            oProy.Guardar(out sError);
            //oProy.Cargar(out sError);

            Response.Redirect("Listar");
            return sError;
        }
        public ActionResult vpIngresar()
        {
            string sError = "";
            Proyecto oProy = new Proyecto();
            oProy.Cargar(out sError);
            ViewBag.Error = sError;

            return PartialView(oProy);
        }

        public ActionResult Detalle(string id)
        {
            try
            {
                string sError = "";
                DetalleProyecto ctrlFiltrosProyectos = new DetalleProyecto();

                ctrlFiltrosProyectos.cargaDetalleProyecto(id, out sError);
                ctrlFiltrosProyectos.cargaJDP(out sError);
                ctrlFiltrosProyectos.cargaImplementadores(out sError);

                ViewBag.Error = sError;
                ViewBag.idProyecto = id;
                return View(ctrlFiltrosProyectos);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        [HttpPost]
        public string DescargaAdjunto(string id)
        {

            string sError = "";
            string sRuta = "";
            DetalleProyecto ctrlFiltrosProyectos = new DetalleProyecto();

            ctrlFiltrosProyectos.cargaDetalleProyecto(id, out sError);

            sRuta = ctrlFiltrosProyectos.dtDetalleProyecto.Rows[0].ItemArray.GetValue(20).ToString();

            byte[] fileBytes = System.IO.File.ReadAllBytes(sRuta);
            //string fileName = "myfile.ext";
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
            return Convert.ToBase64String(fileBytes);

        }

        public ActionResult vpGrillaDetalle(string id, string idproyecto, string estado, string pagina)
        {
            try
            {
			//acapunto			
                string campo = "idcotizacion";

                if (pagina == null)
                {
                    pagina = "1";
                }

                string sError = "";
                DetalleProyecto ctrlGrillaCotizaciones = new DetalleProyecto();

                ctrlGrillaCotizaciones.cargaDetalleProyecto(id, out sError);
                ctrlGrillaCotizaciones.cargaModGrillaDetalle(id, campo, pagina, out sError);
                ctrlGrillaCotizaciones.CargaComboModalNegocio(out sError);
                ctrlGrillaCotizaciones.CargaComboModalServicioNegocio(out sError);
                
                ViewBag.Error = sError;
                ViewBag.Pagina = pagina;
                ViewBag.IdCotizacion = id;
                ViewBag.Idproyecto = idproyecto;
                ViewBag.Estado = estado;
                ViewBag.Facturacion = ctrlGrillaCotizaciones.VerFacturacion(id, out sError);

                return PartialView(ctrlGrillaCotizaciones);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return PartialView();
            }
        }
        [HttpPost]
        public string editarProyecto(string id, string codigo, string nombre, string descripcion, string idSolicitante, string idNegocio, string idPais, string idServicioNegocio, string xmlProrrateo, string cliente, string ruta, string extension)
        {
            string sError = "";
            var filename = "";
            var path = "";
            string nuevoarchivo = "";


            if (ruta != null && !ruta.Equals(""))
            {
                //string[] div = extension.Split('.');
                //string ext = div[1];
                nuevoarchivo = extension;
                filename = Path.GetFileName(nuevoarchivo);
                if (System.IO.Directory.Exists(WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString()))
                {
                    if (System.IO.Directory.Exists(WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"))))
                    {
                        path = WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")));
                        path = WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    }
                }
                else
                {
                    System.IO.Directory.CreateDirectory(WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString());
                    if (System.IO.Directory.Exists(WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"))))
                    {
                        path = WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")));
                        path = WebConfigurationManager.AppSettings["rutaDocumento"] + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    }
                }

                string sRuta = "";
                DetalleProyecto ctrlFiltrosProyectos = new DetalleProyecto();

                ctrlFiltrosProyectos.cargaDetalleProyecto(id, out sError);

                sRuta = ctrlFiltrosProyectos.dtDetalleProyecto.Rows[0].ItemArray.GetValue(20).ToString();

				if (sRuta.Length > 0)
                {
                    System.IO.File.Delete(sRuta);
                }

                path = Path.Combine(path, filename);

                //oProy.rutaFEP.SaveAs(path);
                System.IO.File.WriteAllBytes(path, Convert.FromBase64String(ruta));
            }
            else
            {
                nuevoarchivo = "";
            }
            DetalleProyecto editrDetalle = new DetalleProyecto();
            editrDetalle.UpdateProyecto(id, codigo, nombre, descripcion, idSolicitante, idNegocio, idPais, idServicioNegocio, xmlProrrateo, cliente, path, out sError);

            return sError;
        }

        public ActionResult vpAgregarCotizacion()
        {
            try
            {
                string sError = "";
                DetalleProyecto oCot = new DetalleProyecto();
                //oCot.id = id;
                //oCot.descCotizacion = descCotizacion;
                //oCot.usuarioCreador = usuarioCreador;
                //if (id != null)
                    //oCot.guardarNuevaCotizacion(out sError);

                return PartialView();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return PartialView();
            }
        }

        public String NuevaCotizacion(string id, string descCotizacion)
        {
            try
            {
                string sError = "";
                DetalleProyecto oCot = new DetalleProyecto();
                oCot.id = id;
                oCot.descCotizacion = descCotizacion;
                //oCot.usuarioCreador = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
                if (id != null)
                    oCot.guardarNuevaCotizacion(out sError);

                return sError;
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return e.Message;
            }
        }
        public string AsignarJDPProyecto(string id, string idjefedeproyecto)
        {
            string sError = "";
            Proyecto proyecto = new Proyecto();
            proyecto.AsignarJDP(id, idjefedeproyecto, out sError);
            return sError;
        }
        public Boolean BloquearProyecto(string id)
        {
            try
            {
                string sError = "";
                DetalleProyecto detalleproyecto = new DetalleProyecto();
                detalleproyecto.bloquearProyecto(id, out sError);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public Boolean DesbloquearProyecto(string id)
        {
            try
            {
                string sError = "";
                DetalleProyecto detalleproyecto = new DetalleProyecto();
                detalleproyecto.desbloquearProyecto(id, out sError);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public Boolean EliminarProyecto(string id)
        {
            try
            {
                string sError = "";
                DetalleProyecto detalleproyecto = new DetalleProyecto();
                detalleproyecto.EliminarProyecto(id, out sError);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public string AsignarImplementador(string id, string idImp)
        {
            string sError = "";
            DetalleProyecto ctrlProyecto = new DetalleProyecto();
            ctrlProyecto.AsignarImplementadorCotizacion(id, idImp, out sError);
            return sError;
        }
    }
}