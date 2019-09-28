using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AppWebb01.Models;

using System.IO;
using System.Reflection;



namespace AppWebb01.Controllers
{
    public class reservacionsController : Controller
    {
        private OdontoProEntities db = new OdontoProEntities();

        // GET: reservacions
        public ActionResult Index()
        {
            

            
            var reservacions = db.reservacion.Include(r => r.paciente);
            return View(reservacions.ToList());
        }

        // GET: reservacions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reservacion reservacion = db.reservacion.Find(id);
            if (reservacion == null)
            {
                return HttpNotFound();
            }
            return View(reservacion);
        }

        // GET: reservacions/Create
        public ActionResult Create()
        {
            ViewBag.paciente_id = new SelectList(db.paciente, "paciente_id", "nombre");
            return View();
        }

        // POST: reservacions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "reservacion_id,fecha_reservacion,paciente_id")] reservacion reservacion)
        {
            string fecha = Request.Form["date"];
            reservacion.fecha_reservacion = Convert.ToDateTime(fecha);

            if (ModelState.IsValid)
            {
                try
                {
                    db.insertar_reservacion(reservacion.fecha_reservacion, reservacion.paciente_id);
                    //db.reservacions.Add(reservacion);
                    //db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ViewBag.error = "Este usuario ya tiene una reservacion";
                    
                }
                
            }

            ViewBag.paciente_id = new SelectList(db.paciente, "paciente_id", "nombre", reservacion.paciente_id);
            return View(reservacion);
        }

        // GET: reservacions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reservacion reservacion = db.reservacion.Find(id);
            if (reservacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.paciente_id = new SelectList(db.paciente, "paciente_id", "nombre", reservacion.paciente_id);
            return View(reservacion);
        }

        // POST: reservacions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "reservacion_id,fecha_reservacion,paciente_id")] reservacion reservacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.paciente_id = new SelectList(db.paciente, "paciente_id", "nombre", reservacion.paciente_id);
            return View(reservacion);
        }

        // GET: reservacions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reservacion reservacion = db.reservacion.Find(id);
            if (reservacion == null)
            {
                return HttpNotFound();
            }
            return View(reservacion);
        }

        // POST: reservacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            reservacion reservacion = db.reservacion.Find(id);
            db.reservacion.Remove(reservacion);
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
