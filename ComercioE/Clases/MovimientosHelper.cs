using ComercioE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ComercioE.Clases
{
    
    public class MovimientosHelper : IDisposable
    {
        private static ComercioEContext db = new ComercioEContext();


        public static bool RestarInventario(CompraDetalle compraDetalle, int bodega)
        {

            //InventarioId
            //BodegaId
            //ProductoId
            //Stock
            //fin modificacion

            var inventario = db.Inventarios.Where(i => i.BodegaId == bodega && i.ProductoId == compraDetalle.ProductoId).FirstOrDefault();


            if (inventario != null)
            {

                inventario.Stock = inventario.Stock += compraDetalle.Cantidad;
            }

            db.Entry(inventario).State = EntityState.Modified;
            db.SaveChanges();

            return true;

        }

        public static bool ActualizaInventario(CompraDetalle compraDetalle, int bodega)
        {

            //InventarioId
            //BodegaId
            //ProductoId
            //Stock
            //fin modificacion

            var inventario = db.Inventarios.Where(i => i.BodegaId == bodega &&  i.ProductoId == compraDetalle.ProductoId).FirstOrDefault();


            if (inventario != null)
            {
                        
                inventario.Stock = inventario.Stock += compraDetalle.Cantidad;
            }

            db.Entry(inventario).State = EntityState.Modified;
            db.SaveChanges();

            return true;

        }


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

        public static Respuesta CrearCompra(NuevaCompraVista vistaCompra, string name)
        {
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == name).FirstOrDefault();
                    var compra = new Compra
                    {
                        CompaniaId = user.CompaniaId,
                        
                        Date = vistaCompra.Date,
                       
                        Comentarios = vistaCompra.Comentarios,
                        BodegaId    = vistaCompra.BodegaId,
                        EstadoId = DBHelper.GetEstado("Comprado", db),
                    };

                    db.Compras.Add(compra);
                    db.SaveChanges();

                    var bodega = compra.BodegaId;

                    var detalles = db.CompraDetalleTmps.Where(or => or.UserName == name).ToList();

                    foreach (var item in detalles)
                    {
                        var compraDetalle = new CompraDetalle
                        {
                            Descripcion = item.Descripcion,
                            CompraId = compra.CompraId,
                            Precio = item.Precio,
                            ProductoId = item.ProductoId,
                            Cantidad = item.Cantidad,
                            Impuesto = item.Impuesto,
                        };

                        db.CompraDetalles.Add(compraDetalle);
                        db.CompraDetalleTmps.Remove(item);

                       

                        ActualizaInventario(compraDetalle, bodega);
                       

                    }
                    db.SaveChanges();
                    //confirmar la transaccion
                    

                    transaccion.Commit();
                    return new Respuesta { Exito = true, };
                }
                catch (Exception ex)
                {

                    transaccion.Rollback();
                    return new Respuesta
                    {
                        Mensage = ex.Message,
                        Exito = false,
                    };
                }
            }
        }


       

    }
}