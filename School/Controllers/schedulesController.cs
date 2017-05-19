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
    public class schedulesController : Controller
    {
        private schoolEntities db = new schoolEntities();

        // GET: schedules
        public ActionResult Index()
        {
            var schedule = db.schedule.Include(s => s.subject).Include(s => s.teacher);
            return View(schedule.ToList());
        }

        public ActionResult Search(String searchText)
        {
            var result = db.schedule
                .Where(a => a.cabinet.ToString().Contains(searchText.ToLower())
                    || a.time.ToString().Contains(searchText.ToLower())
                    || a.subject.name.ToLower().Contains(searchText.ToLower())
                    || a.teacher.fio.ToLower().Contains(searchText.ToLower()))
                .ToArray();
            return View(result);
        }

        // GET: schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            schedule schedule = db.schedule.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: schedules/Create
        public ActionResult Create()
        {
            ViewBag.id_subject = new SelectList(db.subject, "id", "name");
            ViewBag.id_teacher = new SelectList(db.teacher, "id", "fio");
            return View();
        }

        // POST: schedules/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_subject,id_teacher,cabinet,time")] schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.schedule.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_subject = new SelectList(db.subject, "id", "name", schedule.id_subject);
            ViewBag.id_teacher = new SelectList(db.teacher, "id", "fio", schedule.id_teacher);
            return View(schedule);
        }

        // GET: schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            schedule schedule = db.schedule.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_subject = new SelectList(db.subject, "id", "name", schedule.id_subject);
            ViewBag.id_teacher = new SelectList(db.teacher, "id", "fio", schedule.id_teacher);
            return View(schedule);
        }

        // POST: schedules/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_subject,id_teacher,cabinet,time")] schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_subject = new SelectList(db.subject, "id", "name", schedule.id_subject);
            ViewBag.id_teacher = new SelectList(db.teacher, "id", "fio", schedule.id_teacher);
            return View(schedule);
        }

        // GET: schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            schedule schedule = db.schedule.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            schedule schedule = db.schedule.Find(id);
            db.schedule.Remove(schedule);
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
