using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmotionCore.Models;

namespace EmotionCore.Controllers
{
    public class EmoEmotionsController : Controller
    {
        private readonly EmotionCoreContext _context;

        public EmoEmotionsController(EmotionCoreContext context)
        {
            _context = context;
        }

        // GET: EmoEmotions
        public async Task<IActionResult> Index()
        {
            var emotionCoreContext = _context.EmoEmotions.Include(e => e.Face);
            return View(await emotionCoreContext.ToListAsync());
        }

        // GET: EmoEmotions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emoEmotion = await _context.EmoEmotions
                .Include(e => e.Face)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (emoEmotion == null)
            {
                return NotFound();
            }

            return View(emoEmotion);
        }

        // GET: EmoEmotions/Create
        public IActionResult Create()
        {
            ViewData["EmoFaceId"] = new SelectList(_context.EmoFaces, "Id", "Id");
      
            return View();
        }

        // POST: EmoEmotions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Score,EmoFaceId,EmotionType")] EmoEmotion emoEmotion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emoEmotion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmoFaceId"] = new SelectList(_context.EmoFaces, "Id", "Id", emoEmotion.EmoFaceId);
            return View(emoEmotion);
        }

        // GET: EmoEmotions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emoEmotion = await _context.EmoEmotions.SingleOrDefaultAsync(m => m.Id == id);
            if (emoEmotion == null)
            {
                return NotFound();
            }
            ViewData["EmoFaceId"] = new SelectList(_context.EmoFaces, "Id", "Id", emoEmotion.EmoFaceId);
            return View(emoEmotion);
        }

        // POST: EmoEmotions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Score,EmoFaceId,EmotionType")] EmoEmotion emoEmotion)
        {
            if (id != emoEmotion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emoEmotion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmoEmotionExists(emoEmotion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmoFaceId"] = new SelectList(_context.EmoFaces, "Id", "Id", emoEmotion.EmoFaceId);
            return View(emoEmotion);
        }

        // GET: EmoEmotions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emoEmotion = await _context.EmoEmotions
                .Include(e => e.Face)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (emoEmotion == null)
            {
                return NotFound();
            }

            return View(emoEmotion);
        }

        // POST: EmoEmotions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emoEmotion = await _context.EmoEmotions.SingleOrDefaultAsync(m => m.Id == id);
            _context.EmoEmotions.Remove(emoEmotion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmoEmotionExists(int id)
        {
            return _context.EmoEmotions.Any(e => e.Id == id);
        }
    }
}
