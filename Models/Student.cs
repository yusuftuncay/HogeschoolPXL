using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required]
        public int GebruikerId { get; set; }
        public Gebruiker Gebruiker { get; set; }
    }
}
