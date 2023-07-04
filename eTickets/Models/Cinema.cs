using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eTickets.Data.Base;
using Microsoft.AspNetCore.Http;

namespace eTickets.Models
{
	public class Cinema : IEntityBase
	{
        [Key]
        public int Id { get; set; }
        [Display(Name = "Logo")]
		[NotMapped]
        public IFormFile Logo { get; set; }
		public string LogoURL { get; set; }

		[Display(Name = "Name")]
		public string Name { get; set; }

		[Display(Name = "Description")]
		public string  Description { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
