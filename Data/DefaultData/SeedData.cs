using HogeschoolPXL.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace HogeschoolPXL.Data.DefaultData
{
    public static class SeedData
    {
        static AppDbContext? _context;
        static RoleManager<IdentityRole>? _roleManager;
        static UserManager<IdentityUser>? _userManager;
        public static async Task EnsurePopulatedAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            await VoegRollenToeAsync();
            await CreateIdentityRecordAsync("admin@pxl.be", "admin@pxl.be", "Admin456!", Roles.Admin);
            await CreateIdentityRecordAsync("lector@pxl.be", "lector@pxl.be", "Lector789!", Roles.Lector);
            await CreateIdentityRecordAsync("student@pxl.be", "student@pxl.be", "Student123!", Roles.Student);
            VoegDataToe();
        }

        #region identity
        private static async Task VoegRollenToeAsync()
        {
            if (_roleManager != null && !_roleManager.Roles.Any())
            {
                await VoegRolToeAsync(Roles.Admin);
                await VoegRolToeAsync(Roles.Lector);
                await VoegRolToeAsync(Roles.Student);
            }
        }
        private static async Task VoegRolToeAsync(string roleName)
        {
            if (_roleManager != null && !await _roleManager.RoleExistsAsync(roleName))
            {
                IdentityRole role = new IdentityRole(roleName);
                await _roleManager.CreateAsync(role);
            }
        }
        private static async Task CreateIdentityRecordAsync(string userName, string email, string pwd, string role)
        {

            if (_userManager != null && await _userManager.FindByEmailAsync(email) == null &&
                    await _userManager.FindByNameAsync(userName) == null)
            {
                var identityUser = new IdentityUser() { Email = email, UserName = userName };
                var result = await _userManager.CreateAsync(identityUser, pwd);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(identityUser, role);
                }
            }
        }
        #endregion

        #region voeg standaard data toe
        public static void VoegDataToe()
        {
            if (!_context.Gebruiker.Any())
            {
                foreach (var g in GetGebruiker())
                {
                    _context.Gebruiker.Add(g);
                    _context.SaveChanges();
                }
            }
            if (!_context.Student.Any())
            {
                foreach (var st in GetStudent())
                {
                    _context.Student.Add(st);
                    _context.SaveChanges();
                }
            }
            if (!_context.Lector.Any())
            {
                foreach (var l in GetLector())
                {
                    _context.Lector.Add(l);
                    _context.SaveChanges();
                }
            }
            if (!_context.Handboek.Any())
            {
                foreach (var h in GetHandboek())
                {
                    _context.Handboek.Add(h);
                    _context.SaveChanges();
                }
            }
            if (!_context.Vak.Any())
            {
                foreach (var v in GetVak())
                {
                    _context.Vak.Add(v);
                    _context.SaveChanges();
                }
            }
            if (!_context.VakLector.Any())
            {
                foreach (var v in GetVakLector())
                {
                    _context.VakLector.Add(v);
                    _context.SaveChanges();
                }
            }
            if (!_context.Academiejaar.Any())
            {
                foreach (var a in GetAcademiejaar())
                {
                    _context.Academiejaar.Add(a);
                    _context.SaveChanges();
                }
            }
            if (!_context.Inschrijving.Any())
            {
                foreach (var i in GetInschrijving())
                {
                    _context.Inschrijving.Add(i);
                    _context.SaveChanges();
                }
            }
        }
        #endregion

        #region standaard data
        private static List<Gebruiker> GetGebruiker()
        {
            List<Gebruiker> gebruiker = new()
            {
                new Gebruiker { Naam = "Tuncay", Voornaam = "Yusuf", Email = "yusuf.tuncay@student.pxl.be" },
                new Gebruiker { Naam = "Palmaers", Voornaam = "Kristof", Email = "kristof.palmaers@pxl.be" },
                new Gebruiker { Naam = "Smets", Voornaam = "Stany", Email = "stany.smets@pxl.be" },
                new Gebruiker { Naam = "DePuydt", Voornaam = "Sander", Email = "sander.depuydt@pxl.be" },
                new Gebruiker { Naam = "Kaya", Voornaam = "Ahmet", Email = "ahmet.kaya@pxl.be" },
                new Gebruiker { Naam = "Demir", Voornaam = "Furkan", Email = "furkan.demir@pxl.be" }
            };
            return gebruiker;
        }
        private static List<Student> GetStudent()
        {
            List<Student> student = new()
            {
                new Student { GebruikerId = 1 },
                new Student { GebruikerId = 5 },
                new Student { GebruikerId = 6 }
            };
            return student;
        }
        private static List<Lector> GetLector()
        {
            List<Lector> lector = new()
            {
                new Lector { GebruikerId = 2 },
                new Lector { GebruikerId = 3 },
                new Lector { GebruikerId = 4 }
            };
            return lector;
        }
        private static List<Handboek> GetHandboek()
        {
            List<Handboek> handboek = new()
            {
                new Handboek { Titel = "C# Web 1 (Syllabus)", Kostprijs = 24.99M, UitgifteDatum = DateTime.Parse("01 01 2022"), Afbeelding = "/img/csharp.jpg"},
                new Handboek { Titel = "IT Organisation (Syllabus)", Kostprijs = 14.99M, UitgifteDatum = DateTime.Parse("02 12 2008"), Afbeelding = "/img/itorg.jpg"},
                new Handboek { Titel = "Security & Privacy (Syllabus)", Kostprijs = 29.99M, UitgifteDatum = DateTime.Parse("20 08 2018"), Afbeelding = null}
            };
            return handboek;
        }
        private static List<Vak> GetVak()
        {
            List<Vak> vak = new()
            {
                new Vak { VakNaam = "C# Web 1", Studiepunten = 6, HandboekId = 1 },
                new Vak { VakNaam = "IT Organisation", Studiepunten = 3, HandboekId = 2 },
                new Vak { VakNaam = "Security & Privacy", Studiepunten = 6, HandboekId = 3 }
            };
            return vak;
        }
        private static List<VakLector> GetVakLector()
        {
            List<VakLector> vakLector = new()
            {
                new VakLector { LectorId = 1, VakId = 1 },
                new VakLector { LectorId = 2, VakId = 2 },
                new VakLector { LectorId = 3, VakId = 3 }
            };
            return vakLector;
        }
        private static List<Academiejaar> GetAcademiejaar()
        {
            List<Academiejaar> academieJaar = new()
            {
                new Academiejaar { Datum = DateTime.Parse("20 09 2021") },
                new Academiejaar { Datum = DateTime.Parse("26 09 2022") }
            };
            return academieJaar;
        }
        private static List<Inschrijving> GetInschrijving()
        {
            List<Inschrijving> inschrijving = new()
            {
                new Inschrijving { StudentId = 1, VakLectorId = 1, AcademiejaarId = 1},
                new Inschrijving { StudentId = 1, VakLectorId = 2, AcademiejaarId = 1},
                new Inschrijving { StudentId = 1, VakLectorId = 3, AcademiejaarId = 1},
                new Inschrijving { StudentId = 2, VakLectorId = 1, AcademiejaarId = 2},
                new Inschrijving { StudentId = 3, VakLectorId = 1, AcademiejaarId = 2}
            };
            return inschrijving;
        }
        #endregion
    }
}
