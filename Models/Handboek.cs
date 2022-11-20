using HogeschoolPXL.CustomModelValidation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HogeschoolPXL.Models
{
    public class Handboek
    {
        [Key]
        public int HandboekId { get; set; }
        [Required]
        public string Titel { get; set; }
        [Required]
        public double KostPrijs { get; set; }
        [Required]
        [CustomHandboek]
        [DataType(DataType.Date)]
        public DateTime UitgifteDatum { get; set; }
        public string? Afbeelding { get; set; }
    }
}
