﻿using System;
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
    public class BodegasController : Controller
    {
        
        private ComercioEContext db = new ComercioEContext();

        // GET: Bodegas
        public ActionResult Index()
        {
            //usuario logeado
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            //var bodegas = db.Bodegas.Include(b => b.Ciudad).Include(b => b.Compañia).Include(b => b.Provincia);
            var bodegas = db.Bodegas.Where(b => b.CompaniaId == user.CompaniaId).Include(b => b.Ciudad).Include(b => b.Provincia);
            return View(bodegas.ToList());
        }

        // GET: Bodegas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bodega bodega = db.Bodegas.Find(id);
            if (bodega == null)
            {
                return HttpNotFound();
            }
            return View(bodega);
        }

        // GET: Bodegas/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(0), "CiudadId", "Nombre");
           // ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre");
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre");
            var bodega = new Bodega { CompaniaId = user.CompaniaId };
            return View(bodega);
        }

        // POST: Bodegas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BodegaId,CompaniaId,Nombre,Telefono,Direccion,ProvinciaId,CiudadId")] Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                db.Bodegas.Add(bodega);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null &&
                       ex.InnerException.InnerException != null &&
                       ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, "Error Ingreso de la bodega");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }

            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(bodega.ProvinciaId), "CiudadId", "Nombre", bodega.CiudadId);
            //ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", bodega.CompaniaId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", bodega.ProvinciaId);
            return View(bodega);
        }

        // GET: Bodegas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bodega bodega = db.Bodegas.Find(id);
            if (bodega == null)
            {
                return HttpNotFound();
            }
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(bodega.ProvinciaId), "CiudadId", "Nombre", bodega.CiudadId);
            //ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", bodega.CompaniaId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", bodega.ProvinciaId);
            return View(bodega);
        }

        // POST: Bodegas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BodegaId,CompaniaId,Nombre,Telefono,Direccion,ProvinciaId,CiudadId")] Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bodega).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(bodega.ProvinciaId), "CiudadId", "Nombre", bodega.CiudadId);
            //ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", bodega.CompaniaId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", bodega.ProvinciaId);
            return View(bodega);
        }

        // GET: Bodegas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bodega bodega = db.Bodegas.Find(id);
            if (bodega == null)
            {
                return HttpNotFound();
            }
            return View(bodega);
        }

        // POST: Bodegas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bodega bodega = db.Bodegas.Find(id);
            db.Bodegas.Remove(bodega);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //drop en cascada, recive el id de provincia y envia la lista de sus ciudades
        public JsonResult GetCiudades(int provinciatId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var ciudades = db.Ciudads.Where(m => m.ProvinciaId == provinciatId);
            return Json(ciudades);
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
