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
        [DisplayName("Lector")]
        public int VakLectorId { get; set; }

        [Required]
        [DisplayName("Academie Jaar")]
        public int AcademieJaarId { get; set; }
        [DisplayName("Student")]
        public Student Student { get; set; }
        [DisplayName("Lector")]
        public VakLector VakLector { get; set; }
        [DisplayName("Academie Jaar")]
        public AcademieJaar AcademieJaar { get; set; }
    }
}
