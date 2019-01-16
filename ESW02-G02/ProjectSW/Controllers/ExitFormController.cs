using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectSW.Data;
using ProjectSW.Models;

namespace ProjectSW.Controllers
{
    [Authorize]
    public class ExitFormController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExitFormController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExitForm
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ExitForm.Include(e => e.Animal);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ExitForm/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exitForm = await _context.ExitForm
                .Include(e => e.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exitForm == null)
            {
                return NotFound();
            }

            return View(exitForm);
        }

        // GET: ExitForm/Create
        [AllowAnonymous]
        public async Task<IActionResult> Create(string AnimalId)
        {
            if (AnimalId == null)
            {
                return NotFound();
            }
            var animal = await _context.Animal.Include(e => e.Breed).FirstOrDefaultAsync(m => m.Id == AnimalId);
            if (animal == null)
            {
                return NotFound();
            }
            ViewData["Animal"] = animal;
            ViewData["BreedName"] = animal.Breed.Name;

            return View();
        }

        // POST: ExitForm/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("Id,AnimalId,ReportId,AdopterName,AdopterAddress,AdopterEmail,Description,Date,Motive,State")] ExitForm exitForm)
        {
            if (ModelState.IsValid)
            {
                exitForm.State = "Pendente";
                _context.Add(exitForm);
                await _context.SaveChangesAsync();
                return RedirectToAction("ExitFormSubmited", "Home");
            }
            ViewData["AnimalId"] = new SelectList(_context.Animal, "Id", "Name", exitForm.AnimalId);
            ViewData["ReportId"] = new SelectList(_context.AnimalMonitoringReport, "Id", "Id", exitForm.ReportId);
            return View(exitForm);
        }

        // GET: ExitForm/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var exitForm = await _context.ExitForm
                .Include(e => e.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exitForm == null)
            {
                return NotFound();
            }
            return View(exitForm);
        }

        // POST: ExitForm/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,AnimalId,ReportId,AdopterName,AdopterAddress,AdopterEmail,Description,Date,Motive,State")] ExitForm exitForm)
        {
            if (id != exitForm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (exitForm.AnimalId == null)
                    {
                        return NotFound();
                    }
                    var animal = await _context.Animal.Include(e => e.Breed).FirstOrDefaultAsync(m => m.Id == exitForm.AnimalId);
                    if (animal == null)
                    {
                        return NotFound();
                    }

                    if(exitForm.State == "Granted" || exitForm.State == "Denied")
                    {
                        _context.AdoptionsHist.Add(new AdoptionsHist
                        {
                            AdopterEmail = exitForm.AdopterEmail,
                            AdopterAddress = exitForm.Description,
                            Motive = exitForm.Motive,
                            AnimalBreedName = animal.Breed.Name,
                            AnimalDateOfBirth = animal.DateOfBirth,
                            AnimalGender = animal.Gender,
                            EntryDate = animal.EntryDate,
                            Result = exitForm.State
                        });
                    }
                    if (exitForm.State == "Granted")
                    {
                        animal.Available = false;
                    }
                    _context.Update(animal);
                    _context.Update(exitForm);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExitFormExists(exitForm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnimalId"] = new SelectList(_context.Animal, "Id", "Name", exitForm.AnimalId);
            return View(exitForm);
        }

        // GET: ExitForm/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exitForm = await _context.ExitForm
                .Include(e => e.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exitForm == null)
            {
                return NotFound();
            }

            return View(exitForm);
        }

        // POST: ExitForm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var exitForm = await _context.ExitForm.FindAsync(id);
            _context.ExitForm.Remove(exitForm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExitFormExists(string id)
        {
            return _context.ExitForm.Any(e => e.Id == id);
        }
    }
}
