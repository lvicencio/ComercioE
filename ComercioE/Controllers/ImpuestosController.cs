using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComercioE.Models;

namespace ComercioE.Controllers
{
    [Authorize(Roles ="User")]
    public class ImpuestosController : Controller
    {
        private ComercioEContext db = new ComercioEContext();

        // GET: Impuestos
        public ActionResult Index()
        {
            // valida si el usuario existe, si no existe se redirecciona al indice
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // var impuestoes = db.Impuestoes.Include(i => i.Compania);

            //muestras los impuestos del usuario de su compañia
            var impuestoes = db.Impuestoes.Where( i => i.CompaniaId == user.CompaniaId);
            return View(impuestoes.ToList());
        }

        // GET: Impuestos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Impuesto impuesto = db.Impuestoes.Find(id);
            if (impuesto == null)
            {
                return HttpNotFound();
            }
            return View(impuesto);
        }

        // GET: Impuestos/Create
        public ActionResult Create()
        {
            // valida si el usuario existe, si no existe se redirecciona al indice
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre");
            var impuesto = new Impuesto
                            {
                                CompaniaId = user.CompaniaId,
                            };
            return View(impuesto);
        }

        // POST: Impuestos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ImpuestoId,Descripcion,Valor,CompaniaId")] Impuesto impuesto)
        {
            if (ModelState.IsValid)
            {
                db.Impuestoes.Add(impuesto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

           // ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", impuesto.CompaniaId);
            return View(impuesto);
        }

        // GET: Impuestos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Impuesto impuesto = db.Impuestoes.Find(id);
            if (impuesto == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", impuesto.CompaniaId);
            return View(impuesto);
        }

        // POST: Impuestos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ImpuestoId,Descripcion,Valor,CompaniaId")] Impuesto impuesto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(impuesto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", impuesto.CompaniaId);
            return View(impuesto);
        }

        // GET: Impuestos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Impuesto impuesto = db.Impuestoes.Find(id);
            if (impuesto == null)
            {
                return HttpNotFound();
            }
            return View(impuesto);
        }

        // POST: Impuestos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Impuesto impuesto = db.Impuestoes.Find(id);
            db.Impuestoes.Remove(impuesto);
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
