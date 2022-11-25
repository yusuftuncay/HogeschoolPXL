using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class VakLector
    {
        [Key]
        public int VakLectorId { get; set; }
        [Required]
        [DisplayName("Lector")]
        public int LectorId { get; set; }
        [Required]
        [DisplayName("Vak")]
        public int VakId { get; set; }
        public Lector Lector { get; set; }
        public Vak Vak { get; set; }
    }
}
