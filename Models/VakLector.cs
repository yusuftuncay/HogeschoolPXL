using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class VakLector
    {
        [Key]
        public int VakLectorId { get; set; }
        [Required]
        public int LectorId { get; set; }
        [Required]
        public int VakId { get; set; }
        public Lector Lector { get; set; }
        public Vak Vak { get; set; }
    }
}
