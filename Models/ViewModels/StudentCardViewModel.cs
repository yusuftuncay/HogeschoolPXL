using HogeschoolPXL.Data;
using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models.ViewModels
{
    public class StudentCardViewModel
    {
        public StudentCardViewModel(AppDbContext context, Inschrijving? inschrijving)
        {
            if (inschrijving != null)
            {
                Inschrijving = inschrijving;
                VoorNaam = inschrijving.Student.Gebruiker.Voornaam;
                Naam = inschrijving.Student.Gebruiker.Naam;
                Email = inschrijving.Student.Gebruiker.Email;

                InschrijvingId = context.Inschrijving.Where(x => x.Student.StudentId == inschrijving.StudentId)
                    .Select(x => x.InschrijvingId)/*.Take(5)*/.ToList().Count();
                
                Vak = context.Inschrijving.Where(x => x.Student.StudentId == inschrijving.Student.StudentId)
                    .Select(x => x.VakLector.Vak.VakNaam).ToList();

                Academiejaar = context.Inschrijving.Where(x => x.Student.StudentId == inschrijving.Student.StudentId)
                    .Select(x => x.Academiejaar.Datum)
                    .OrderBy(x => x.Day).ToList();
            }
        }

        public Inschrijving Inschrijving { get; set; }
        public string VoorNaam { get; set; }
        public string Naam { get; set; }
        public string Email { get; set; }
        public int InschrijvingId { get; set; }
        public List<string> Vak { get; set; }
        [DataType(DataType.Date)]
        public List<DateTime> Academiejaar { get; set; }
    }
}
