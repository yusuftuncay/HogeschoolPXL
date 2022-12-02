using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class Academiejaar
    {
        [Key]
        public int AcademiejaarId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }
    }
}
