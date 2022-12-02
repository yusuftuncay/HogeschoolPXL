using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HogeschoolPXL.Data;
using HogeschoolPXL.Models;

namespace HogeschoolPXL.Controllers
{
    public class HandboekController : Controller
    {
        private readonly AppDbContext _context;

        public HandboekController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Handboek
        public async Task<IActionResult> Index()
        {
              return View(await _context.Handboek.ToListAsync());
        }

        // GET: Handboek/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Handboek == null)
            {
                return NotFound();
            }

            var handboek = await _context.Handboek
                .FirstOrDefaultAsync(m => m.HandboekId == id);
            if (handboek == null)
            {
                return NotFound();
            }

            return View(handboek);
        }

        // GET: Handboek/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Handboek/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HandboekId,Titel,Kostprijs,UitgifteDatum,Afbeelding")] Handboek handboek)
        {
            if (ModelState.IsValid)
            {
                _context.Add(handboek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(handboek);
        }

        // GET: Handboek/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Handboek == null)
            {
                return NotFound();
            }

            var handboek = await _context.Handboek.FindAsync(id);
            if (handboek == null)
            {
                return NotFound();
            }
            return View(handboek);
        }

        // POST: Handboek/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HandboekId,Titel,Kostprijs,UitgifteDatum,Afbeelding")] Handboek handboek)
        {
            if (id != handboek.HandboekId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(handboek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HandboekExists(handboek.HandboekId))
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
            return View(handboek);
        }

        // GET: Handboek/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Handboek == null)
            {
                return NotFound();
            }

            var handboek = await _context.Handboek
                .FirstOrDefaultAsync(m => m.HandboekId == id);
            if (handboek == null)
            {
                return NotFound();
            }

            return View(handboek);
        }

        // POST: Handboek/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Handboek == null)
            {
                return Problem("Entity set 'AppDbContext.Handboek'  is null.");
            }
            var handboek = await _context.Handboek.FindAsync(id);
            if (handboek != null)
            {
                _context.Handboek.Remove(handboek);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HandboekExists(int id)
        {
          return _context.Handboek.Any(e => e.HandboekId == id);
        }
    }
}
