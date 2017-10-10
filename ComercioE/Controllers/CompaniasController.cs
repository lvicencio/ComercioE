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
    public class CompaniasController : Controller
    {
        private ComercioEContext db = new ComercioEContext();

        // GET: Companias
        public ActionResult Index()
        {
            var companias = db.Companias.Include(c => c.Ciudad).Include(c => c.Provincia);
            return View(companias.ToList());
        }

        // GET: Companias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compania compania = db.Companias.Find(id);
            if (compania == null)
            {
                return HttpNotFound();
            }
            return View(compania);
        }

        // GET: Companias/Create
        public ActionResult Create()
        {
            ViewBag.CiudadId = new SelectList(db.Ciudads, "CiudadId", "Nombre");
            ViewBag.ProvinciaId = new SelectList(db.Provincias, "ProvinciaId", "Nombre");
            return View();
        }

        // POST: Companias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompaniaId,Nombre,Telefono,Direccion,Logo,ProvinciaId,CiudadId")] Compania compania)
        {
            if (ModelState.IsValid)
            {
                db.Companias.Add(compania);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CiudadId = new SelectList(db.Ciudads, "CiudadId", "Nombre", compania.CiudadId);
            ViewBag.ProvinciaId = new SelectList(db.Provincias, "ProvinciaId", "Nombre", compania.ProvinciaId);
            return View(compania);
        }

        // GET: Companias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compania compania = db.Companias.Find(id);
            if (compania == null)
            {
                return HttpNotFound();
            }
            ViewBag.CiudadId = new SelectList(db.Ciudads, "CiudadId", "Nombre", compania.CiudadId);
            ViewBag.ProvinciaId = new SelectList(db.Provincias, "ProvinciaId", "Nombre", compania.ProvinciaId);
            return View(compania);
        }

        // POST: Companias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompaniaId,Nombre,Telefono,Direccion,Logo,ProvinciaId,CiudadId")] Compania compania)
        {
            if (ModelState.IsValid)
            {
                db.Entry(compania).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CiudadId = new SelectList(db.Ciudads, "CiudadId", "Nombre", compania.CiudadId);
            ViewBag.ProvinciaId = new SelectList(db.Provincias, "ProvinciaId", "Nombre", compania.ProvinciaId);
            return View(compania);
        }

        // GET: Companias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compania compania = db.Companias.Find(id);
            if (compania == null)
            {
                return HttpNotFound();
            }
            return View(compania);
        }

        // POST: Companias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Compania compania = db.Companias.Find(id);
            db.Companias.Remove(compania);
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
