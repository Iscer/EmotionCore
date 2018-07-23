using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmotionCore.Models;

namespace EmotionCore.Controllers
{
    [Produces("application/json")]
    [Route("api/EmoPicturesAPI")]
    public class EmoPicturesAPIController : Controller
    {
        private readonly EmotionCoreContext _context;

        public EmoPicturesAPIController(EmotionCoreContext context)
        {
            _context = context;
        }

        // GET: api/EmoPicturesAPI
        [HttpGet]
        public IEnumerable<EmoPicture> GetEmoPictures()
        {
            return _context.EmoPictures.Include(f => f.Faces);
        }

        // GET: api/EmoPicturesAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmoPicture([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emoPicture = await _context.EmoPictures.SingleOrDefaultAsync(m => m.Id == id);

            if (emoPicture == null)
            {
                return NotFound();
            }

            return Ok(emoPicture);
        }

        // PUT: api/EmoPicturesAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmoPicture([FromRoute] int id, [FromBody] EmoPicture emoPicture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emoPicture.Id)
            {
                return BadRequest();
            }

            _context.Entry(emoPicture).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmoPictureExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EmoPicturesAPI
        [HttpPost]
        public async Task<IActionResult> PostEmoPicture([FromBody] EmoPicture emoPicture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.EmoPictures.Add(emoPicture);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmoPicture", new { id = emoPicture.Id }, emoPicture);
        }

        // DELETE: api/EmoPicturesAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmoPicture([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emoPicture = await _context.EmoPictures.SingleOrDefaultAsync(m => m.Id == id);
            if (emoPicture == null)
            {
                return NotFound();
            }

            _context.EmoPictures.Remove(emoPicture);
            await _context.SaveChangesAsync();

            return Ok(emoPicture);
        }

        private bool EmoPictureExists(int id)
        {
            return _context.EmoPictures.Any(e => e.Id == id);
        }
    }
}