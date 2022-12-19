using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DisplayName("Wachtwoord")]
        public string Password { get; set; }
    }
}
