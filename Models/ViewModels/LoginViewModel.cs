using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
        [Required]
        [DisplayName("Wachtwoord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
