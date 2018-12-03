using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectSW.Models;

namespace ProjectSW.Controllers
{
    /// <summary>Controlador Home, onde são executadas as ações da pagina inicial</summary>
    public class HomeController : Controller
    {

        /// <summary>Ação que resulta na pagina inicial</summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>Ação que resulta na pagina da descrição</summary>
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        /// <summary>Ação que resulta na pagina com a lista de animais para adoção</summary>
        public IActionResult ListAnimals()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        /// <summary>Ação que representa uma pagina em desenvolvimento</summary>
        public IActionResult Development()
        {
            return View();
        }

        public IActionResult EmailSent()
        {
            return View();
        }
    }
}
