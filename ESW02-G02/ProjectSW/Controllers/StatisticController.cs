using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectSW.Data;

namespace ProjectSW.Controllers
{
    public class StatisticController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ProjectSWUser> _userManager;

        public StatisticController(ApplicationDbContext context, UserManager<ProjectSWUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult MostAdopted()
        {
            return View();
        }
    }
}