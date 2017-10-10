using ComercioE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComercioE.Clases
{
    public class CombosHelper : IDisposable
    {
        private static ComercioEContext db = new ComercioEContext();
        public static List<Provincia> GetProvincias()
        {
            var provincias = db.Provincias.ToList();
            provincias.Add(new Provincia
            {
                ProvinciaId = 0,
                Nombre = "[Seleccione una Provincia]",
            });
            return provincias.OrderBy(p => p.Nombre).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}