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
    public class PrestamoController : Controller
    {
        private LIB360Entities db = new LIB360Entities();

        // GET: Prestamo
        public ActionResult Index()
        {
            if (Session["UserEmail"] == null && Session["UserName"] == null && Session["UserID"] == null && Session["UserRol"] == null)
            {
                return RedirectToAction("SignIn", "Usuario");
            }
            var prestamo = db.Prestamo.Include(p => p.Libro).Include(p => p.Usuario).OrderByDescending(p=>p.FechaInicio);
            Session["IDPrestamo"] =null;
            return View(prestamo.ToList());
        }

        // GET: Prestamo/Details/5
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
            Prestamo prestamo = db.Prestamo.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            return View(prestamo);
        }

        // GET: Prestamo/Create
        public ActionResult Create()
        {
            if (Session["UserEmail"] == null && Session["UserName"] == null && Session["UserID"] == null && Session["UserRol"] == null)
            {
                return RedirectToAction("SignIn", "Usuario");
            }
            ViewBag.IDLibro = new SelectList(db.Libro, "IDLibro", "Nombre");
            ViewBag.IDUsuario = new SelectList(db.Usuario, "IDUsuario", "Nombre");
            return PartialView();
        }

        // POST: Prestamo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDLibro,IDUsuario,FechaInicio,FechaFin,Regresado,IDPrestamo")] Prestamo prestamo)
        {
            //14 dias
            DateTime dt = (DateTime)prestamo.FechaInicio;
            System.TimeSpan duration = new System.TimeSpan(14, 0, 0, 0);
            prestamo.FechaFin = dt.Add(duration);
            prestamo.Regresado = 0;
            if (ModelState.IsValid)
            {
                db.Prestamo.Add(prestamo);
                var libro = db.Libro.FirstOrDefault(x => x.IDLibro == prestamo.IDLibro);
                if (libro.Cantidad == 0 || prestamo.FechaInicio < DateTime.Now)
                {
                    return PartialView(prestamo);
                }
                libro.Cantidad = libro.Cantidad-1;
                db.Entry(libro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDLibro = new SelectList(db.Libro, "IDLibro", "Nombre", prestamo.IDLibro);
            ViewBag.IDUsuario = new SelectList(db.Usuario, "IDUsuario", "Nombre", prestamo.IDUsuario);
            return PartialView(prestamo);
        }

        // GET: Prestamo/Edit/5
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
            Prestamo prestamo = db.Prestamo.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDLibro = new SelectList(db.Libro, "IDLibro", "Nombre", prestamo.IDLibro);
            ViewBag.IDUsuario = new SelectList(db.Usuario, "IDUsuario", "Nombre", prestamo.IDUsuario);
            Session["IDPrestamo"] = Convert.ToInt32(id);
            return PartialView(prestamo);
        }

        // POST: Prestamo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDLibro,IDUsuario,FechaInicio,FechaFin,Regresado,IDPrestamo")] Prestamo prestamo)
        {
            int id = (int)Convert.ToInt64(Session["IDPrestamo"]);
            var pres = db.Prestamo.Find(id);
            prestamo = pres;
            prestamo.Regresado = 1;
            if (ModelState.IsValid)
            {
                db.Entry(prestamo).State = EntityState.Modified;
                var libro = db.Libro.FirstOrDefault(x => x.IDLibro == prestamo.IDLibro);
                libro.Cantidad = libro.Cantidad + 1;
                db.Entry(libro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDLibro = new SelectList(db.Libro, "IDLibro", "Nombre", prestamo.IDLibro);
            ViewBag.IDUsuario = new SelectList(db.Usuario, "IDUsuario", "Nombre", prestamo.IDUsuario);
            return PartialView(prestamo);
        }

        // GET: Prestamo/Delete/5
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
            Prestamo prestamo = db.Prestamo.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            return PartialView(prestamo);
        }

        // POST: Prestamo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prestamo prestamo = db.Prestamo.Find(id);
            db.Prestamo.Remove(prestamo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult PrestamoUsuario()
        {
            if (Session["UserEmail"] == null && Session["UserName"] == null && Session["UserID"] == null && Session["UserRol"] == null)
            {
                return RedirectToAction("SignIn", "Usuario");
            }
            int id = (int)Convert.ToInt64(Session["UserID"]);
            //var prestamo = db.Prestamo.Include(p => p.Libro).Include(p => p.Usuario).OrderByDescending(p => p.FechaInicio);
            var prestamo = db.Prestamo.Where(p => p.IDUsuario == id).Include(p => p.Libro).Include(p => p.Usuario).OrderByDescending(p => p.FechaInicio);
            Session["IDPrestamo"] = null;
            return View(prestamo.ToList());
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
