using Newtonsoft.Json;
using System.Web.Services;
using System.Web.Script.Serialization;

namespace SistemaGestionCotizaciones
{
    /// <summary>
    /// Summary description for WebServiceNutanix
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebServiceNutanix : System.Web.Services.WebService
    {
        Models.NutanixFunciones _fun = new Models.NutanixFunciones();

        [WebMethod]
        public string creaNutanixVM(int _idComponente, string _sistemaOperativo, string _siteSec, string _vlanSec, string _rolSet,
        string _charSet, string _dbName)
        {

            NutanixEntrada NutanixIn = new NutanixEntrada();
            NutanixIn.idComponente = _idComponente;

            NutanixIn.sistemaOperativo = _sistemaOperativo;
            NutanixIn.siteSec = _siteSec;
            NutanixIn.vlanSec = _vlanSec;
            NutanixIn.rolSet = _rolSet;
            NutanixIn.charSet = _charSet;
            NutanixIn.dbName = _dbName;

            string resultado = _fun.parametrizacionComponente(NutanixIn,_idComponente);

            return resultado;  
        }

        [WebMethod]
        public string infoNutanixVM(string _taskUUID)
        {

            var datosVM = _fun.llamadaAPI_Task(_taskUUID);
            NutanixTask task = new JavaScriptSerializer().Deserialize<NutanixTask>(datosVM);

            var nombreVM = _fun.llamadaAPI_IP(task.entity_reference_list[0].uuid);

            return nombreVM;
        }

    }//fin clase
}//fin namespace
