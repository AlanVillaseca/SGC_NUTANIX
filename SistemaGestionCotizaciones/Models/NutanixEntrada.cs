using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class NutanixEntrada 
    {
        public int idComponente { get; set; }
        public string sistemaOperativo { get; set; }
        public string siteSec { get; set; }
        public string vlanSec { get; set; }
        public string rolSet { get; set; }
        public string charSet { get; set; }
        public string dbName { get; set; }
    }

    public class NutanixParametros
    {
        public int idCotizacion { get; set; }
        public int idProyecto { get; set; }
        public string plantillaComp { get; set; }
        public string descripcionComp { get; set; }
        public string catCosto { get; set; }
        public string estadoComp { get; set; }
        public int idComponente { get; set; }
        public int idAmbiente { get; set; }
        public string ambienteComp { get; set; }
        public int idTipo { get; set; }
        public string esAplicacion { get; set; }
        public int vcoresComp { get; set; }
        public int ramComp { get; set; }
        public int idNegocio { get; set; }
        public string negocio { get; set; }
    }

    public class NutanixPlantillas
    {
        public string plantillaJson { get; set; }
        public string plantillaScript { get; set; }
    }


    public class NutanixTask
    {
        public string status { get; set; }
        public string last_update_time { get; set; }
        public string logical_timestamp { get; set; }
        public List<entity_reference_list> entity_reference_list { get; set; }
        public string start_time { get; set; }
        public string creation_time { get; set; }
        public string start_time_usecs { get; set; }
        public List<cluster_reference> cluster_reference { get; set; }
        public List<subtask_reference_list> subtask_reference_list { get; set; }
        public string completion_time { get; set; }
        public string creation_time_usecs { get; set; }
        public string progress_message { get; set; }
        public string operation_type { get; set; }
        public string completion_time_usecs { get; set; }
        public string percentage_complete { get; set; }
        public string api_version { get; set; }
        public string uuid { get; set; }
    }
    
    public class entity_reference_list
{
        public string kind { get; set; }
        public string uuid { get; set; }
    }

    public class cluster_reference
{
        public string kind { get; set; }
        public string uuid { get; set; }
    }
    
    public class subtask_reference_list { }