using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComercioE.Models;
using ComercioE.Clases;

namespace ComercioE.Controllers
{
    [Authorize(Roles = "User")]
    public class OrdersController : Controller
    {
         private ComercioEContext db = new ComercioEContext();

        //public object Date { get; private set; }


        public  ActionResult DeleteProducto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orderDetalleTmp = db.OrderDetalleTmps.Where(tp => tp.UserName == User.Identity.Name && tp.ProductoId == id).FirstOrDefault();

            if (orderDetalleTmp == null)
            {
                return HttpNotFound();
            }
            db.OrderDetalleTmps.Remove(orderDetalleTmp);
            db.SaveChanges();
            return RedirectToAction("Create");
        }
        public ActionResult AddProducto()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.ProductoId = new SelectList(CombosHelper.GetProductos(user.CompaniaId), "ProductoId", "Descripcion");
            return View();
        }

        [HttpPost]
        public ActionResult AddProducto(AddProductoVista vista)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var orderDetalleTmp = db.OrderDetalleTmps.Where(tp => tp.UserName == User.Identity.Name && tp.ProductoId == vista.ProductoId).FirstOrDefault();

                if (orderDetalleTmp == null)
                {
                    var producto = db.Productoes.Find(vista.ProductoId);

                    orderDetalleTmp = new OrderDetalleTmp
                    {
                        Descripcion = producto.Descripcion,
                        Precio = producto.Precio,
                        ProductoId = producto.ProductoId,
                        Cantidad = vista.Cantidad,
                        Impuesto = producto.Impuesto.Valor,
                        UserName = User.Identity.Name,

                    };
                    db.OrderDetalleTmps.Add(orderDetalleTmp);
                }
                else
                {
                    orderDetalleTmp.Cantidad += vista.Cantidad;
                    db.Entry(orderDetalleTmp).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            
            ViewBag.ProductoId = new SelectList(CombosHelper.GetProductos(user.CompaniaId), "ProductoId", "Descripcion");

            return View(vista);
        }


        // GET: Orders
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var orders = db.Orders.Where(o => o.CompaniaId == user.CompaniaId).Include(o => o.Cliente).Include(o => o.Estado);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.ClienteId = new SelectList(CombosHelper.GetClientes(user.CompaniaId), "ClienteId", "FullName");
            //ViewBag.EstadoId = new SelectList(db.Estadoes, "EstadoId", "Descripcion");

            var vista = new NuevaOrdenVista
            {
                Date = DateTime.Now,
                Detalles = db.OrderDetalleTmps.Where(o => o.UserName == User.Identity.Name).ToList(),
            };

            return View(vista); //se manda el objeto vista, el cual debe ser recibido en la vista de create
            //la cual se debe modificar para recibirla (en la partde arriba cambiar el Order por NuevaOrdenVista
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken] 
        //se cambio el modelo order, por el NuevaOrdenVista, que es el que envia lavista
        public ActionResult Create(NuevaOrdenVista vista)
        {
            if (ModelState.IsValid)
            {
                //crear un modelo, para verificar una respuesta de la accion
                var respuesta = MovimientosHelper.CrearOrder(vista, User.Identity.Name);
                if (respuesta.Exito)
                {
                    return RedirectToAction("Index");
                }
                //db.Orders.Add(order);
                //db.SaveChanges();
                ModelState.AddModelError(string.Empty, respuesta.Mensage);

                return RedirectToAction("Index");
            }

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.ClienteId = new SelectList(CombosHelper.GetClientes(user.CompaniaId), "ClienteId", "FullName");
            return View(vista);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "ClienteId", "UserName", order.ClienteId);
            ViewBag.EstadoId = new SelectList(db.Estadoes, "EstadoId", "Descripcion", order.EstadoId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompaniaId,OrderId,ClienteId,EstadoId,Date,Comentarios")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(db.Clientes, "ClienteId", "UserName", order.ClienteId);
            ViewBag.EstadoId = new SelectList(db.Estadoes, "EstadoId", "Descripcion", order.EstadoId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
