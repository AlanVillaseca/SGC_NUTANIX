using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PetaPoco;


namespace SistemaGestionCotizaciones.Models.Security
{

    public partial class SecurityDB : Database
    {
        public SecurityDB()
            : base("falabellaDesarrollo")
        {
            CommonConstruct();
        }

        public SecurityDB(string connectionStringName)
            : base(connectionStringName)
        {
            CommonConstruct();
        }

        partial void CommonConstruct();

        public interface IFactory
        {
            SecurityDB GetInstance();
        }

        public static IFactory Factory { get; set; }
        public static SecurityDB GetInstance()
        {
            if (_instance != null)
                return _instance;

            if (Factory != null)
                return Factory.GetInstance();
            else
                return new SecurityDB();
        }

        [ThreadStatic]
        static SecurityDB _instance;

        public override void OnBeginTransaction()
        {
            if (_instance == null)
                _instance = this;
        }

        public override void OnEndTransaction()
        {
            if (_instance == this)
                _instance = null;
        }


        public class Record<T> where T : new()
        {
            public static SecurityDB repo { get { return SecurityDB.GetInstance(); } }
            public bool IsNew() { return repo.IsNew(this); }
            public object Insert() { return repo.Insert(this); }

            public void Save() { repo.Save(this); }
            public int Update() { return repo.Update(this); }

            public int Update(IEnumerable<string> columns) { return repo.Update(this, columns); }
            public static int Update(string sql, params object[] args) { return repo.Update<T>(sql, args); }
            public static int Update(Sql sql) { return repo.Update<T>(sql); }
            public int Delete() { return repo.Delete(this); }
            public static int Delete(string sql, params object[] args) { return repo.Delete<T>(sql, args); }
            public static int Delete(Sql sql) { return repo.Delete<T>(sql); }
            public static int Delete(object primaryKey) { return repo.Delete<T>(primaryKey); }
            public static bool Exists(object primaryKey) { return repo.Exists<T>(primaryKey); }
            public static bool Exists(string sql, params object[] args) { return repo.Exists<T>(sql, args); }
            public static T SingleOrDefault(object primaryKey) { return repo.SingleOrDefault<T>(primaryKey); }
            public static T SingleOrDefault(string sql, params object[] args) { return repo.SingleOrDefault<T>(sql, args); }
            public static T SingleOrDefault(Sql sql) { return repo.SingleOrDefault<T>(sql); }
            public static T FirstOrDefault(string sql, params object[] args) { return repo.FirstOrDefault<T>(sql, args); }
            public static T FirstOrDefault(Sql sql) { return repo.FirstOrDefault<T>(sql); }
            public static T Single(object primaryKey) { return repo.Single<T>(primaryKey); }
            public static T Single(string sql, params object[] args) { return repo.Single<T>(sql, args); }
            public static T Single(Sql sql) { return repo.Single<T>(sql); }
            public static T First(string sql, params object[] args) { return repo.First<T>(sql, args); }
            public static T First(Sql sql) { return repo.First<T>(sql); }
            public static List<T> Fetch(string sql, params object[] args) { return repo.Fetch<T>(sql, args); }
            public static List<T> Fetch(Sql sql) { return repo.Fetch<T>(sql); }
            public static List<T> Fetch(long page, long itemsPerPage, string sql, params object[] args) { return repo.Fetch<T>(page, itemsPerPage, sql, args); }
            public static List<T> Fetch(long page, long itemsPerPage, Sql sql) { return repo.Fetch<T>(page, itemsPerPage, sql); }
            public static List<T> SkipTake(long skip, long take, string sql, params object[] args) { return repo.SkipTake<T>(skip, take, sql, args); }
            public static List<T> SkipTake(long skip, long take, Sql sql) { return repo.SkipTake<T>(skip, take, sql); }
            public static Page<T> Page(long page, long itemsPerPage, string sql, params object[] args) { return repo.Page<T>(page, itemsPerPage, sql, args); }
            public static Page<T> Page(long page, long itemsPerPage, Sql sql) { return repo.Page<T>(page, itemsPerPage, sql); }
            public static IEnumerable<T> Query(string sql, params object[] args) { return repo.Query<T>(sql, args); }
            public static IEnumerable<T> Query(Sql sql) { return repo.Query<T>(sql); }

        }

    }

