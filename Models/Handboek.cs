using HogeschoolPXL.CustomModelValidation;
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
        //[Range(4.99, 100)]
        [Column(TypeName = "decimal(8, 2)")]
        [DataType(DataType.Currency)]
        public decimal? KostPrijs { get; set; }
        [Required]
        [CustomHandboek]
        [DataType(DataType.Date)]
        public DateTime UitgifteDatum { get; set; }
        public string? Afbeelding { get; set; }
    }
}
