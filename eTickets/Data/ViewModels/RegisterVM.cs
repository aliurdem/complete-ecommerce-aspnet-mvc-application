using System.ComponentModel.DataAnnotations;

namespace eTickets.Data.ViewModels
{
	public class RegisterVM
	{
		[Display(Name = "Full Name")]
		[Required(ErrorMessage = "Full Name is Required")]
		public string FullName { get; set; }

		[Display(Name = "Email address")]
        [Required(ErrorMessage ="Email is Required")]
        public string EmailAddress { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name ="Confirm password")]
		[Required(ErrorMessage ="Confirm Password is required")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage ="Password do not Match")]
		public string ConfirmPassword { get; set; }
	}
}
