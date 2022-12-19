using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HogeschoolPXL.Data;
using HogeschoolPXL.Models;
using Microsoft.AspNetCore.Authorization;

namespace HogeschoolPXL.Controllers
{
    [Authorize]
    public class LectorController : Controller
    {
        private readonly AppDbContext _context;

        public LectorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Lector
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Lector.Include(l => l.Gebruiker);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Lector/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lector == null)
            {
                return NotFound();
            }

            var lector = await _context.Lector
                .Include(l => l.Gebruiker)
                .FirstOrDefaultAsync(m => m.LectorId == id);
            if (lector == null)
            {
                return NotFound();
            }

            return View(lector);
        }

        // GET: Lector/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Show both Naam and Voornaam (instead of just the Naam) while creating a Lector
            // Don't show Gebruikers in SelectList that are already Students or Lectors
            var students = _context.Student.Select(x => x.GebruikerId);
            var lectors = _context.Lector.Select(x => x.GebruikerId);
            var result = _context.Gebruiker
                .Where(x => !students.Contains(x.GebruikerId))
                .Where(x => !lectors.Contains(x.GebruikerId))
                .Select(x => new
                    {
                        x.GebruikerId,
                        Naam = x.Voornaam + " " + x.Naam
                    }).ToList();

            // Error if there is no Gebruiker without a role
            if (result.Count == 0)
            {
                ModelState.AddModelError("", "Geen Gebruikers beschikbaar die geen rol toegekend hebben");
                return View();
            }
            ViewData["GebruikerId"] = new SelectList(result, "GebruikerId", "Naam");

            return View();
        }

        // POST: Lector/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("LectorId,GebruikerId")] Lector lector)
        {
            // Update ModelState to exclude nested model (Gebruiker) from model Lector
            await TryUpdateModelAsync(lector);
            ModelState.Remove("Gebruiker");

            if (ModelState.IsValid)
            {
                _context.Add(lector);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Show both Naam and Voornaam (instead of just the Naam) while creating a Lector
            // Don't show Gebruikers in SelectList that are already Students or Lectors
            var students = _context.Student.Select(x => x.GebruikerId);
            var lectors = _context.Lector.Select(x => x.GebruikerId);
            var result = _context.Gebruiker
                .Where(x => !students.Contains(x.GebruikerId))
                .Where(x => !lectors.Contains(x.GebruikerId))
                .Select(x => new
                    {
                        x.GebruikerId,
                        Naam = x.Voornaam + " " + x.Naam
                    }).ToList();
            ViewData["GebruikerId"] = new SelectList(result, "GebruikerId", "Naam");

            return View(lector);
        }

        // GET: Lector/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lector == null)
            {
                return NotFound();
            }

            var lector = await _context.Lector.FindAsync(id);
            if (lector == null)
            {
                return NotFound();
            }

            // Show both Naam and Voornaam (instead of just the Naam) while creating a Lector
            // Don't show Gebruikers in SelectList that are already Students or Lectors
            var students = _context.Student.Select(x => x.GebruikerId);
            var lectors = _context.Lector.Select(x => x.GebruikerId);
            var result = _context.Gebruiker
                .Where(x => !students.Contains(x.GebruikerId))
                .Where(x => !lectors.Contains(x.GebruikerId))
                .Select(x => new
                    {
                        x.GebruikerId,
                        Naam = x.Voornaam + " " + x.Naam
                    }).ToList();
            ViewData["GebruikerId"] = new SelectList(result, "GebruikerId", "Naam");

            return View(lector);
        }

        // POST: Lector/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("LectorId,GebruikerId")] Lector lector)
        {
            // Update ModelState to exclude nested model (Gebruiker) from model Lector
            await TryUpdateModelAsync(lector);
            ModelState.Remove("Gebruiker");

            if (id != lector.LectorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LectorExists(lector.LectorId))
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

            // Show both Naam and Voornaam (instead of just the Naam) while creating a Lector
            // Don't show Gebruikers in SelectList that are already Students or Lectors
            var students = _context.Student.Select(x => x.GebruikerId);
            var lectors = _context.Lector.Select(x => x.GebruikerId);
            var result = _context.Gebruiker
                .Where(x => !students.Contains(x.GebruikerId))
                .Where(x => !lectors.Contains(x.GebruikerId))
                .Select(x => new
                    {
                        x.GebruikerId,
                        Naam = x.Voornaam + " " + x.Naam
                    }).ToList();
            ViewData["GebruikerId"] = new SelectList(result, "GebruikerId", "Naam");

            return View(lector);
        }

        // GET: Lector/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lector == null)
            {
                return NotFound();
            }

            var lector = await _context.Lector
                .Include(l => l.Gebruiker)
                .FirstOrDefaultAsync(m => m.LectorId == id);
            if (lector == null)
            {
                return NotFound();
            }

            return View(lector);
        }

        // POST: Lector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lector == null)
            {
                return Problem("Entity set 'AppDbContext.Lector'  is null.");
            }
            var lector = await _context.Lector.FindAsync(id);
            if (lector != null)
            {
                _context.Lector.Remove(lector);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LectorExists(int id)
        {
          return _context.Lector.Any(e => e.LectorId == id);
        }
    }
}
