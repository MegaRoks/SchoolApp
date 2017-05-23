using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using School;

namespace School.Controllers
{
    public class classroomsController : Controller
    {
        private schoolEntities db = new schoolEntities();

        [Authorize(Roles ="user")]
        // GET: classrooms
        public ActionResult Index()
        {
            var classrooms = db.classrooms.Include(c => c.teacher);
            return View(classrooms.ToList());
        }

        [Authorize(Roles = "user")]
        public ActionResult Search(String searchText)
        {
            var result = db.classrooms
                .Where(a => a.name.ToLower().Contains(searchText.ToLower()) 
                    || a.y_year.ToString().Contains(searchText.ToLower()) 
                    || a.teacher.fio.ToLower().Contains(searchText.ToLower()))
                .ToArray();
            return View(result);
        }

        [Authorize(Roles = "admin")]
        // GET: classrooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classrooms classrooms = db.classrooms.Find(id);
            if (classrooms == null)
            {
                return HttpNotFound();
            }
            return View(classrooms);
        }

        [Authorize(Roles = "admin")]
        // GET: classrooms/Create
        public ActionResult Create()
        {
            ViewBag.id_teacher = new SelectList(db.teacher, "id", "fio");
            return View();
        }

        [Authorize(Roles = "admin")]
        // POST: classrooms/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,y_year,id_teacher")] classrooms classrooms)
        {
            if (ModelState.IsValid)
            {
                db.classrooms.Add(classrooms);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_teacher = new SelectList(db.teacher, "id", "fio", classrooms.id_teacher);
            return View(classrooms);
        }

        [Authorize(Roles = "admin")]
        // GET: classrooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classrooms classrooms = db.classrooms.Find(id);
            if (classrooms == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_teacher = new SelectList(db.teacher, "id", "fio", classrooms.id_teacher);
            return View(classrooms);
        }

        [Authorize(Roles = "admin")]
        // POST: classrooms/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,y_year,id_teacher")] classrooms classrooms)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classrooms).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_teacher = new SelectList(db.teacher, "id", "fio", classrooms.id_teacher);
            return View(classrooms);
        }

        [Authorize(Roles = "admin")]
        // GET: classrooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            classrooms classrooms = db.classrooms.Find(id);
            if (classrooms == null)
            {
                return HttpNotFound();
            }
            return View(classrooms);
        }

        [Authorize(Roles = "admin")]
        // POST: classrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            classrooms classrooms = db.classrooms.Find(id);
            db.classrooms.Remove(classrooms);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
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
