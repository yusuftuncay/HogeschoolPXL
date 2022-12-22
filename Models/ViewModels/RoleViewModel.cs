using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models.ViewModels
{
    public class RoleViewModel
    {
        [Required]
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters allowed")]
        public string RoleName { get; set; }
    }
}
