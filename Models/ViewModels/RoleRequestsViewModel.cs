using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models.ViewModels
{
	public class RoleRequestsViewModel
    {
        [Key]
        public int Id { get; set; }
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters allowed")]
        public string Voornaam { get; set; }
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters allowed")]
        public string Naam { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters allowed")]
        public string Role { get; set; }
    }
}
