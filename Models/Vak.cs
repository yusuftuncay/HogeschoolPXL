namespace HogeschoolPXL.Models
{
    public class Vak
    {
        public int VakId { get; set; }
        public string VakNaam { get; set; }
        public int StudiePunten { get; set; }
        public int HandboekId { get; set; }
        public Handboek Handboek { get; set; }
    }
}
