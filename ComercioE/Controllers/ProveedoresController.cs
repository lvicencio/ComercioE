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
    [Authorize(Roles = "Admin")]
    public class ProveedoresController : Controller
    {
        private ComercioEContext db = new ComercioEContext();

        // GET: Proveedores
        public ActionResult Index()
        {
            var proveedores = db.Proveedores.Include(p => p.Ciudad).Include(p => p.Provincia);
            return View(proveedores.ToList());
        }

        // GET: Proveedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // GET: Proveedores/Create
        public ActionResult Create()
        {
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(0), "CiudadId", "Nombre");
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre");
            return View();
        }

        // POST: Proveedores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProveedorId,UserName,Nombre,Apellido,Telefono,Direccion,Foto,ProvinciaId,CiudadId,FotoFile")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                db.Proveedores.Add(proveedor);
                try
                {
                    db.SaveChanges();

                    if (proveedor.FotoFile != null)
                    {
                        var folder = "~/Content/Proveedores";
                        var file = string.Format("{0}.jpg", proveedor.ProveedorId);
                        var respuesta = FilesHelper.UploadPhoto(proveedor.FotoFile, folder, file);
                        if (respuesta)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            proveedor.Foto = pic;

                            db.Entry(proveedor).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                    }
                    return RedirectToAction("Index"); ;
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, "Error Ingreso de Proveedor");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(proveedor.ProvinciaId), "CiudadId", "Nombre", proveedor.CiudadId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", proveedor.ProvinciaId);

            return View(proveedor);
        }

        // GET: Proveedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(proveedor.ProvinciaId), "CiudadId", "Nombre", proveedor.CiudadId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", proveedor.ProvinciaId);

            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProveedorId,UserName,Nombre,Apellido,Telefono,Direccion,Foto,ProvinciaId,CiudadId,FotoFile")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                if (proveedor.FotoFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Proveedores";
                    var file = string.Format("{0}.jpg", proveedor.ProveedorId);
                    var respuesta = FilesHelper.UploadPhoto(proveedor.FotoFile, folder, file);
                    if (respuesta)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        proveedor.Foto = pic;

                    }
                }

                db.Entry(proveedor).State = EntityState.Modified;
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
                        ModelState.AddModelError(string.Empty, "Error en la Edición de proveedor");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(proveedor.ProvinciaId), "CiudadId", "Nombre", proveedor.CiudadId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", proveedor.ProvinciaId);
            return View(proveedor);
        }

        // GET: Proveedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proveedor proveedor = db.Proveedores.Find(id);
            db.Proveedores.Remove(proveedor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
