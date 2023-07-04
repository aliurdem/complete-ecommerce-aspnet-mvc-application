using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTickets.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eTickets.Data.Card
{
	public class ShoppingCard
	{
        public AppDbContext _context { get; set; }

		public string ShoppingCardId { get; set; }
        public List<ShoppingCardItem> ShoppingCardItems { get; set; }

        public ShoppingCard(AppDbContext context)
		{
			_context = context;
		}

		public static ShoppingCard GetShoppingCard(IServiceProvider service)
		{
			ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
			var context = service.GetService<AppDbContext>(); 
			string cardId = session.GetString("CardId") ?? Guid.NewGuid().ToString();
			session.SetString("CardId", cardId); 

			return new ShoppingCard(context) {ShoppingCardId  = cardId}; 
		}

		public List<ShoppingCardItem> GetShoppingCardItems() => ShoppingCardItems ?? (ShoppingCardItems = _context.ShoppingCardItems.Where(n => n.ShoppingCardId == ShoppingCardId).Include(n => n.Movie).ToList());
		

		public double GetShoppingCardTotal() => _context.ShoppingCardItems.Where(n => n.ShoppingCardId == ShoppingCardId).Select(n=> n.Movie.Price*n.Amount).Sum();

		public void AddItemToCard(Movie movie)
		{
			var shoppingCartItem = _context.ShoppingCardItems.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCardId == ShoppingCardId);

			if (shoppingCartItem == null)
			{
				shoppingCartItem = new ShoppingCardItem()
				{
					ShoppingCardId = ShoppingCardId,
					Movie = movie,
					Amount = 1
				};
				_context.ShoppingCardItems.Add(shoppingCartItem);
			}
			else 
			{
				shoppingCartItem.Amount++; 
			}

			_context.SaveChanges();
		}

		public void RemoveItemCard(Movie movie) 
		{
			var shoppingCartItem = _context.ShoppingCardItems.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCardId == ShoppingCardId);

			if (shoppingCartItem != null)
			{
				if (shoppingCartItem.Amount > 1)
				{
					shoppingCartItem.Amount--;
				}
				else
				{
					_context.ShoppingCardItems.Remove(shoppingCartItem);
				}
			}
			
			_context.SaveChanges();
		}

		public async Task ClearShoppingCardAsync() 
		{
			var items = await _context.ShoppingCardItems.Where(n => n.ShoppingCardId == ShoppingCardId).Include(n => n.Movie).ToListAsync();
			_context.ShoppingCardItems.RemoveRange(items); 
			await _context.SaveChangesAsync();
		}
	}
}
