using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectSW.Models;

namespace ProjectSW.Controllers
{
    public class UserController : Controller
    {
        private readonly ProjectSWContext _context;

        public UserController(ProjectSWContext context)
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

        public IActionResult UserList()
        {
            return View(_context.Users.ToList());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}