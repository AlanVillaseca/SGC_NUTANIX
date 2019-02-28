using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCEmail.Models;
using System.Net;
using System.Net.Mail;
using System.Web.Configuration;
using log4net;
using System.Reflection;

namespace SistemaGestionCotizaciones.Models
{
    public class EnviarMail
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Boolean enviar(string email, string asunto, string cuerpo)
        {            
            try
            {                
                SmtpClient client = new SmtpClient(WebConfigurationManager.AppSettings["smtpServer"], int.Parse(WebConfigurationManager.AppSettings["smtpPort"]));
                client.UseDefaultCredentials = false;
                client.EnableSsl = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential(WebConfigurationManager.AppSettings["smtpUser"], WebConfigurationManager.AppSettings["smtpPass"]);
                client.Send(new MailMessage(WebConfigurationManager.AppSettings["adminEmail"], email, asunto, cuerpo));
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
            return true;
        
        }
    }
}