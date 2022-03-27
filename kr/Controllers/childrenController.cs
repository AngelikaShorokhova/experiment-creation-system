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
    public class childrenController : Controller
    {
        private dbEntities db = new dbEntities();

        // GET: children
        public ActionResult Index()
        {
            var child = db.child.Include(c => c.gender1).Include(c => c.school_type1);
            return View(child.ToList());
        }

        // GET: children/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            child child = db.child.Find(id);
            if (child == null)
            {
                return HttpNotFound();
            }
            return View(child);
        }

        // GET: children/Create
        public ActionResult Create()
        {
            ViewBag.gender = new SelectList(db.gender, "id", "name");
            ViewBag.school_type = new SelectList(db.school_type, "id", "type");
            return View();
        }

        // POST: children/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,login,surname,name,patronymic_name,birth_date,school_type,parent_name,contact,gender")] child child)
        {
            if (ModelState.IsValid)
            {
                child.login = User.Identity.Name;
                db.child.Add(child);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.gender = new SelectList(db.gender, "id", "name", child.gender);
            ViewBag.school_type = new SelectList(db.school_type, "id", "type", child.school_type);
            return View(child);
        }

        // GET: children/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            child child = db.child.Find(id);
            if (child == null)
            {
                return HttpNotFound();
            }
            ViewBag.gender = new SelectList(db.gender, "id", "name", child.gender);
            ViewBag.school_type = new SelectList(db.school_type, "id", "type", child.school_type);
            return View(child);
        }

        // POST: children/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,login,surname,name,patronymic_name,birth_date,school_type,parent_name,contact,gender")] child child)
        {
            if (ModelState.IsValid)
            {
                db.Entry(child).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.gender = new SelectList(db.gender, "id", "name", child.gender);
            ViewBag.school_type = new SelectList(db.school_type, "id", "type", child.school_type);
            return View(child);
        }

        // GET: children/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            child child = db.child.Find(id);
            if (child == null)
            {
                return HttpNotFound();
            }
            return View(child);
        }

        // POST: children/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            child child = db.child.Find(id);
            db.child.Remove(child);
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
