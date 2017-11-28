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
    public class ComprasController : Controller
    {
        private ComercioEContext db = new ComercioEContext();

        // GET: Compras
        public ActionResult Index()
        {
            var compras = db.Compras.Include(c => c.Bodega).Include(c => c.Estado);
            return View(compras.ToList());
        }

        // GET: Compras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compra compra = db.Compras.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            return View(compra);
        }

        // GET: Compras/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.ClienteId = new SelectList(CombosHelper.GetClientes(user.CompaniaId), "ClienteId", "FullName");

            var vistaCompra = new NuevaCompraVista
            {
                Date = DateTime.Now,
                Detalles = db.CompraDetalleTmps.Where(d => d.UserName == User.Identity.Name).ToList(),
                
            };


            ViewBag.BodegaId = new SelectList(CombosHelper.GetBodegas(), "BodegaId", "Nombre");
            // ViewBag.EstadoId = new SelectList(db.Estadoes, "EstadoId", "Descripcion");
            return View(vistaCompra);
        }

        // POST: Compras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NuevaCompraVista vistaCompra)
        {
            if (ModelState.IsValid)
            {
                var respuesta = MovimientosHelper.CrearCompra(vistaCompra, User.Identity.Name);
                if (respuesta.Exito)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, respuesta.Mensage);

                return RedirectToAction("Index");
            }

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.ClienteId = new SelectList(CombosHelper.GetClientes(user.CompaniaId), "ClienteId", "FullName");
            vistaCompra.Detalles = db.CompraDetalleTmps.Where(o => o.UserName == User.Identity.Name).ToList();

            //ViewBag.BodegaId = new SelectList(db.Bodegas, "BodegaId", "Nombre", vistaCompra.BodegaId);
            //  ViewBag.EstadoId = new SelectList(db.Estadoes, "EstadoId", "Descripcion", vistaCompra.EstadoId);
            return View(vistaCompra);
        }

        // GET: Compras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compra compra = db.Compras.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            ViewBag.BodegaId = new SelectList(db.Bodegas, "BodegaId", "Nombre", compra.BodegaId);
            ViewBag.EstadoId = new SelectList(db.Estadoes, "EstadoId", "Descripcion", compra.EstadoId);
            return View(compra);
        }

        // POST: Compras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompraId,CompaniaId,EstadoId,Date,Comentarios,BodegaId")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                db.Entry(compra).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BodegaId = new SelectList(db.Bodegas, "BodegaId", "Nombre", compra.BodegaId);
            ViewBag.EstadoId = new SelectList(db.Estadoes, "EstadoId", "Descripcion", compra.EstadoId);
            return View(compra);
        }

        // GET: Compras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compra compra = db.Compras.Find(id);
            if (compra == null)
            {
                return HttpNotFound();
            }
            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Compra compra = db.Compras.Find(id);
            db.Compras.Remove(compra);
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
