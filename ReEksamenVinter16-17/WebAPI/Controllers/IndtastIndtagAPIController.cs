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
using WebAPI.DAL;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class IndtastIndtagAPIController : ApiController
    {
        private KalorieIndholdDbContext db = new KalorieIndholdDbContext();

        // GET: api/IndtastIndtagAPI
        public IQueryable<Indtag> GetIndtastIndtag()
        {
            return db.IndtastIndtag;
        }

        // GET: api/IndtastIndtagAPI/5
        [ResponseType(typeof(Indtag))]
        public IHttpActionResult GetIndtag(int id)
        {
            Indtag indtag = db.IndtastIndtag.Find(id);
            if (indtag == null)
            {
                return NotFound();
            }

            return Ok(indtag);
        }

        // PUT: api/IndtastIndtagAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutIndtag(int id, Indtag indtag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != indtag.ID)
            {
                return BadRequest();
            }

            db.Entry(indtag).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IndtagExists(id))
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

        // POST: api/IndtastIndtagAPI
        [ResponseType(typeof(Indtag))]
        public IHttpActionResult PostIndtag(Indtag indtag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.IndtastIndtag.Add(indtag);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = indtag.ID }, indtag);
        }

        // DELETE: api/IndtastIndtagAPI/5
        [ResponseType(typeof(Indtag))]
        public IHttpActionResult DeleteIndtag(int id)
        {
            Indtag indtag = db.IndtastIndtag.Find(id);
            if (indtag == null)
            {
                return NotFound();
            }

            db.IndtastIndtag.Remove(indtag);
            db.SaveChanges();

            return Ok(indtag);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IndtagExists(int id)
        {
            return db.IndtastIndtag.Count(e => e.ID == id) > 0;
        }
    }
}