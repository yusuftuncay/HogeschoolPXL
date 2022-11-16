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
    public class AcademieJaarController : Controller
    {
        private readonly AppDbContext _context;

        public AcademieJaarController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AcademieJaar
        public async Task<IActionResult> Index()
        {
              return View(await _context.AcademieJaar.ToListAsync());
        }

        // GET: AcademieJaar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AcademieJaar == null)
            {
                return NotFound();
            }

            var academieJaar = await _context.AcademieJaar
                .FirstOrDefaultAsync(m => m.AcademieJaarId == id);
            if (academieJaar == null)
            {
                return NotFound();
            }

            return View(academieJaar);
        }

        // GET: AcademieJaar/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AcademieJaar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AcademieJaarId,Datum")] AcademieJaar academieJaar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academieJaar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(academieJaar);
        }

        // GET: AcademieJaar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AcademieJaar == null)
            {
                return NotFound();
            }

            var academieJaar = await _context.AcademieJaar.FindAsync(id);
            if (academieJaar == null)
            {
                return NotFound();
            }
            return View(academieJaar);
        }

        // POST: AcademieJaar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AcademieJaarId,Datum")] AcademieJaar academieJaar)
        {
            if (id != academieJaar.AcademieJaarId)
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
                    if (!AcademieJaarExists(academieJaar.AcademieJaarId))
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

        // GET: AcademieJaar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AcademieJaar == null)
            {
                return NotFound();
            }

            var academieJaar = await _context.AcademieJaar
                .FirstOrDefaultAsync(m => m.AcademieJaarId == id);
            if (academieJaar == null)
            {
                return NotFound();
            }

            return View(academieJaar);
        }

        // POST: AcademieJaar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AcademieJaar == null)
            {
                return Problem("Entity set 'AppDbContext.AcademieJaar'  is null.");
            }
            var academieJaar = await _context.AcademieJaar.FindAsync(id);
            if (academieJaar != null)
            {
                _context.AcademieJaar.Remove(academieJaar);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademieJaarExists(int id)
        {
          return _context.AcademieJaar.Any(e => e.AcademieJaarId == id);
        }
    }
}
