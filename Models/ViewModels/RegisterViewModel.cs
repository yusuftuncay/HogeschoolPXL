using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models.ViewModels
{
    public class RegisterViewModel : LoginViewModel
    {
        [Required]
        [DataType(DataType.Password)]
		[DisplayName("Bevestig Wachtwoord")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Roles { get; set; }
        [Required]
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters allowed")]
		public string Voornaam { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters allowed")]
		public string Naam { get; set; }
    }
}
