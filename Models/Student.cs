using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required]
        [DisplayName("Student")]
        public int GebruikerId { get; set; }
        [DisplayName("Student")]
        public Gebruiker Gebruiker { get; set; }
    }
}
