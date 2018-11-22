using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectSW.Areas.Identity.Data;
using ProjectSW.Models;

[assembly: HostingStartup(typeof(ProjectSW.Areas.Identity.IdentityHostingStartup))]
namespace ProjectSW.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ProjectSWContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ProjectSWContextConnection")));

                services.AddDefaultIdentity<ProjectSWUser>()
                    .AddEntityFrameworkStores<ProjectSWContext>();
            });
        }
    }
}