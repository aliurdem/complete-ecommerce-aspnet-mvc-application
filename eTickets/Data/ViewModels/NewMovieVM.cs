using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eTickets.Data;
using eTickets.Data.Base;
using Microsoft.AspNetCore.Http;

namespace eTickets.Models
{
	public class NewMovieVM 
	{
		public int Id { get; set; }

		[Required(ErrorMessage ="Name is Required")]
        [Display(Name = "Movie Name")]
        public string Name { get; set; }
		
		[Display(Name = "Movie Description")]
		[Required(ErrorMessage = "Description is Required")]
		public string Description { get; set; }


        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage = "Profile Picture is Required")]
        [NotMapped]
        public IFormFile Image { get; set; }

        public string ImageURL { get; set; }

		[Display(Name = "Start Date")]
		[Required(ErrorMessage = "Start Date is Required")]
		public DateTime StartDate  { get; set; }

		[Display(Name = "End Date")]
		[Required(ErrorMessage = "End Date is Required")]
		public DateTime EndDate { get; set; }

		[Display(Name = "Price in $")]
		[Required(ErrorMessage = "Price is Required")]
		public double Price { get; set; }

		[Display(Name = "Select a Category")]
		[Required(ErrorMessage = "Movie Category is Required")]
		public MovieCategory MovieCategory { get; set; }


		// Relationships 

		[Display(Name = "Select Actor(s)")]
		[Required(ErrorMessage = "Movie Actors is Required")]
		public List<int> ActorIds { get; set; }


		[Display(Name = "Select a Cinema")]
		[Required(ErrorMessage = "Movie Cinema is Required")]
		public int CinemaId { get; set; }


		[Display(Name = "Select a Producer")]
		public int? ProducerId { get; set; }
    }
}
