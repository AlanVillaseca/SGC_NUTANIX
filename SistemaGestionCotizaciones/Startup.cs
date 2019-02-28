using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SistemaGestionCotizaciones.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace SistemaGestionCotizaciones
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
