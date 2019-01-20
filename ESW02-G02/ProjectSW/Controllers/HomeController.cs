using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSW.Data;
using ProjectSW.Models;

namespace ProjectSW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ExitFormSubmited()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        

        public IActionResult Privacy()
        {
            return View();
        }

        
        public async Task<IActionResult> DetailsAnimal(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animal
                .Include(a => a.Breed)
                .FirstOrDefaultAsync(m => m.Id == id);
            animal.Attachments = _context.Attachment.Where(att => att.AnimalId == animal.Id).ToList();
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }
        public async Task<IActionResult> ListAnimals()
        {
            var applicationDbContext = _context.Animal.Include(a => a.Breed).Where(a => a.Available);
            return View(await applicationDbContext.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
