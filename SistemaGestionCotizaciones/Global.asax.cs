using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Helpers;
using System.Security.Claims;
using System.Reflection;
using log4net;
using SistemaGestionCotizaciones.Controllers;

namespace SistemaGestionCotizaciones
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly ILog  logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var ex = Server.GetLastError();
            var action = "Error";
            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;
                switch (httpEx.GetHttpCode())
                {
                    case 403:
                        action = "Forbidden";
                        break;
                    case 404:
                        action = "NotFound";
                        break;
                    case 500:
                        action = "Expired";
                        break;
                    // others if any
                }
            }
            // log the error using log4net.
            logger.Error(ex.Message, ex);

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
            httpContext.Response.Headers.Add("Content-Type", "text/html");
            httpContext.Response.TrySkipIisCustomErrors = true;

            var routeData = new RouteData();
            routeData.Values["controller"] = "ErrorPage";
            routeData.Values["action"] = action;

            var controller = new ErrorPageController();
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
    }
}
