using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Security.Claims;

namespace ExtensionMethods
{
    public static class UserExtended
    {
        public static string GetDisplayName(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Name);
            return claim == null ? null : claim.Value;
        }
        public static string GetUserName(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.WindowsAccountName);
            return claim == null ? null : claim.Value;
        }
        public static string GetEmail(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Email);
            return claim == null ? null : claim.Value;
        }
        public static string GetRoles(this IPrincipal user)
        {
            var roles = ((ClaimsIdentity)user.Identity).FindAll(ClaimTypes.Role);
            if (roles == null || roles.Count() == 0) 
                return string.Empty;

            var lst = new StringBuilder();
            foreach (var rol in roles)
                lst.Append(rol.Value + ",");
            
            return lst.ToString().Substring(0,lst.Length - 1);
        }
        public static string GetPermisos(this IPrincipal user)
        {
            var permisos = ((ClaimsIdentity)user.Identity).FindAll(ClaimTypes.UserData);
            if (permisos == null || permisos.Count() == 0) 
                return string.Empty;

            var lst = new StringBuilder();
            foreach (var permiso in permisos)
                lst.Append(permiso.Value + ";");
            
            return lst.ToString().Substring(0, lst.Length - 1);
        }

        private const int Read = 1;
        private const int Write = 2;
        private const int Delete = 3;
        private const int Aprobar = 4;
        private const int Rechazar = 5;
        private const int nivelRWD = 1;
        private const int nivelAR = 2;

        public static bool HasReadAccess(this IPrincipal user, string objeto)
        {
            return HasAccess(user, objeto, Read, nivelRWD);
        }
        public static bool HasWriteAccess(this IPrincipal user, string objeto)
        {
            return HasAccess(user, objeto, Write, nivelRWD);
        }
        public static bool HasDeleteAccess(this IPrincipal user, string objeto)
        {
            return HasAccess(user, objeto, Delete, nivelRWD);
        }
        public static bool HasAprobarAccess(this IPrincipal user, string objeto)
        {
            return HasAccess(user, objeto, Aprobar, nivelAR);
        }
        public static bool HasRechazarAccess(this IPrincipal user, string objeto)
        {
            return HasAccess(user, objeto, Rechazar, nivelAR);
        }

        // accion: 1:Read, 2:Write, 3:Delete, 4:Aprobar, 5:Rechazar
        // nivelacceso: 1: para RWD, 2 para AR.
        private static bool HasAccess(IPrincipal user, string objeto, int accion, int nivelacceso)
        {
            var permisos = user.GetPermisos();
            if (string.IsNullOrEmpty(permisos)) return false;

            var permiso = permisos.Split(';').FirstOrDefault(item => item.StartsWith(objeto));
            if (string.IsNullOrEmpty(permiso)) return false;

            var pl = permiso.Split(',');
            return (int.Parse(pl[nivelacceso]) >= accion) ? true : false;
        }
    }
}
