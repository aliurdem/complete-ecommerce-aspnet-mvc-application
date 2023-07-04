using eTickets.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data
{
	public class AppDbContext : IdentityDbContext<ApplicationUser>
	{
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Actor_Movie> Actors_Movies { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
 

        //Orders Releated Tables 
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<ShoppingCardItem> ShoppingCardItems { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Actor_Movie>()
				.HasKey(am => new
				{
					am.MovieId,
					am.ActorId
				});

			modelBuilder.Entity<Producer>()
				.HasMany(p => p.Movies)
				.WithOne(m => m.Producer)
				.HasForeignKey(p => p.ProducerId)
				.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<Actor_Movie>()
				.HasOne(am => am.Actor)
				.WithMany(a => a.Actors_Movies)
				.HasForeignKey(am => am.ActorId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Actor_Movie>()
				.HasOne(am => am.Movie)
				.WithMany(m => m.Actors_Movies)
				.HasForeignKey(am => am.MovieId)
				.OnDelete(DeleteBehavior.Cascade);

			base.OnModelCreating(modelBuilder);
		}
	}
}
