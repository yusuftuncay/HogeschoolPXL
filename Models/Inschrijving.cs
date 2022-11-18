using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class Inschrijving
    {
        [Key]
        public int InschrijvingId { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int VakLectorId { get; set; }
        [Required]
        public int AcademieJaarId { get; set; }
        public Student Student { get; set; }
        public VakLector VakLector { get; set; }
        public AcademieJaar AcademieJaar { get; set; }
    }
}
