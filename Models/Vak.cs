using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class Vak
    {
        [Key]
        public int VakId { get; set; }
        [Required]
        [DisplayName("Vaknaam")]
        public string VakNaam { get; set; }
        [Required]
        [Range(2, 8)]
        public int Studiepunten { get; set; }
        [Required]
        [DisplayName("Handboek")]
        public int HandboekId { get; set; }
        public Handboek Handboek { get; set; }
    }
}
