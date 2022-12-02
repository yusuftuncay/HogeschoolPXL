using HogeschoolPXL.CustomModelValidation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HogeschoolPXL.Models
{
    public class Handboek
    {
        [Key]
        public int HandboekId { get; set; }
        [Required]
        public string Titel { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8, 2)")]
        [Range(4.99, 79.99)]
        public decimal? Kostprijs { get; set; }
        [Required]
        [DisplayName("Uitgiftedatum")]
        [CustomHandboek]
        [DataType(DataType.Date)]
        public DateTime UitgifteDatum { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? Afbeelding { get; set; }
    }
}
