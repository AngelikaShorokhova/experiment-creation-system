using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using kr.Models;

namespace kr.Controllers
{
    public class questionsController : Controller
    {
        private dbEntities db = new dbEntities();

        // GET: questions
        public ActionResult Index()
        {
            var question = db.question.Include(q => q.question_type).Include(q => q.test);
            return View(question.ToList());
        }

        // GET: questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            question question = db.question.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: questions/Create
        public ActionResult Create()
        {
            ViewBag.type_id = new SelectList(db.question_type, "id", "type");
            ViewBag.test_id = new SelectList(db.test, "id", "name");
            return View();
        }

        // POST: questions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,test_id,name,text,type_id,time")] question question)
        {
            if (ModelState.IsValid)
            {
                int test_id = db.test.OrderByDescending(q => q.id).First().id;
                question.test_id = test_id;
                db.question.Add(question);
                db.SaveChanges();
                return RedirectToAction("DetailsCreateQuestions", "Tests", new { id = test_id });
            }

            ViewBag.type_id = new SelectList(db.question_type, "id", "type", question.type_id);
            ViewBag.test_id = new SelectList(db.test, "id", "name", question.test_id);
            return View(question);
        }

        // GET: questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            question question = db.question.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.type_id = new SelectList(db.question_type, "id", "type", question.type_id);
            ViewBag.test_id = new SelectList(db.test, "id", "name", question.test_id);
            return View(question);
        }

        // POST: questions/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,test_id,name,text,type_id,time")] question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.type_id = new SelectList(db.question_type, "id", "type", question.type_id);
            ViewBag.test_id = new SelectList(db.test, "id", "name", question.test_id);
            return View(question);
        }

        // GET: questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            question question = db.question.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            question question = db.question.Find(id);
            db.question.Remove(question);
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
