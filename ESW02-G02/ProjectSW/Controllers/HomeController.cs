using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectSW.Data;
using ProjectSW.Models;
using static System.Net.WebRequestMethods;

namespace ProjectSW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Ação que resulta na pagina principal do site</summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>Ação que resulta numa submissão de um exitForm para adotar um animal</summary>
        public IActionResult ExitFormSubmited()
        {
            return View();
        }

        [Authorize]
        /// <summary>Ação que resulta na pagina com as estatisticas de adoção</summary>
        public IActionResult Statistics()
        {
            return View();
        }

        /// <summary>Ação que resulta numa pagina com os detalhes de um animal</summary>
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

        /// <summary>Ação que resulta numa pagina com uma lista de animais</summary>
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