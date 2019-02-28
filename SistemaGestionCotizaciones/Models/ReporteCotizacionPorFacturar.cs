﻿
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Security.Claims;
using System.Web;
using Atech.DataAccessLayer;


namespace SistemaGestionCotizaciones.Models
{
    public class ReporteCotizacionPorFacturar
    {
        public string dbConn;

        public List<Dictionary<object, string>> dtCotizacionPorFacturarJson { get; set; }

        public ReporteCotizacionPorFacturar()
        {
            this.dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
        }
        private string GetUserName()
        {
            var claim = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }

        public DataTable ReportePorFacturar(out string msgError, string desde, string hasta)
        {
            DataTable table = new DataTable();
            SqlAccess cdal = new SqlAccess(dbConn);
            string codError = "";

            cdal.Consultar("USP_SEL_REPORTE_POR_FACTURAR_00", out table,out codError, out msgError, desde, hasta);

            return table;
        }
    }
}