using ComercioE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComercioE.Clases
{
    
    public class MovimientosHelper : IDisposable
    {
        private static ComercioEContext db = new ComercioEContext();





        public void Dispose()
        {
            db.Dispose();
        }

        //se realiza un Transac, en donde se involucran 3 modificaciones en tablas diferentes
        public static Respuesta CrearOrder(NuevaOrdenVista vista, string userName)
        {

            using (var transacction = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
                    var order = new Order
                    {
                        CompaniaId = user.CompaniaId,
                        ClienteId = vista.ClienteId,
                        Date = vista.Date,
                        Comentarios = vista.Comentarios,
                        //manda el codigo del estado, si no existe, lo creara en la db
                        EstadoId = DBHelper.GetEstado("Creado", db),
                    };

                    db.Orders.Add(order);
                    db.SaveChanges();

                    var detalles = db.OrderDetalleTmps.Where(or => or.UserName == userName).ToList();

                    foreach (var item in detalles)
                    {
                        var orderDetalle = new OrderDetalle
                        {
                            Descripcion = item.Descripcion,
                            OrderId = order.OrderId,
                            Precio = item.Precio,
                            ProductoId = item.ProductoId,
                            Cantidad = item.Cantidad,
                            Impuesto = item.Impuesto,
                        };

                        db.OrderDetalles.Add(orderDetalle);
                        db.OrderDetalleTmps.Remove(item);

                    }
                    db.SaveChanges();
                    //confirmar la transaccion
                    transacction.Commit();
                    return new Respuesta { Exito = true, };
                }
                catch (Exception ex)
                {
                    transacction.Rollback();
                    return new Respuesta {
                        Mensage = ex.Message,
                        Exito = false,
                    };
                    
                }

            }
        }
    }
}