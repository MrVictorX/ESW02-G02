using ProjectSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace ProjectSW.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(ProjectSWContext context)
        {
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    }
}
