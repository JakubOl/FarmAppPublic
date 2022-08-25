using System.ComponentModel.DataAnnotations;

namespace Models.Dtos
{
	public class RegisterUserDto
	{
		public string FirstName { get; set; } = "";

		public string LastName { get; set; } = "";

		[Required]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string EmailAddress { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is required")]
		[StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
		[DataType(DataType.Password)]
		[Compare("Password")]
        [Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; }
	}
}
