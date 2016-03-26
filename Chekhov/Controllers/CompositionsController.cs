using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Chekhov;
using Microsoft.AspNet.Identity;

namespace Chekhov.Controllers
{
    public class CompositionsController : ODataController
    {
        private ChekhovContext db = new ChekhovContext();

        // GET: odata/Compositions
        [Authorize, EnableQuery]
        public IQueryable<Composition> GetCompositions()
        {
            var id = User.Identity.GetUserId();
            return db.Compositions.Where(c => c.UserId == id).OrderBy(c => -c.Id);
        }

        // GET: odata/Compositions(5)
        [EnableQuery]
        public SingleResult<Composition> GetComposition([FromODataUri] long key)
        {
            return SingleResult.Create(db.Compositions.Where(composition => composition.Id == key));
        }

        // PUT: odata/Compositions(5)
        public IHttpActionResult Put([FromODataUri] long key, Delta<Composition> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Composition composition = db.Compositions.Find(key);
            if (composition == null)
            {
                return NotFound();
            }

            patch.Put(composition);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompositionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(composition);
        }

        // POST: odata/Compositions
        [Authorize]
        public IHttpActionResult Post(Composition composition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            composition.UserId = User.Identity.GetUserId();
            db.Compositions.Add(composition);
            db.SaveChanges();

            return Created(composition);
        }

        // PATCH: odata/Compositions(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] long key, Delta<Composition> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Composition composition = db.Compositions.Find(key);
            if (composition == null)
            {
                return NotFound();
            }

            patch.Patch(composition);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompositionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(composition);
        }

        // DELETE: odata/Compositions(5)
        public IHttpActionResult Delete([FromODataUri] long key)
        {
            Composition composition = db.Compositions.Find(key);
            if (composition == null)
            {
                return NotFound();
            }

            db.Compositions.Remove(composition);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompositionExists(long key)
        {
            return db.Compositions.Count(e => e.Id == key) > 0;
        }
    }
}
