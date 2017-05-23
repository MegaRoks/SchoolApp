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
    public class subjectsController : Controller
    {
        private schoolEntities db = new schoolEntities();

        [Authorize(Roles = "user")]
        // GET: subjects
        public ActionResult Index()
        {
            return View(db.subject.ToList());
        }

        [Authorize(Roles = "user")]
        public ActionResult Search(String searchText)
        {
            var result = db.subject
                .Where(a => a.name.ToLower().Contains(searchText.ToLower())
                    || a.year.ToString().Contains(searchText.ToLower()))
                .ToArray();
            return View(result);
        }

        [Authorize(Roles = "admin")]
        // GET: subjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subject subject = db.subject.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        [Authorize(Roles = "admin")]
        // GET: subjects/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        // POST: subjects/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,year")] subject subject)
        {
            if (ModelState.IsValid)
            {
                db.subject.Add(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subject);
        }

        [Authorize(Roles = "admin")]
        // GET: subjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subject subject = db.subject.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        [Authorize(Roles = "admin")]
        // POST: subjects/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,year")] subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subject);
        }

        [Authorize(Roles = "admin")]
        // GET: subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subject subject = db.subject.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        [Authorize(Roles = "admin")]
        // POST: subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subject subject = db.subject.Find(id);
            db.subject.Remove(subject);
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
