using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AppWebb01.Models;

namespace AppWebb01.Controllers
{
    public class pacientesController : Controller
    {
        OdontoProEntities db = new OdontoProEntities();
        // GET: pacientes
        public ActionResult Index()
        {
            string search = "";
            List<mostrar_pacientes_Result> lista = null;
            lista = db.mostrar_pacientes(search).ToList();
            if (lista.Count()>0)
            {
                return View(lista);
            }
            else return View();

        }

        //[HttpPost]
        //public PartialViewResult Index(string nombre)
        //{

        //    List<mostrar_pacientes_Result> lista = null;
        //    lista = db.mostrar_pacientes(nombre).ToList();
        //    if (lista.Count() > 0)
        //    {
        //        return PartialView("_buscar", lista);
        //    }
        //    else {
        //        lista = db.mostrar_pacientes("").ToList();
        //        return PartialView("_buscar", lista);
        //    }
        //}

        [HttpGet]
        public ActionResult Search() {
            List<mostrar_pacientes_Result> lista = null;
            lista = db.mostrar_pacientes("").ToList();

            return View(lista);
            
        }

        [HttpPost]
        public ActionResult Search(string text)
        {
            if (text==null) {
                text = "";
                List<mostrar_pacientes_Result> lista = null;
                lista = db.mostrar_pacientes(text).ToList();

                return PartialView("_buscar", lista);
            } else {
                List<mostrar_pacientes_Result> lista = null;
                lista = db.mostrar_pacientes(text).ToList();

                return PartialView("_buscar", lista);
            }
            
        }



        // GET: pacientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paciente paciente = db.paciente.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        // GET: pacientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: pacientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "paciente_id,nombre,apellidos,direccion,estado,ciudad,fecha_nac,telefono,email,edad")] paciente paciente)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    db.insertar_paciente(paciente.nombre, paciente.apellidos, paciente.direccion, paciente.estado, paciente.ciudad, paciente.fecha_nac, paciente.telefono, paciente.email, paciente.edad);
                    return RedirectToAction("Index");
                }
                catch (Exception )
                {

                    ViewBag.error = "Este paciente ya esta registrado";
                }
                //db.pacientes.Add(paciente);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }

            return View(paciente);
        }

        // GET: pacientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paciente paciente = db.paciente.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        // POST: pacientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "paciente_id,nombre,apellidos,direccion,estado,ciudad,fecha_nac,telefono,email,edad")] paciente paciente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paciente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paciente);
        }

        // GET: pacientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            paciente paciente = db.paciente.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        // POST: pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            paciente paciente = db.paciente.Find(id);
            db.paciente.Remove(paciente);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Mostrar_reserva( int? id) {


            reservacion reserva = db.reservacion.Single(g => g.paciente_id == id);
            return View(reserva);
        }



        // GET: reservacions/Create
        public ActionResult CreateTratamiento()
        {
            return View();
        }

        // POST: tratamientos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTratamiento([Bind(Include = "tratamiento_id,diagnostico,tratamiento1,observaciones,fecha_inicio,paciente_id")] tratamiento tratamiento, int? id)
        {
            string fecha = Request.Form["fechatrat"];
            tratamiento.fecha_inicio = Convert.ToDateTime(fecha);

            tratamiento.paciente_id = id;

            if (ModelState.IsValid)
            {
                try
                {
                    db.insertar_tratamiento(tratamiento.diagnostico, tratamiento.tratamiento1, tratamiento.observaciones, tratamiento.fecha_inicio, tratamiento.paciente_id);
                    //db.reservacions.Add(reservacion);
                    //db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ViewBag.error = "Error";

                }

            }

            ViewBag.paciente_id = new SelectList(db.paciente, "paciente_id", "nombre", tratamiento.paciente_id);
            return View(tratamiento);
        }




        // GET: reservacions/Create
        public ActionResult CreateReserva()
        {
            return View();
        }

        // POST: reservacions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReserva([Bind(Include = "reservacion_id,fecha_reservacion,paciente_id")] reservacion reservacion, int? id)
        {
            string fecha = Request.Form["date"];
            reservacion.fecha_reservacion = Convert.ToDateTime(fecha);

            reservacion.paciente_id = id;

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
