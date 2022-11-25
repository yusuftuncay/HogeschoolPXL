using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class Vak
    {
        [Key]
        public int VakId { get; set; }
        [Required]
        [DisplayName("Vak Naam")]
        public string VakNaam { get; set; }
        [Required]
        [DisplayName("Studie Punten")]
        public int StudiePunten { get; set; }
        [Required]
        [DisplayName("Handboek")]
        public int HandboekId { get; set; }
        public Handboek Handboek { get; set; }
    }
}
