using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectSW.Areas.Identity.Data;

namespace ProjectSW.Models
{
    public class ProjectSWContext : IdentityDbContext<ProjectSWUser>
    {
        public ProjectSWContext(DbContextOptions<ProjectSWContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<ProjectSW.Models.Job> Job { get; set; }
        // public DbSet<ProjectSW.Models.Employee> Employee { get; set; }
    }
}
