using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class AcademieJaar
    {
        [Key]
        public int AcademieJaarId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }
    }
}
