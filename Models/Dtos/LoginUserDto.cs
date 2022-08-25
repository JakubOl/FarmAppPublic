using System.ComponentModel.DataAnnotations;

namespace Models.Dtos
{
    public class LoginUserDto
    {
		[Required]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
		public bool Remember { get; set; } = false;
	}
}
