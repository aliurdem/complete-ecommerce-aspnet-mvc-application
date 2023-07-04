using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eTickets.Data.Base;
using Microsoft.AspNetCore.Http;

namespace eTickets.Models
{
	public class Actor : IEntityBase
	{
		[Key]
		public int Id { get; set; }

		[Display(Name = "Profile Picture")]
		[Required (ErrorMessage ="Profile Picture is Required")]
		[NotMapped]
		public IFormFile ProfilePicture { get; set; }

        public string ProfilePictureURL { get; set; }

        [Display(Name = "Full Name")]
		[Required (ErrorMessage ="Full Name is Required")]

		public string FullName { get; set; }

		[Display(Name = "Bio")]
		[Required(ErrorMessage = "Biograpyh is Required")]

		public string Bio { get; set; }

        public List<Actor_Movie> Actors_Movies{ get; set; }
    }
}
