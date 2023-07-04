using System.IO;
using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Controllers
{
	public class ProducersController : Controller
	{

		private readonly IProducersService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProducersController(IProducersService service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
		{
			var allProcuders = await _service.GetAllAsync();
			return View(allProcuders);
		}

		// Get Producer Details 

		public async Task<IActionResult> Details(int id)
		{
			var producerDetails = await _service.GetByIdAsync(id);

			if(producerDetails == null) return View("NotFound");
			
			return View(producerDetails);	
		}

		public IActionResult Create()
		{
			return View(); 
		}

		[HttpPost]
		public async Task<IActionResult> Create([Bind("FullName,ProfilePicture,Bio")]Producer producer)
		{
			if(!ModelState.IsValid) 
			{
				return View(producer);
			}
            if (producer.ProfilePicture != null)
            {
                string folder = "images/ProducerImages";
                folder += Guid.NewGuid().ToString() + producer.ProfilePicture.FileName;
                producer.ProfilePictureURL = "/" + folder;

                string serveFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                await producer.ProfilePicture.CopyToAsync(new FileStream(serveFolder, FileMode.Create));
            }
            await _service.AddAsync(producer);
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Edit(int id)
		{
			var producerDetails = await _service.GetByIdAsync(id);
			if (producerDetails == null) return View("NotFound");
			return View(producerDetails);
		}

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePicture,Bio")] Producer producer)
        {
            if (!ModelState.IsValid)
            {
                return View(producer);
            }
            if (producer.ProfilePicture != null)
            {
                string folder = "images/ProducerImages";
                folder += Guid.NewGuid().ToString() + producer.ProfilePicture.FileName;
                producer.ProfilePictureURL = "/" + folder;

                string serveFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                await producer.ProfilePicture.CopyToAsync(new FileStream(serveFolder, FileMode.Create));
            }
            if (id == producer.Id)
			{
				await _service.UpdateAsync(id,producer);
				return RedirectToAction(nameof(Index));
            }
			return View(producer);

        }

		//GET : producer/delete/1
		public async Task<IActionResult> Delete(int id)
		{
			var producerDetails = await _service.GetByIdAsync(id);
			if (producerDetails == null) return View("Not Found");
			return View(producerDetails); 
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var producerDetails = await _service.GetByIdAsync(id);
			if (producerDetails == null) return View("Not Found"); 

			await _service.DeleteAsync(id);

			return RedirectToAction(nameof(Index));

		}
    }
}
