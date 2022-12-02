﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HogeschoolPXL.Data;
using HogeschoolPXL.Models;
using HogeschoolPXL.Models.ViewModels;

namespace HogeschoolPXL.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Student.Include(s => s.Gebruiker);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var student = await _context.Inschrijving
                .Include(x => x.Student).ThenInclude(g => g.Gebruiker)
                .Include(x => x.Academiejaar)
                .Include(x => x.VakLector).ThenInclude(x => x.Vak)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            var studentCard = new StudentCard(_context, student);
            return View(studentCard);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            // Show both Naam and Voornaam (instead of just the Naam) while editing a Student
            var result = _context.Gebruiker.Select(x => new
            {
                x.GebruikerId,
                Naam = x.Voornaam + " " + x.Naam
            });
            ViewData["GebruikerId"] = new SelectList(result, "GebruikerId", "Naam");

            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,GebruikerId")] Student student)
        {
            // Update ModelState to exclude nested model (Gebruiker) from model Student
            await TryUpdateModelAsync(student);
            ModelState.Remove("Gebruiker");

            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GebruikerId"] = new SelectList(_context.Gebruiker, "GebruikerId", "Voornaam", student.GebruikerId);
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            // Show both Naam and Voornaam (instead of just the Naam) while editing a Student
            var result = _context.Gebruiker.Select(x => new
            {
                x.GebruikerId,
                Naam = x.Voornaam + " " + x.Naam
            });
            ViewData["GebruikerId"] = new SelectList(result, "GebruikerId", "Naam");

            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,GebruikerId")] Student student)
        {
            // Update ModelState to exclude nested model (Gebruiker) from model Student
            await TryUpdateModelAsync(student);
            ModelState.Remove("Gebruiker");

            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            ViewData["GebruikerId"] = new SelectList(_context.Gebruiker, "GebruikerId", "Voornaam", student.GebruikerId);
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Gebruiker)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Student == null)
            {
                return Problem("Entity set 'AppDbContext.Student'  is null.");
            }
            var student = await _context.Student.FindAsync(id);
            if (student != null)
            {
                _context.Student.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
          return _context.Student.Any(e => e.StudentId == id);
        }
    }
}
