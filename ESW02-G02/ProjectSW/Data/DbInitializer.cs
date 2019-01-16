using ProjectSW.Data;
using ProjectSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // context.Database.EnsureCreated();

            // Look for any breeds.
            if (context.Breed.Any())
            {
                return;   // DB has been seeded
            }

            var breeds = new Breed[]
            {
            new Breed{Name="Bulldog"},
            new Breed{Name="Poodle"},
            new Breed{Name="Pug"},
            new Breed{Name="Golden Retriever"},
            new Breed{Name="German Shepard"},
            new Breed{Name="Beagle"},
            new Breed{Name="Greyhound"},
            new Breed{Name="Rottweiler"}
            };
            foreach (Breed b in breeds)
            {
                context.Breed.Add(b);
            }
            context.SaveChanges();

            // Look for any Users.
            //if (context.User.Any())
            //{
            //    return;   // DB has been seeded
            //}

            var Users = new ProjectSWUser[]
            {
            new ProjectSWUser{Name="Teste1", Address="Morada 1", DateOfBirth=DateTime.Parse("1995-09-01"), UserType="Administrador", Email="teste1@hotmail.com", EmailConfirmed=true},
            new ProjectSWUser{Name="Teste2", Address="Morada 2", DateOfBirth=DateTime.Parse("1993-11-11"), UserType="Voluntario", Email="teste2@hotmail.com", EmailConfirmed=true},
            new ProjectSWUser{Name="Teste3", Address="Morada 3", DateOfBirth=DateTime.Parse("1990-01-01"), UserType="Voluntario", Email="teste3@hotmail.com", EmailConfirmed=true},
            new ProjectSWUser{Name="Teste4", Address="Morada 4", DateOfBirth=DateTime.Parse("2000-09-07"), UserType="Voluntario", Email="teste4@hotmail.com", EmailConfirmed=true},
            new ProjectSWUser{Name="Teste5", Address="Morada 5", DateOfBirth=DateTime.Parse("1980-03-01"), UserType="Funcionario", Email="teste5@hotmail.com", EmailConfirmed=true},
            new ProjectSWUser{Name="Teste6", Address="Morada 6", DateOfBirth=DateTime.Parse("2005-09-01"), UserType="Funcionario", Email="teste6@hotmail.com", EmailConfirmed=true},
            new ProjectSWUser{Name="Teste7", Address="Morada 7", DateOfBirth=DateTime.Parse("2005-05-01"), UserType="Funcionario", Email="teste7@hotmail.com", EmailConfirmed=true},
            new ProjectSWUser{Name="Teste8", Address="Morada 8", DateOfBirth=DateTime.Parse("2005-09-10"), UserType="Funcionario", Email="teste8@hotmail.com", EmailConfirmed=true}
            };

            foreach (ProjectSWUser u in Users)
            {
                context.User.Add(u);
            }
            context.SaveChanges();


            // Look for any Animals.
            if (context.Animal.Any())
            {
                return;   // DB has been seeded
            }

            var Animals = new Animal[]
            {
            new Animal{BreedId=1, Name="Animal 1", Size="Pequeno", Gender="Masculino", DateOfBirth=DateTime.Parse("1995-09-01"), EntryDate=DateTime.Parse("1990-09-01"), Available=true},
            new Animal{BreedId=1, Name="Animal 2", Size="Pequeno", Gender="Feminino", DateOfBirth=DateTime.Parse("1993-11-11"), EntryDate=DateTime.Parse("1995-09-01"), Available=true},
            new Animal{BreedId=2, Name="Animal 3", Size="Medio", Gender="Masculino", DateOfBirth=DateTime.Parse("1990-01-01"), EntryDate=DateTime.Parse("1997-09-01"), Available=true},
            new Animal{BreedId=2, Name="Animal 4", Size="Medio", Gender="Feminino", DateOfBirth=DateTime.Parse("2000-09-07"), EntryDate=DateTime.Parse("1997-09-01"), Available=true},
            new Animal{BreedId=3, Name="Animal 5", Size="Grande", Gender="Masculino", DateOfBirth=DateTime.Parse("1980-03-01"), EntryDate=DateTime.Parse("1992-09-01"), Available=true},
            new Animal{BreedId=3, Name="Animal 6", Size="Grande", Gender="Feminino", DateOfBirth=DateTime.Parse("2005-09-01"), EntryDate=DateTime.Parse("1992-09-01"), Available=false},
            new Animal{BreedId=4, Name="Animal 7", Size="Medio", Gender="Masculino", DateOfBirth=DateTime.Parse("2005-05-01"), EntryDate=DateTime.Parse("1988-09-01"), Available=false},
            new Animal{BreedId=4, Name="Animal 8", Size="Pequeno", Gender="Feminino", DateOfBirth=DateTime.Parse("2005-09-10"), EntryDate=DateTime.Parse("1995-09-01"), Available=false}
            };

            foreach (Animal a in Animals)
            {
                context.Animal.Add(a);
            }
            context.SaveChanges();
        }
    }
}
