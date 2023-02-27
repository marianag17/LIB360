using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lib360;

namespace Lib360.Controllers
{
    public class UsuarioController : Controller
    {
        private LIB360Entities db = new LIB360Entities();

        // GET: Usuario
        public ActionResult Index()
        {
            Session["IDUsuario"] = null;
            return View(db.Usuario.ToList());
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            List<SelectListItem> Roles = new List<SelectListItem>();
            Roles.Add(new SelectListItem() { Text = "Admin", Value = "1" });
            Roles.Add(new SelectListItem() { Text = "User", Value = "2" });
            ViewBag.Roles = Roles;
            List<SelectListItem> Estados = new List<SelectListItem>();
            Estados.Add(new SelectListItem() { Text = "Activo", Value = "Activo" });
            Estados.Add(new SelectListItem() { Text = "Inactivo", Value = "Inactivo" });
            ViewBag.Estados = Estados;
            return PartialView("Create");
        }

        // POST: Usuario/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,Apellido,Telefono,DPI,Correo,Estado,Rol,Contrasena,IDUsuario")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Usuario.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return PartialView(usuario);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            List<SelectListItem> Roles = new List<SelectListItem>();
            Roles.Add(new SelectListItem() { Text = "Admin", Value = "1" });
            Roles.Add(new SelectListItem() { Text = "User", Value = "2" });
            ViewBag.Roles = Roles;
            List<SelectListItem> Estados = new List<SelectListItem>();
            Estados.Add(new SelectListItem() { Text = "Activo", Value = "Activo" });
            Estados.Add(new SelectListItem() { Text = "Inactivo", Value = "Inactivo" });
            ViewBag.Estados = Estados;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            Session["IDUsuario"] = Convert.ToInt32(id);
            return PartialView("Edit",usuario);
        }

        // POST: Usuario/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Nombre,Apellido,Telefono,DPI,Correo,Estado,Rol,Contrasena,IDUsuario")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.IDUsuario = Convert.ToInt32(Session["IDUsuario"]);
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(usuario);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            Session["IDUsuario"] = Convert.ToInt32(id);
            return PartialView("Delete", usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            id = Convert.ToInt32(Session["IDUsuario"]);
            Usuario usuario = db.Usuario.Find(id);
            db.Usuario.Remove(usuario);
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
