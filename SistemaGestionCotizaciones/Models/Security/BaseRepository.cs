﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGestionCotizaciones.Models.Security
{
    public class BaseRepository
    {
        protected PetaPoco.Database _database;

        public BaseRepository(PetaPoco.Database database)
        {
            _database = database;
        }
    }
}