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
    [Route("api/EmoFacesAPI")]
    public class EmoFacesAPIController : Controller
    {
        private readonly EmotionCoreContext _context;

        public EmoFacesAPIController(EmotionCoreContext context)
        {
            _context = context;
        }

        // GET: api/EmoFacesAPI
        [HttpGet]
        public IEnumerable<EmoFace> GetEmoFaces()
        {
            return _context.EmoFaces.Include(e => e.Emotions);
        }

        // GET: api/EmoFacesAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmoFace([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emoFace = await _context.EmoFaces.SingleOrDefaultAsync(m => m.Id == id);

            if (emoFace == null)
            {
                return NotFound();
            }

            return Ok(emoFace);
        }

        // PUT: api/EmoFacesAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmoFace([FromRoute] int id, [FromBody] EmoFace emoFace)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emoFace.Id)
            {
                return BadRequest();
            }

            _context.Entry(emoFace).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmoFaceExists(id))
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

        // POST: api/EmoFacesAPI
        [HttpPost]
        public async Task<IActionResult> PostEmoFace([FromBody] EmoFace emoFace)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.EmoFaces.Add(emoFace);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmoFace", new { id = emoFace.Id }, emoFace);
        }

        // DELETE: api/EmoFacesAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmoFace([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emoFace = await _context.EmoFaces.SingleOrDefaultAsync(m => m.Id == id);
            if (emoFace == null)
            {
                return NotFound();
            }

            _context.EmoFaces.Remove(emoFace);
            await _context.SaveChangesAsync();

            return Ok(emoFace);
        }

        private bool EmoFaceExists(int id)
        {
            return _context.EmoFaces.Any(e => e.Id == id);
        }
    }
}