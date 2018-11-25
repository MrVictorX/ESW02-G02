using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectSW.Models;

namespace ProjectSW.Controllers
{
    public class AccountController : Controller
    {
        private readonly ProjectSWContext _context;

        public AccountController(ProjectSWContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewProfile()
        {
            return View();
        }

        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult List()
        {
            var accounts = _context.Users;
            return View(accounts.ToList());
        }
    }
}