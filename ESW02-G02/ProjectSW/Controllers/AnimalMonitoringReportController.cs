﻿using System;
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
    public class AnimalMonitoringReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimalMonitoringReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AnimalMonitoringReport
        /// <summary>Ação que resulta na lista de AnimalReports</summary>
        public async Task<IActionResult> Index(string exitFormId)
        {
            var applicationDbContext = _context.AnimalMonitoringReport.Include(a => a.Employee).Include(a => a.Employee.Account).Where(a => a.ExitFormId == exitFormId);
            ViewData["ExitFormId"] = exitFormId;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AnimalMonitoringReport/Details/5
        /// <summary>Ação que resulta numa pagina com os detalhes de uma AnimalReports</summary>
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animalMonitoringReport = await _context.AnimalMonitoringReport
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animalMonitoringReport == null)
            {
                return NotFound();
            }

            return View(animalMonitoringReport);
        }

        // GET: AnimalMonitoringReport/Create
        [Authorize(Roles = "Administrador, Funcionario")]
        /// <summary>Ação que resulta numa pagina com o formulario da criação de uma AnimalReports</summary>
        public IActionResult Create(string exitFormId)
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee.Include(a => a.Account), "Id", "Account.Email");
            ViewData["ExitFormId"] = exitFormId;
            return View();
        }

        // POST: AnimalMonitoringReport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>Ação que resulta na criação de uma AnimalReports</summary>
        public async Task<IActionResult> Create([Bind("Id,ExitFormId,Description,EntryDate,EmployeeId")] AnimalMonitoringReport animalMonitoringReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animalMonitoringReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "AnimalMonitoringReport", new { exitFormId = animalMonitoringReport.ExitFormId });
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "Account.Email", animalMonitoringReport.EmployeeId);
            ViewData["ExitFormId"] = ViewBag.ExitFormId;
            return View(animalMonitoringReport);
        }

        // GET: AnimalMonitoringReport/Edit/5
        [Authorize(Roles = "Administrador, Funcionario")]
        /// <summary>Ação que resulta numa pagina com os detalhes de uma AnimalReports de forma a poderem ser editados</summary>
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
            ViewData["EmployeeId"] = new SelectList(_context.Employee.Include(a => a.Account), "Id", "Account.Email");
            return View(animalMonitoringReport);
        }

        // POST: AnimalMonitoringReport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>Ação que resulta numa pagina com a edição de uma AnimalReports</summary>
        public async Task<IActionResult> Edit(string id, [Bind("Id,ExitFormId,Description,EntryDate,EmployeeId")] AnimalMonitoringReport animalMonitoringReport)
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
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "Account.Email", animalMonitoringReport.EmployeeId);
            return View(animalMonitoringReport);
        }

        // GET: AnimalMonitoringReport/Delete/5
        [Authorize(Roles = "Administrador")]
        /// <summary>Ação que resulta num prompt para apagar uma AnimalReports</summary>
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animalMonitoringReport = await _context.AnimalMonitoringReport
                .Include(a => a.Employee)
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
        /// <summary>Ação que resulta numa AnimalReports apagada</summary>
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