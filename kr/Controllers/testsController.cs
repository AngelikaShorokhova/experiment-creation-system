using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using kr.Models;
using System.IO;
using ClosedXML.Excel;

namespace kr.Controllers
{
    public class testsController : Controller
    {
        private dbEntities db = new dbEntities();

        public ActionResult Index(string searchString)
        {
            var test = db.test.Include(t => t.researcher).Include(t => t.status);
            if (!String.IsNullOrEmpty(searchString))
            {
                test = test.Where(s => s.name.Contains(searchString));
            }
            return View(test.ToList());
        }
        public ActionResult IndexForChilren()
        {
            var test = db.test.Include(t => t.researcher).Include(t => t.status);
            return View(test.ToList());
        }

        // GET: tests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            test test = db.test.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        public ActionResult DetailsList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            test test = db.test.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        // GET: tests/Create
        public ActionResult Create()
        {
            ViewBag.researcher_id = new SelectList(db.researcher, "id", "login");
            ViewBag.status_id = new SelectList(db.status, "id", "status1");
            return View();
        }

        // POST: tests/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,status_id,name,description,end_date,end_count,count,researcher_id")] test test)
        {
            if (ModelState.IsValid)
            {
                db.test.Add(test);
                db.SaveChanges();
                return RedirectToAction("DetailsCreateQuestions", "Tests", new { id = test.id });
            }

            ViewBag.researcher_id = new SelectList(db.researcher, "id", "login", test.researcher_id);
            ViewBag.status_id = new SelectList(db.status, "id", "status1", test.status_id);
            return View(test);
        }

        // GET: tests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            test test = db.test.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            ViewBag.researcher_id = new SelectList(db.researcher, "id", "login", test.researcher_id);
            ViewBag.status_id = new SelectList(db.status, "id", "status1", test.status_id);
            return View(test);
        }

        // POST: tests/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,status_id,name,description,end_date,end_count,count,researcher_id")] test test)
        {
            if (ModelState.IsValid)
            {
                db.Entry(test).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.researcher_id = new SelectList(db.researcher, "id", "login", test.researcher_id);
            ViewBag.status_id = new SelectList(db.status, "id", "status1", test.status_id);
            return View(test);
        }

        // GET: tests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            test test = db.test.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        // POST: tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            test test = db.test.Find(id);
            db.test.Remove(test);
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

        public ActionResult DetailsCreateQuestions(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            test test = db.test.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            var questions = db.question.Include(q => q.question_type).Include(q => q.test).Where(q => q.test_id == test.id);
            testQuestion testQuestion = new testQuestion { test = test, questions = questions.ToList() };
            return View(testQuestion);
        }
                
    }
}
