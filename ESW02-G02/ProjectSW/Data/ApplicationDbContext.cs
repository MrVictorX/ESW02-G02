using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectSW.Models;

namespace ProjectSW.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProjectSW.Data.ProjectSWUser> User { get; set; }

        public DbSet<ProjectSW.Models.Job> Job { get; set; }

        public DbSet<ProjectSW.Models.Employee> Employee { get; set; }

        public DbSet<ProjectSW.Models.Animal> Animal { get; set; }

        public DbSet<ProjectSW.Models.ExitForm> ExitForm { get; set; }

        public DbSet<ProjectSW.Models.AnimalMonitoringReport> AnimalMonitoringReport { get; set; }

        public DbSet<ProjectSW.Models.Attachment> Attachment { get; set; }
    }
}
