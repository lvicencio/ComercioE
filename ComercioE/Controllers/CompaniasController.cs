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
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(), "CiudadId", "Nombre");
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre");
            return View();
        }

        // POST: Companias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompaniaId,Nombre,Telefono,Direccion,Logo,ProvinciaId,CiudadId,LogoFile")] Compania compania)
        {
            if (ModelState.IsValid)
            {
               
                db.Companias.Add(compania);
                try
                {
                    db.SaveChanges();

                    if (compania.LogoFile != null)
                    {
                        var folder = "~/Content/Logos";
                        var file = string.Format("{0}.jpg", compania.CompaniaId);
                        var respuesta = FilesHelper.UploadPhoto(compania.LogoFile, folder, file);
                        if (respuesta)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            compania.Logo = pic;

                            db.Entry(compania).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        
                    }

                    
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null &&
                       ex.InnerException.InnerException != null &&
                       ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, "Error de Ingreso");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }

            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(), "CiudadId", "Nombre", compania.CiudadId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", compania.ProvinciaId);
            return View(compania);
        }

        // GET: Companias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var compania = db.Companias.Find(id);
            if (compania == null)
            {
                return HttpNotFound();
            }
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(), "CiudadId", "Nombre", compania.CiudadId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", compania.ProvinciaId);
            return View(compania);
        }

        // POST: Companias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompaniaId,Nombre,Telefono,Direccion,Logo,ProvinciaId,CiudadId,LogoFile")] Compania compania)
        {
            if (ModelState.IsValid)
            {
                if (compania.LogoFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Logos";
                    var file = string.Format("{0}.jpg", compania.CompaniaId);
                    var respuesta = FilesHelper.UploadPhoto(compania.LogoFile, folder, file);
                    if (respuesta)
                    {
                         pic = string.Format("{0}/{1}", folder, file);
                        compania.Logo = pic;

                    }
                }
                
                db.Entry(compania).State = EntityState.Modified;
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
                        ModelState.AddModelError(string.Empty, "Error en la Edición");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(), "CiudadId", "Nombre", compania.CiudadId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", compania.ProvinciaId);
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
