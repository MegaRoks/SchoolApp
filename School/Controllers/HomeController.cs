using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace School.Controllers
{
    [Authorize(Roles = "user")]
    public class HomeController : Controller
    {
        private schoolEntities db = new schoolEntities();
        public ActionResult Index()
        {
            ViewBag.teacherList = new SelectList(db.teacher, "id", "fio");
            ViewBag.discipleList = new SelectList(db.disciple, "id", "fio");
            ViewBag.scheduleList = new SelectList(db.schedule, "id", "fio");
            ViewBag.classroomsList = new SelectList(db.classrooms, "id", "name");
            ViewBag.subjectList = new SelectList(db.subject, "id", "name");
            return View();
        }


        public ActionResult Query1(DateTimeOffset queryText1, DateTimeOffset queryText2)
        {
            var result = db.teacher
                .Include(c => c.classrooms)
                .Include(c => c.schedule)
                .ToArray()
                .Where(c => DateTimeOffset.Parse(c.v_date) > queryText1 && DateTimeOffset.Parse(c.v_date) < queryText2);
            return View(result.ToArray());
        }

        public ActionResult Query2(string queryText)
        {
            var result = db.teacher
                .Include(c => c.classrooms)
                .Include(c => c.schedule)
                .Where(c => c.fio.ToLower().Contains(queryText.ToLower()))
                .SelectMany(c => c.classrooms)
                .ToArray();
            return View(result.ToArray());
        }
        public ActionResult Query3(string queryText)
        {
            var result = db.schedule
                .Include(c => c.teacher)
                .Include(c => c.subject)
                .Where(c => c.subject.name.ToLower().Contains(queryText.ToLower()))
                .Select(c => c.teacher)
                .ToArray();
            return View(result.ToArray());
        }

        public ActionResult Query4(string queryText1)
        {
            var result = db.schedule
                .Include(c => c.subject)
                .Include(c => c.teacher)
                .Where(c => c.subject.name.ToLower().Contains(queryText1.ToLower()))
                .ToArray();
            return View(result.ToArray());
        }

        public ActionResult Query5(string queryText1)
        {
            var result = db.disciple
                .Where(c => c.fio.ToLower().Contains(queryText1.ToLower()))
                .ToArray();
            return View(result.ToArray());
        }

        public ActionResult Query6(string queryText1)
        {
            var result = db.disciple
                .Include(c => c.classrooms)
                .Where(c => c.id_class.ToString().ToLower().Contains(queryText1.ToLower()))
                .ToArray();
            return View(result.ToArray());
        }
    }
}