using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebb01.Models;

using System.Data;
using System.Data.Entity;
using System.Net;

using System.IO;
using System.Reflection;

namespace AppWebb01.Controllers
{
    public class tratamientosController : Controller
    {
        private OdontoProEntities db = new OdontoProEntities();
        // GET: tratamientos
        public ActionResult Index()
        {

            List<tratamiento> obj= db.tratamiento.ToList();
            return View(obj);
        }

        public ActionResult Create() {
            ViewBag.paciente_id = new SelectList(db.paciente, "paciente_id", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "diagnostico, tratamiento, observaciones, fecha_inicio, paciente_id")] tratamiento obj) {

            
            if (ModelState.IsValid)
            {
                try
                {
                    db.insertar_tratamiento(obj.diagnostico, obj.tratamiento1, obj.observaciones, obj.fecha_inicio, obj.paciente_id);

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ViewBag.error = "Este usuario ya tiene una reservacion";

                }

            }

            ViewBag.paciente_id = new SelectList(db.paciente, "paciente_id", "nombre", obj.paciente_id);
            return View(obj);
        }

        // GET: tratamientos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tratamiento tratamiento = db.tratamiento.Find(id);
            if (tratamiento == null)
            {
                return HttpNotFound();
            }
            return View(tratamiento);
        }


        // GET: reservacions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tratamiento tratamiento = db.tratamiento.Find(id);
            if (tratamiento == null)
            {
                return HttpNotFound();
            }
            ViewBag.paciente_id = new SelectList(db.tratamiento, "tratamiento_id", "tratamiento", tratamiento.tratamiento_id);
            return View(tratamiento);
        }

        // POST: tratamientos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( tratamiento tratamiento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tratamiento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.paciente_id = new SelectList(db.paciente, "paciente_id", "nombre", tratamiento.paciente_id);
            return View(tratamiento);
        }

        // GET: tratamientos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tratamiento tratamiento = db.tratamiento.Find(id);
            if (tratamiento == null)
            {
                return HttpNotFound();
            }
            return View(tratamiento);
        }

        // POST: tratamientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tratamiento tratamiento = db.tratamiento.Find(id);
            db.tratamiento.Remove(tratamiento);
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