using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HogeschoolPXL.Data;
using HogeschoolPXL.Models;

namespace HogeschoolPXL.Controllers
{
    public class VakLectorController : Controller
    {
        private readonly AppDbContext _context;

        public VakLectorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: VakLector
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.VakLector.Include(v => v.Lector).ThenInclude(x => x.Gebruiker).Include(v => v.Vak);
            return View(await appDbContext.ToListAsync());
        }

        // GET: VakLector/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VakLector == null)
            {
                return NotFound();
            }

            var vakLector = await _context.VakLector
                .Include(v => v.Lector)
                .ThenInclude(g => g.Gebruiker)
                .Include(v => v.Vak)
                .FirstOrDefaultAsync(m => m.VakLectorId == id);
            if (vakLector == null)
            {
                return NotFound();
            }

            return View(vakLector);
        }

        // GET: VakLector/Create
        public IActionResult Create()
        {
            ViewData["LectorId"] = _context.Lector.Select(x => new SelectListItem(
                x.Gebruiker.Voornaam + " " + x.Gebruiker.Naam, x.LectorId.ToString()));
            ViewData["VakId"] = new SelectList(_context.Vak, "VakId", "VakNaam");

            return View();
        }

        // POST: VakLector/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VakLectorId,LectorId,VakId")] VakLector vakLector)
        {
            // Update ModelState to exclude nested model (Vak and Lector) from model VakLector
            await TryUpdateModelAsync(vakLector);
            ModelState.Remove("Vak");
            ModelState.Remove("Lector");

            // Check if combination Vak and Lector exists
            if (_context.VakLector
                .Where(x => x.LectorId == vakLector.LectorId)
                .Where(x => x.VakId == vakLector.VakId)
                .Select(x => x.VakLectorId).Any())
            {
                ViewData["LectorId"] = _context.Lector.Select(x => new SelectListItem(
                x.Gebruiker.Voornaam + " " + x.Gebruiker.Naam, x.LectorId.ToString()));
                ViewData["VakId"] = new SelectList(_context.Vak, "VakId", "VakNaam");

                ModelState.AddModelError("", "Combinatie Vak en Lector bestaat al"); 
                return View(vakLector);
            }

            if (ModelState.IsValid)
            {
                _context.Add(vakLector);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LectorId"] = _context.Lector.Select(x => new SelectListItem(
                x.Gebruiker.Voornaam + " " + x.Gebruiker.Naam, x.LectorId.ToString()));
            ViewData["VakId"] = new SelectList(_context.Vak, "VakId", "VakNaam", vakLector.VakId);
            return View(vakLector);
        }

        // GET: VakLector/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VakLector == null)
            {
                return NotFound();
            }

            var vakLector = await _context.VakLector.FindAsync(id);
            if (vakLector == null)
            {
                return NotFound();
            }
            ViewData["LectorId"] = _context.Lector.Select(x => new SelectListItem(
                x.Gebruiker.Voornaam + " " + x.Gebruiker.Naam, x.LectorId.ToString()));
            ViewData["VakId"] = new SelectList(_context.Vak, "VakId", "VakNaam", vakLector.VakId);
            return View(vakLector);
        }

        // POST: VakLector/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VakLectorId,LectorId,VakId")] VakLector vakLector)
        {
            // Update ModelState to exclude nested model (Vak and Lector) from model VakLector
            await TryUpdateModelAsync(vakLector);
            ModelState.Remove("Vak");
            ModelState.Remove("Lector");

            // Check if combination Vak and Lector exists
            if (_context.VakLector
                .Where(x => x.LectorId == vakLector.LectorId)
                .Where(x => x.VakId == vakLector.VakId)
                .Select(x => x.VakLectorId).Any())
            {
                ViewData["LectorId"] = _context.Lector.Select(x => new SelectListItem(
                x.Gebruiker.Voornaam + " " + x.Gebruiker.Naam, x.LectorId.ToString()));
                ViewData["VakId"] = new SelectList(_context.Vak, "VakId", "VakNaam");

                ModelState.AddModelError("", "Combinatie Vak en Lector bestaat al");
                return View(vakLector);
            }

            if (id != vakLector.VakLectorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vakLector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VakLectorExists(vakLector.VakLectorId))
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
            ViewData["LectorId"] = _context.Lector.Select(x => new SelectListItem(
                x.Gebruiker.Voornaam + " " + x.Gebruiker.Naam, x.LectorId.ToString()));
            ViewData["VakId"] = new SelectList(_context.Vak, "VakId", "VakNaam", vakLector.VakId);
            return View(vakLector);
        }

        // GET: VakLector/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VakLector == null)
            {
                return NotFound();
            }

            var vakLector = await _context.VakLector
                .Include(v => v.Lector)
                .ThenInclude(g => g.Gebruiker)
                .Include(v => v.Vak)
                .FirstOrDefaultAsync(m => m.VakLectorId == id);
            if (vakLector == null)
            {
                return NotFound();
            }

            return View(vakLector);
        }

        // POST: VakLector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VakLector == null)
            {
                return Problem("Entity set 'AppDbContext.VakLector'  is null.");
            }
            var vakLector = await _context.VakLector.FindAsync(id);
            if (vakLector != null)
            {
                _context.VakLector.Remove(vakLector);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VakLectorExists(int id)
        {
          return _context.VakLector.Any(e => e.VakLectorId == id);
        }
    }
}
