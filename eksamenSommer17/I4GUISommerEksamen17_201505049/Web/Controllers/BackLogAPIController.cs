using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Web.DAL;
using Web.Models;

namespace Web.Controllers
{
    public class BackLogAPIController : ApiController
    {
        private Context db = new Context();

        // GET: api/BackLogAPI
        public IQueryable<BackLog> GetBackLogs()
        {
            return db.BackLogs;
        }

        // GET: api/BackLogAPI/5
        [ResponseType(typeof(BackLog))]
        public IHttpActionResult GetBackLog(int id)
        {
            BackLog backLog = db.BackLogs.Find(id);
            if (backLog == null)
            {
                return NotFound();
            }

            return Ok(backLog);
        }

        // PUT: api/BackLogAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBackLog(int id, BackLog backLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != backLog.BackLogId)
            {
                return BadRequest();
            }

            db.Entry(backLog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BackLogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BackLogAPI
        [ResponseType(typeof(BackLog))]
        public IHttpActionResult PostBackLog(BackLog backLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BackLogs.Add(backLog);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = backLog.BackLogId }, backLog);
        }

        // DELETE: api/BackLogAPI/5
        [ResponseType(typeof(BackLog))]
        public IHttpActionResult DeleteBackLog(int id)
        {
            BackLog backLog = db.BackLogs.Find(id);
            if (backLog == null)
            {
                return NotFound();
            }

            db.BackLogs.Remove(backLog);
            db.SaveChanges();

            return Ok(backLog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BackLogExists(int id)
        {
            return db.BackLogs.Count(e => e.BackLogId == id) > 0;
        }
    }
}