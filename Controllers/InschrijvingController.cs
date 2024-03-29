﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HogeschoolPXL.Data;
using HogeschoolPXL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HogeschoolPXL.Controllers
{
    [Authorize(Roles = "Admin,Lector,Student")]
    public class InschrijvingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public InschrijvingController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Inschrijving
        public async Task<IActionResult> Index(string? category = null)
        {
            var appDbContextModel = _context.Inschrijving
                .Where(x => category == null || x.Student.Gebruiker.Voornaam == category)
                .Include(i => i.Academiejaar)
                .Include(i => i.Student).ThenInclude(x => x.Gebruiker)
                .Include(i => i.VakLector).ThenInclude(x => x.Vak);
            return View(await appDbContextModel.ToListAsync());
        }

        // GET: Inschrijving/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inschrijving == null)
            {
                return NotFound();
            }

            var inschrijving = await _context.Inschrijving
                .Include(i => i.Academiejaar)
                .Include(i => i.Student).ThenInclude(x => x.Gebruiker)
                .Include(i => i.VakLector).ThenInclude(x => x.Vak)
                .FirstOrDefaultAsync(m => m.InschrijvingId == id);
            if (inschrijving == null)
            {
                return NotFound();
            }

            return View(inschrijving);
        }

        // GET: Inschrijving/Create
        [Authorize(Roles = "Admin,Student")]
        public IActionResult Create()
        {
            ViewData["AcademiejaarId"] = _context.Academiejaar.Select(x => new SelectListItem
                (x.Datum.ToShortDateString().ToString(), x.AcademiejaarId.ToString()));
            ViewData["StudentId"] = _context.Student.Select(x => new SelectListItem
                (x.Gebruiker.Voornaam + " " + x.Gebruiker.Naam, x.StudentId.ToString()));
            ViewData["VakLectorId"] = _context.VakLector.Select(x => new SelectListItem
                (x.Vak.VakNaam, x.VakId.ToString()));

            if (!_context.Student.Any() || !_context.Vak.Any())
            {
                ModelState.AddModelError("", "Create een Student en/of Vak voordat je een Inschrijving creëert!");
                return View();
            }

            return View();
        }

        // POST: Inschrijving/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> Create([Bind("InschrijvingId,StudentId,VakLectorId,AcademiejaarId")] Inschrijving inschrijving)
        {
            ViewData["AcademiejaarId"] = _context.Academiejaar.Select(x => new SelectListItem(
                x.Datum.ToShortDateString().ToString(), x.AcademiejaarId.ToString()));
            ViewData["StudentId"] = _context.Student.Select(x => new SelectListItem(
                x.Gebruiker.Voornaam + " " + x.Gebruiker.Naam, x.StudentId.ToString()));
            ViewData["VakLectorId"] = _context.VakLector.Select(x => new SelectListItem(
                x.Vak.VakNaam, x.VakId.ToString()));

            // Update ModelState to exclude nested models (Student, VakLector and Academiejaar) from model Inschrijving
            await TryUpdateModelAsync(inschrijving);
            ModelState.Remove("Student");
            ModelState.Remove("VakLector");
            ModelState.Remove("Academiejaar");

            if (!_context.Student.Any() || !_context.Vak.Any())
            {
                ModelState.AddModelError("", "Create een Student en/of Vak voordat je een Inschrijving creëert!");
                return View();
            }

            if (_context.Inschrijving.Where(x => x.StudentId == inschrijving.StudentId && x.VakLectorId == inschrijving.VakLectorId && x.AcademiejaarId == inschrijving.AcademiejaarId)
                .Select(x => x.InschrijvingId).Any())
            {
                ModelState.AddModelError("", "Een record met precies dezelfde waarden voor alle drie de velden bestaat al in de database");
                return View();
            }

            if (ModelState.IsValid)
            {
                _context.Add(inschrijving);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(inschrijving);
        }

        // GET: Inschrijving/Edit/5
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Inschrijving == null)
            {
                return NotFound();
            }

            var inschrijving = await _context.Inschrijving.FindAsync(id);
            if (inschrijving == null)
            {
                return NotFound();
            }
            ViewData["AcademiejaarId"] = _context.Academiejaar.Select(x => new SelectListItem(
                x.Datum.ToShortDateString().ToString(), x.AcademiejaarId.ToString()));
            ViewData["StudentId"] = _context.Student.Select(x => new SelectListItem(
                x.Gebruiker.Voornaam + " " + x.Gebruiker.Naam, x.StudentId.ToString()));
            ViewData["VakLectorId"] = _context.VakLector.Select(x => new SelectListItem(
                x.Vak.VakNaam, x.VakId.ToString()));
            return View(inschrijving);
        }

        // POST: Inschrijving/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> Edit(int id, [Bind("InschrijvingId,StudentId,VakLectorId,AcademiejaarId")] Inschrijving inschrijving)
        {
            // Update ModelState to exclude nested models (Student, VakLector and Academiejaar) from model Inschrijving
            await TryUpdateModelAsync(inschrijving);
            ModelState.Remove("Student");
            ModelState.Remove("VakLector");
            ModelState.Remove("Academiejaar");

            if (_context.Inschrijving.Where(x => x.StudentId == inschrijving.StudentId && x.VakLectorId == inschrijving.VakLectorId && x.AcademiejaarId == inschrijving.AcademiejaarId)
                .Select(x => x.InschrijvingId).Any())
            {
                ModelState.AddModelError("", "Een record met precies dezelfde waarden voor alle drie de velden bestaat al in de database");
                return View();
            }

            if (id != inschrijving.InschrijvingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inschrijving);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InschrijvingExists(inschrijving.InschrijvingId))
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
            ViewData["AcademiejaarId"] = _context.Academiejaar.Select(x => new SelectListItem(
                x.Datum.ToString(), x.AcademiejaarId.ToString()));
            ViewData["StudentId"] = _context.Student.Select(x => new SelectListItem(
                x.Gebruiker.Voornaam + " " + x.Gebruiker.Naam, x.StudentId.ToString()));
            ViewData["VakLectorId"] = _context.VakLector.Select(x => new SelectListItem(
                x.Vak.VakNaam, x.VakId.ToString()));
            return View(inschrijving);
        }

        // GET: Inschrijving/Delete/5
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Inschrijving == null)
            {
                return NotFound();
            }

            var inschrijving = await _context.Inschrijving
                .Include(i => i.Academiejaar)
                .Include(i => i.Student).ThenInclude(g => g.Gebruiker)
                .Include(i => i.VakLector).ThenInclude(v => v.Vak)
                .FirstOrDefaultAsync(m => m.InschrijvingId == id);
            if (inschrijving == null)
            {
                return NotFound();
            }

            return View(inschrijving);
        }

        // POST: Inschrijving/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Inschrijving == null)
            {
                return Problem("Entity set 'AppDbContext.Inschrijving'  is null.");
            }
            var inschrijving = await _context.Inschrijving.FindAsync(id);
            if (inschrijving != null)
            {
                _context.Inschrijving.Remove(inschrijving);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InschrijvingExists(int id)
        {
          return _context.Inschrijving.Any(e => e.InschrijvingId == id);
        }
    }
}
