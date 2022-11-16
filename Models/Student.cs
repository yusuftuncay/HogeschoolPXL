namespace HogeschoolPXL.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public int GebruikerId { get; set; }
        public Gebruiker Gebruiker { get; set; }
    }
}
