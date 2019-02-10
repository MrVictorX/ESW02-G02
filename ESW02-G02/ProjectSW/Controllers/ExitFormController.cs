using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectSW.Data;
using ProjectSW.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Create(string animalId)
        {
            if (animalId == null)
            {
                return NotFound();
            }
            var animal = await _context.Animal.Include(e => e.Breed).FirstOrDefaultAsync(m => m.Id == animalId);
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
        public async Task<IActionResult> Create([Bind("Id,AnimalId,AdopterName,AdopterAddress,AdopterEmail,AdopterCitizenCard,AdopterPostalCode,Description,Date,Motive,State")] ExitForm exitForm)
        {
            if (ModelState.IsValid)
            {
                exitForm.State = "Pendente";
                _context.Add(exitForm);
                await _context.SaveChangesAsync();

                var admins = _context.User.Where(u => u.UserType.Equals("Administrador"));

                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
                //Antes de fazer push remover chave
                var client = new SendGridClient(apiKey);
                var msg1 = new SendGridMessage()
                {
                    From = new EmailAddress("QuintaDoMiao@exemplo.com", "Quinta do Miao"),
                    PlainTextContent = "Novo pedido de adoção",
                    Subject = "Novo pedido de adoção",
                    HtmlContent = $"Olá.<br><br>Um pedido de adoção foi efetuado, por favor verifique a lista de pedidos e dê uma resposta.<br><br>" +
                    "Cumprimentos,<br>Quinta do Mião."
                };
                foreach (ProjectSWUser admin in admins)
                {
                    msg1.AddTo(new EmailAddress(admin.Email, admin.Name));
                }
                var response1 = await client.SendEmailAsync(msg1);

                var msg2 = new SendGridMessage()
                {
                    From = new EmailAddress("QuintaDoMiao@exemplo.com", "Quinta do Miao"),
                    PlainTextContent = "Pedido de adoção",
                    Subject = "Pedido de adoção",
                    HtmlContent = $"Olá {exitForm.AdopterName}.<br>O seu pedido de adoção foi efetuado, dentro de uma semana irá receber um e-mail acerca do estado do seu pedido.<br><br>" +
                    "Cumprimentos,<br>Quinta do Mião."
                };
                msg2.AddTo(new EmailAddress(exitForm.AdopterEmail, exitForm.AdopterName));
                var response2 = await client.SendEmailAsync(msg2);

                return RedirectToAction("ExitFormSubmited", "Home");
            }
            ViewData["AnimalId"] = new SelectList(_context.Animal, "Id", "Name", exitForm.AnimalId);
            return View(exitForm);
        }

        // GET: ExitForm/Edit/5
        [Authorize(Roles = "Administrador, Funcionario")]
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
        public async Task<IActionResult> Edit(string id, [Bind("Id,AnimalId,AdopterName,AdopterAddress,AdopterEmail,AdopterCitizenCard,AdopterPostalCode,Description,Date,Motive,State")] ExitForm exitForm)
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

                    var apiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
                    //Antes de fazer push remover chave
                    var client = new SendGridClient(apiKey);

                    var msg = new SendGridMessage()
                    {
                        From = new EmailAddress("QuintaDoMiao@exemplo.com", "Quinta do Miao"),
                        PlainTextContent = "Pedido de adoção",
                        Subject = "Pedido de adoção",
                    };


                    if (exitForm.State == "Granted" || exitForm.State == "Denied")
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

                        Adopter adopter = await _context.Adopter.FirstOrDefaultAsync(m => m.CitizenCard == exitForm.AdopterCitizenCard);

                        if (adopter == null)
                        {
                            _context.Adopter.Add(new Adopter
                            {
                                Email = exitForm.AdopterEmail,
                                Address = exitForm.AdopterAddress,
                                CitizenCard = exitForm.AdopterCitizenCard,
                                PostalCode = exitForm.AdopterPostalCode
                            });
                        }
                    }
                    if (exitForm.State == "Granted")
                    {
                        animal.Available = false;

                        foreach(ExitForm ef in _context.ExitForm.Where(e => e.AnimalId == animal.Id))
                        {
                            if (ef.Id != exitForm.Id)
                            {
                                ef.State = "Denied";
                            }
                            else
                            {
                                ef.State = "Granted";
                            }
                        }

                        msg.AddContent(MimeType.Text, $"Olá {exitForm.AdopterName}.<br><br>O seu pedido de adoção do {animal.Name} foi aceite." +
                            $"<br>Poderá comparecer no local para se efetuar a entrevista e terminar o processo de avaliação e levar o mesmo." +
                            $"<br><br>Com os melhores cumprimentos,<br>Quinta do Mião.");
                    }
                    else if (exitForm.State == "Denied")
                    {
                        msg.AddContent(MimeType.Text, $"Olá {exitForm.AdopterName}.<br><br>Pedimos desculpa mas o seu pedido foi recusado.<br>Obrigado pelo contacto.<br><br>Com os melhores cumprimentos,<br>Quinta do Mião.");
                    }
                    _context.Update(animal);

                    msg.AddTo(new EmailAddress(exitForm.AdopterEmail, exitForm.AdopterName));
                    var response2 = await client.SendEmailAsync(msg);

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
        [Authorize(Roles = "Administrador, Funcionario")]
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
