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
    public class AcademiejaarController : Controller
    {
        private readonly AppDbContext _context;

        public AcademiejaarController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Academiejaar
        public async Task<IActionResult> Index()
        {
              return View(await _context.Academiejaar.ToListAsync());
        }

        // GET: Academiejaar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Academiejaar == null)
            {
                return NotFound();
            }

            var academieJaar = await _context.Academiejaar
                .FirstOrDefaultAsync(m => m.AcademiejaarId == id);
            if (academieJaar == null)
            {
                return NotFound();
            }

            return View(academieJaar);
        }

        // GET: Academiejaar/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Academiejaar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AcademiejaarId,Datum")] Academiejaar academieJaar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academieJaar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(academieJaar);
        }

        // GET: Academiejaar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Academiejaar == null)
            {
                return NotFound();
            }

            var academieJaar = await _context.Academiejaar.FindAsync(id);
            if (academieJaar == null)
            {
                return NotFound();
            }
            return View(academieJaar);
        }

        // POST: Academiejaar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AcademiejaarId,Datum")] Academiejaar academieJaar)
        {
            if (id != academieJaar.AcademiejaarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academieJaar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademiejaarExists(academieJaar.AcademiejaarId))
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
            return View(academieJaar);
        }

        // GET: Academiejaar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Academiejaar == null)
            {
                return NotFound();
            }

            var academieJaar = await _context.Academiejaar
                .FirstOrDefaultAsync(m => m.AcademiejaarId == id);
            if (academieJaar == null)
            {
                return NotFound();
            }

            return View(academieJaar);
        }

        // POST: Academiejaar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Academiejaar == null)
            {
                return Problem("Entity set 'AppDbContext.Academiejaar'  is null.");
            }
            var academieJaar = await _context.Academiejaar.FindAsync(id);
            if (academieJaar != null)
            {
                _context.Academiejaar.Remove(academieJaar);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademiejaarExists(int id)
        {
          return _context.Academiejaar.Any(e => e.AcademiejaarId == id);
        }
    }
}
