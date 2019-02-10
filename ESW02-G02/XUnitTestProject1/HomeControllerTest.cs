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
    public class ApplicationDbContextFixture7
    {
        public ApplicationDbContext DbContext { get; set; }

        public ApplicationDbContextFixture7()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            DbContext = new ApplicationDbContext(options);

            DbContext.Database.EnsureCreated();

        }
    }

    public class HomeControllerTest : IClassFixture<ApplicationDbContextFixture7>
    {
        private ApplicationDbContext _context;
        private readonly ITestOutputHelper output;

        public HomeControllerTest(ApplicationDbContextFixture7 contextFixture, ITestOutputHelper output)
        {
            _context = contextFixture.DbContext;
            this.output = output;
        }

        [Fact]
        public async Task Index_CanLoadFromContext()
        {
            var controller = new HomeController(_context);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
         
        }

        [Fact]
        public async Task ExitFormSubmited_CanLoadFromContext()
        {
            var controller = new HomeController(_context);

            var result = controller.ExitFormSubmited();

            var viewResult = Assert.IsType<ViewResult>(result);
           
        }

        [Fact]
        public async Task DetailsAnimal_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var controller = new HomeController(_context);

            var result = await controller.DetailsAnimal(null);

            Assert.IsType<NotFoundResult>(result);
        }

        //[Fact]
        //public async Task DetailsAnimal_RetunrsViewResult_WhenAnimalExists()
        //{
        //    var connection = new SqliteConnection("DataSource=:memory:");
        //    connection.Open();
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseSqlite(connection)
        //        .Options;

        //    using (var context = new ApplicationDbContext(options))
        //    {
        //        context.Database.EnsureCreated();
        //        context.Breed.Add(new Breed { Name = "Bulldog" });
        //        context.Animal.Add(new Animal { BreedId = (context.Breed.FirstOrDefault(m => m.Name.Contains("Bulldog"))).Id, Name = "Max", Size = "Pequeno", Gender = "Macho", DateOfBirth = new DateTime(2017, 08, 08), Available = false, Foto = null, Attachments = null });
        //        context.SaveChanges();
        //    }
        //    using (var context = new ApplicationDbContext(options))
        //    {
        //        var controller = new HomeController(context);

        //        var animal = _context.Animal.FirstOrDefault(a => a.Name == "Max");
        //        var result = await controller.DetailsAnimal(animal.Id);

        //        var viewResult = Assert.IsType<ViewResult>(result);
        //    }
        //}

        [Fact]
        public async Task ListAnimals_CanLoadFromContext()
        {
            var controller = new HomeController(_context);

            var result = await controller.ListAnimals();

            var viewResult = Assert.IsType<ViewResult>(result);

        }

    }
}