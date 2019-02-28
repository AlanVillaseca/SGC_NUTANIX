using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using SistemaGestionCotizaciones.Models;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace SistemaGestionCotizaciones.Controllers
{
    public class CargasController : Controller
    {
        // GET: Cargas
        public ActionResult Index()
        {

            if (TempData["shortMessage"] != null) {

                ViewBag.Message = TempData["shortMessage"].ToString();
            
            }


            return View();
        }

        [HttpPost]
        public string EnviarCarga(string id)
        {
            string serror = "";
            string[] divide = id.Split('|');
            Models.Carga carga = new Models.Carga();
            List<string> lista = new List<string>();

            for (int i = 1; i < divide.Length; i++ )
            {
                carga.CargaArchivo(divide[i], divide[0], out serror);
                lista.Add(serror);
            }

            int validar = 0;
            foreach(string db in lista)
            {
                if(db != "")
                {
                    validar = 1;
                }
            }

            if(validar == 1)
            {
                serror = "Error";
            }
           
            return serror;
        }



        private void WriteDelimitedData(DataTable dt)
        {
            
            string attachment = "attachment; filename=city.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }


        [HttpPost]
        public void CargarArchivo(HttpPostedFileBase file_archivo)
        {

            string resultado = "";
            string linea = "";
            string cadena = "";
            string serrors = "";

            List<string> listaErrores = new List<string>();
            List<int> largoArray = new List<int>();
            List<Models.Carga> lista = new List<Models.Carga>();

            if (file_archivo != null)
            {
                resultado = "Exitoso";

                StreamReader file = new StreamReader(file_archivo.InputStream);

                while ((linea = file.ReadLine()) != null)
                {
                    Models.Carga ca = new Models.Carga();
                    ca.cadena = linea.ToString();
                    cadena = cadena + linea.ToString() + "|";
                    lista.Add(ca);
                }


                foreach (var db in lista)
                {
                    string[] divide = db.cadena.Split(';');
                    largoArray.Add(divide.Length);
                }

                bool encontrado = false;

                for (int i = 0; i < largoArray.Count(); i++)
                {
                    for (int j = 1; j < largoArray.Count(); j++)
                    {
                        if (largoArray[i] == largoArray[j])
                        {

                        }
                        else
                        {
                            encontrado = true;
                        }
                    }
                }

                if (encontrado.Equals(true))
                {
                    cadena = "0";
                }


                if (Request.Form["DescargaArchivo"] != null)
                {




                    Carga ctrlCarga = new Carga();

                    DataTable dt = ctrlCarga.ReporteConsumoVariable(cadena, serrors);


                    WriteDelimitedData(dt);

                    //string attachment = "attachment; filename=ConsumoVariable.xls";
                    //Response.ClearContent();
                    //Response.Clear();
                    //Response.AddHeader("content-disposition", attachment);
                    //Response.ContentType = "application/vnd.ms-excel";
                    //string tab = "";
                    //foreach (DataColumn dc in dt.Columns)
                    //{
                    //    Response.Write(tab + dc.ColumnName);
                    //    tab = "\t";
                    //}
                    //Response.Write("\n");
                    //int i;
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    tab = "";
                    //    for (i = 0; i < dt.Columns.Count; i++)
                    //    {
                    //        Response.Write(tab + dr[i].ToString());
                    //        tab = "\t";
                    //    }
                    //    Response.Write("\n");
                    //}
                    //Response.Flush();
                    //Response.End();


                }
                else if (Request.Form["CargarArchivo"] != null)
                {

                    TempData["shortMessage"] = cadena;

                    Response.Redirect(Url.Action("Index", "Cargas"));
                    
                }

                


            }
            else
            {
                resultado = "dato nulo";
            }




            
            
        }
    }
}