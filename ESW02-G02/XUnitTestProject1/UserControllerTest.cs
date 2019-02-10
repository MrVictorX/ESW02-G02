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
    public class ApplicationDbContextFixture4
    {
        public ApplicationDbContext DbContext { get; set; }

        public ApplicationDbContextFixture4()
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
        }
    }

    public class UserControllerTest : IClassFixture<ApplicationDbContextFixture4>
    {
        private ApplicationDbContext _context;


        public UserControllerTest(ApplicationDbContextFixture4 contextFixture)
        {
            _context = contextFixture.DbContext;
        }

        [Fact]
        public async Task UserList_CanLoadFromContext()
        {
            var controller = new UserController(_context);

            var result = controller.UserList();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProjectSWUser>>(
                viewResult.ViewData.Model);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var controller = new UserController(_context);

            var result = controller.DeleteUser(null);

            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_WhenUserDoesntExist()
        {
            var controller = new UserController(_context);

            var result = controller.DeleteUser("0");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WhenUserExist()
        {
            var controller = new UserController(_context);

            var user = await _context.User.FirstOrDefaultAsync(a => a.Name == "Paulo");
            var result =  controller.DeleteUser(user.Id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProjectSWUser>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.Equal(user.Id, model.Id);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToActionResult()
        {
            var controller = new UserController(_context);
            var user = await _context.User.FirstOrDefaultAsync((a => a.Name == "Maria"));

            var result = await controller.DeleteConfirmed(user.Id);

            Assert.IsType<RedirectToActionResult>(result);
        }

    }

}

        