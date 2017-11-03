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
    [Authorize(Roles ="User")]
    public class ClientesController : Controller
    {
        private ComercioEContext db = new ComercioEContext();

        // GET: Clientes
        public ActionResult Index()
        {
            //busqueda de usuario logeado
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var clientes = db.Clientes.Where(c => c.CompaniaId == user.CompaniaId).Include(c => c.Ciudad).Include(c => c.Provincia);
            return View(clientes.ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(), "CiudadId", "Nombre");
            //ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre");
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre");
            var cliente = new Cliente { CompaniaId = user.CompaniaId,};
            return View(cliente);
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClienteId,CompaniaId,UserName,Nombre,Apellido,Phone,Direccion,ProvinciaId,CiudadId")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Clientes.Add(cliente);
                try
                {
                    db.SaveChanges();
                    UsersHelper.CreateUserASP(cliente.UserName, "Cliente");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, "Error Ingreso de Cliente");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }

            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(), "CiudadId", "Nombre", cliente.CiudadId);
          //  ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", cliente.CompaniaId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetCiudades(), "ProvinciaId", "Nombre", cliente.ProvinciaId);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(), "CiudadId", "Nombre", cliente.CiudadId);
            //  ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", cliente.CompaniaId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetCiudades(), "ProvinciaId", "Nombre", cliente.ProvinciaId);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClienteId,CompaniaId,UserName,Nombre,Apellido,Phone,Direccion,ProvinciaId,CiudadId")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                //validar un cambio de correo (copy/paste) desde el usercontroller
                return RedirectToAction("Index");
            }
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(), "CiudadId", "Nombre", cliente.CiudadId);
            //  ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", cliente.CompaniaId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetCiudades(), "ProvinciaId", "Nombre", cliente.ProvinciaId);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            db.Clientes.Remove(cliente);
            db.SaveChanges();
            UsersHelper.DeleteUser(cliente.UserName);
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
