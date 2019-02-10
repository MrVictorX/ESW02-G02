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

namespace UnitTestProject1
{
    public class ApplicationDbContextFixture2
    {
        public ApplicationDbContext DbContext { get; set; }

        public ApplicationDbContextFixture2()
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
                new AnimalMonitoringReport { Description = "2", EntryDate = new DateTime(2017, 08, 08),  EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Administrador"))).Id},
                new AnimalMonitoringReport { Description = "3", EntryDate = new DateTime(2017, 08, 08), EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Voluntario"))).Id},
                 new AnimalMonitoringReport { Description = "4", EntryDate = new DateTime(2017, 08, 08), EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Funcionario"))).Id },
                  new AnimalMonitoringReport { Description = "5", EntryDate = new DateTime(2017, 08, 08), EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Administrador"))).Id},
                   new AnimalMonitoringReport { Description = "6", EntryDate = new DateTime(2017, 08, 08), EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Funcionario"))).Id },
                    new AnimalMonitoringReport { Description = "7", EntryDate = new DateTime(2017, 08, 08), EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Funcionario"))).Id}
                    );
            DbContext.SaveChanges();
        }
    }

    public class AnimalMonitoringReportControllerTest : IClassFixture<ApplicationDbContextFixture2>
    {
        private ApplicationDbContext _context;


        public AnimalMonitoringReportControllerTest(ApplicationDbContextFixture2 contextFixture)
        {
            _context = contextFixture.DbContext;
        }

        [Fact]
        public async Task Index_CanLoadFromContext()
        {
            var controller = new AnimalMonitoringReportController(_context);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<AnimalMonitoringReport>>(
                viewResult.ViewData.Model);
        }

        [Fact]
        public async Task CreateGet_ReturnsViewresult()
        {
            var controller = new AnimalMonitoringReportController(_context);

            var result = controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CreateGet_SetEmployeeIdInViewData()
        {
            var controller = new AnimalMonitoringReportController(_context);

            var result = controller.Create();

            var viewdata = controller.ViewData["EmployeeId"];
            Assert.NotNull(viewdata);
            Assert.IsType<SelectList>(viewdata);
            Assert.True((viewdata as SelectList).Count() > 0);

        }

        [Fact]
        public async Task CreatePost_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            var controller = new AnimalMonitoringReportController(_context);
            controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");
            AnimalMonitoringReport animal = new AnimalMonitoringReport { EmployeeId = (_context.Employee.First(m => m.Type.Contains("Funcionario"))).Id, Description = "123", EntryDate = new DateTime(2017, 08, 08) };
            var result = await controller.Create(animal);

            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task CreatePost_SetEmployeeIdInViewData_WhenModelStateInValid()
        {
            var controller = new AnimalMonitoringReportController(_context);
            controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");
            AnimalMonitoringReport animal = new AnimalMonitoringReport { Description = "123", EntryDate = new DateTime(2017, 08, 08), EmployeeId = (_context.Employee.First(m => m.Type.Contains("Funcionario"))).Id };
            var result = await controller.Create(animal);

            var viewdata = controller.ViewData["EmployeeId"];
            Assert.NotNull(viewdata);
            Assert.IsType<SelectList>(viewdata);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var controller = new AnimalMonitoringReportController(_context);

            var result = await controller.Delete(null);

            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_WhenAnimalMonitoringDoesntExist()
        {
            var controller = new AnimalMonitoringReportController(_context);

            var result = await controller.Delete("0");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WhenAnimalMonitoringExist()
        {
            var controller = new AnimalMonitoringReportController(_context);

            var animal = await _context.AnimalMonitoringReport.FirstOrDefaultAsync(a => a.Description == "1");
            var result = await controller.Delete(animal.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AnimalMonitoringReport>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.Equal(animal.Id, model.Id);

            Assert.NotNull(model.Employee);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToActionResult()
        {
            var controller = new AnimalMonitoringReportController(_context);
            var animal = await _context.AnimalMonitoringReport.FirstOrDefaultAsync(a => a.Employee.Account.Name == "João");
           
            var result = await controller.DeleteConfirmed(animal.Id);

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task EditGet_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var controller = new AnimalMonitoringReportController(_context);

            var result = await controller.Edit(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGet_ReturnsNotFoundResult_WhenAnimalMonitoringDoesnsExist()
        {
            var controller = new AnimalMonitoringReportController(_context);

            var result = await controller.Edit("0");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGet_ReturnsViewResult_WhenAnimalMonitoringExists()
        {
            var controller = new AnimalMonitoringReportController(_context);

            var animal = await _context.AnimalMonitoringReport.FirstOrDefaultAsync(a => a.Description == "3");

            var result = await controller.Edit(animal.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AnimalMonitoringReport>(
                viewResult.ViewData.Model);
            Assert.NotNull(model);

            var viewdata = controller.ViewData["EmployeeId"];
            Assert.NotNull(viewdata);
            Assert.IsType<SelectList>(viewdata);
            Assert.True((viewdata as SelectList).Count() > 0);
        }


        [Fact]
        public async Task EditPost_ReturnsNotFoundResult_WhenIdDoesntMatchAnimalMonitoringId()
        {
            var controller = new AnimalMonitoringReportController(_context);
            var animal = _context.AnimalMonitoringReport.FirstOrDefault(a => a.Description == "4");

            var result = await controller.Edit("5", animal);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ReturnsNotFoundResult_WhenAnimalMonitoringDoesntExist()
        {
            var controller = new AnimalMonitoringReportController(_context);

            var result = await controller.Edit("5", new AnimalMonitoringReport { Id = "5" });

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ReturnsViewResult_WhenModelStateIsInValid()
        {
            var controller = new AnimalMonitoringReportController(_context);

            var animal = await _context.AnimalMonitoringReport.FirstOrDefaultAsync(a => a.Description == "5");

            controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");

            var result = await controller.Edit(animal.Id, animal);

            Assert.IsType<ViewResult>(result);

            var viewdata = controller.ViewData["EmployeeId"];
            Assert.NotNull(viewdata);
            Assert.IsType<SelectList>(viewdata);
            Assert.True((viewdata as SelectList).Count() > 0);

        }

        [Fact]
        public async Task EditPost_ReturnsRedirectToActionResult_WhenAnimalIsUpdated()
        {
            var controller = new AnimalMonitoringReportController(_context);
            var animal = await _context.AnimalMonitoringReport.FirstOrDefaultAsync(a => a.Description == "7");
            animal.EntryDate = new DateTime(2017, 08, 05);
            var result = await controller.Edit(animal.Id, animal);

            Assert.IsType<RedirectToActionResult>(result);
            AnimalMonitoringReport animalUpdated = _context.AnimalMonitoringReport.FirstOrDefault(a => a.Description == "7");
            Assert.Equal(animal.EntryDate, animalUpdated.EntryDate);
        }


        [Fact]
        public async Task Details_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var controller = new AnimalMonitoringReportController(_context);

            var result = await controller.Details(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_RetunrsViewResult_WhenAnimalExists()
        {
            var controller = new AnimalMonitoringReportController(_context);

            var animal = await _context.AnimalMonitoringReport.FirstOrDefaultAsync(a => a.Description == "3");
            var result = await controller.Details(animal.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AnimalMonitoringReport>(viewResult.ViewData.Model);
            Assert.Equal(animal.Id, model.Id);
        }
    }
}