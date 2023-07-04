using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eTickets.Data.Base;
using Microsoft.AspNetCore.Http;

namespace eTickets.Models
{
	public class Producer : IEntityBase
	{
		[Key]
        public int Id { get; set; }
        
        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage ="The Profile Picture is Required")]
		[NotMapped]
		public IFormFile ProfilePicture { get; set; }

		public string ProfilePictureURL { get; set; }

        [Display(Name = "Full Name")]
		[Required(ErrorMessage = "The Full Name is Required")]
		public string FullName { get; set; }
        
        [Display(Name = "Bigraphy")]
		[Required(ErrorMessage = "The Bigraphy is Required")]

		public string Bio { get; set; }
        
        public List<Movie> Movies { get; set; }
    }
}
