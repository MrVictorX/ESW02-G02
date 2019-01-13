using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectSW.Data;
using ProjectSW.Models;

namespace ProjectSW.Controllers
{
    [Authorize]
    public class AnimalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Animals
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Animal.Include(a => a.Breed);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(string id)
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

        // GET: Animals/Create
        public IActionResult Create()
        {
            ViewData["BreedId"] = new SelectList(_context.Set<Breed>(), "Id", "Name");
            return View();
        }

        // POST: Animals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Size,Gender,BreedId,EntryDate,Foto")] Animal animal, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                var filePath = Path.GetTempFileName();

                if (foto.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await foto.CopyToAsync(stream);
                    }
                }
                using (var memoryStream = new MemoryStream())
                {
                    await foto.CopyToAsync(memoryStream);
                    animal.Foto = memoryStream.ToArray();
                }
                
                _context.Add(animal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BreedId"] = new SelectList(_context.Set<Breed>(), "Id", "Id", animal.BreedId);
            return View(animal);
        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            ViewData["BreedId"] = new SelectList(_context.Set<Breed>(), "Id", "Id", animal.BreedId);
            return View(animal);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Size,Gender,BreedId,EntryDate,Foto")] Animal animal, IFormFile foto, IFormFile attachment)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            if (animal.Attachments == null)
            {
                animal.Attachments = new List<Attachment>();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var filePath = Path.GetTempFileName();

                    if(foto != null) { 
                        if (foto.Length > 0)
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await foto.CopyToAsync(stream);
                            }
                        }
                        using (var memoryStream = new MemoryStream())
                        {
                            await foto.CopyToAsync(memoryStream);
                            animal.Foto = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        animal.Foto = _context.Animal.Select(a => a.Foto).ToList().First();
                    }
                    var att = new Attachment { Name = attachment.FileName };
                    if (attachment.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await attachment.CopyToAsync(stream);
                        }
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        await attachment.CopyToAsync(memoryStream);
                        att.File = memoryStream.ToArray();
                        //verificar possibilidade de juntar varios pdfs num só e guardar só esse
                        var list = new List<Attachment>
                        {
                            att
                        };
                        _context.Add(att);
                        animal.Attachments = list;
                    }
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.Id))
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
            ViewData["BreedId"] = new SelectList(_context.Set<Breed>(), "Id", "Id", animal.BreedId);
            return View(animal);
        }

        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animal
                .Include(a => a.Breed)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var animal = await _context.Animal.FindAsync(id);
            var attachment = _context.Attachment.Select(att => att).Where(att => att.AnimalId == id).ToList();
            if (attachment != null)
                foreach (var att in attachment)
                {
                    _context.Attachment.Remove(att);
                }
            _context.Animal.Remove(animal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimalExists(string id)
        {
            return _context.Animal.Any(e => e.Id == id);
        }

        // eliminar attachment
        [HttpPost, ActionName("Delete-Attachment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAttachment(string id)
        {
            var attachment = _context.Attachment.Select(att => att).Where(att => att.AnimalId == id).First();
            if(attachment != null)
            _context.Attachment.Remove(attachment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
