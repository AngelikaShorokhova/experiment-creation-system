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
    public class researchersController : Controller
    {
        private dbEntities db = new dbEntities();

        // GET: researchers
        public ActionResult Index()
        {
            return View(db.researcher.ToList());
        }

        // GET: researchers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            researcher researcher = db.researcher.Find(id);
            if (researcher == null)
            {
                return HttpNotFound();
            }
            return View(researcher);
        }

        // GET: researchers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: researchers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,login,surname,name,patronymic_name,bitrh_date,job,count")] researcher researcher)
        {
            if (ModelState.IsValid)
            {
                researcher.login = User.Identity.Name;
                db.researcher.Add(researcher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(researcher);
        }

        // GET: researchers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            researcher researcher = db.researcher.Find(id);
            if (researcher == null)
            {
                return HttpNotFound();
            }
            return View(researcher);
        }

        // POST: researchers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,login,surname,name,patronymic_name,bitrh_date,job,count")] researcher researcher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(researcher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(researcher);
        }

        // GET: researchers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            researcher researcher = db.researcher.Find(id);
            if (researcher == null)
            {
                return HttpNotFound();
            }
            return View(researcher);
        }

        // POST: researchers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            researcher researcher = db.researcher.Find(id);
            db.researcher.Remove(researcher);
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
