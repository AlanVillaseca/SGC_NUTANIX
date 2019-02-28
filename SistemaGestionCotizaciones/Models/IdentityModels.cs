using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.Validation;
using System.DirectoryServices;
using System.Data;
using System.Web.Script.Serialization;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.Web.Configuration;
using Atech.DataAccessLayer;

namespace SistemaGestionCotizaciones.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class Permiso
    {
        public int id { get; set; }
        public string Applicacion { get; set; }
        public string NivelAccesoRWD { get; set; }
        public string NivelAccesoAR { get; set; }

        public string ToString()
        {
            return string.Format("{0},{1},{2}", Applicacion, NivelAccesoRWD, NivelAccesoAR);
        }
    }
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string code, string name, string description)
            : base()
        {
            this.Code = code;
            base.Name = name;
            this.Description = description;
        }

        public virtual string Description { get; set; }
        public virtual string Code { get; set; }
    }
    public class ApplicationUserRole : IdentityUserRole
    {
        public ApplicationUserRole()
            : base()
        { }

        public ApplicationRole Role { get; set; }
    }
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        [NotMapped]
        public List<string> xRoles { get; set; }
        [NotMapped]
        public List<Permiso> Permisos { get; set; }

        public ApplicationUser()
        {
        }
        public ApplicationUser(string username)
        {
            this.UserName = username;
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            //var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, this.Email));
            claims.Add(new Claim(ClaimTypes.WindowsAccountName, this.UserName));
            claims.Add(new Claim(ClaimTypes.Name, this.DisplayName, "http://www.w3.org/2001/XMLSchema#string"));
            claims.Add(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, this.Id, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
            foreach (var rol in this.xRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }
            foreach (var permiso in this.Permisos)
            {
                claims.Add(new Claim(ClaimTypes.UserData, permiso.ToString()));
            }
            var userIdentity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }

        public bool LoadProperties()
        {
            //try
            //{
            //get user properties from AD
            //var rootQuery = ConfigurationManager.ConnectionStrings["ADConnectionString"].ConnectionString;

            string[] LDAP = WebConfigurationManager.AppSettings["LDAP"].Split('|');

            foreach (var ldap in LDAP)
            {
                try
                {
                    PrincipalContext context = new PrincipalContext(ContextType.Domain, ldap);
                    UserPrincipal qbeUser = new UserPrincipal(context);
                    qbeUser.UserPrincipalName = this.UserName;
                    PrincipalSearcher search = new PrincipalSearcher(qbeUser);
                    Principal result = search.FindOne();
                    //var dirSearch = new DirectorySearcher(new DirectoryEntry(rootQuery));
                    //dirSearch.Filter = String.Format("(sAMAccountName={0})", this.UserName);
                    //var result = search.FindOne();

                    this.Email = result.UserPrincipalName; //result..GetDirectoryEntry().Properties["userPrincipalName"].Value.ToString();
                    this.DisplayName = result.DisplayName; //result.GetDirectoryEntry().Properties["displayName"].Value.ToString();

                    //Get user profile
                    var sError = "";
                    bool isROL = false;
                    DataTable dt = null;
                    string codError = "";

                    var dbConn = ConfigurationManager.ConnectionStrings["falabellaDesarrollo"].ConnectionString;
                    SqlAccess cDAL = new SqlAccess(dbConn);
                    var res = cDAL.Consultar("USP_SEL_USUARIO_ROLES", out dt,out codError, out sError, this.UserName);
                    if (!res) throw new Exception(sError);

                    xRoles = new List<string>();
                    foreach (DataRow row in dt.Rows)
                    {
                        xRoles.Add(row["rol"].ToString());
                        isROL = true;
                    }
                    if (!isROL) throw new Exception("El usuario " + this.UserName + " no está registrado en el sistema SGC");

                    res = cDAL.Consultar("USP_SEL_USUARIO_PERMISOS", out dt,out codError, out sError, this.UserName);
                    if (!res) throw new Exception(sError);

                    Permisos = new List<Permiso>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Permisos.Add(
                            new Permiso()
                            {
                                Applicacion = row["objeto"].ToString(),
                                NivelAccesoRWD = row["nivelaccesorwd"].ToString(),
                                NivelAccesoAR = row["nivelaccesoar"].ToString()
                            }
                            );
                    }
                }
                catch (Exception e)
                {

                }
            }


            return true;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}

        }
        private bool ParseUserName(string path, out string accountName, out string domainName)
        {
            bool retVal = false;
            accountName = String.Empty;
            domainName = String.Empty;
            string[] userPath = path.Split(new char[] { '\\' });
            if (userPath.Length > 0)
            {
                retVal = true;
                accountName = userPath[userPath.Length - 1];
                if (userPath.Length > 1)
                {
                    domainName = userPath[userPath.Length - 2];
                }
            }
            return retVal;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        //: base("falabellaDesarrollo", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("ModelBuilder is NULL");
            }

            //base.OnModelCreating(modelBuilder);
            EntityTypeConfiguration<IdentityUser> table = modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers2");
            table.HasMany<IdentityUserRole>((IdentityUser u) => u.Roles).WithRequired().HasForeignKey<string>((IdentityUserRole ur) => ur.UserId);
            table.HasMany<IdentityUserClaim>((IdentityUser u) => u.Claims).WithRequired().HasForeignKey<string>((IdentityUserClaim uc) => uc.UserId);
            table.HasMany<IdentityUserLogin>((IdentityUser u) => u.Logins).WithRequired().HasForeignKey<string>((IdentityUserLogin ul) => ul.UserId);
            StringPropertyConfiguration stringPropertyConfiguration = table.Property((IdentityUser u) => u.UserName).IsRequired().HasMaxLength(new int?(256));
            IndexAttribute indexAttribute = new IndexAttribute("UserNameIndex") { IsUnique = true };
            stringPropertyConfiguration.HasColumnAnnotation("Index", new IndexAnnotation(indexAttribute));
            table.Property((IdentityUser u) => u.Email).HasMaxLength(new int?(256));

            modelBuilder.Entity<IdentityUserRole>().HasKey((IdentityUserRole r) => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles2");
            modelBuilder.Entity<IdentityUserLogin>().HasKey((IdentityUserLogin l) => new { LoginProvider = l.LoginProvider, ProviderKey = l.ProviderKey, UserId = l.UserId }).ToTable("AspNetUserLogins2");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims2");

            EntityTypeConfiguration<IdentityRole> entityTypeConfiguration = modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles2");
            StringPropertyConfiguration stringPropertyConfiguration1 = entityTypeConfiguration.Property((IdentityRole r) => r.Name).IsRequired().HasMaxLength(new int?(256));
            IndexAttribute indexAttribute1 = new IndexAttribute("RoleNameIndex") { IsUnique = true };
            stringPropertyConfiguration1.HasColumnAnnotation("Index", new IndexAnnotation(indexAttribute1));
            entityTypeConfiguration.HasMany<IdentityUserRole>((IdentityRole r) => r.Users).WithRequired().HasForeignKey<string>((IdentityUserRole ur) => ur.RoleId);



            /* //Defining the keys and relations
            modelBuilder.Entity<ApplicationUser>().ToTable("SecUsuario");
            modelBuilder.Entity<ApplicationRole>().HasKey<string>(r => r.Id).ToTable("SecRol");
            modelBuilder.Entity<ApplicationUser>().HasMany<ApplicationUserRole>((ApplicationUser u) => u.UserRoles);
            modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("SecRolUsuario");
            */
        }
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            return base.ValidateEntity(entityEntry, items);
        }
    }

    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        ApplicationDbContext context;

        public ApplicationUserStore(ApplicationDbContext dbContext)
        {
            this.context = dbContext;
        }
        /*
              public void Dispose()
              {
                  context.Dispose();
              }


              public Task CreateAsync(ApplicationUser user)
              {
                  throw new NotImplementedException();
              }

              public Task DeleteAsync(ApplicationUser user)
              {
                  throw new NotImplementedException();
              }

              public Task<ApplicationUser> FindByIdAsync(string userId)
              {
                  return base.FindByIdAsync(userId);
                  //throw new NotImplementedException();
              }

              public Task<ApplicationUser> FindByNameAsync(string userName)
              {
                  throw new NotImplementedException();
              }

              public Task UpdateAsync(ApplicationUser user)
              {
                  throw new NotImplementedException();
              }
         */
    }

}


