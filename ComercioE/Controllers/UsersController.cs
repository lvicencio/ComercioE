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
    public class UsersController : Controller
    {
        private ComercioEContext db = new ComercioEContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Ciudad).Include(u => u.Compañia).Include(u => u.Provincia);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(), "CiudadId", "Nombre");
            ViewBag.CompaniaId = new SelectList(CombosHelper.GetCompanias(), "CompaniaId", "Nombre");
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre");
            return View();
        }

        // POST: Users/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,Nombre,Apellido,Telefono,Direccion,Foto,ProvinciaId,CiudadId,CompaniaId,FotoFile")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                try
                {
                    db.SaveChanges();

                    if (user.FotoFile != null)
                    {
                        var folder = "~/Content/Users";
                        var file = string.Format("{0}.jpg", user.UserId);
                        var respuesta = FilesHelper.UploadPhoto(user.FotoFile, folder, file);
                        if (respuesta)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            user.Foto = pic;

                            db.Entry(user).State = EntityState.Modified;
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
                        ModelState.AddModelError(string.Empty, "Error Ingreso de Usuario");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }

            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(), "CiudadId", "Nombre", user.CiudadId);
            ViewBag.CompaniaId = new SelectList(CombosHelper.GetCompanias(), "CompaniaId", "Nombre", user.CompaniaId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", user.ProvinciaId);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(), "CiudadId", "Nombre", user.CiudadId);
            ViewBag.CompaniaId = new SelectList(CombosHelper.GetCompanias(), "CompaniaId", "Nombre", user.CompaniaId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", user.ProvinciaId);
            return View(user);
        }

        // POST: Users/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,Nombre,Apellido,Telefono,Direccion,Foto,ProvinciaId,CiudadId,CompaniaId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(), "CiudadId", "Nombre", user.CiudadId);
            ViewBag.CompaniaId = new SelectList(CombosHelper.GetCompanias(), "CompaniaId", "Nombre", user.CompaniaId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", user.ProvinciaId);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
