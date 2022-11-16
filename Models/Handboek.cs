using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models
{
    public class Handboek
    {
        public int HandboekId { get; set; }
        public string Titel { get; set; }
        public double KostPrijs { get; set; }
        [DataType(DataType.Date)]
        public DateTime UitgifteDatum { get; set; }
        public string Afbeelding { get; set; }
    }
}
