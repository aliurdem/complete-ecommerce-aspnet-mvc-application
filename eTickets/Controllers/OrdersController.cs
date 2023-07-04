using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using eTickets.Data.Card;
using eTickets.Data.Services;
using eTickets.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace eTickets.Controllers
{
	public class OrdersController : Controller
	{
		private readonly IMoviesService _moviesService;
		private readonly ShoppingCard _shoppingCard;
		private readonly IOrdersService _ordersService;


		public OrdersController(IMoviesService moviesService, ShoppingCard shoppingCard, IOrdersService ordersService)
		{
			StripeConfiguration.ApiKey = "sk_test_51NGzhpAyeHS4LOSadbtxwnpFBVW7yefsBt8KZBw5RHZhznWHpAZq6y6Lgc5okXFssnetYpyqm3QZPeHTIVICdgOs00NETHTos9";
			_moviesService = moviesService;
			_shoppingCard = shoppingCard;
			_ordersService = ordersService;
		}

		public async Task<IActionResult> Index()
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			string userRole = User.FindFirstValue(ClaimTypes.Role);

			var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);
			return View(orders);
		}

		public IActionResult ShoppingCard()
		{

			var items = _shoppingCard.GetShoppingCardItems();
			_shoppingCard.ShoppingCardItems = items;
			var response = new ShoppingCardVM()
			{
				ShoppingCard = _shoppingCard,
				ShoppingCardTotal = _shoppingCard.GetShoppingCardTotal()
			};

			return View(response);
		}
		public async Task<IActionResult> AddItemToShoppingCard(int id)
		{
			var movie = await _moviesService.GetMovieByIdAsync(id);

			if (movie != null)
			{
				_shoppingCard.AddItemToCard(movie);
			}
			return RedirectToAction(nameof(ShoppingCard));

		}
		public async Task<IActionResult> RemoveItemFromShoppingCard(int id)
		{
			var movie = await _moviesService.GetMovieByIdAsync(id);

			if (movie != null)
			{
				_shoppingCard.RemoveItemCard(movie);
			}
			return RedirectToAction(nameof(ShoppingCard));

		}

		public async Task<IActionResult> CompleteOrder() 
		{
			return View("OrderCompleted"); 
		}

		[HttpPost]
		public async Task<IActionResult> CreateCheckoutSession()
		{
			var items = _shoppingCard.GetShoppingCardItems();
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

			var domain = "http://localhost:3739/";
			var options = new SessionCreateOptions
			{
				LineItems = new List<SessionLineItemOptions>()
			,
				Mode = "payment",
				SuccessUrl = domain + "Orders/CompleteOrder",
				CancelUrl = domain + "Orders/ShoppingCard",
			};
			foreach (var item in _shoppingCard.ShoppingCardItems)
			{

				var sessionLineItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = ((long)item.Movie.Price * item.Amount) * 100,
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.Movie.Name,
						},
					},
					Quantity = 1
				};
				options.LineItems.Add(sessionLineItem);
			}

			var service = new SessionService();
			Session session = service.Create(options);

			await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);
			await _shoppingCard.ClearShoppingCardAsync();

			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult(303);
		}
	}
}


