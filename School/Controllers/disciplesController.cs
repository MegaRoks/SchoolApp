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
    public class disciplesController : Controller
    {
        private schoolEntities db = new schoolEntities();

        [Authorize(Roles = "user")]
        // GET: disciples
        public ActionResult Index()
        {
            var disciple = db.disciple.Include(d => d.classrooms);
            return View(disciple.ToList());
        }

        [Authorize(Roles = "user")]
        public ActionResult Search(String searchText)
        {
            var result = db.disciple
                .Where(a => a.fio.ToLower().Contains(searchText.ToLower())
                    || a.p_year.ToString().Contains(searchText.ToLower())
                    || a.classrooms.name.ToLower().Contains(searchText.ToLower()))
                .ToArray();
            return View(result);
        }

        [Authorize(Roles = "admin")]
        // GET: disciples/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            disciple disciple = db.disciple.Find(id);
            if (disciple == null)
            {
                return HttpNotFound();
            }
            return View(disciple);
        }

        [Authorize(Roles = "admin")]
        // GET: disciples/Create
        public ActionResult Create()
        {
            ViewBag.id_class = new SelectList(db.classrooms, "id", "name");
            return View();
        }

        [Authorize(Roles = "admin")]
        // POST: disciples/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fio,id_class,p_year")] disciple disciple)
        {
            if (ModelState.IsValid)
            {
                db.disciple.Add(disciple);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_class = new SelectList(db.classrooms, "id", "name", disciple.id_class);
            return View(disciple);
        }

        [Authorize(Roles = "admin")]
        // GET: disciples/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            disciple disciple = db.disciple.Find(id);
            if (disciple == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_class = new SelectList(db.classrooms, "id", "name", disciple.id_class);
            return View(disciple);
        }

        [Authorize(Roles = "admin")]
        // POST: disciples/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fio,id_class,p_year")] disciple disciple)
        {
            if (ModelState.IsValid)
            {
                db.Entry(disciple).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_class = new SelectList(db.classrooms, "id", "name", disciple.id_class);
            return View(disciple);
        }

        [Authorize(Roles = "admin")]
        // GET: disciples/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            disciple disciple = db.disciple.Find(id);
            if (disciple == null)
            {
                return HttpNotFound();
            }
            return View(disciple);
        }

        [Authorize(Roles = "admin")]
        // POST: disciples/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            disciple disciple = db.disciple.Find(id);
            db.disciple.Remove(disciple);
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
