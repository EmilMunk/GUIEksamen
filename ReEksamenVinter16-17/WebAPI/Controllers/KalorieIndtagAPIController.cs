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
    public class KalorieIndtagAPIController : ApiController
    {
        private KalorieIndholdDbContext db = new KalorieIndholdDbContext();

        // GET: api/KalorieIndtagAPI
        public IQueryable<KalorieIndtag> GetIndtag()
        {
            return db.Indtag;
        }

        // GET: api/KalorieIndtagAPI/5
        [ResponseType(typeof(KalorieIndtag))]
        public IHttpActionResult GetKalorieIndtag(int id)
        {
            KalorieIndtag kalorieIndtag = db.Indtag.Find(id);
            if (kalorieIndtag == null)
            {
                return NotFound();
            }

            return Ok(kalorieIndtag);
        }

        // PUT: api/KalorieIndtagAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutKalorieIndtag(int id, KalorieIndtag kalorieIndtag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kalorieIndtag.ID)
            {
                return BadRequest();
            }

            db.Entry(kalorieIndtag).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KalorieIndtagExists(id))
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

        // POST: api/KalorieIndtagAPI
        [ResponseType(typeof(KalorieIndtag))]
        public IHttpActionResult PostKalorieIndtag(KalorieIndtag kalorieIndtag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Indtag.Add(kalorieIndtag);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = kalorieIndtag.ID }, kalorieIndtag);
        }

        // DELETE: api/KalorieIndtagAPI/5
        [ResponseType(typeof(KalorieIndtag))]
        public IHttpActionResult DeleteKalorieIndtag(int id)
        {
            KalorieIndtag kalorieIndtag = db.Indtag.Find(id);
            if (kalorieIndtag == null)
            {
                return NotFound();
            }

            db.Indtag.Remove(kalorieIndtag);
            db.SaveChanges();

            return Ok(kalorieIndtag);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KalorieIndtagExists(int id)
        {
            return db.Indtag.Count(e => e.ID == id) > 0;
        }
    }
}