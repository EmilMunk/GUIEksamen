using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FrokostKlub.DAL;
using FrokostKlub.Models;

namespace FrokostKlub.Controllers
{
    public class MembersController : Controller
    {
        private MemberContext db = new MemberContext();

        // GET: Members
        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }

        public ActionResult Meetings(int? id)
        {
            var lunchMeetings = from p in db.LunchMeetings.Include(l => l.Members) where p.MemberId == id select p;
            return View("~/Views/LunchMeetings/Index.cshtml", lunchMeetings.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberId,Name,PhoneNr")] Member member)
        {
            DateTime addedDateTime = new DateTime();
            if (ModelState.IsValid)
            {
                List<LunchMeeting> tempList = (from p in db.LunchMeetings select p).ToList();
                db.Members.Add(member);
                db.SaveChanges();
                foreach (var i in tempList)
                {
                    if (((i.Date.DayOfWeek != DayOfWeek.Saturday) && (i.Date.DayOfWeek != DayOfWeek.Sunday)) && addedDateTime != i.Date)
                    {
                        addedDateTime = i.Date;
                        db.LunchMeetings.Add(new LunchMeeting() {Date = i.Date, Members = member});
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberId,Name,PhoneNr")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
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
