using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class Lector
    {
        [Range(0, 1000)]
        public int LectorId { get; set; }
        public int GebruikerId { get; set; }
        public Gebruiker Gebruiker { get; set; }
    }
}
