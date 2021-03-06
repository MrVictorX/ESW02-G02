﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectSW.Data;
using ProjectSW.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ProjectSW.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ProjectSWUser> _userManager;

        public JobsController(ApplicationDbContext context, UserManager<ProjectSWUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Jobs
        /// <summary>Ação que resulta na lista de Tarefas</summary>
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Job.Include(j => j.Employee).Include(j => j.Employee.Account);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Jobs/Details/5
        /// <summary>Ação que resulta numa pagina com os detalhes de uma tarefa</summary>
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .Include(j => j.Employee).Include(j => j.Employee.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Jobs/Create
        [Authorize(Roles = "Administrador, Funcionario")]
        /// <summary>Ação que resulta numa pagina com o formulario da criação de uma tarefa</summary>
        public IActionResult Create()
        {
            //ViewData["EmployeeId"] = new SelectList(_context.Employee.Include(j => j.Account), "Account.Email", "Account.Email");
            ViewData["EmployeeId"] = new SelectList(_context.Employee.Join(
                   _context.User,
                   employee => employee.AccountId,
                   account => account.Id,
                      (employee, account) => new  // result selector
                      {
                          employeeId = employee.Id,
                          accountMail = account.Email
                      }), "employeeId", "accountMail");

            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>Ação que resulta na criação de uma tarefa</summary>
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,Name,Day,Hour,Description")] Job job)
        {
            if (ModelState.IsValid)
            {
                _context.Add(job);
                await _context.SaveChangesAsync();

                var employee = await _userManager.FindByIdAsync(_context.Employee.Where(e => e.Id.Equals(job.EmployeeId)).FirstOrDefault().AccountId);

                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
                //Antes de fazer push remover chave
                var client = new SendGridClient(apiKey);
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress("QuintaDoMiao@exemplo.com", "Quinta do Miao"),
                    PlainTextContent = "Nova tarefa",
                    Subject = "Nova tarefa",
                    HtmlContent = $"Olá.<br><br>Foi-lhe atribuido uma nova tarefa a completar, por favor verifique a lista de tarefas.<br><br>" +
                    "Cumprimentos,<br>Quinta do Mião."
                };
                msg.AddTo(new EmailAddress(employee.Email, employee.Name));
                var response = await client.SendEmailAsync(msg);
         
                return RedirectToAction(nameof(Index));
            }

            ViewData["EmployeeId"] = new SelectList(_context.Employee.Join(
                _context.User,
                employee => employee.AccountId,
                account => account.Id,
                (employee, account) => new  // result selector
                {
                    employeeId = employee.Id,
                    accountMail = account.Email
                }), "employeeId", "accountMail");

            return View(job);
        }


        // GET: Jobs/Edit/5
        [Authorize(Roles = "Administrador, Funcionario")]
        /// <summary>Ação que resulta numa pagina com os detalhes de uma tarefa de forma a poderem ser editados</summary>
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee.Join(
                   _context.User,
                   employee => employee.AccountId,
                   account => account.Id,
                      (employee, account) => new  // result selector
                      {
                          employeeId = employee.Id,
                          accountMail = account.Email
                      }), "employeeId", "accountMail");
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>Ação que resulta numa pagina com a edição de uma tarefa</summary>
        public async Task<IActionResult> Edit(string id, [Bind("Id,EmployeeId,Name,Day,Hour,Description")] Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employee.Join(
                   _context.User,
                   employee => employee.AccountId,
                   account => account.Id,
                      (employee, account) => new  // result selector
                      {
                          employeeId = employee.Id,
                          accountMail = account.Email
                      }), "employeeId", "accountMail");
            return View(job);
        }

        // GET: Jobs/Delete/5
        [Authorize(Roles = "Administrador")]
        /// <summary>Ação que resulta num prompt para apagar uma tarefa</summary>
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .Include(j => j.Employee.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        /// <summary>Ação que resulta numa tarefa apagada</summary>
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var job = await _context.Job.FindAsync(id);
            _context.Job.Remove(job);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(string id)
        {
            return _context.Job.Any(e => e.Id == id);
        }
    }
}
