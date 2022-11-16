namespace HogeschoolPXL.Models
{
    public class VakLector
    {
        public int VakLectorId { get; set; }
        public int LectorId { get; set; }
        public int VakId { get; set; }
        public Lector Lector { get; set; }
        public Vak Vak { get; set; }
    }
}