    [TableName("SecAccion")]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class SecAccion : SecurityDB.Record<SecAccion>
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("codigo")]
        public string Codigo { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("prioridad")]
        public short Prioridad { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }

    [TableName("SecAplicacion")]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class SecAplicacion : SecurityDB.Record<SecAplicacion>
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("codigo")]
        public string Codigo { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }

    [TableName("SecRol")]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class SecRol : SecurityDB.Record<SecRol>
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("codigo")]
        [Required(ErrorMessage = "Este campo no puede ser estar en blanco.")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "El largo debe ser entre 2-15 caracteres.")]
        public string Codigo { get; set; }

        [Column("nombre")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "El largo debe ser entre 0-50 caracteres.")]
        public string Nombre { get; set; }

        [Column("descripcion")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "El largo debe ser entre 0-255 caracteres.")]
        public string Descripcion { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

        [ResultColumn]
        public List<SecUsuario> Usuarios { get; set; }

        [ResultColumn]
        public List<SecRolPermiso> Permisos { get; set; }

        public SecRol()
        {
            Id = 0;
            Usuarios = new List<SecUsuario>();
            Permisos = new List<SecRolPermiso>();
        }
        public SecRol(string rolCodigo)
        {
            Id = 0;
            Codigo = rolCodigo;
            Usuarios = new List<SecUsuario>();
            Permisos = new List<SecRolPermiso>();
        }
    }

    [TableName("SecRolUsuario")]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class SecRolUsuario : SecurityDB.Record<SecRolUsuario>
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("idsecusuario")]
        public int Idsecusuario { get; set; }

        [Column("idsecrol")]
        public int Idsecrol { get; set; }
    }

    [TableName("SecRolGrupoPerfil")]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class SecPermiso : SecurityDB.Record<SecPermiso>
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("idsecrol")]
        public int Idsecrol { get; set; }

        [Column("idsecaplicacion")]
        public int Idsecaplicacion { get; set; }

        [Column("idsecaccion")]
        public int? Idsecaccion { get; set; }

        [Column("idsecaccion2")]
        public int? Idsecaccion2 { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

        [ResultColumn]
        public SecAccion Accion { get; set; }

        [ResultColumn]
        public SecAccion Accion2 { get; set; }

        [ResultColumn]
        public SecAplicacion Aplicacion { get; set; }
        public SecPermiso()
        {
            Id = 0;
        }
    }

    [TableName("SecUsuario")]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class SecUsuario : SecurityDB.Record<SecUsuario>
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        [Required(ErrorMessage = "Este campo no puede ser estar en blanco.")]
        [Display(Name = "Usuario")]
        //[StringLength(32, MinimumLength = 3, ErrorMessage = "El largo debe ser entre 3-32 caracteres.")]
        public string Username { get; set; }

        [Column("displayname")]
        [Display(Name = "Nombre")]
        public string Displayname { get; set; }

        [Column("email")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Column("lastlogin")]
        [Display(Name = "Fecha Ultimo login")]
        public DateTime? Lastlogin { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

        [Column("idnegocio")]
        public int? Idnegocio { get; set; }
        
        [Column("idpais")]
        public int? Idpais { get; set; }

        [Column("idcentrocosto")]
        public string Idcentrocosto { get; set; }

        [ResultColumn]
        public string Paisnegocio { get; set; }

        [ResultColumn]
        public List<SecRol> Roles { get; set; }

        public SecUsuario()
        {
            Id = 0;
            Roles = new List<SecRol>();
        }
    }

    [TableName("SecRolPermiso")]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class SecRolPermiso : SecurityDB.Record<SecRolPermiso>
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("idsecrol")]
        public int Idsecrol { get; set; }

        [Column("codigorol")]
        public string Codigorol { get; set; }

        [Column("nombrerol")]
        [Display(Name = "Nombre Rol")]
        public string Nombrerol { get; set; }
        [Column("idsecaplicacion")]
        public int Idsecaplicacion { get; set; }

        [Column("codigoaplicacion")]
        public string Codigoaplicacion { get; set; }

        [Column("nombreaplicacion")]
        [Display(Name = "Aplicacion")]
        public string Nombreaplicacion { get; set; }

        [Column("idsecaccion")]
        public int? Idsecaccion { get; set; }

        [Column("codigoaccion")]
        public string Codigocaccion { get; set; }

        [Column("nombreaccion")]
        [Display(Name = "Accion(Leer/Escribir/Borrar)")]
        public string Nombrecaccion { get; set; }

        [Column("idsecaccion2")]
        public int? Idsecaccion2 { get; set; }

        [Column("codigoaccion2")]
        public string Codigocaccion2 { get; set; }

        [Column("nombreaccion2")]
        [Display(Name = "Accion(Aprobar/Rechazar)")]
        public string Nombrecaccion2 { get; set; }
    
        [Column("activo")]
        public bool Activo { get; set; }
    }

    [TableName("vwNegocioPais")]
    [PrimaryKey("idnegocio,idpais")]
    [ExplicitColumns]
    public class vwNegocioPais
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("idpais")]
        public int Idpais { get; set; }

        [Column("idnegocio")]
        public int Idnegocio { get; set; }

        [Column("rut")]
        public string Rut { get; set; }

        [Column("razonsocial")]
        public string Razonsocial { get; set; }
    }

    [TableName("vwCentroCosto")]
    [PrimaryKey("idcentrocosto,idpais")]
    [ExplicitColumns]
    public class vwCentroCosto
    {
        [Column("idcentrocosto")]
        public string Idcentrocosto { get; set; }
        [Column("gerencia")]
        public string Gerencia { get; set; }
        [Column("idpais")]
        public string Idpais { get; set; }
    }
}