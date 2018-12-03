using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectSW.Models;

namespace ProjectSW.Controllers
{
    /// <summary>Controlador Users, onde são executadas as ações relacionadas com os utilizadores</summary>
    public class UserController : Controller
    {
        private readonly ProjectSWContext _context;

        /// <summary>Construtor que guarda o contexto da build</summary>
        public UserController(ProjectSWContext context)
        {
            _context = context;
        }

        /// <summary>Ação que resulta na pagina com a lista de todos os utilizadores</summary>
        public IActionResult UserList()
        {
            return View(_context.Users.ToList());
        }

        public IActionResult DeleteUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UserList));
        }
    }
}