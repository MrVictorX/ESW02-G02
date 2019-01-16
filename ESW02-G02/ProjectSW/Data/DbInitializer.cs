using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectSW.Models;

namespace ProjectSW.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Breed.Any())
            {
                context.Breed.Add(new Breed { Name = "Shih-Tzu" });
                context.Breed.Add(new Breed { Name = "Boxer" });
                context.Breed.Add(new Breed { Name = "Dalmata" });
                context.Breed.Add(new Breed { Name = "Buldogue" });
                context.Breed.Add(new Breed { Name = "Poodle" });
                context.Breed.Add(new Breed { Name = "Beagle" });
                context.Breed.Add(new Breed { Name = "Pastor-Alemão" });
                context.Breed.Add(new Breed { Name = "Golden Retriever" });
                context.Breed.Add(new Breed { Name = "Chihuahua" });
                context.Breed.Add(new Breed { Name = "Pug" });
                context.Breed.Add(new Breed { Name = "Yorkshire Terrier" });
                context.Breed.Add(new Breed { Name = "Buldogue Françês" });
                context.Breed.Add(new Breed { Name = "Husky Siberiano" });

                context.SaveChanges();
            }
        }
    }
}
