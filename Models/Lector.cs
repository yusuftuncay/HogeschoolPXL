using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class Lector
    {
        [Key]
        public int LectorId { get; set; }
        [Required]
        public int GebruikerId { get; set; }
        public Gebruiker Gebruiker { get; set; }
    }
}
