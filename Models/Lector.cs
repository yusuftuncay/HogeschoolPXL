using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class Lector
    {
        [Key]
        public int LectorId { get; set; }
        [Required]
        [DisplayName("Lector")]
        public int GebruikerId { get; set; }
        [DisplayName("Lector")]
        public Gebruiker Gebruiker { get; set; }
    }
}
