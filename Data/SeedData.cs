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
            if (!context.Student.Any())
            {
                foreach (var st in GetStudent())
                {
                    context.Student.Add(st);
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
        private static List<Vak> GetVak()
        {
            List<Vak> vak = new()
            {
                new Vak { VakNaam = "C# Web 1", StudiePunten = 6, HandboekId = 1 }
            };
            return vak;
        }
        private static List<Handboek> GetHandboek()
        {
            List<Handboek> handboek = new()
            {
                new Handboek { Titel = "C# Web 1", KostPrijs = 19.99, UitgifteDatum = DateTime.Parse("01 01 2022")}
            };
            return handboek;
        }
        #endregion
    }
}
