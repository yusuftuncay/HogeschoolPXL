﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HogeschoolPXL.Data;
using HogeschoolPXL.Models;
using Microsoft.AspNetCore.Authorization;

namespace HogeschoolPXL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GebruikerController : Controller
    {
        private readonly AppDbContext _context;

        public GebruikerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Gebruiker
        public async Task<IActionResult> Index()
        {
              return View(await _context.Gebruiker.ToListAsync());
        }

        // GET: Gebruiker/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Gebruiker == null)
            {
                return NotFound();
            }

            var gebruiker = await _context.Gebruiker
                .FirstOrDefaultAsync(m => m.GebruikerId == id);
            if (gebruiker == null)
            {
                return NotFound();
            }

            return View(gebruiker);
        }

        // GET: Gebruiker/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gebruiker/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GebruikerId,Naam,Voornaam,Email")] Gebruiker gebruiker)
        {
            // Check if combination VoorNaam and Naam exists
            // Also included Email in this check, because there could be more Gebruikers with the same Naam en VoorNaam
            if (_context.Gebruiker
                .Where(x => x.Voornaam == gebruiker.Voornaam)
                .Where(x => x.Naam == gebruiker.Naam)
                .Where(x => x.Email == gebruiker.Email)
                .Select(x => x.GebruikerId).Any())
            {
                ModelState.AddModelError("", "Gebruiker met deze VoorNaam, Naam en Email bestaat al");
                return View(gebruiker);
            }

            if (ModelState.IsValid)
            {
                _context.Add(gebruiker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gebruiker);
        }

        // GET: Gebruiker/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Gebruiker == null)
            {
                return NotFound();
            }

            var gebruiker = await _context.Gebruiker.FindAsync(id);
            if (gebruiker == null)
            {
                return NotFound();
            }
            return View(gebruiker);
        }

        // POST: Gebruiker/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GebruikerId,Naam,Voornaam,Email")] Gebruiker gebruiker)
        {
            if (id != gebruiker.GebruikerId)
            {
                return NotFound();
            }

            // Check if combination VoorNaam and Naam exists
            // Also included Email in this check, because there could be more Gebruikers with the same Naam en VoorNaam
            if (_context.Gebruiker
                .Where(x => x.Voornaam == gebruiker.Voornaam)
                .Where(x => x.Naam == gebruiker.Naam)
                .Where(x => x.Email == gebruiker.Email)
                .Select(x => x.GebruikerId).Any())
            {
                ModelState.AddModelError("", "Gebruiker met deze VoorNaam, Naam en Email bestaat al");
                return View(gebruiker);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gebruiker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GebruikerExists(gebruiker.GebruikerId))
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
            return View(gebruiker);
        }

        // GET: Gebruiker/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Gebruiker == null)
            {
                return NotFound();
            }

            var gebruiker = await _context.Gebruiker
                .FirstOrDefaultAsync(m => m.GebruikerId == id);
            if (gebruiker == null)
            {
                return NotFound();
            }

            return View(gebruiker);
        }

        // POST: Gebruiker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Gebruiker == null)
            {
                return Problem("Entity set 'AppDbContext.Gebruiker'  is null.");
            }
            var gebruiker = await _context.Gebruiker.FindAsync(id);
            if (gebruiker != null)
            {
                _context.Gebruiker.Remove(gebruiker);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GebruikerExists(int id)
        {
          return _context.Gebruiker.Any(e => e.GebruikerId == id);
        }
    }
}
