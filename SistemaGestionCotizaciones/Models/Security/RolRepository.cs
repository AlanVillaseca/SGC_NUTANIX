using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGestionCotizaciones.Models.Security
{
    public class RolRelator
    {
        public SecRol current;
        public SecRol Map(SecRol rol, SecUsuario usuario)
        {
            // Terminating call.  Since we can return null from this function
            // we need to be ready for PetaPoco to callback later with null
            // parameters
            if (rol == null)
                return current;

            // Is this the same tag as the current one we're processing
            if (current != null && current.Id == rol.Id)
            {
                // Yes, just add this usuario to the current tag's collection of articles
                if (usuario != null) current.Usuarios.Add(usuario);

                // Return null to indicate we're not done with this tag yet
                return null;
            }

            // This is a different tag to the current one, or this is the 
            // first time through and we don't have an tag yet

            // Save the current tag
            var prev = current;

            // Setup the new current tag
            current = rol;
            if (usuario != null && usuario.Id != int.MinValue)
            {
                current.Usuarios.Add(usuario);
            }

            return prev;
        }
    }

    public class RolRepository : BaseRepository
    {
        public RolRepository(PetaPoco.Database database)
            : base(database)
        {
        }

        public SecRol RetrieveById(int rolId)
        {
            return _database.Fetch<SecRol, SecUsuario, SecRol>(
                new RolRelator().Map,
                "select * from SecRol " +
                "left outer join SecRolUsuario on SecRolUsuario.idsecrol = SecRol.id " +
                "left outer join SecUsuario on SecUsuario.id = SecRolUsuario.idsecusuario " +
                "where SecRol.id=@0 order by SecRol.nombre asc", rolId).SingleOrDefault<SecRol>();
        }

        public List<SecRol> RetrieveAll()
        {
            return _database.Fetch<SecRol, SecUsuario, SecRol>(
                new RolRelator().Map,  ///<---TODO not needed
                "select * from SecRol " +
                "left outer join SecRolUsuario on SecRolUsuario.idsecrol = SecRol.id " +
                "left outer join SecUsuario on SecUsuario.id = SecRolUsuario.idsecusuario " +
                "order by SecRol.nombre asc").ToList();
        }

        public List<SecRol> RetrieveAllForUsuario(int usuarioId)
        {
            return _database.Fetch<SecRol, SecUsuario, SecRol>(
                new RolRelator().Map, ///<---TODO not needed
                "select * from SecRol " +
                "left outer join SecRolUsuario on SecRolUsuario.idsecrol = SecRol.id " +
                "left outer join SecUsuario on SecUsuario.id = SecRolUsuario.idsecusuario " +
                "where SecRolUsuario.idsecusuario=@0 order by SecRol.nombre asc", usuarioId).ToList<SecRol>();
        }

        /// <summary>
        /// deletes all objects related to secRol table
        /// </summary>
        /// <param name="rolId"></param>
        /// <returns></returns>
        public bool Delete(int rolId)
        {
            using (var scope = _database.GetTransaction())
            {
                _database.Execute("delete from SecRolUsuario where idsecrol=@0", rolId);
                _database.Execute("delete from SecRolGrupoPerfil where idsecrol=@0", rolId);
                _database.Delete("SecRol", "id", null, rolId);

                scope.Complete();
            }

            return true;
        }

        public bool Update(SecRol rol)
        {
            _database.Update(rol, new string[] { "nombre", "descripcion", "activo" });

            return true;
        }

        public bool Insert(SecRol rol)
        {
            _database.Insert("SecRol", "id", true, rol);

            return true;
        }
    }
}