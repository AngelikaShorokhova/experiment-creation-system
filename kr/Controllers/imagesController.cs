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
    public class imagesController : Controller
    {
        private dbEntities db = new dbEntities();

        // GET: images
        public ActionResult Index()
        {
            var image = db.image.Include(i => i.question).Include(i=>i.question.test);
            return View(image.ToList());
        }

        // GET: images/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            image image = db.image.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // GET: images/Create
        public ActionResult Create(int? id)
        {
            var question = db.question.Where(q => q.id == id);
            ViewBag.question_id = new SelectList(question, "id", "name");
            return View();
        }

        // POST: images/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,link,question_id")] image image)
        {
            if (ModelState.IsValid)
            {

                db.image.Add(image);
                db.SaveChanges();
                return RedirectToAction("DetailsCreateImages", "Questions", new { id = image.question_id });
            }

            ViewBag.question_id = new SelectList(db.question, "id", "name", image.question_id);
            return View(image);
        }

        // GET: images/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            image image = db.image.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            ViewBag.question_id = new SelectList(db.question, "id", "name", image.question_id);
            return View(image);
        }

        // POST: images/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,link,question_id")] image image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.question_id = new SelectList(db.question, "id", "name", image.question_id);
            return View(image);
        }

        // GET: images/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            image image = db.image.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            image image = db.image.Find(id);
            db.image.Remove(image);
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
