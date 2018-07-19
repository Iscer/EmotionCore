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
    public class EmoPicturesController : Controller
    {
        private readonly EmotionCoreContext _context;

        public EmoPicturesController(EmotionCoreContext context)
        {
            _context = context;
        }

        // GET: EmoPictures
        public async Task<IActionResult> Index()
        {
            return View(await _context.EmoPictures.ToListAsync());
        }

        // GET: EmoPictures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emoPicture = await _context.EmoPictures
                .SingleOrDefaultAsync(m => m.Id == id);
            if (emoPicture == null)
            {
                return NotFound();
            }

            return View(emoPicture);
        }

        // GET: EmoPictures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmoPictures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Path")] EmoPicture emoPicture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emoPicture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(emoPicture);
        }

        // GET: EmoPictures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emoPicture = await _context.EmoPictures.SingleOrDefaultAsync(m => m.Id == id);
            if (emoPicture == null)
            {
                return NotFound();
            }
            return View(emoPicture);
        }

        // POST: EmoPictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Path")] EmoPicture emoPicture)
        {
            if (id != emoPicture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emoPicture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmoPictureExists(emoPicture.Id))
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
            return View(emoPicture);
        }

        // GET: EmoPictures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emoPicture = await _context.EmoPictures
                .SingleOrDefaultAsync(m => m.Id == id);
            if (emoPicture == null)
            {
                return NotFound();
            }

            return View(emoPicture);
        }

        // POST: EmoPictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emoPicture = await _context.EmoPictures.SingleOrDefaultAsync(m => m.Id == id);
            _context.EmoPictures.Remove(emoPicture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmoPictureExists(int id)
        {
            return _context.EmoPictures.Any(e => e.Id == id);
        }
    }
}
