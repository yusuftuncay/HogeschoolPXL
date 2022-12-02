using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class Inschrijving
    {
        [Key]
        public int InschrijvingId { get; set; }
        [Required]
        [DisplayName("Student")]
        public int StudentId { get; set; }
        [Required]
        [DisplayName("Vak")]
        public int VakLectorId { get; set; }
        [Required]
        [DisplayName("Academiejaar")]
        public int AcademiejaarId { get; set; }
        public Student Student { get; set; }
        [DisplayName("Vak")]
        public VakLector VakLector { get; set; }
        public Academiejaar Academiejaar { get; set; }
    }
}
