using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectSW.Data;
using ProjectSW.Models;

namespace ProjectSW.Controllers
{
    public class AnimalMonitoringReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimalMonitoringReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AnimalMonitoringReport
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnimalMonitoringReport.ToListAsync());
        }

        // GET: AnimalMonitoringReport/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animalMonitoringReport = await _context.AnimalMonitoringReport
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animalMonitoringReport == null)
            {
                return NotFound();
            }

            return View(animalMonitoringReport);
        }

        // GET: AnimalMonitoringReport/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnimalMonitoringReport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Description,EntryDate")] AnimalMonitoringReport animalMonitoringReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animalMonitoringReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(animalMonitoringReport);
        }

        // GET: AnimalMonitoringReport/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animalMonitoringReport = await _context.AnimalMonitoringReport.FindAsync(id);
            if (animalMonitoringReport == null)
            {
                return NotFound();
            }
            return View(animalMonitoringReport);
        }

        // POST: AnimalMonitoringReport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Description,EntryDate")] AnimalMonitoringReport animalMonitoringReport)
        {
            if (id != animalMonitoringReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animalMonitoringReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalMonitoringReportExists(animalMonitoringReport.Id))
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
            return View(animalMonitoringReport);
        }

        // GET: AnimalMonitoringReport/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animalMonitoringReport = await _context.AnimalMonitoringReport
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animalMonitoringReport == null)
            {
                return NotFound();
            }

            return View(animalMonitoringReport);
        }

        // POST: AnimalMonitoringReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var animalMonitoringReport = await _context.AnimalMonitoringReport.FindAsync(id);
            _context.AnimalMonitoringReport.Remove(animalMonitoringReport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimalMonitoringReportExists(string id)
        {
            return _context.AnimalMonitoringReport.Any(e => e.Id == id);
        }
    }
}
