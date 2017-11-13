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
            //filtrar usuario por compañia
            var query = (from cl in db.Clientes
                         join cc in db.CompaniaClientes on cl.ClienteId equals cc.ClienteId
                         join co in db.Companias on cc.CompaniaId equals co.CompaniaId
                         where co.CompaniaId == user.CompaniaId
                         select new { cl }).ToList();


            var clientes = new List<Cliente>();
            foreach (var item in query)
            {
                clientes.Add(item.cl);
            }



           // var clientes = db.Clientes.Where(c => c.CompaniaId == user.CompaniaId).Include(c => c.Ciudad).Include(c => c.Provincia);
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
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(0), "CiudadId", "Nombre");
            //ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre");
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre");
            //var cliente = new Cliente { CompaniaId = user.CompaniaId,};
            return View();
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
                //se realiza una transaccion, para grabar el cliente, y su CompaniaCliente
                using (var transaccion = db.Database.BeginTransaction())
                {
                    db.Clientes.Add(cliente);
                    try
                    {
                        db.SaveChanges();
                        UsersHelper.CreateUserASP(cliente.UserName, "Cliente");

                        //busqueda de usuario 
                        var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                        var companiaUser = new CompaniaCliente
                        {
                            CompaniaId = user.CompaniaId,
                            ClienteId = cliente.ClienteId,
                        };

                        db.CompaniaClientes.Add(companiaUser);
                        db.SaveChanges();

                        transaccion.Commit();

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                                              
                            transaccion.Rollback();
                            ModelState.AddModelError(string.Empty, ex.Message);
                       
                    } 
                }
                //fin using(transaccion)
            }

            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(cliente.ProvinciaId), "CiudadId", "Nombre", cliente.CiudadId);
          //  ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", cliente.CompaniaId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", cliente.ProvinciaId);
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
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(cliente.ProvinciaId), "CiudadId", "Nombre", cliente.CiudadId);
            //  ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", cliente.CompaniaId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", cliente.ProvinciaId);
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
            ViewBag.CiudadId = new SelectList(CombosHelper.GetCiudades(cliente.ProvinciaId), "CiudadId", "Nombre", cliente.CiudadId);
            //  ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", cliente.CompaniaId);
            ViewBag.ProvinciaId = new SelectList(CombosHelper.GetProvincias(), "ProvinciaId", "Nombre", cliente.ProvinciaId);
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
            //busqueda de usuario 
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var companiaCliente = db.CompaniaClientes.Where(cc => cc.CompaniaId == user.CompaniaId && cc.ClienteId == cliente.ClienteId).FirstOrDefault();

            using (var transaccion = db.Database.BeginTransaction()) 
            {
                try
                {
                    db.CompaniaClientes.Remove(companiaCliente);
                    db.Clientes.Remove(cliente);
                    db.SaveChanges();
                    transaccion.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    transaccion.Rollback();
                    return View(cliente);
                }
                                 
                //return RedirectToAction("Index");
            }
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
