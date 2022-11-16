using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class Student
    {
        [Range(1000,2000)]
        public int StudentId { get; set; }
        public int GebruikerId { get; set; }
        public Gebruiker Gebruiker { get; set; }
    }
}
