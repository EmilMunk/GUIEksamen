using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAPI.DAL;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class KalorieIndtagWebController : Controller
    {
        private KalorieIndholdDbContext db = new KalorieIndholdDbContext();

        // GET: KalorieIndtagWeb
        public ActionResult Index()
        {
            return View(db.Indtag.ToList());
        }

        // GET: KalorieIndtagWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KalorieIndtag kalorieIndtag = db.Indtag.Find(id);
            if (kalorieIndtag == null)
            {
                return HttpNotFound();
            }
            return View(kalorieIndtag);
        }

        // GET: KalorieIndtagWeb/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KalorieIndtagWeb/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Day,Indtag")] KalorieIndtag kalorieIndtag)
        {
            if (ModelState.IsValid)
            {
                db.Indtag.Add(kalorieIndtag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kalorieIndtag);
        }

        // GET: KalorieIndtagWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KalorieIndtag kalorieIndtag = db.Indtag.Find(id);
            if (kalorieIndtag == null)
            {
                return HttpNotFound();
            }
            return View(kalorieIndtag);
        }

        // POST: KalorieIndtagWeb/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Day,Indtag")] KalorieIndtag kalorieIndtag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kalorieIndtag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kalorieIndtag);
        }

        // GET: KalorieIndtagWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KalorieIndtag kalorieIndtag = db.Indtag.Find(id);
            if (kalorieIndtag == null)
            {
                return HttpNotFound();
            }
            return View(kalorieIndtag);
        }

        // POST: KalorieIndtagWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KalorieIndtag kalorieIndtag = db.Indtag.Find(id);
            db.Indtag.Remove(kalorieIndtag);
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
