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
    public class EmoFacesController : Controller
    {
        private readonly EmotionCoreContext _context;

        public EmoFacesController(EmotionCoreContext context)
        {
            _context = context;
        }

        // GET: EmoFaces
        public async Task<IActionResult> Index()
        {
            var emotionCoreContext = _context.EmoFaces.Include(e => e.Picture);
            return View(await emotionCoreContext.ToListAsync());
        }

        // GET: EmoFaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emoFace = await _context.EmoFaces
                .Include(e => e.Picture)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (emoFace == null)
            {
                return NotFound();
            }

            return View(emoFace);
        }

        // GET: EmoFaces/Create
        public IActionResult Create()
        {
            ViewData["EmoPictureId"] = new SelectList(_context.EmoPictures, "Id", "Name");
            return View();
        }

        // POST: EmoFaces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmoPictureId,X,Y,Width,Height")] EmoFace emoFace)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emoFace);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmoPictureId"] = new SelectList(_context.EmoPictures, "Id", "Id", emoFace.EmoPictureId);
            return View(emoFace);
        }

        // GET: EmoFaces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emoFace = await _context.EmoFaces.SingleOrDefaultAsync(m => m.Id == id);
            if (emoFace == null)
            {
                return NotFound();
            }
            ViewData["EmoPictureId"] = new SelectList(_context.EmoPictures, "Id", "Id", emoFace.EmoPictureId);
            return View(emoFace);
        }

        // POST: EmoFaces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmoPictureId,X,Y,Width,Height")] EmoFace emoFace)
        {
            if (id != emoFace.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emoFace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmoFaceExists(emoFace.Id))
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
            ViewData["EmoPictureId"] = new SelectList(_context.EmoPictures, "Id", "Id", emoFace.EmoPictureId);
            return View(emoFace);
        }

        // GET: EmoFaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emoFace = await _context.EmoFaces
                .Include(e => e.Picture)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (emoFace == null)
            {
                return NotFound();
            }

            return View(emoFace);
        }

        // POST: EmoFaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emoFace = await _context.EmoFaces.SingleOrDefaultAsync(m => m.Id == id);
            _context.EmoFaces.Remove(emoFace);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmoFaceExists(int id)
        {
            return _context.EmoFaces.Any(e => e.Id == id);
        }
    }
}
