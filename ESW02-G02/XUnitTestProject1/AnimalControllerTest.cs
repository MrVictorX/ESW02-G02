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
    public class ApplicationDbContextFixture
    {
        public ApplicationDbContext DbContext { get; set; }

        public ApplicationDbContextFixture()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            DbContext = new ApplicationDbContext(options);

            DbContext.Database.EnsureCreated();

            DbContext.Breed.AddRange(
                    new Breed { Name = "Bulldog" },
                    new Breed { Name = "Beagle" },
                    new Breed { Name = "Husky" }
                    );
            DbContext.SaveChanges();

            DbContext.Animal.AddRange(
                new Animal { BreedId = (DbContext.Breed.First(m => m.Name.Contains("Bulldog"))).Id, Name = "Max", Size = "Pequeno", Gender = "Macho", DateOfBirth = new DateTime(2017, 08, 08) },
                new Animal { BreedId = (DbContext.Breed.First(m => m.Name.Contains("Beagle"))).Id, Name = "Julio", Size = "Grande", Gender = "Macho", DateOfBirth = new DateTime(2017, 12, 08) },
                new Animal { BreedId = (DbContext.Breed.First(m => m.Name.Contains("Husky"))).Id, Name = "Bili", Size = "Médio", Gender = "Macho", DateOfBirth = new DateTime(2017, 06, 08)}
                );
            DbContext.SaveChanges();
        }
    }

    public class AnimalControllerTest: IClassFixture<ApplicationDbContextFixture>
    {
        private ApplicationDbContext _context;
        

        public AnimalControllerTest(ApplicationDbContextFixture contextFixture)
        {
            _context = contextFixture.DbContext;
        }

        [Fact]
        public async Task Index_CanLoadFromContext()
        {
            var controller = new AnimalsController(_context);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Animal>>(
                viewResult.ViewData.Model);
        }

        [Fact]
        public async Task CreateGet_ReturnsViewresult()
        {
            var controller = new AnimalsController(_context);

            var result = controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CreateGet_SetsBreedIdInViewData()
        {
            var controller = new AnimalsController(_context);

            var result = controller.Create();

            var viewdata = controller.ViewData["BreedId"];
            Assert.NotNull(viewdata);
            Assert.IsType<SelectList>(viewdata);
            Assert.True((viewdata as SelectList).Count() > 0);
        }

        [Fact]
        public async Task CreatePost_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            var controller = new AnimalsController(_context);
            controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");
            Animal beagle = new Animal { BreedId = (_context.Breed.First(m => m.Name.Contains("Beagle"))).Id, Name = "Lulu", Size = "Grande", Gender = "Macho", DateOfBirth = new DateTime(2017, 12, 08) };
            var result = await controller.Create(beagle, null);

            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async Task CreatePost_SetsMarcaIdInViewData_WhenModelStateInValid()
        {
            var controller = new AnimalsController(_context);
            controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");
            Animal beagle = new Animal { BreedId = (_context.Breed.First(m => m.Name.Contains("Beagle"))).Id, Name = "Lulu", Size = "Grande", Gender = "Macho", DateOfBirth = new DateTime(2017, 12, 08) };
            var result = await controller.Create(beagle, null);

            var viewdata = controller.ViewData["BreedId"];
            Assert.NotNull(viewdata);
            Assert.IsType<SelectList>(viewdata);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var controller = new AnimalsController(_context);

            var result = await controller.Delete(null);

            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_WhenAnimalDoesntExist()
        {
            var controller = new AnimalsController(_context);

            var result = await controller.Delete("0");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WhenAnimalExist()
        {
            var controller = new AnimalsController(_context);

            var result = await controller.Delete("3");

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Animal>(viewResult.ViewData.Model);
            Assert.NotNull(model);
            Assert.Equal("3", model.Id);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToActionResult()
        {
            var controller = new AnimalsController(_context);

            var result = await controller.DeleteConfirmed("2");

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task EditGet_ReturnsNotFoundResult_WhenIdIsNull()
        {
            var controller = new AnimalsController(_context);

            var result = await controller.Edit(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGet_ReturnsNotFoundResult_WhenAnimalDoesnsExist()
        {
            var controller = new AnimalsController(_context);

            var result = await controller.Edit("0");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGet_ReturnsViewResult_WhenAnimalExists()
        {
            var controller = new AnimalsController(_context);

            var result = await controller.Edit("1");

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Animal>(
                viewResult.ViewData.Model);
            Assert.NotNull(model);
            
            var viewdata2 = controller.ViewData["BreedId"];
            Assert.NotNull(viewdata2);
            Assert.IsType<SelectList>(viewdata2);
            Assert.True((viewdata2 as SelectList).Count() > 0);
        }


        [Fact]
        public async Task EditPost_ReturnsNotFoundResult_WhenIdDoesntMatchAnimalId()
        {
            var controller = new AnimalsController(_context);
            Animal aluguer = _context.Animal.FirstOrDefault(a => a.Id == "1");

            var result = await controller.Edit("2");

            Assert.IsType<NotFoundResult>(result);
        }
        /*
        [Fact]
        public async Task EditPost_ReturnsNotFoundResult_WhenAluguerDoesntExist()
        {
            var controller = new AlugueresController(_context);

            var result = await controller.Edit(5, new Aluguer { AluguerId = 5 });

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditPost_ReturnsViewResult_WhenModelStateIsInValid()
        {
            var controller = new AlugueresController(_context);
            Aluguer aluguer = _context.Aluguer.FirstOrDefault(a => a.AluguerId == 1);
            controller.ModelState.AddModelError("Erro", "Erro adicionado para teste");

            var result = await controller.Edit(1, aluguer);

            Assert.IsType<ViewResult>(result);

            // Estes testes deveriam estar separados em diferentes métodos de teste!
            var viewdata1 = controller.ViewData["CarroId"];
            Assert.NotNull(viewdata1);
            Assert.IsType<SelectList>(viewdata1);
            Assert.True((viewdata1 as SelectList).Count() > 0);

            var viewdata2 = controller.ViewData["UserId"];
            Assert.NotNull(viewdata2);
            Assert.IsType<SelectList>(viewdata2);
            Assert.True((viewdata2 as SelectList).Count() > 0);
        }

        [Fact]
        public async Task EditPost_ReturnsRedirectToActionResult_WhenAluguerIsUpdated()
        {
            var controller = new AlugueresController(_context);
            Aluguer aluguer = _context.Aluguer.FirstOrDefault(a => a.AluguerId == 1);
            aluguer.LocalDeEntrega += "N";

            var result = await controller.Edit(1, aluguer);

            Assert.IsType<RedirectToActionResult>(result);
            Aluguer aluguerUpdated = _context.Aluguer.FirstOrDefault(a => a.AluguerId == 1);
            Assert.Equal(aluguer.LocalDeEntrega, aluguerUpdated.LocalDeEntrega);
        }*/
    }
}
