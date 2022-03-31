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
        public ActionResult Index(string searchString)
        {
            var test_instance = db.test_instance.Include(t => t.child).Include(t => t.test);
            if (!String.IsNullOrEmpty(searchString))
            {
                test_instance = test_instance.Where(s => s.test.name.Contains(searchString));
            }
            return View(test_instance.ToList());
        }
                
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
            var answers = db.answer.Include(a => a.image).Include(a => a.question).Include(a => a.test_instance);
            answers = answers.Where(a => a.test_ins_id == test_instance.id);
            test_insAnswer test_InsAnswer = new test_insAnswer { test_Instance = test_instance, answers = answers };
            return View(test_InsAnswer);
        }

        public ActionResult DetailsResult(int? id)
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
            var answers = db.answer.Include(a => a.image).Include(a => a.question).Include(a => a.test_instance);
            answers = answers.Where(a => a.test_ins_id == test_instance.id);
            test_insAnswer test_InsAnswer = new test_insAnswer { test_Instance = test_instance, answers = answers };
            return View(test_InsAnswer);
        }

        // GET: test_instance/Create
        public ActionResult Create(int? id)
        {
            ViewBag.child_id = new SelectList(db.child, "id", "login");
            var test = db.test.Where(q => q.id == id);
            ViewBag.test_id = new SelectList(test, "id", "name");
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
                child child = db.child.First(q => q.login == User.Identity.Name);
                if (child != null)
                    test_instance.child_id = child.id;
                test_instance.start_time = DateTime.Now;
                db.test_instance.Add(test_instance);
                db.SaveChanges();
                var question = db.question.Where(q => q.test_id == test_instance.test_id).OrderBy(q => q.id).First();
                return RedirectToAction("Create", "Answers", new { test_ins_id = test_instance.id, q_id = question.id });
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
        public ActionResult Edit([Bind(Include = "result")] test_instance test_instance)
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
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Результаты эксперимента");

            var test_instance = db.test_instance.Where(q => q.test_id == 1);

            worksheet.Cell("A" + 1).Value = "Идентификатор ребенка";
            worksheet.Cell("B" + 1).Value = "Начало тестирования";
            worksheet.Cell("C" + 1).Value = "Завершение тестирования";
            worksheet.Cell("D" + 1).Value = "Эксперимент";

            int i = 2;
            foreach(var test_Instance in test_instance)            
            {
                worksheet.Cell("A" + i).Value = test_Instance.child_id;
                string start = ((DateTime)test_Instance.start_time).ToString("HH:mm dd.MM.yyyy").ToString();
                worksheet.Cell("B" + i).Value = start;
                string end = ((DateTime)test_Instance.end_time).ToString("HH:mm dd.MM.yyyy").ToString();
                worksheet.Cell("C" + i).Value = end;
                worksheet.Cell("D" + i).Value = test_Instance.test.name;
                i++;
            }

            var rngTable = worksheet.Range("A1:D" + i);
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            test test = db.test.Where(q => q.id == 1).First();


            worksheet.Columns().AdjustToContents();

            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{test.name}_results.xlsx");
            }
        }
        public FileResult exportChildResultFull(int id)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Результаты эксперимента");

            test_instance test_Instance = db.test_instance.Where(q => q.id == id).First();

            worksheet.Cell("A" + 1).Value = "Идентификатор ребенка";
            worksheet.Cell("A" + 2).Value = test_Instance.child_id;
            worksheet.Cell("B" + 1).Value = "Начало тестирования";
            string start = ((DateTime)test_Instance.start_time).ToString("HH:mm dd.MM.yyyy").ToString();
            worksheet.Cell("B" + 2).Value = start;
            worksheet.Cell("C" + 1).Value = "Завершение тестирования";
            string end = ((DateTime)test_Instance.end_time).ToString("HH:mm dd.MM.yyyy").ToString();
            worksheet.Cell("C" + 2).Value = end;
            worksheet.Cell("D" + 1).Value = "Эксперимент";
            worksheet.Cell("D" + 2).Value = test_Instance.test.name;

            var rngTable = worksheet.Range("A1:D" + 2);
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            worksheet.Cell("A" + 4).Value = "Вопрос";
            worksheet.Cell("B" + 4).Value = "Ответ";

            var answers = db.answer.Where(q => q.test_ins_id == test_Instance.id);
            int i = 5;
            foreach(answer answer in answers)
            {
                worksheet.Cell("A" + i).Value = answer.question.name;
                worksheet.Cell("B" + i).Value = answer.image_id;
                i++;
            }

            rngTable = worksheet.Range("A4:B" + i);
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            worksheet.Columns().AdjustToContents(); 

            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{test_Instance.id}_{test_Instance.test.name}_results.xlsx");
            }
        }

        public FileResult export(string SearchString)
        {
            string s = SearchString;
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Результаты эксперимента");

            

            worksheet.Cell("A" + 1).Value = "s";

            var rngTable = worksheet.Range("A1:D" + 2);
            rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            worksheet.Cell("A" + 4).Value = "Вопрос";
            worksheet.Cell("B" + 4).Value = "Ответ";


            worksheet.Columns().AdjustToContents();

            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"1_results.xlsx");
            }
        }
    }
}
