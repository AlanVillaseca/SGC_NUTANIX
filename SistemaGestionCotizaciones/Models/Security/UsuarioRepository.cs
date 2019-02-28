using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGestionCotizaciones.Models.Security
{
    public class UsuarioRelator
    {
        public SecUsuario current;
        public SecUsuario Map(SecUsuario usuario, SecRol rol)
        {
            // Terminating call.  Since we can return null from this function
            // we need to be ready for PetaPoco to callback later with null
            // parameters
            if (usuario == null)
                return current;

            // Is this the same usuario as the current one we're processing
            if (current != null && current.Id == usuario.Id)
            {
                // Yes, just add this tag to the current usuario's collection of tags
                current.Roles.Add(rol);

                // Return null to indicate we're not done with this usuario yet
                return null;
            }

            // This is a different author to the current one, or this is the 
            // first time through and we don't have an author yet

            // Save the current author
            var prev = current;

            // Setup the new current author
            current = usuario;
            if (rol.Id != 0) current.Roles.Add(rol);
            //current.Tags = new List<Tag>();
            //current.posts.Add(p);

            // Return the now populated previous author (or null if first time through)
            return prev;
        }
    }
    public class UsuarioRepository : BaseRepository
    {
        private RolRepository _rolRepository { get; set; }
        public UsuarioRepository(RolRepository rolRepository, PetaPoco.Database database)
            : base(database)
        {
            _rolRepository = rolRepository;
        }

        public SecUsuario RetrieveById(int usuarioId)
        {
            return _database.Fetch<SecUsuario, SecRol, SecUsuario>(
                new UsuarioRelator().Map,
                "select * from SecUsuario " +
                "left outer join SecRolUsuario on SecRolUsuario.idsecusuario = SecUsuario.id " +
                "left outer join SecRol on SecRol.id = SecRolUsuario.idsecrol " +
                "where SecUsuario.id=@0 ", usuarioId).Single();
        }
        public SecUsuario RetrieveByUsername(string username)
        {
            return _database.Fetch<SecUsuario>(
                "select * from SecUsuario " +
                "where SecUsuario.username=@0 ", username).SingleOrDefault();
        }

        public List<SecUsuario> RetrieveAll()
        {
            var userlist = _database.Fetch<SecUsuario, SecRol, SecUsuario>(
                new UsuarioRelator().Map,
                "SELECT" +
                "    *," +
                "    (SELECT SUBSTRING(" +
                "        (SELECT" +
                "            REPLACE((" +
                "            SELECT" +
                "                p.Nombre + ' - ' + n.Descripcion + ', '" +
                "            FROM Negocio_Pais_SecUsuario nps" +
                "            INNER JOIN Pais p ON p.Idpais = nps.Idpais" +
                "            INNER JOIN Negocio n ON n.Idnegocio = nps.Idnegocio" +
                "            WHERE nps.Idsecusuario = su.id" +
                "            FOR XML PATH('')), '', ''))," +
                "            1," +
                "            (LEN(" +
                "            (SELECT" +
                "                REPLACE((" +
                "                SELECT" +
                "	                p.Nombre + ' - ' + n.Descripcion + ', '" +
                "                FROM Negocio_Pais_SecUsuario nps" +
                "                INNER JOIN Pais p ON p.Idpais = nps.Idpais" +
                "                INNER JOIN Negocio n ON n.Idnegocio = nps.Idnegocio" +
                "                WHERE nps.Idsecusuario = su.id" +
                "                FOR XML PATH('')), '', ''))" +
                "    ) - 1))) AS 'Paisnegocio'" +
                "FROM SecUsuario su" +
                "LEFT OUTER JOIN SecRolUsuario sru ON sru.idsecusuario = su.id" +
                "LEFT OUTER JOIN SecRol sr ON sr.id = sru.idsecrol" +
                "ORDER BY su.username ASC").ToList();

            var paisneg = GetNegocioPaisAll();

            //foreach(var user in userlist)
            //{
            //    var item = paisneg.Select(e => e).Where(e => e.Idpais == user.Idpais && e.Idnegocio == user.Idnegocio).SingleOrDefault();
            //    if (item == null) 
            //    {
            //        user.Paisnegocio = "N/A";
            //    }
            //    else
            //    {
            //    user.Paisnegocio = item.Nombre;
            //    }
            //}

            return userlist;

        }

        public List<vwCentroCosto> GetCentroCostoAll()
        {
            return _database.Fetch<vwCentroCosto>(
                    "select * from centrocosto where habilitado = 1 order by gerencia asc"
                ).ToList();
        }

        public List<vwNegocioPais> GetNegocioPaisAll()
        {
            return _database.Fetch<vwNegocioPais>(
                "select * from vwNegocioPais " +
                "order by nombre asc").ToList();
        }

        public bool Delete(int usuarioId)
        {
            using (var scope = _database.GetTransaction())
            {
                _database.Execute("delete from SecRolUsuario where idsecusuario=@0", usuarioId);
                _database.Delete("SecUsuario", "id", null, usuarioId);

                scope.Complete();
            }

            return true;
        }

        public bool UpdateLogin(SecUsuario usuario)
        {
            using (var scope = _database.GetTransaction())
            {
                _database.Update(usuario, new string[] { "lastlogin" });

                scope.Complete();
            }

            return true;
        }

        public bool Update(SecUsuario usuario)
        {
            using (var scope = _database.GetTransaction())
            {
                _database.Update(usuario, new string[] {"idnegocio", "idpais", "activo"});
                _database.Execute("delete from SecRolUsuario where idsecusuario=@0", usuario.Id);
                PersistRoles(usuario);

                scope.Complete();
            }

            return true;
        }

        public bool Insert(SecUsuario usuario)
        {
            using (var scope = _database.GetTransaction())
            {
                _database.Insert("SecUsuario", "id", true, usuario);
                PersistRoles(usuario);

                scope.Complete();
            }

            return true;
        }

        private void PersistRoles(SecUsuario usuario)
        {
            if (usuario.Roles != null && usuario.Roles.Count > 0)
            {
                foreach (var rol in usuario.Roles)
                {
                    if (!string.IsNullOrEmpty(rol.Codigo) && rol.Id != 0)
                    {
                        _database.Execute("insert into SecRolUsuario (idsecusuario, idsecrol) values (@0, @1)", usuario.Id, rol.Id);
                    }
                    else
                        throw new ApplicationException(string.Format("Usuario {0} con roles invalidos", usuario.Username));
                }
            }
        }
    }
}