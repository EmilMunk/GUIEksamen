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
    public class KalorieIndholdsController : ApiController
    {
        private KalorieIndholdDbContext db = new KalorieIndholdDbContext();

        // GET: api/KalorieIndholds
        public IQueryable<KalorieIndhold> GetKalorier()
        {
            return db.Kalorier;
        }

        // GET: api/KalorieIndholds/5
        [ResponseType(typeof(KalorieIndhold))]
        public IHttpActionResult GetKalorieIndhold(int id)
        {
            KalorieIndhold kalorieIndhold = db.Kalorier.Find(id);
            if (kalorieIndhold == null)
            {
                return NotFound();
            }

            return Ok(kalorieIndhold);
        }

        // PUT: api/KalorieIndholds/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutKalorieIndhold(int id, KalorieIndhold kalorieIndhold)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kalorieIndhold.ID)
            {
                return BadRequest();
            }

            db.Entry(kalorieIndhold).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KalorieIndholdExists(id))
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

        // POST: api/KalorieIndholds
        [ResponseType(typeof(KalorieIndhold))]
        public IHttpActionResult PostKalorieIndhold(KalorieIndhold kalorieIndhold)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Kalorier.Add(kalorieIndhold);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = kalorieIndhold.ID }, kalorieIndhold);
        }

        // DELETE: api/KalorieIndholds/5
        [ResponseType(typeof(KalorieIndhold))]
        public IHttpActionResult DeleteKalorieIndhold(int id)
        {
            KalorieIndhold kalorieIndhold = db.Kalorier.Find(id);
            if (kalorieIndhold == null)
            {
                return NotFound();
            }

            db.Kalorier.Remove(kalorieIndhold);
            db.SaveChanges();

            return Ok(kalorieIndhold);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KalorieIndholdExists(int id)
        {
            return db.Kalorier.Count(e => e.ID == id) > 0;
        }
    }
}