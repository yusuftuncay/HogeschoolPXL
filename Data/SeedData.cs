using HogeschoolPXL.Data;
using HogeschoolPXL.Models;

namespace HogeschoolPXL.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (!context.Gebruiker.Any())
            {
                foreach (var g in GetGebruiker())
                {
                    context.Gebruiker.Add(g);
                    context.SaveChanges();
                }
            }
            if (!context.Student.Any())
            {
                foreach (var st in GetStudent())
                {
                    context.Student.Add(st);
                    context.SaveChanges();
                }
            }
            if (!context.Lector.Any())
            {
                foreach (var l in GetLector())
                {
                    context.Lector.Add(l);
                    context.SaveChanges();
                }
            }
            if (!context.Handboek.Any())
            {
                foreach (var h in GetHandboek())
                {
                    context.Handboek.Add(h);
                    context.SaveChanges();
                }
            }
            if (!context.Vak.Any())
            {
                foreach (var v in GetVak())
                {
                    context.Vak.Add(v);
                    context.SaveChanges();
                }
            }
            if (!context.VakLector.Any())
            {
                foreach (var v in GetVakLector())
                {
                    context.VakLector.Add(v);
                    context.SaveChanges();
                }
            }
            if (!context.AcademieJaar.Any())
            {
                foreach (var a in GetAcademieJaar())
                {
                    context.AcademieJaar.Add(a);
                    context.SaveChanges();
                }
            }
            if (!context.Inschrijving.Any())
            {
                foreach (var i in GetInschrijving())
                {
                    context.Inschrijving.Add(i);
                    context.SaveChanges();
                }
            }
        }

        #region data
        private static List<Gebruiker> GetGebruiker()
        {
            List<Gebruiker> gebruiker = new()
            {
                new Gebruiker { Naam = "Tuncay", VoorNaam = "Yusuf", Email = "yusuf.tuncay@student.pxl.be" },
                new Gebruiker { Naam = "Palmaers", VoorNaam = "Kristof", Email = "kristof.palmaers@pxl.be" }
            };
            return gebruiker;
        }
        private static List<Student> GetStudent()
        {
            List<Student> student = new()
            {
                new Student { GebruikerId = 1 }
            };
            return student;
        }
        private static List<Lector> GetLector()
        {
            List<Lector> lector = new()
            {
                new Lector { GebruikerId = 2 }
            };
            return lector;
        }
        private static List<Handboek> GetHandboek()
        {
            List<Handboek> handboek = new()
            {
                new Handboek { Titel = "Syllabus C# Web 1", KostPrijs = 24.99M, UitgifteDatum = DateTime.Parse("01 01 2022")}
            };
            return handboek;
        }
        private static List<Vak> GetVak()
        {
            List<Vak> vak = new()
            {
                new Vak { VakNaam = "C# Web 1", StudiePunten = 6, HandboekId = 1 }
            };
            return vak;
        }
        private static List<VakLector> GetVakLector()
        {
            List<VakLector> vakLector = new()
            {
                new VakLector { LectorId = 1, VakId = 1 }
            };
            return vakLector;
        }
        private static List<AcademieJaar> GetAcademieJaar()
        {
            List<AcademieJaar> academieJaar = new()
            {
                new AcademieJaar { Datum = DateTime.Parse("20 09 2021") }
            };
            return academieJaar;
        }
        private static List<Inschrijving> GetInschrijving()
        {
            List<Inschrijving> inschrijving = new()
            {
                new Inschrijving { StudentId = 1, VakLectorId = 1, AcademieJaarId = 1}
            };
            return inschrijving;
        }
        #endregion
    }
}
