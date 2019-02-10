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
    public class ApplicationDbContextFixture3
    {
        public ApplicationDbContext DbContext { get; set; }

        public ApplicationDbContextFixture3()
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
        }
    }

    public class EmployeeControllerTest : IClassFixture<ApplicationDbContextFixture3>
    {
        private ApplicationDbContext _context;


        public EmployeeControllerTest(ApplicationDbContextFixture3 contextFixture)
        {
            _context = contextFixture.DbContext;
        }

        [Fact]
        public async Task Index_CanLoadFromContext()
        {
            var controller = new EmployeesController(_context);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Employee>>(
                viewResult.ViewData.Model);
        }

        [Fact]
        public async Task CreateGet_ReturnsViewresult()
        {
            var controller = new EmployeesController(_context);

            var result = controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CreateGet_SetsBreedIdInViewData()
        {
            var controller = new EmployeesController(_context);

            var result = controller.Create();

            var viewdata = controller.ViewData["AccountId"];
            Assert.NotNull(viewdata);
            Assert.IsType<SelectList>(viewdata);
            Assert.True((viewdata as SelectList).Count() > 0);
        }

        [Fact]
        public async Task CreatePost_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            var controller = new EmployeesController(_context);
            controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");
            Employee employee = new Employee { AccountId = (_context.User.First(m => m.Name.Contains("João"))).Id, Type = "Funcionario", AditionalInformation = "NADA" };
            var result = await controller.Create(employee);

            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task CreatePost_SetsAccountIdInViewData_WhenModelStateInValid()
        {
            var controller = new EmployeesController(_context);
            controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");
            Employee employee = new Employee { AccountId = (_context.User.First(m => m.Name.Contains("João"))).Id, Type = "Funcionario", AditionalInformation = "NADA" };
            var result = await controller.Create(employee);

            var viewdata = controller.ViewData["AccountId"];
            Assert.NotNull(viewdata);
            Assert.IsType<SelectList>(viewdata);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var controller = new EmployeesController(_context);

            var result = await controller.Delete(null);

            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_WhenAnimalDoesntExist()
        {
            var controller = new EmployeesController(_context);

            var result = await controller.Delete("0");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WhenAnimalExist()
        {
            var controller = new EmployeesController(_context);

            var employee = await _context.Employee.FirstOrDefaultAsync(a => a.Account.Name == "Paulo");
            var result = await controller.Delete(employee.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Employee>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.Equal(employee.Id, model.Id);

            Assert.NotNull(model.Account);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToActionResult()
        {
            var controller = new EmployeesController(_context);
            var emeployee = await _context.Employee.FirstOrDefaultAsync((a => a.Account.Name == "Paulo"));

            var result = await controller.DeleteConfirmed(emeployee.Id);

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task EditGet_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var controller = new EmployeesController(_context);

            var result = await controller.Edit(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGet_ReturnsNotFoundResult_WhenEmployeeDoesnsExist()
        {
            var controller = new EmployeesController(_context);

            var result = await controller.Edit("0");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGet_ReturnsViewResult_WhenAnimalExists()
        {
            var controller = new EmployeesController(_context);

            var employee = await _context.Employee.FirstOrDefaultAsync(a => a.Account.Name == "Carmo");

            var result = await controller.Edit(employee.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Employee>(
                viewResult.ViewData.Model);
            Assert.NotNull(model);

            var viewdata2 = controller.ViewData["AccountId"];
            Assert.NotNull(viewdata2);
            Assert.IsType<SelectList>(viewdata2);
            Assert.True((viewdata2 as SelectList).Count() > 0);
        }


        [Fact]
        public async Task EditPost_ReturnsNotFoundResult_WhenIdDoesntMatchEmployeeId()
        {
            var controller = new EmployeesController(_context);
            Employee employee = _context.Employee.FirstOrDefault(a => a.Account.Name == "Rui");

            var result = await controller.Edit("1", employee);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ReturnsNotFoundResult_WhenEmployeeDoesntExist()
        {
            var controller = new EmployeesController(_context);

            var result = await controller.Edit("5", new Employee { Id = "5" });

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ReturnsViewResult_WhenModelStateIsInValid()
        {
            var controller = new EmployeesController(_context);

            var employee = await _context.Employee.FirstOrDefaultAsync(a => a.Account.Name == "Rita");

            controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");

            var result = await controller.Edit(employee.Id, employee);

            Assert.IsType<ViewResult>(result);


            var viewdata2 = controller.ViewData["AccountId"];
            Assert.NotNull(viewdata2);
            Assert.IsType<SelectList>(viewdata2);
            Assert.True((viewdata2 as SelectList).Count() > 0);
        }

        [Fact]
        public async Task EditPost_ReturnsRedirectToActionResult_WhenEmployeeIsUpdated()
        {
            var controller = new EmployeesController(_context);
            var employee = await _context.Employee.FirstOrDefaultAsync(a => a.Account.Name == "Maria");
            employee.AditionalInformation = "N";

            var result = await controller.Edit(employee.Id, employee);

            Assert.IsType<RedirectToActionResult>(result);
            Employee employeeUpdated = _context.Employee.FirstOrDefault(a => a.Account.Name == "Maria");
            Assert.Equal(employee.Type, employeeUpdated.Type);
        }


        [Fact]
        public async Task Details_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var controller = new EmployeesController(_context);

            var result = await controller.Details(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_RetunrsViewResult_WhenEmployeeExists()
        {

            var controller = new EmployeesController(_context);

            var employee = await _context.Employee.FirstOrDefaultAsync(a => a.Account.Name == "Carmo");
            var result = await controller.Details(employee.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Employee>(viewResult.ViewData.Model);
            Assert.Equal(employee.Id, model.Id);

        }
    }
}
