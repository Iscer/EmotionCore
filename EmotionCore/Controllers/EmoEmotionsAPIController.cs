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
    [Route("api/EmoEmotionsAPI")]
    public class EmoEmotionsAPIController : Controller
    {
        private readonly EmotionCoreContext _context;

        public EmoEmotionsAPIController(EmotionCoreContext context)
        {
            _context = context;
        }

        // GET: api/EmoEmotionsAPI
        [HttpGet]
        public IEnumerable<EmoEmotion> GetEmoEmotions()
        {
            return _context.EmoEmotions;
        }

        // GET: api/EmoEmotionsAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmoEmotion([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emoEmotion = await _context.EmoEmotions.SingleOrDefaultAsync(m => m.Id == id);

            if (emoEmotion == null)
            {
                return NotFound();
            }

            return Ok(emoEmotion);
        }

        // PUT: api/EmoEmotionsAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmoEmotion([FromRoute] int id, [FromBody] EmoEmotion emoEmotion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emoEmotion.Id)
            {
                return BadRequest();
            }

            _context.Entry(emoEmotion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmoEmotionExists(id))
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

        // POST: api/EmoEmotionsAPI
        [HttpPost]
        public async Task<IActionResult> PostEmoEmotion([FromBody] EmoEmotion emoEmotion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.EmoEmotions.Add(emoEmotion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmoEmotion", new { id = emoEmotion.Id }, emoEmotion);
        }

        // DELETE: api/EmoEmotionsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmoEmotion([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emoEmotion = await _context.EmoEmotions.SingleOrDefaultAsync(m => m.Id == id);
            if (emoEmotion == null)
            {
                return NotFound();
            }

            _context.EmoEmotions.Remove(emoEmotion);
            await _context.SaveChangesAsync();

            return Ok(emoEmotion);
        }

        private bool EmoEmotionExists(int id)
        {
            return _context.EmoEmotions.Any(e => e.Id == id);
        }
    }
}