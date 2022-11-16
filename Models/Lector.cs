namespace HogeschoolPXL.Models
{
    public class Lector
    {
        public int LectorId { get; set; }
        public int GebruikerId { get; set; }
        public Gebruiker Gebruiker { get; set; }
    }
}
