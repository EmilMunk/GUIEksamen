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
    public class IndtastIndtagController : Controller
    {
        private KalorieIndholdDbContext db = new KalorieIndholdDbContext();

        // GET: IndtastIndtag
        public ActionResult Index()
        {
            var liste = db.IndtastIndtag.ToList();

            double total = 0;

            foreach (var i in liste)
                total += i.Kalorier;

            ViewBag.TotalAmount = total;

            var IndexListe = (from l in db.IndtastIndtag
                where l.Day == DateTime.Today
                select l
            ).ToList();

            return View(db.IndtastIndtag.ToList());
        }

        // GET: IndtastIndtag/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Indtag indtag = db.IndtastIndtag.Find(id);
            if (indtag == null)
            {
                return HttpNotFound();
            }
            return View(indtag);
        }

        // GET: IndtastIndtag/Create
        public ActionResult Create()
        {
            //ViewBag.ListeAfMadvarer = db.Kalorier.ToList();
            Indtag indtag = new Indtag();
            List<KalorieIndhold> mad = db.Kalorier.ToList();
            List<string> types = new List<string>();
            foreach (var i in mad)
                types.Add(i.MadVare);

            ViewData["types"] = new SelectList(types, indtag.MadVare);

          return View(indtag);
        }

        // POST: IndtastIndtag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Day,MadVare,Amount")] Indtag indtag)
        {
            if (ModelState.IsValid)
            {
                indtag.Day = DateTime.Today;
                var kcal = (from k in db.Kalorier
                    where k.MadVare == indtag.MadVare
                    select k.Energi).First();

                indtag.Kalorier = double.Parse(kcal) * indtag.Amount /100;

                db.IndtastIndtag.Add(indtag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(indtag);
        }

        // GET: IndtastIndtag/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Indtag indtag = db.IndtastIndtag.Find(id);
            if (indtag == null)
            {
                return HttpNotFound();
            }
            return View(indtag);
        }

        // POST: IndtastIndtag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Day,MadVare,Amount")] Indtag indtag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(indtag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(indtag);
        }

        // GET: IndtastIndtag/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Indtag indtag = db.IndtastIndtag.Find(id);
            if (indtag == null)
            {
                return HttpNotFound();
            }
            return View(indtag);
        }

        // POST: IndtastIndtag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Indtag indtag = db.IndtastIndtag.Find(id);
            db.IndtastIndtag.Remove(indtag);
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
