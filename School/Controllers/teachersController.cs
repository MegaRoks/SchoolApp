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
    
    public class teachersController : Controller
    {
        private schoolEntities db = new schoolEntities();

        [Authorize(Roles = "user")]
        // GET: teachers
        public ActionResult Index()
        {
            return View(db.teacher.ToList());
        }

        [Authorize(Roles = "user")]
        public ActionResult Search(String searchText)
        {
            var result = db.teacher
                .Where(a => a.fio.ToLower().Contains(searchText.ToLower())
                    || a.specialism.ToLower().Contains(searchText.ToLower())
                    || a.r_date.ToString().Contains(searchText.ToLower())
                    || a.v_date.ToString().Contains(searchText.ToLower()))
                .ToArray();
            return View(result);
        }

        [Authorize(Roles = "admin")]
        // GET: teachers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            teacher teacher = db.teacher.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        [Authorize(Roles = "admin")]
        // GET: teachers/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        // POST: teachers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fio,specialism,r_date,v_date")] teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.teacher.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teacher);
        }

        [Authorize(Roles = "admin")]
        // GET: teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            teacher teacher = db.teacher.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        [Authorize(Roles = "admin")]
        // POST: teachers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fio,specialism,r_date,v_date")] teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        [Authorize(Roles = "admin")]
        // GET: teachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            teacher teacher = db.teacher.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        [Authorize(Roles = "admin")]
        // POST: teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            teacher teacher = db.teacher.Find(id);
            db.teacher.Remove(teacher);
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
