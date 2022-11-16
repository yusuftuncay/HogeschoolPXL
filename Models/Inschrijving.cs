namespace HogeschoolPXL.Models
{
    public class Inschrijving
    {
        public int InschrijvingId { get; set; }
        public int StudentId { get; set; }
        public int VakLectorId { get; set; }
        public int AcademieJaarId { get; set; }
        public Student Student { get; set; }
        public VakLector VakLector { get; set; }
    }
}
