using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using kr.Models;
using Rotativa;

namespace kr.Controllers
{
    public class test_instanceController : Controller
    {
        private dbEntities db = new dbEntities();

        // GET: test_instance
        public ActionResult Index()
        {
            var test_instance = db.test_instance.Include(t => t.child).Include(t => t.test);
            return View(test_instance.ToList());
        }

        // GET: test_instance/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            test_instance test_instance = db.test_instance.Find(id);
            if (test_instance == null)
            {
                return HttpNotFound();
            }
            return View(test_instance);
        }

        // GET: test_instance/Create
        public ActionResult Create()
        {
            ViewBag.child_id = new SelectList(db.child, "id", "login");
            ViewBag.test_id = new SelectList(db.test, "id", "name");
            return View();
        }

        // POST: test_instance/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,test_id,child_id,start_time,end_time,result")] test_instance test_instance)
        {
            if (ModelState.IsValid)
            {
                test_instance.start_time = DateTime.Now;
                db.test_instance.Add(test_instance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.child_id = new SelectList(db.child, "id", "login", test_instance.child_id);
            ViewBag.test_id = new SelectList(db.test, "id", "name", test_instance.test_id);
            return View(test_instance);
        }

        // GET: test_instance/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            test_instance test_instance = db.test_instance.Find(id);
            if (test_instance == null)
            {
                return HttpNotFound();
            }
            ViewBag.child_id = new SelectList(db.child, "id", "login", test_instance.child_id);
            ViewBag.test_id = new SelectList(db.test, "id", "name", test_instance.test_id);
            return View(test_instance);
        }

        // POST: test_instance/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. 
        // Дополнительные сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,test_id,child_id,start_time,end_time,result")] test_instance test_instance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(test_instance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.child_id = new SelectList(db.child, "id", "login", test_instance.child_id);
            ViewBag.test_id = new SelectList(db.test, "id", "name", test_instance.test_id);
            return View(test_instance);
        }

        // GET: test_instance/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            test_instance test_instance = db.test_instance.Find(id);
            if (test_instance == null)
            {
                return HttpNotFound();
            }
            return View(test_instance);
        }

        // POST: test_instance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            test_instance test_instance = db.test_instance.Find(id);
            db.test_instance.Remove(test_instance);
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

        public ActionResult ExportPersonalInfoPDF(int id)
        {
            test_instance test_Instance = db.test_instance.First(q => q.id == id);
            child child = db.child.First(q => q.id == test_Instance.child_id);
            return new ActionAsPdf("Details", new { id = test_Instance.id })
            {
                FileName = $"Result_{child.surname}_{child.name.ElementAt(0)}_{child.patronymic_name.ElementAt(0)}.pdf"
            };
        }
        public ActionResult exportResults()
        {
            ViewBag.gender = new SelectList(db.gender, "id", "name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult exportResults([Bind(Include = "id,name")] gender gender, string year)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var results = db.test_instance.Include(t => t.child).Include(t => t.test);
                //if (gender != null && !string.IsNullOrEmpty(year))
                //    results = results.Where(q => q.child.gender == gender.id && q.child.birth_date.Value.Year == int.Parse(year));
                //else if (gender != null)
                //    results = results.Where(q => q.child.gender == gender.id);
                //else
                //{
                //    try
                //    {
                //        results = results.Where(q => q.child.birth_date.Value.Year == int.Parse(year));
                //    }
                //    catch { }
                //}
                var worksheet = workbook.Worksheets.Add("Результаты");

                worksheet.Cell("A1").Value = "ФИО";
                worksheet.Cell("B1").Value = "Дата рождения";
                worksheet.Cell("C1").Value = "Начало тестирования";
                worksheet.Cell("D1").Value = "Окончание тестирования";
                worksheet.Cell("E1").Value = "Эксперимент";
                worksheet.Row(1).Style.Font.Bold = true;

                int i = 2;
                foreach(var result in results)
                {
                    worksheet.Cell(i, 1).Value = result.child.surname.ToString() + " " + result.child.name.ToString() + " " + result.child.patronymic_name.ToString();
                    worksheet.Cell(i, 2).Value = ((DateTime)result.child.birth_date).ToString("dd.MM.yyyy").ToString();
                    worksheet.Cell(i, 3).Value = ((DateTime)result.start_time).ToString("HH:mm dd.MM.yyyy").ToString();
                    worksheet.Cell(i, 4).Value = ((DateTime)result.end_time).ToString("HH:mm dd.MM.yyyy").ToString();
                    worksheet.Cell(i, 5).Value = result.test.name;
                    i++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                           {
                                FileDownloadName = $"results_{DateTime.Now.ToString("dd.MM.yyyy")}.xlsx"
                           };
                }
            }
        }
    }
}
