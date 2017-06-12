using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.DAL;
using Web.Models;

namespace Web.Controllers
{
    public class BackLogWebController : Controller
    {
        private Context db = new Context();

        // GET: BackLogWeb
        public ActionResult Index()
        {
            var returnListe = db.BackLogs.OrderBy(o => o.Priority).ToList();

            return View(returnListe);
        }

        // GET: BackLogWeb/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BackLog backLog = db.BackLogs.Find(id);
            if (backLog == null)
            {
                return HttpNotFound();
            }
            return View(backLog);
        }

        // GET: BackLogWeb/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BackLogWeb/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BackLogId,Description,Priority,EstimatedTime,Responsible,States")] BackLog backLog)
        {
            if (ModelState.IsValid)
            {
                backLog.States = BackLog.State.IsToDo;
                db.BackLogs.Add(backLog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(backLog);
        }

        // GET: BackLogWeb/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BackLog backLog = db.BackLogs.Find(id);
            if (backLog == null)
            {
                return HttpNotFound();
            }
            return View(backLog);
        }

        // POST: BackLogWeb/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BackLogId,Description,Priority,EstimatedTime,Responsible,States")] BackLog backLog)
        {
            if (ModelState.IsValid)
            {
                //backLog.States = BackLog.State.IsToDo;
                db.Entry(backLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(backLog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("BackLogWeb/SumbitNewState/{id}")]
        public ActionResult SumbitNewState(BackLog.State newState, int? id)
        {
            BackLog backLog = new BackLog();
            if (ModelState.IsValid)
            {
                //backLog.States = BackLog.State.IsToDo;
                backLog = db.BackLogs.Find(id);
                backLog.States = newState;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        // GET: BackLogWeb/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BackLog backLog = db.BackLogs.Find(id);
            if (backLog == null)
            {
                return HttpNotFound();
            }
            return View(backLog);
        }

        // POST: BackLogWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BackLog backLog = db.BackLogs.Find(id);
            db.BackLogs.Remove(backLog);
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
