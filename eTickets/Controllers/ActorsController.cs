using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace eTickets.Controllers
{
	public class ActorsController : Controller
	{
		private readonly IActorsService _service;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ActorsController(IActorsService service, IWebHostEnvironment webHostEnvironment)
		{
			_service = service;
			_webHostEnvironment = webHostEnvironment;
		}

		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			var data = await _service.GetAllAsync();
			return View(data);
		}

		//Get: Actors/Create
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create([Bind("FullName,ProfilePicture,Bio")] Actor actor)
		{
			if (!ModelState.IsValid)
			{
				return View(actor);
			}
			if (actor.ProfilePicture != null)
			{
				string folder = "images/ActorImages";
				folder += Guid.NewGuid().ToString() +actor.ProfilePicture.FileName;

				actor.ProfilePictureURL = "/" + folder;

				string serveFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

				await actor.ProfilePicture.CopyToAsync(new FileStream(serveFolder, FileMode.Create));
			}
			await _service.AddAsync(actor);
			return RedirectToAction(nameof(Index));
		}

		//Get: Actors/Details/1
		[AllowAnonymous]
		public async Task<IActionResult> Details(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);

			if (actorDetails == null) return View("NotFound");
			return View(actorDetails);
		}

		//Get: Actors/Edit/1
		public async Task<IActionResult> Edit(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);
			if (actorDetails == null) return View("NotFound");
			return View(actorDetails);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePicture,Bio")] Actor actor)
		{
			if (!ModelState.IsValid)
			{
				return View(actor);
			}
            if (actor.ProfilePicture != null)
            {
                string folder = "images/ActorImages";
                folder += Guid.NewGuid().ToString() + actor.ProfilePicture.FileName;

                actor.ProfilePictureURL = "/" + folder;

                string serveFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                await actor.ProfilePicture.CopyToAsync(new FileStream(serveFolder, FileMode.Create));
            }
            await _service.UpdateAsync(id, actor);
			return RedirectToAction(nameof(Index));
		}

		//Get: Actors/Delete/1
		public async Task<IActionResult> Delete(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);
			if (actorDetails == null) return View("NotFound");
			return View(actorDetails);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);
			if (actorDetails == null) return View("NotFound");
			await _service.DeleteAsync(id); 
			
			return RedirectToAction(nameof(Index));
		}
	}
}
