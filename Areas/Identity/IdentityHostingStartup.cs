using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(ApplicationPortal.Areas.Identity.IdentityHostingStartup))]
namespace ApplicationPortal.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                // services.AddDbContext<ApplicationPortalIdentityDbContext>(options =>
                //     options.UseSqlServer(
                //         context.Configuration.GetConnectionString("ApplicationPortalIdentityDbContextConnection")));

                // services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //     .AddEntityFrameworkStores<ApplicationPortalIdentityDbContext>();
            });
        }
    }
}