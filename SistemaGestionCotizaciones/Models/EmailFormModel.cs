using System.ComponentModel.DataAnnotations;

namespace MVCEmail.Models
{
    public class EmailFormModel
    {
        public string FromName = "Sistema Gestión de Cotizaciones";
        public string FromEmail = "sgc@falabella.cl";
        public string Message = "Probando Alertas por Mail";
    }
}