using System;
using System.Collections.Generic;
using ProjectSW.Data;
using ProjectSW.Controllers;
using ProjectSW.Models;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTestProject1
{
    public class ApplicationDbContextFixture5
    {
        public ApplicationDbContext DbContext { get; set; }

        public ApplicationDbContextFixture5()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            DbContext = new ApplicationDbContext(options);

            DbContext.Database.EnsureCreated();


            DbContext.User.AddRange(
                        new ProjectSWUser { Name = "João", Address = "joao@ip.pt", DateOfBirth = new DateTime(1999, 08, 08), UserType = "Funcionario", FotoFile = null },
                        new ProjectSWUser { Name = "Maria", Address = "maria@ip.pt", DateOfBirth = new DateTime(1999, 08, 08), UserType = "Funcionario", FotoFile = null },
                        new ProjectSWUser { Name = "Rita", Address = "rita@ip.pt", DateOfBirth = new DateTime(1999, 08, 08), UserType = "Funcionario", FotoFile = null }
                        );
            DbContext.SaveChanges();

            DbContext.Employee.AddRange(
                    new Employee { AccountId = (DbContext.User.First(m => m.Name.Contains("João"))).Id, Type = "Funcionario", AditionalInformation = "NADA" },
                    new Employee { AccountId = (DbContext.User.First(m => m.Name.Contains("Maria"))).Id, Type = "Voluntario", AditionalInformation = "NADA" },
                     new Employee { AccountId = (DbContext.User.First(m => m.Name.Contains("Rita"))).Id, Type = "Administrador", AditionalInformation = "NADA" }
                    );
            DbContext.SaveChanges();


            DbContext.AnimalMonitoringReport.AddRange(
                new AnimalMonitoringReport { Description = "1", EntryDate = new DateTime(2017, 08, 08), EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Funcionario"))).Id },
                new AnimalMonitoringReport { Description = "2", EntryDate = new DateTime(2017, 08, 08), EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Administrador"))).Id },
                new AnimalMonitoringReport { Description = "3", EntryDate = new DateTime(2017, 08, 08), EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Voluntario"))).Id },
                 new AnimalMonitoringReport { Description = "4", EntryDate = new DateTime(2017, 08, 08), EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Funcionario"))).Id },
                  new AnimalMonitoringReport { Description = "5", EntryDate = new DateTime(2017, 08, 08), EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Administrador"))).Id },
                   new AnimalMonitoringReport { Description = "6", EntryDate = new DateTime(2017, 08, 08), EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Funcionario"))).Id },
                    new AnimalMonitoringReport { Description = "7", EntryDate = new DateTime(2017, 08, 08), EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Funcionario"))).Id }
                    );
            DbContext.SaveChanges();

            DbContext.Breed.AddRange(
                   new Breed { Name = "Bulldog" },
                   new Breed { Name = "Beagle" },
                   new Breed { Name = "Husky" }
                   );
            DbContext.SaveChanges();

            DbContext.Animal.AddRange(
               new Animal { BreedId = (DbContext.Breed.First(m => m.Name.Contains("Bulldog"))).Id, Name = "Max", Size = "Pequeno", Gender = "Femea", DateOfBirth = new DateTime(2017, 08, 08) },
               new Animal { BreedId = (DbContext.Breed.First(m => m.Name.Contains("Beagle"))).Id, Name = "Julio", Size = "Grande", Gender = "Femea", DateOfBirth = new DateTime(2017, 12, 08) },
               new Animal { BreedId = (DbContext.Breed.First(m => m.Name.Contains("Husky"))).Id, Name = "Bili", Size = "Médio", Gender = "Femea", DateOfBirth = new DateTime(2017, 06, 08) },
               new Animal { BreedId = (DbContext.Breed.First(m => m.Name.Contains("Bulldog"))).Id, Name = "Ju", Size = "Médio", Gender = "Femea", DateOfBirth = new DateTime(2017, 06, 08) },
               new Animal { BreedId = (DbContext.Breed.First(m => m.Name.Contains("Beagle"))).Id, Name = "Lulu", Size = "Grande", Gender = "Femea", DateOfBirth = new DateTime(2017, 12, 08) });
            DbContext.SaveChanges();


            DbContext.ExitForm.AddRange(
                new ExitForm { AnimalId = (DbContext.Animal.First(m => m.Name.Contains("Max"))).Id, ReportId = (DbContext.AnimalMonitoringReport.First(m => m.Description.Contains("1"))).Id, AdopterName = "Ana", AdopterAddress = "123", AdopterEmail = "ana@ip.pt", Date = new DateTime(2017, 08, 08), Motive = "n", Description = "n", State = "n" },
                new ExitForm { AnimalId = (DbContext.Animal.First(m => m.Name.Contains("Julio"))).Id, ReportId = (DbContext.AnimalMonitoringReport.First(m => m.Description.Contains("2"))).Id, AdopterName = "Cátia", AdopterAddress = "123", AdopterEmail = "catia@ip.pt", Date = new DateTime(2017, 08, 08), Motive = "n", Description = "n", State = "n" },
                new ExitForm { AnimalId = (DbContext.Animal.First(m => m.Name.Contains("Bili"))).Id, ReportId = (DbContext.AnimalMonitoringReport.First(m => m.Description.Contains("3"))).Id, AdopterName = "Nuno", AdopterAddress = "123", AdopterEmail = "nuno@ip.pt", Date = new DateTime(2017, 08, 08), Motive = "n", Description = "n", State = "n" },
                new ExitForm { AnimalId = (DbContext.Animal.First(m => m.Name.Contains("Ju"))).Id, ReportId = (DbContext.AnimalMonitoringReport.First(m => m.Description.Contains("4"))).Id, AdopterName = "Toi", AdopterAddress = "123", AdopterEmail = "toi@ip.pt", Date = new DateTime(2017, 08, 08), Motive = "n", Description = "n", State = "n" },
                new ExitForm { AnimalId = (DbContext.Animal.First(m => m.Name.Contains("Lulu"))).Id, ReportId = (DbContext.AnimalMonitoringReport.First(m => m.Description.Contains("5"))).Id, AdopterName = "Lara", AdopterAddress = "123", AdopterEmail = "lara@ip.pt", Date = new DateTime(2017, 08, 08), Motive = "n", Description = "n", State = "n" });
            DbContext.SaveChanges();
        }


        public class ExitFormControllerTest : IClassFixture<ApplicationDbContextFixture5>
        {
            private ApplicationDbContext _context;
            private readonly ITestOutputHelper output;

            public ExitFormControllerTest(ApplicationDbContextFixture5 contextFixture, ITestOutputHelper output)
            {
                _context = contextFixture.DbContext;
                this.output = output;
            }

            [Fact]
            public async Task Index_CanLoadFromContext()
            {
                var controller = new ExitFormController(_context);

                var result = await controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<ExitForm>>(
                    viewResult.ViewData.Model);
            }

            

            [Fact]
            public async Task CreatePost_ReturnsViewResult_WhenModelStateIsInvalid()
            {
                var controller = new ExitFormController(_context);
                controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");
                ExitForm exitForm  = new ExitForm { AnimalId = (_context.Animal.First(m => m.Name.Contains("Max"))).Id, ReportId = (_context.AnimalMonitoringReport.First(m => m.Description.Contains("1"))).Id, AdopterName = "Ana", AdopterAddress = "123", AdopterEmail = "ana@ip.pt", Date = new DateTime(2017, 08, 08), Motive = "n", Description = "n", State = "n" };
                var result = await controller.Create(exitForm);

                Assert.IsType<ViewResult>(result);
            }


            [Fact]
            public async Task CreatePost_SetsAnimalIdInViewData_WhenModelStateInValid()
            {
                var controller = new ExitFormController(_context);
                controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");
                ExitForm exitForm = new ExitForm { AnimalId = (_context.Animal.First(m => m.Name.Contains("Lulu"))).Id, AdopterName = "Jose", AdopterAddress = "123", AdopterEmail = "jose@ip.pt", Date = new DateTime(2017, 08, 08), Motive = "n", Description = "n", State = "n" };
                var result = await controller.Create(exitForm);

                var viewdata = controller.ViewData["AnimalId"];
                Assert.NotNull(viewdata);
                Assert.IsType<SelectList>(viewdata);
            }

            [Fact]
            public async Task Delete_ReturnsNotFoundResult_WhenIdIsNull()
            {
                var controller = new ExitFormController(_context);

                var result = await controller.Delete(null);

                var viewResult = Assert.IsType<NotFoundResult>(result);
            }

            [Fact]
            public async Task Delete_ReturnsNotFoundResult_WhenExitFormDoesntExist()
            {
                var controller = new ExitFormController(_context);

                var result = await controller.Delete("0");

                Assert.IsType<NotFoundResult>(result);
            }

                 

            [Fact]
            public async Task EditGet_ReturnsNotFoundResult_WhenIdIsNull()
            {
                var controller = new ExitFormController(_context);

                var result = await controller.Edit(null);

                Assert.IsType<NotFoundResult>(result);
            }

            [Fact]
            public async Task EditGet_ReturnsNotFoundResult_WhenExitFormDoesnsExist()
            {
                var controller = new ExitFormController(_context);

                var result = await controller.Edit("0");

                Assert.IsType<NotFoundResult>(result);
            }
            
            [Fact]
            public async Task EditPost_ReturnsNotFoundResult_WhenIdDoesntMatchExitFormId()
            {
                var controller = new ExitFormController(_context);
                var adopter = _context.ExitForm.FirstOrDefault(a => a.Animal.Name == "Max");

                var result = await controller.Edit("1", adopter);

                Assert.IsType<NotFoundResult>(result);
            }

            [Fact]
            public async Task EditPost_ReturnsNotFoundResult_WhenExitFormDoesntExist()
            {
                var controller = new ExitFormController(_context);

                var result = await controller.Edit("9", new ExitForm { Id = "5" });

                Assert.IsType<NotFoundResult>(result);
            }

            [Fact]
            public async Task EditPost_ReturnsViewResult_WhenModelStateIsInValid()
            {
                var controller = new ExitFormController(_context);

                var adopter = await _context.ExitForm.FirstOrDefaultAsync(a => a.AdopterName == "Lara");

                controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");

                var result = await controller.Edit(adopter.Id, adopter);

                Assert.IsType<ViewResult>(result);


                var viewdata2 = controller.ViewData["AnimalId"];
                Assert.NotNull(viewdata2);
                Assert.IsType<SelectList>(viewdata2);
                Assert.True((viewdata2 as SelectList).Count() > 0);
            }

            [Fact]
            public async Task EditPost_ReturnsRedirectToActionResult_WhenExitFormIsUpdated()
            {
                var controller = new ExitFormController(_context);
                var adopter = await _context.ExitForm.FirstOrDefaultAsync(a => a.AdopterName == "Ana");
                adopter.Description = "12345";

                var result = await controller.Edit(adopter.Id, adopter);

                Assert.IsType<RedirectToActionResult>(result);
                ExitForm exitFormUpdated = _context.ExitForm.FirstOrDefault(a => a.AdopterName == "Ana");
                Assert.Equal(adopter.AdopterName, exitFormUpdated.AdopterName);
            }


            [Fact]
            public async Task Details_ReturnsNotFoundResult_WhenIdIsNull()
            {
                var controller = new ExitFormController(_context);

                var result = await controller.Details(null);

                Assert.IsType<NotFoundResult>(result);
            }

            [Fact]
            public async Task Details_RetunrsViewResult_WhenExitFormExists()
            {

                var controller = new ExitFormController(_context);

                var adopter = await _context.ExitForm.FirstOrDefaultAsync(a => a.AdopterName == "Ana");
                var result = await controller.Details(adopter.Id);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<ExitForm>(viewResult.ViewData.Model);
                Assert.Equal(adopter.Id, model.Id);

            }
        }
    }
}
