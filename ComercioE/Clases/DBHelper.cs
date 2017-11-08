using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComercioE.Models;

namespace ComercioE.Clases
{
    public class DBHelper
    {
        public static Respuesta GuardarCambios (ComercioEContext db)
        {
            try
            {
                db.SaveChanges();
                return new Respuesta { Exito = true, };
            }
            catch (Exception ex)
            {

                var respuesta = new Respuesta { Exito = false, };
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    respuesta.Mensage = "Se ha grabado con exito";
                }
                else if(ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    respuesta.Mensage = "No se pudo eliminar";
                }
                else
                {
                    respuesta.Mensage = ex.Message;
                }
                return respuesta;
            }
        }

        public static int GetEstado(string descripcion, ComercioEContext db)
        {
            ////busca el codigo del estado, si no existe, lo creara en la db
            var estado = db.Estadoes.Where(e => e.Descripcion == descripcion).FirstOrDefault();
            if (estado == null)
            {
                estado = new Estado
                {
                    Descripcion = descripcion,
                };
                db.Estadoes.Add(estado);
                db.SaveChanges();
            }
            return estado.EstadoId;
        }
    }
}