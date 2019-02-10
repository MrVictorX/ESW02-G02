using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSW.Data;
using ProjectSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Views.Home.Components.MostAdopted
{
    [ViewComponent(Name = "MostAdopted")]
    public class MostAdoptedViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;


        public MostAdoptedViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>Ação que resulta numa viewComponent com um grafico com a idade dos animais mais adotados</summary>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Animal> animals = new List<Animal>();
            var applicationDbContext = _context.Animal;

            foreach (Animal a in await applicationDbContext.ToListAsync())
            {
                if (!a.Available)
                {
                    animals.Add(a);
                }
            }

            return View(animals);
        }
    }
}
