using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HogeschoolPXL.Data;
using HogeschoolPXL.Models;
using System.IO;

namespace HogeschoolPXL.Controllers
{
    public class VakController : Controller
    {
        private readonly AppDbContext _context;

        public VakController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Vak
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Vak.Include(v => v.Handboek);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Vak/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vak == null)
            {
                return NotFound();
            }

            var vak = await _context.Vak
                .Include(v => v.Handboek)
                .FirstOrDefaultAsync(m => m.VakId == id);
            if (vak == null)
            {
                return NotFound();
            }

            return View(vak);
        }

        // GET: Vak/Create
        public IActionResult Create()
        {
            if (!_context.Handboek.Any())
            {
                ModelState.AddModelError("", "Eerst een Handboek aanmaken!");
                return View();
            }

            ViewData["HandboekId"] = new SelectList(_context.Handboek, "HandboekId", "Titel");
            return View();
        }

        // POST: Vak/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VakId,VakNaam,Studiepunten,HandboekId")] Vak vak)
        {
            // Update ModelState to exclude nested model (Handboek) from model Vak
            await TryUpdateModelAsync(vak);
            ModelState.Remove("Handboek");

            if (!_context.Handboek.Any())
            {
                ModelState.AddModelError("", "Er moet eerst een Handboek bestaan om een Vak te kunnen maken");
                return View();
            }

            if (ModelState.IsValid)
            {
                _context.Add(vak);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HandboekId"] = new SelectList(_context.Handboek, "HandboekId", "Titel", vak.HandboekId);
            return View(vak);
        }

        // GET: Vak/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vak == null)
            {
                return NotFound();
            }

            var vak = await _context.Vak.FindAsync(id);
            if (vak == null)
            {
                return NotFound();
            }
            ViewData["HandboekId"] = new SelectList(_context.Handboek, "HandboekId", "Titel", vak.HandboekId);
            return View(vak);
        }

        // POST: Vak/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VakId,VakNaam,Studiepunten,HandboekId")] Vak vak)
        {
            // Update ModelState to exclude nested model (Handboek) from model Vak
            await TryUpdateModelAsync(vak);
            ModelState.Remove("Handboek");

            if (id != vak.VakId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vak);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VakExists(vak.VakId))
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
            ViewData["HandboekId"] = new SelectList(_context.Handboek, "HandboekId", "Titel", vak.HandboekId);
            return View(vak);
        }

        // GET: Vak/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vak == null)
            {
                return NotFound();
            }

            var vak = await _context.Vak
                .Include(v => v.Handboek)
                .FirstOrDefaultAsync(m => m.VakId == id);
            if (vak == null)
            {
                return NotFound();
            }

            return View(vak);
        }

        // POST: Vak/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vak == null)
            {
                return Problem("Entity set 'AppDbContext.Vak'  is null.");
            }
            var vak = await _context.Vak.FindAsync(id);
            if (vak != null)
            {
                _context.Vak.Remove(vak);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VakExists(int id)
        {
          return _context.Vak.Any(e => e.VakId == id);
        }
    }
}
