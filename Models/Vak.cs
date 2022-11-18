using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class Vak
    {
        [Key]
        public int VakId { get; set; }
        [Required]
        public string VakNaam { get; set; }
        [Required]
        public int StudiePunten { get; set; }
        [Required]
        public int HandboekId { get; set; }
        public Handboek Handboek { get; set; }
    }
}
