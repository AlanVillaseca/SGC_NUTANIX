using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaGestionCotizaciones.Models.Security;
using System.Security.Principal;
using System.Security.Claims;
using System.Data;
using System.Text;
using SistemaGestionCotizaciones.Models;

namespace SistemaGestionCotizaciones.Controllers.Security
{
    public class RolController : BaseController
    {
        //
        // GET: /rol/
        public ActionResult Index()
        {
			return View( _roles.RetrieveAll());
        }

        //
        // GET: /rol/Create
        public ActionResult Create()
        {
            return View(new SecRol());
        } 

        //
        // POST: /rol/Create
        [HttpPost]
        public ActionResult Create(SecRol rol)
        {
            _roles.Insert(rol);
            return RedirectToAction("Index");
        }
        
        //
        // GET: /rol/Delete/5
        public ActionResult Delete(int id)
        {
            return View( _roles.RetrieveById(id));
        }

        //
        // POST: /rol/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            _roles.Delete(id);
            return RedirectToAction("Index");
        }


        public String SecRolFileXls()
        {
            string serror = "";

            DataTable tabla = new DataTable();


            SecRolDB ctrlSecRol = new SecRolDB();


            tabla = ctrlSecRol.TablaSecRol(out serror);


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