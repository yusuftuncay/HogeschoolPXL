using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class Gebruiker
    {
        [Key]
        public int GebruikerId { get; set; }
        [Required]
        public string Naam { get; set; }
        [Required]
        public string VoorNaam { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
