using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Quote.Models;

namespace Quote.Controllers
{
    public class QuotationController : Controller
    {
        private QuotationContext db = new QuotationContext();

        // GET: /Quotation/
        public ActionResult Index(string filter)
        {
            var quotations = db.Quotations.Include(q => q.Category);

            if (!string.IsNullOrEmpty(filter))
            {
                var fdb = (from x in quotations
                           where x.Author.ToLower().Contains(filter.ToLower())
                           || x.Category.Name.ToLower().Contains(filter.ToLower())
                           || x.Quote.ToLower().Contains(filter.ToLower())
                           orderby x.Author
                           select x).ToList();

                return View(fdb.ToList());
            }
            else
            {
                return View(quotations.ToList());
            }
           
        }

        // GET: /Quotation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }
            return View(quotation);
        }

        // GET: /Quotation/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "name");
            return View();
        }

        // POST: /Quotation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="QuotationID,Quote,Author,Date,CategoryID")] Quotation quotation)
        {
            if (ModelState.IsValid)
            {
                db.Quotations.Add(quotation);
               
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "name", quotation.CategoryID);
            return View(quotation);
        }

        // GET: /Quotation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "name", quotation.CategoryID);
            return View(quotation);
        }

        // POST: /Quotation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="QuotationID,Quote,Author,Date,CategoryID")] Quotation quotation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quotation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "name", quotation.CategoryID);
            return View(quotation);
        }

        // GET: /Quotation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }
            return View(quotation);
        }

        // POST: /Quotation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Quotation quotation = db.Quotations.Find(id);
            db.Quotations.Remove(quotation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CreateCategory()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory([Bind(Include = "CategoryID,Name")]Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", category.CategoryID);
            return View(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool DoesExist(String name)
        {
            if (db.Categories.SingleOrDefault(x => x.Name == name) == null)
                return true;
            else
                return false;
        }

        public ActionResult Hide(int id)
        {
            var cookie = Request.Cookies.Get("HideCookie");
            List<string> hideItems;

            if (cookie == null)
            {
                cookie = new HttpCookie("HideCookie");
                hideItems = new List<string>() { id.ToString() };
                cookie.Value = id.ToString();
                cookie.Expires = DateTime.Now.AddYears(1);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                cookie = Request.Cookies.Get("HideCookie");
                hideItems = cookie.Value.Split(',').ToList();
                hideItems.Add(id.ToString());
                cookie.Value = string.Join(",", hideItems);
            }

            var quotes = from q in db.Quotations where !hideItems.Select(x => int.Parse(x)).Contains(q.QuotationID) select q;

            return View(quotes.ToList());
        }
    }
}
