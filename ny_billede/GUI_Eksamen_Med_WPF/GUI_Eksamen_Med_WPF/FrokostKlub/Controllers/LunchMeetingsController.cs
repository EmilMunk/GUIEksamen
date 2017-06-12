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
    public class LunchMeetingsController : Controller
    {
        private MemberContext db = new MemberContext();

        // GET: LunchMeetings
        public ActionResult Index()
        {
            DateTime addDate = new DateTime();
            List<LunchMeeting> tempList = new List<LunchMeeting>();
            var lunchMeetings = from p in db.LunchMeetings.Include(l => l.Members) orderby p.Date, p.Date select p;
            foreach (var i in lunchMeetings.ToList())
            {
                if (i.Date != addDate)
                {
                    addDate = i.Date;
                    tempList.Add(i);
                }
            }
            return View(tempList);
        }

        // GET: LunchMeetings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LunchMeeting lunchMeeting = db.LunchMeetings.Find(id);
            var memberlist = from p in db.LunchMeetings where p.MeetingName == lunchMeeting.MeetingName select p;
            

            if (lunchMeeting == null)
            {
                return HttpNotFound();
            }
            return View(memberlist.ToList());
        }

        // GET: LunchMeetings/Create
        public ActionResult Create()
        {
            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "Name");
            return View();
        }

        // POST: LunchMeetings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MeetingId,Date,MemberId,MeetingName")] LunchMeeting lunchMeeting)
        {
            if (ModelState.IsValid)
            {
                if (lunchMeeting.MemberId == 0)
                {
                    foreach (var member in db.Members)
                    {
                        db.LunchMeetings.Add(new LunchMeeting() {Date = lunchMeeting.Date, MeetingName = lunchMeeting.MeetingName, Members = member});
                    }
                }
                else
                {
                    db.LunchMeetings.Add(lunchMeeting);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "Name", lunchMeeting.MemberId);
            return View(lunchMeeting);
        }

        // GET: LunchMeetings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LunchMeeting lunchMeeting = db.LunchMeetings.Find(id);
            if (lunchMeeting == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "Name", lunchMeeting.MemberId);
            return View(lunchMeeting);
        }

        // POST: LunchMeetings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MeetingId,Date,MemberId")] LunchMeeting lunchMeeting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lunchMeeting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "Name", lunchMeeting.MemberId);
            return View(lunchMeeting);
        }

        // GET: LunchMeetings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LunchMeeting lunchMeeting = db.LunchMeetings.Find(id);
            if (lunchMeeting == null)
            {
                return HttpNotFound();
            }
            return View(lunchMeeting);
        }

        public ActionResult DontAttend(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LunchMeeting lunchMeeting = db.LunchMeetings.Find(id);
            if (lunchMeeting == null)
            {
                return HttpNotFound();
            }
            return View(lunchMeeting);
        }

        // POST: LunchMeetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LunchMeeting lunchMeeting = db.LunchMeetings.Find(id);
            var deleteList = from p in db.LunchMeetings where p.MeetingName == lunchMeeting.MeetingName select p;
            foreach (var i in deleteList.ToList())
            {
                db.LunchMeetings.Remove(i);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("DontAttend")]
        [ValidateAntiForgeryToken]
        public ActionResult DontAttendConfirmed(int id)
        {
            LunchMeeting lunchMeeting = db.LunchMeetings.Find(id);
            db.LunchMeetings.Remove(lunchMeeting);
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
