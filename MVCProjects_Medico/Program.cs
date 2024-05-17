using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCProjects_MedicoBusiness.Services.Abstracts;
using MVCProjects_MedicoBusiness.Services.Concretes;
using MVCProjects_MedicoCore.Models;
using MVCProjects_MedicoCore.RepositoryAbstracts;
using MVCProjects_MedicoData.DAL;
using MVCProjects_MedicoData.RepositoryConcretes;

namespace MVCProjects_Medico
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });


            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();
          

            app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
             );
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}