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
    public class ApplicationDbContextFixture6
    {
        public ApplicationDbContext DbContext { get; set; }

        public ApplicationDbContextFixture6()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            DbContext = new ApplicationDbContext(options);

            DbContext.Database.EnsureCreated();

            DbContext.User.AddRange(
                        new ProjectSWUser { Name = "João", Address = "joao@ip.pt", DateOfBirth = new DateTime(1999, 08, 08), UserType = "Funcionario", FotoFile = null },
                        new ProjectSWUser { Name = "Maria", Address = "maria@ip.pt", DateOfBirth = new DateTime(1999, 08, 08), UserType = "Funcionario", FotoFile = null },
                        new ProjectSWUser { Name = "Rita", Address = "rita@ip.pt", DateOfBirth = new DateTime(1999, 08, 08), UserType = "Funcionario", FotoFile = null },
                        new ProjectSWUser { Name = "Paulo", Address = "rita@ip.pt", DateOfBirth = new DateTime(1999, 08, 08), UserType = "Funcionario", FotoFile = null },
                        new ProjectSWUser { Name = "Rui", Address = "rita@ip.pt", DateOfBirth = new DateTime(1999, 08, 08), UserType = "Funcionario", FotoFile = null },
                        new ProjectSWUser { Name = "Carmo", Address = "rita@ip.pt", DateOfBirth = new DateTime(1999, 08, 08), UserType = "Funcionario", FotoFile = null }
                        );
            DbContext.SaveChanges();

            DbContext.Employee.AddRange(
                    new Employee { AccountId = (DbContext.User.First(m => m.Name.Contains("João"))).Id, Type = "Funcionario", AditionalInformation = "NADA" },
                    new Employee { AccountId = (DbContext.User.First(m => m.Name.Contains("Maria"))).Id, Type = "Voluntario", AditionalInformation = "NADA" },
                    new Employee { AccountId = (DbContext.User.First(m => m.Name.Contains("Rita"))).Id, Type = "Administrador", AditionalInformation = "NADA" },
                    new Employee { AccountId = (DbContext.User.First(m => m.Name.Contains("Paulo"))).Id, Type = "Funcionario", AditionalInformation = "NADA" },
                    new Employee { AccountId = (DbContext.User.First(m => m.Name.Contains("Rui"))).Id, Type = "Voluntario", AditionalInformation = "NADA" },
                    new Employee { AccountId = (DbContext.User.First(m => m.Name.Contains("Carmo"))).Id, Type = "Administrador", AditionalInformation = "NADA" }
                    );
            DbContext.SaveChanges();

            DbContext.Job.AddRange(
                new Job { EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Funcionario"))).Id, Name = "Tarefa1", Day = new DateTime(1999, 08, 08), Hour = new DateTime(1999, 08, 08), Description = "12" },
                new Job { EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Administrador"))).Id, Name = "Tarefa2", Day = new DateTime(1999, 08, 08), Hour = new DateTime(1999, 08, 08), Description = "12" },
                new Job { EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Voluntario"))).Id, Name = "Tarefa3", Day = new DateTime(1999, 08, 08), Hour = new DateTime(1999, 08, 08), Description = "12" },
                new Job { EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Administrador"))).Id, Name = "Tarefa4", Day = new DateTime(1999, 08, 08), Hour = new DateTime(1999, 08, 08), Description = "12" },
                new Job { EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Funcionario"))).Id, Name = "Tarefa5", Day = new DateTime(1999, 08, 08), Hour = new DateTime(1999, 08, 08), Description = "12" },
                new Job { EmployeeId = (DbContext.Employee.First(m => m.Type.Contains("Voluntario"))).Id, Name = "Tarefa6", Day = new DateTime(1999, 08, 08), Hour = new DateTime(1999, 08, 08), Description = "12" });

            DbContext.SaveChanges();

        }
    }

    public class JobsControllerTest : IClassFixture<ApplicationDbContextFixture6>
    {
        private ApplicationDbContext _context;
        private readonly ITestOutputHelper output;
        private readonly UserManager<ProjectSWUser> _userManager;

        public JobsControllerTest(ApplicationDbContextFixture6 contextFixture, ITestOutputHelper output, UserManager<ProjectSWUser> userManager)
        {
            _context = contextFixture.DbContext;
            this.output = output;
            _userManager = userManager;
        }

        [Fact]
        public async Task Index_CanLoadFromContext()
        {
            var controller = new JobsController(_context, _userManager);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Job>>(
                viewResult.ViewData.Model);
        }

        [Fact]
        public async Task CreateGet_ReturnsViewresult()
        {
            var controller = new JobsController(_context, _userManager);

            var result = controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CreateGet_SetsEmployeeIdInViewData()
        {
            var controller = new JobsController(_context, _userManager);

            var result = controller.Create();

            var viewdata = controller.ViewData["EmployeeId"];
            Assert.NotNull(viewdata);
            Assert.IsType<SelectList>(viewdata);
            Assert.True((viewdata as SelectList).Count() > 0);
        }

        [Fact]
        public async Task CreatePost_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            var controller = new JobsController(_context, _userManager);
            controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");
            Job job = new Job { EmployeeId = (_context.Employee.First(m => m.Type.Contains("Funcionario"))).Id, Name = "Tarefa1", Day = new DateTime(1999, 08, 08), Hour = new DateTime(1999, 08, 08), Description = "12" };
            var result = await controller.Create(job);

            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task CreatePost_SetsAccountIdInViewData_WhenModelStateInValid()
        {
            var controller = new JobsController(_context, _userManager);
            controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");
            Job job = new Job { EmployeeId = (_context.Employee.First(m => m.Type.Contains("Funcionario"))).Id, Name = "Tarefa1", Day = new DateTime(1999, 08, 08), Hour = new DateTime(1999, 08, 08), Description = "12" };
            var result = await controller.Create(job);

            var viewdata = controller.ViewData["EmployeeId"];
            Assert.NotNull(viewdata);
            Assert.IsType<SelectList>(viewdata);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var controller = new JobsController(_context, _userManager);

            var result = await controller.Delete(null);

            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_WhenAnimalDoesntExist()
        {
            var controller = new JobsController(_context, _userManager);

            var result = await controller.Delete("0");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WhenAnimalExist()
        {
            var controller = new JobsController(_context, _userManager);

            var job = await _context.Job.FirstOrDefaultAsync(a => a.Name == "Tarefa2");
            var result = await controller.Delete(job.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Job>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.Equal(job.Id, model.Id);

            Assert.NotNull(model.Employee);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToActionResult()
        {
            var controller = new JobsController(_context, _userManager);
            var job = await _context.Job.FirstOrDefaultAsync(a => a.Name == "Tarefa1");
            var result = await controller.DeleteConfirmed(job.Id);
            output.WriteLine("{0}", job.Id);

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task EditGet_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var controller = new JobsController(_context, _userManager);

            var result = await controller.Edit(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGet_ReturnsNotFoundResult_WhenJobDoesnsExist()
        {
            var controller = new JobsController(_context, _userManager);

            var result = await controller.Edit("0");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGet_ReturnsViewResult_WhenJobExists()
        {
            var controller = new JobsController(_context, _userManager);

            var job = await _context.Job.FirstOrDefaultAsync(a => a.Name == "Tarefa4");

            var result = await controller.Edit(job.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Job>(
                viewResult.ViewData.Model);
            Assert.NotNull(model);

            var viewdata2 = controller.ViewData["EmployeeId"];
            Assert.NotNull(viewdata2);
            Assert.IsType<SelectList>(viewdata2);
            Assert.True((viewdata2 as SelectList).Count() > 0);
        }


        [Fact]
        public async Task EditPost_ReturnsNotFoundResult_WhenIdDoesntMatchEmployeeId()
        {
            var controller = new JobsController(_context, _userManager);
            Job job = _context.Job.FirstOrDefault(a => a.Name == "Tarefa5");

            var result = await controller.Edit("1", job);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ReturnsNotFoundResult_WhenJobDoesntExist()
        {
            var controller = new JobsController(_context, _userManager);

            var result = await controller.Edit("5", new Job { Id = "5" });

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ReturnsViewResult_WhenModelStateIsInValid()
        {
            var controller = new JobsController(_context, _userManager);

            var job = await _context.Job.FirstOrDefaultAsync(a => a.Name == "Tarefa4");

            controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");

            var result = await controller.Edit(job.Id, job);

            Assert.IsType<ViewResult>(result);


            var viewdata2 = controller.ViewData["EmployeeId"];
            Assert.NotNull(viewdata2);
            Assert.IsType<SelectList>(viewdata2);
            Assert.True((viewdata2 as SelectList).Count() > 0);
        }

        [Fact]
        public async Task EditPost_ReturnsRedirectToActionResult_WhenJobIsUpdated()
        {
            var controller = new JobsController(_context, _userManager);
            var job = await _context.Job.FirstOrDefaultAsync(a => a.Name == "Tarefa6");
            job.Description = "No";

            var result = await controller.Edit(job.Id, job);

            Assert.IsType<RedirectToActionResult>(result);
            Job jobUpdated = _context.Job.FirstOrDefault(a => a.Name == "Tarefa6");
            Assert.Equal(job.Name, jobUpdated.Name);
        }


        [Fact]
        public async Task Details_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var controller = new JobsController(_context, _userManager);

            var result = await controller.Details(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_RetunrsViewResult_WhenEmployeeExists()
        {

            var controller = new JobsController(_context, _userManager);

            var job = await _context.Job.FirstOrDefaultAsync(a => a.Name == "Tarefa3");
            var result = await controller.Details(job.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Job>(viewResult.ViewData.Model);
            Assert.Equal(job.Id, model.Id);

        }
    }
}