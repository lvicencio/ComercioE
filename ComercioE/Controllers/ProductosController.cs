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
    public class ProductosController : Controller
    {
        private ComercioEContext db = new ComercioEContext();

        // GET: Productos
        public ActionResult Index(string Buscar)
        {
            var user = db.Users.Where( u =>u.UserName == User.Identity.Name).FirstOrDefault();

            var productoes = db.Productoes.Include(p => p.Categoria).Include(p => p.Impuesto);

            if (Buscar != null)
            {
                productoes = productoes.Where(p => p.CompaniaId == user.CompaniaId && p.Descripcion.Contains(Buscar));

                return View(productoes.ToList());
            }
            else
                //var productoes = db.Productoes.Include(p => p.Categoria).Include(p => p.Compania).Include(p => p.Impuesto);
                productoes = productoes.Where(p => p.CompaniaId == user.CompaniaId);

                return View(productoes.ToList());
            
        }

        //public ActionResult Index()
        //{
        //    var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
        //    //var productoes = db.Productoes.Include(p => p.Categoria).Include(p => p.Compania).Include(p => p.Impuesto);
        //    var productoes = db.Productoes.Include(p => p.Categoria).Include(p => p.Impuesto)
        //        .Where(p => p.CompaniaId == user.CompaniaId);
        //    return View(productoes.ToList());
        //}

        // GET: Productos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productoes.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CategoriaId = new SelectList(CombosHelper.GetCategorias(user.CompaniaId), "CategoriaId", "Descripcion");
            //ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre");
            ViewBag.ImpuestoId = new SelectList(CombosHelper.GetImpuestos(user.CompaniaId), "ImpuestoId", "Descripcion");
            var producto = new Producto { CompaniaId = user.CompaniaId, };
            return View(producto);
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductoId,CompaniaId,Descripcion,BarCode,CategoriaId,ImpuestoId,Precio,Imagen,Comentarios,ImagenFile")] Producto producto)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.Productoes.Add(producto);
                try
                {
                    db.SaveChanges();

                    if (producto.ImagenFile != null)
                    {
                        var folder = "~/Content/Productos";
                        var file = string.Format("{0}.jpg", producto.ProductoId);
                        var respuesta = FilesHelper.UploadPhoto(producto.ImagenFile, folder, file);
                        if (respuesta)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            producto.Imagen = pic;

                            db.Entry(producto).State = EntityState.Modified;
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
            
            ViewBag.CategoriaId = new SelectList(CombosHelper.GetCategorias(user.CompaniaId), "CategoriaId", "Descripcion", producto.CategoriaId);
            //ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", producto.CompaniaId);
            ViewBag.ImpuestoId = new SelectList(CombosHelper.GetImpuestos(user.CompaniaId), "ImpuestoId", "Descripcion", producto.ImpuestoId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productoes.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaId = new SelectList(CombosHelper.GetCategorias(producto.CompaniaId), "CategoriaId", "Descripcion", producto.CategoriaId);
            //ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", producto.CompaniaId);
            ViewBag.ImpuestoId = new SelectList(CombosHelper.GetImpuestos(producto.CompaniaId), "ImpuestoId", "Descripcion", producto.ImpuestoId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductoId,CompaniaId,Descripcion,BarCode,CategoriaId,ImpuestoId,Precio,Imagen,Comentarios,ImagenFile")] Producto producto)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {

                if (producto.ImagenFile != null)
                {
                    var folder = "~/Content/Productos";
                    var file = string.Format("{0}.jpg", producto.ProductoId);
                    var respuesta = FilesHelper.UploadPhoto(producto.ImagenFile, folder, file);
                    if (respuesta)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        producto.Imagen = pic;

                    }
                }


                db.Entry(producto).State = EntityState.Modified;
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
            ViewBag.CategoriaId = new SelectList(CombosHelper.GetCategorias(producto.CompaniaId), "CategoriaId", "Descripcion", producto.CategoriaId);
            //ViewBag.CompaniaId = new SelectList(db.Companias, "CompaniaId", "Nombre", producto.CompaniaId);
            ViewBag.ImpuestoId = new SelectList(CombosHelper.GetImpuestos(producto.CompaniaId), "ImpuestoId", "Descripcion", producto.ImpuestoId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productoes.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productoes.Find(id);
            db.Productoes.Remove(producto);
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
