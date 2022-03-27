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
    public class answersController : Controller
    {
        private dbEntities db = new dbEntities();

        // GET: answers
        public ActionResult Index()
        {
            var answer = db.answer.Include(a => a.image).Include(a => a.question).Include(a => a.test_instance);
            return View(answer.ToList());
        }

        // GET: answers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            answer answer = db.answer.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // GET: answers/Create
        public ActionResult Create(int q_id)
        {
            test_instance test_Instance = db.test_instance.First();
            question question = db.question.First();
            var image = db.image.Include(i => i.question).Where(q => q.question_id == question.id);
            childrenQuestion childrenQuestion = new childrenQuestion { images = image.ToList(), test_Instance = test_Instance, question = question };
            ViewBag.image_id = new SelectList(db.image, "id", "link");
            ViewBag.question_id = new SelectList(db.question.Where(i => i.id == q_id), "id", "name");
            ViewBag.test_ins_id = new SelectList(db.test_instance, "id", "result");
            return View(childrenQuestion);
        }

        // POST: answers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,test_ins_id,image_id,question_id")] answer answer)
        {
            if (ModelState.IsValid)
            {
                answer.test_ins_id = 1;
                answer.image_id = 1;
                db.answer.Add(answer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.image_id = new SelectList(db.image, "id", "link", answer.image_id);
            ViewBag.question_id = new SelectList(db.question, "id", "name", answer.question_id);
            ViewBag.test_ins_id = new SelectList(db.test_instance, "id", "result", answer.test_ins_id);
            return View(answer);
        }

        // GET: answers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            answer answer = db.answer.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.image_id = new SelectList(db.image, "id", "link", answer.image_id);
            ViewBag.question_id = new SelectList(db.question, "id", "name", answer.question_id);
            ViewBag.test_ins_id = new SelectList(db.test_instance, "id", "result", answer.test_ins_id);
            return View(answer);
        }

        // POST: answers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,test_ins_id,image_id,question_id")] answer answer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.image_id = new SelectList(db.image, "id", "link", answer.image_id);
            ViewBag.question_id = new SelectList(db.question, "id", "name", answer.question_id);
            ViewBag.test_ins_id = new SelectList(db.test_instance, "id", "result", answer.test_ins_id);
            return View(answer);
        }

        // GET: answers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            answer answer = db.answer.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            answer answer = db.answer.Find(id);
            db.answer.Remove(answer);
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
