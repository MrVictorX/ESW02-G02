using Microsoft.AspNetCore.Http;
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
            new Breed{Name="Boxer"},
            new Breed{Name="Rottweiler"}
            };
            foreach (Breed b in breeds)
            {
                context.Breed.Add(b);
            }
            context.SaveChanges();

            var Users = new ProjectSWUser[]
            {
            new ProjectSWUser{Name="Victor Xavier", Address="Morada 1", DateOfBirth=DateTime.Parse("1995-09-01"), UserType="Administrador", Email="teste1@hotmail.com", EmailConfirmed=true},
            new ProjectSWUser{Name="Adriana Ramos", Address="Morada 2", DateOfBirth=DateTime.Parse("1993-11-11"), UserType="Voluntario", Email="teste2@hotmail.com", EmailConfirmed=true},
            new ProjectSWUser{Name="David Reis", Address="Morada 3", DateOfBirth=DateTime.Parse("1990-01-01"), UserType="Voluntario", Email="teste3@hotmail.com", EmailConfirmed=true},
            new ProjectSWUser{Name="Inês Antunes", Address="Morada 4", DateOfBirth=DateTime.Parse("2000-09-07"), UserType="Voluntario", Email="teste4@hotmail.com", EmailConfirmed=true},
            new ProjectSWUser{Name="João Valada", Address="Morada 5", DateOfBirth=DateTime.Parse("1980-03-01"), UserType="Funcionario", Email="teste5@hotmail.com", EmailConfirmed=true},
            new ProjectSWUser{Name="Miguel Figueira", Address="Morada 6", DateOfBirth=DateTime.Parse("2005-09-01"), UserType="Funcionario", Email="teste6@hotmail.com", EmailConfirmed=true},
            new ProjectSWUser{Name="Mariana Luciano", Address="Morada 7", DateOfBirth=DateTime.Parse("2005-05-01"), UserType="Funcionario", Email="teste7@hotmail.com", EmailConfirmed=true},
            new ProjectSWUser{Name="Marcos Letras", Address="Morada 8", DateOfBirth=DateTime.Parse("2005-09-10"), UserType="Funcionario", Email="teste8@hotmail.com", EmailConfirmed=true}
            };

            foreach (ProjectSWUser u in Users)
            {
                context.User.Add(u);
            }
            context.SaveChanges();

            if (context.Employee.Any())
            {
                return;
            }

            var Employees = new Employee[]
            {
                new Employee{AccountId="teste1@hotmail.com", Account=context.User.Where(a => a.Email.Equals("teste1@hotmail.com")).First(), Type="Administrador"},
                new Employee{AccountId="teste2@hotmail.com", Account=context.User.Where(a => a.Email.Equals("teste2@hotmail.com")).First(), Type="Voluntario"},
                new Employee{AccountId="teste3@hotmail.com", Account=context.User.Where(a => a.Email.Equals("teste3@hotmail.com")).First(), Type="Voluntario"},
                new Employee{AccountId="teste4@hotmail.com", Account=context.User.Where(a => a.Email.Equals("teste4@hotmail.com")).First(), Type="Voluntario"},
                new Employee{AccountId="teste5@hotmail.com", Account=context.User.Where(a => a.Email.Equals("teste5@hotmail.com")).First(), Type="Funcionario"},
                new Employee{AccountId="teste6@hotmail.com", Account=context.User.Where(a => a.Email.Equals("teste6@hotmail.com")).First(), Type="Funcionario"},
                new Employee{AccountId="teste7@hotmail.com", Account=context.User.Where(a => a.Email.Equals("teste7@hotmail.com")).First(), Type="Funcionario"},
                new Employee{AccountId="teste8@hotmail.com", Account=context.User.Where(a => a.Email.Equals("teste8@hotmail.com")).First(), Type="Funcionario"}
            };

            foreach (Employee e in Employees)
            {
                context.Employee.Add(e);
            }
            context.SaveChanges();

            // Look for any Animals.
            if (context.Animal.Any())
            {
                return;   // DB has been seeded
            }

            var Animals = new Animal[]
            {
            new Animal{BreedId=1, Name="Boris", Size="Pequeno", Gender="Masculino", DateOfBirth=DateTime.Parse("2015-09-01"), EntryDate=DateTime.Parse("2016-02-20"), Available=true, Foto = System.IO.File.ReadAllBytes("./wwwroot/images/bulldogF.jpg")},
            new Animal{BreedId=2, Name="Balu", Size="Pequeno", Gender="Feminino", DateOfBirth=DateTime.Parse("2010-11-11"), EntryDate=DateTime.Parse("2014-07-26"), Available=true, Foto = System.IO.File.ReadAllBytes("./wwwroot/images/poodle.jpg")},
            new Animal{BreedId=3, Name="Mogli", Size="Medio", Gender="Masculino", DateOfBirth=DateTime.Parse("2014-01-01"), EntryDate=DateTime.Parse("2018-09-01"), Available=true, Foto = System.IO.File.ReadAllBytes("./wwwroot/images/pug.jpg")},
            new Animal{BreedId=4, Name="Bobi", Size="Medio", Gender="Feminino", DateOfBirth=DateTime.Parse("2015-09-07"), EntryDate=DateTime.Parse("2019-04-11"), Available=true, Foto = System.IO.File.ReadAllBytes("./wwwroot/images/golden-retriever.jpg")},
            new Animal{BreedId=5, Name="Nala", Size="Grande", Gender="Masculino", DateOfBirth=DateTime.Parse("2010-03-01"), EntryDate=DateTime.Parse("2018-08-15"), Available=true, Foto = System.IO.File.ReadAllBytes("./wwwroot/images/pastor-alemao.jpg")},
            new Animal{BreedId=6, Name="Gato", Size="Grande", Gender="Feminino", DateOfBirth=DateTime.Parse("2007-09-01"), EntryDate=DateTime.Parse("2013-01-21"), Available=false, Foto = System.IO.File.ReadAllBytes("./wwwroot/images/Beagle.jpg")},
            new Animal{BreedId=7, Name="Joli", Size="Medio", Gender="Masculino", DateOfBirth=DateTime.Parse("2012-05-01"), EntryDate=DateTime.Parse("2018-02-10"), Available=false, Foto = System.IO.File.ReadAllBytes("./wwwroot/images/boxer.jpg")},
            new Animal{BreedId=8, Name="Mantorras", Size="Pequeno", Gender="Feminino", DateOfBirth=DateTime.Parse("2009-09-10"), EntryDate=DateTime.Parse("2017-10-10"), Available=false, Foto = System.IO.File.ReadAllBytes("./wwwroot/images/rottweiler.jpg")}
            };

            foreach (Animal a in Animals)
            {
                context.Animal.Add(a);
            }
            context.SaveChanges();

            if (context.ExitForm.Any())
            {
                return;   // DB has been seeded
            }

            var ExitForms = new ExitForm[]
            {
            new ExitForm{AdopterName="Hugo Modesto", AdopterAddress="rua Dulce Maria Cardoso", AdopterEmail="hugomodesto.2@gmail.com", Motive=""},
            new ExitForm{AdopterName="Patricia Guerreiro", AdopterAddress="rua Marques de Pombal", AdopterEmail="patriciaGG@gmail.com", Description="Casa grande com quintal" , Motive="Tenho um cão em casa e seria bom que houvesse mais um cão para fazerem companhia enquanto ninguem está."},
            new ExitForm{AdopterName="Patricia Paquete", AdopterAddress="rua Luis Camões", AdopterEmail="pPaquetep@hotmail.com", Motive="Sempre quis ter um animal."},
            new ExitForm{AdopterName="Paulo David", AdopterAddress="rua Vasco da Gama", AdopterEmail="pdX123@outlook.com", Motive=""},
            new ExitForm{AdopterName="Ines Xavier", AdopterAddress="rua Fernando Pessoa", AdopterEmail="ImnXavier@hotmail.com", Description="Passei a minha vida com cães e gatos",  Motive="Mudei-me para a minha casa e senti falta de ter um cão para cuidar."},
            new ExitForm{AdopterName="Catarina Martins", AdopterAddress="rua José Saramago", AdopterEmail="catarina23.M@sapo.pt", Motive="Sinto-me sozinha"},
            new ExitForm{AdopterName="João Martins", AdopterAddress="rua Antero de Quental", AdopterEmail="johnyBgood@gmail.com", Description="Viajo muito em trabalho", Motive="Viajo muito de carrinha e quero ter companhia para viagens"},
            new ExitForm{AdopterName="Gonçalo Esteves", AdopterAddress="rua Sophia de Mello Breyner Andresen", AdopterEmail="goncEst@hotmail.com", Motive="Não tenho motivo especial"},
            new ExitForm{AdopterName="Marco Salgado", AdopterAddress="rua Miguel Torga", AdopterEmail="oGrandeSal@gmail.com", Description="Trabalho em casa", Motive="seria bom ter companhia em casa."},
            };

            foreach (ExitForm e in ExitForms)
            {
                context.ExitForm.Add(e);
            }
            context.SaveChanges();

            //if (context.Job.Any())
            //{
            //    return;   // DB has been seeded
            //}

            //var Jobs = new Job[]
            //{
            //    new Job{Name="Lavar Balu", Day=DateTime.Parse("2019-02-15"), Hour=DateTime.}
            //};

            //foreach (Job j in Jobs)
            //{
            //    context.Job.Add(j);
            //}
            //context.SaveChanges();
        }
    }
}
