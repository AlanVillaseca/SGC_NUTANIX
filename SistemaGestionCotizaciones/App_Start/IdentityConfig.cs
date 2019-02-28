using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using SistemaGestionCotizaciones.Models;
using System.DirectoryServices.AccountManagement;
using System.Web.Configuration;

namespace SistemaGestionCotizaciones
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {


        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            //var manager = new ApplicationUserManager(new ApplicationUserStore(context.Get<ApplicationDbContext>()));
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(10);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        private log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {

            string[] LDAP = WebConfigurationManager.AppSettings["LDAP"].Split('|');
            int contCatch = 0;

            foreach (var ldap in LDAP)
            {
                try
                {

                    PrincipalContext context = new PrincipalContext(ContextType.Domain, ldap);
                    if (context.ValidateCredentials(userName, password))
                    {

                        /* VALIDAMOS QUE ESTE REGISTRADO EN EL SISTEMA */

                        var user = new ApplicationUser(userName);
                        if (!user.LoadProperties())
                            return SignInStatus.Failure;

                        await SignInAsync(user, isPersistent, shouldLockout);
                        return SignInStatus.Success;
                    }

                    //return SignInStatus.Failure;
                }
                catch (Exception ex)
                {
                    contCatch++;
                    logger.Info("[PasswordSignInAsync]: " + ex.Message);
                    if (contCatch == LDAP.Length)
                        return SignInStatus.RequiresVerification;
                }
            }

            return SignInStatus.Failure;

        }
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
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
}
