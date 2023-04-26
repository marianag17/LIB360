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
    public class LibroController : Controller
    {
        private LIB360Entities db = new LIB360Entities();

        // GET: Libro
        public ActionResult Index()
        {
            if (Session["UserEmail"] == null && Session["UserName"] == null && Session["UserID"] == null && Session["UserRol"] == null)
            {
                return RedirectToAction("SignIn", "Usuario");
            }
            Session["IDLibro"] = null;
            Session["CantLibro"] = null;
            return View(db.Libro.ToList());
        }

        // GET: Libro/Details/5
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
            Libro libro = db.Libro.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return View(libro);
        }

        // GET: Libro/Create
        [HttpGet]
        public ActionResult Create(int? id)
        {
            if (Session["UserEmail"] == null && Session["UserName"] == null && Session["UserID"] == null && Session["UserRol"] == null)
            {
                return RedirectToAction("SignIn", "Usuario");
            }
            return PartialView("Create");
        }


        // POST: Libro/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,Autor,Edicion,Pasillo,Cantidad")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                db.Libro.Add(libro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return PartialView(libro);
        }

        // GET: Libro/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserEmail"] == null && Session["UserName"] == null && Session["UserID"] == null && Session["UserRol"] == null)
            {
                return RedirectToAction("SignIn", "Usuario");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libro.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            Session["IDLibro"] = Convert.ToInt32(id);
            var lib = db.Libro.Where(j => j.IDLibro == id).FirstOrDefault();
            int? cantidad = lib.Cantidad;
            Session["CantLibro"] = cantidad;
            return PartialView("Edit",libro); 
        }

        // POST: Libro/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Nombre,Autor,Edicion,Pasillo")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                libro.IDLibro = Convert.ToInt32(Session["IDLibro"]);
                libro.Cantidad = Convert.ToInt32(Session["CantLibro"]);
                db.Entry(libro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(libro);
        }

        // GET: Libro/Delete/5
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
            Libro libro = db.Libro.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            Session["IDLibro"] = Convert.ToInt32(id);
            var lib = db.Libro.Where(j => j.IDLibro == id).FirstOrDefault();
            return PartialView("Delete",libro);
        }

        // POST: Libro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            id = Convert.ToInt32(Session["IDLibro"]);
            Libro libro = db.Libro.Find(id);
            db.Libro.Remove(libro);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult VerLibros()
        {
            if (Session["UserEmail"] == null && Session["UserName"] == null && Session["UserID"] == null && Session["UserRol"] == null)
            {
                return RedirectToAction("SignIn", "Usuario");
            }
            return View(db.Libro.ToList());
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
