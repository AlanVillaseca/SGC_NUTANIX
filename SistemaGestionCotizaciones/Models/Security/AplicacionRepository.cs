using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGestionCotizaciones.Models.Security
{
    public class AplicacionRepository : BaseRepository
    {
        public AplicacionRepository(PetaPoco.Database database)
            : base(database)
        {
        }

        public SecAplicacion RetrieveById(int id)
        {
            return _database.Fetch<SecAplicacion>(
                "select * from SecAplicacion " +
                "where id = @0", id).SingleOrDefault<SecAplicacion>();
        }
        public List<SecAplicacion> RetrieveAll()
        {
            return _database.Fetch<SecAplicacion>(
                "select * from SecAplicacion " +
                "where activo = 1 order by nombre asc").ToList();
        }
        public bool Delete(int id)
        {
            using (var scope = _database.GetTransaction())
            {
                _database.Delete("SecAplicacion", "id", null, id);

                scope.Complete();
            }

            return true;
        }

        public bool Update(SecAplicacion aplicacion)
        {
            using (var scope = _database.GetTransaction())
            {
                _database.Update(aplicacion);

                scope.Complete();
            }

            return true;
        }

        public bool Insert(SecAplicacion aplicacion)
        {
            using (var scope = _database.GetTransaction())
            {
                _database.Insert("SecAplicacion", "id", true, aplicacion);

                scope.Complete();
            }

            return true;
        }
    }
}