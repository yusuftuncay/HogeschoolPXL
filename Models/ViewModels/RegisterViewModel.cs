using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models.ViewModels
{
    public class RegisterViewModel : LoginViewModel
    {
        [Required]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
