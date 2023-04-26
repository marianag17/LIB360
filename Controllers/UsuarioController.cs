using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Xml;
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
            if (Session["UserEmail"] == null && Session["UserName"] == null && Session["UserID"] == null && Session["UserRol"] == null)
            {
                 return RedirectToAction("SignIn","Usuario");
            }
            return View(db.Usuario.ToList());
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserEmail"] == null && Session["UserName"] == null && Session["UserID"] == null && Session["UserRol"] == null)
            {
                return RedirectToAction("SignIn", "Usuario");
            }
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
        public ActionResult Create(int? id)
        {
            if (Session["UserEmail"] == null && Session["UserName"] == null && Session["UserID"] == null && Session["UserRol"] == null)
            {
                return RedirectToAction("SignIn", "Usuario");
            }
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

            if (usuario.Contrasena == null || usuario.Correo == null || usuario.Nombre == null || usuario.Apellido == null)
            {
                return PartialView(usuario);
            }
            if (ModelState.IsValid)
            {
                string pass = Encrypt.GetSHA256(usuario.Contrasena);
                usuario.Contrasena = pass;
                db.Usuario.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return PartialView(usuario);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserEmail"] == null && Session["UserName"] == null && Session["UserID"] == null && Session["UserRol"] == null)
            {
                return RedirectToAction("SignIn", "Usuario");
            }
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
            if (Session["UserEmail"] == null && Session["UserName"] == null && Session["UserID"] == null && Session["UserRol"] == null)
            {
                return RedirectToAction("SignIn", "Usuario");
            }
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

        [HttpGet]
        public ActionResult SignIn()
        {
            Session["UserEmail"] =null;
            Session["UserName"] = null;
            Session["UserID"] = null;
            Session["UserRol"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(Usuario usuario)
        {
            if (usuario.Contrasena == null || usuario.Correo == null)
            {
                return View();
            }
            var us = db.Usuario.Where(x => x.Correo == usuario.Correo).FirstOrDefault();
            if (us != null)
            {
                String nombre = us.Nombre as String;
                string enc = Encrypt.GetSHA256(usuario.Contrasena);
                if (enc == us.Contrasena)
                {
                    Session["UserEmail"] = Convert.ToString(us.Correo);
                    Session["UserName"] =nombre;
                    Session["UserID"] = Convert.ToString(us.IDUsuario);
                    Session["UserRol"] = us.Rol;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
            
        }


        [HttpGet]
        public ActionResult SignUp()
        {
            Session["UserEmail"] = null;
            Session["UserName"] = null;
            Session["UserID"] = null;
            Session["UserRol"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Usuario usuario)
        {
            if (usuario.Contrasena == null || usuario.Correo == null || usuario.Nombre == null || usuario.Apellido == null)
            {
                return View();
            }
            if (ModelState.IsValid)
            {
                var us = db.Usuario.Where(x => x.Correo == usuario.Correo).FirstOrDefault();
                if (us == null)
                {
                    string pass = Encrypt.GetSHA256(usuario.Contrasena);
                    usuario.Contrasena = pass;
                    usuario.Rol = 2;
                    usuario.Estado = "Activo";
                    db.Usuario.Add(usuario);
                    db.SaveChanges();
                    Session["UserEmail"] = usuario.Correo;
                    Session["UserName"] = usuario.Nombre;
                    Session["UserID"] = usuario.IDUsuario;
                    Session["UserRol"] = usuario.Rol;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
                
            }
            return View();
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
