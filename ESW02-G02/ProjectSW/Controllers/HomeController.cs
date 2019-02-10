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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MostAdopted()
        {
            List<Animal> animals = new List<Animal>();
            var applicationDbContext = _context.Animal;

            foreach (Animal a in await applicationDbContext.ToListAsync())
            {
                if (!a.Available)
                {
                    animals.Add(a);
                }
            }
            return View(animals);
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

        public async Task<IActionResult> ShowHeatMap()
        {
            List<Location> locations = new List<Location>();
            var applicationDbContext = _context.Adopter;
    
            foreach(Adopter a in await applicationDbContext.ToListAsync())
            {
                locations.Add(GetLocationRequest(a.PostalCode));
            }


            return View(locations);
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

        private Location GetLocationRequest(string postalcode)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.geonames.org/postalCodeLookupJSON?postalcode="+ postalcode + "&country=PT&username=tesing_software");
            request.Method = Http.Get;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("Error: Response status " + response.StatusCode);
                }

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            var locations = JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());
                            Location location = new Location { Lat = locations.postalcodes[0].lat, Lng = locations.postalcodes[0].lng };
                            return location;
                        }
                    }//end of reader
                }//End of stream
            }//End of response

            return null;
        }
    }
}
