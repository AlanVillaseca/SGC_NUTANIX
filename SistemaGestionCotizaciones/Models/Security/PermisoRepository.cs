using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGestionCotizaciones.Models.Security
{
    public class PermisoRepository : BaseRepository
    {
        public PermisoRepository(PetaPoco.Database database)
            : base(database)
        {
        }

        public List<SecRolPermiso> RetrieveByRolId(int id)
        {
            return _database.Fetch<SecRolPermiso>(
                "select * from SecRolPermiso " +
                "where idsecrol = @0 order by nombreaplicacion asc", id).ToList();
        }
        public List<SecAccion> RetrieveAccionByGrupo(int grupo)
        {
            return _database.Fetch<SecAccion>(
                "select * from SecAccion " +
                "where grupo = @0 order by prioridad asc", grupo).ToList();
        }

        public bool Delete(int id)
        {
            using (var scope = _database.GetTransaction())
            {
                _database.Delete("SecRolGrupoPerfil", "id", null, id);

                scope.Complete();
            }

            return true;
        }

        public bool Update(SecPermiso permiso)
        {
            using (var scope = _database.GetTransaction())
            {
                _database.Update(permiso);

                scope.Complete();
            }

            return true;
        }
        public bool UpdateRolPermiso(SecRol rol)
        {
            using (var scope = _database.GetTransaction())
            {
                _database.Update(rol, new string[] { "nombre", "descripcion", "activo" });
                _database.Execute("delete from SecRolGrupoPerfil where idsecrol=@0", rol.Id);
                foreach (var permiso in rol.Permisos)
                {
                    _database.Insert("SecRolGrupoPerfil", "id", true,
                        new SecPermiso()
                        {
                            Idsecrol = permiso.Idsecrol,
                            Idsecaplicacion = permiso.Idsecaplicacion,
                            Idsecaccion = permiso.Idsecaccion,
                            Idsecaccion2 = permiso.Idsecaccion2
                        }
                        );
                }
                scope.Complete();
            }

            return true;
        }

        public bool Insert(SecPermiso permiso)
        {
            using (var scope = _database.GetTransaction())
            {
                _database.Insert("SecRolGrupoPerfil", "id", true, permiso);

                scope.Complete();
            }

            return true;
        }
    }
}