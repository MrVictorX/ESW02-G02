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

            /*[Display(Name = "Raça")]
        public int BreedId { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Tamanho")]
        public string Size { get; set; }

        [Display(Name = "Genero")]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de nascimento")]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de entrada")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Disponivel para adoção")]
        public bool Available { get; set; }
        
        [Display(Name = "Foto")]
        public byte[] Foto { get; set; }

        [Display(Name = "Raça")]
        public Breed Breed { get; set; }

        [Display(Name = "Anexos")]
        public  List<Attachment> Attachments { get; set; }*/

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
        public void CreateGet_SetsMarcaIdInViewData()
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
    }
}
