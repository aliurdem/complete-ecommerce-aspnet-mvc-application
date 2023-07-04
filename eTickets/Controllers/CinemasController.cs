using eTickets.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;

namespace eTickets.Controllers
{
    public class CinemasController : Controller
    {
        private readonly ICinemasService _service;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public CinemasController(ICinemasService service, IWebHostEnvironment webHostEnvironment)
		{
			_service = service;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<IActionResult> Index()
        {
            var allCinemas = await _service.GetAllAsync();
            return View(allCinemas);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Logo,Description")] Cinema cinema)
        {
			if (!ModelState.IsValid)
			{
				return View(cinema);
			}

			if (cinema.Logo != null)
			{
				string folder = "images/ActorImages";
				folder += Guid.NewGuid().ToString() + cinema.Logo.FileName;
				cinema.LogoURL = "/" + folder;

				string serveFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

				await cinema.Logo.CopyToAsync(new FileStream(serveFolder, FileMode.Create));
			}
			await _service.AddAsync(cinema);
			return RedirectToAction(nameof(Index));
		}

        public async Task<IActionResult> Details(int id)
        {
            var cinema = await _service.GetByIdAsync(id);
            return View(cinema);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cinemaDetails = await _service.GetByIdAsync(id);
            if (cinemaDetails == null) return View("Not found !");
            return View(cinemaDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Logo,Description")] Cinema cinema)
        {
            if (!ModelState.IsValid)
            {
                return View(cinema);
            }
            if (cinema.Logo != null)
            {
                string folder = "images/ActorImages";
                folder += Guid.NewGuid().ToString() + cinema.Logo.FileName;
                cinema.LogoURL = "/" + folder;

                string serveFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                await cinema.Logo.CopyToAsync(new FileStream(serveFolder, FileMode.Create));
            }
            await _service.UpdateAsync(id, cinema);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cinema = await _service.GetByIdAsync(id);
            if (cinema == null) return View("Not Found");
            return View(cinema);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var cinema = await _service.GetByIdAsync(id);
            if (cinema == null)
                return View("NotFound");
            await _service.DeleteAsync(id);

            return RedirectToAction("Index");
        }

      
    }
}
