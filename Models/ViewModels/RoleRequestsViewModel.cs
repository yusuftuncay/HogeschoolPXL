using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HogeschoolPXL.Models.ViewModels
{
	public class RoleRequestsViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
