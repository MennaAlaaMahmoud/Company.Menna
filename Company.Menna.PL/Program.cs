using Company.Menna.BLL.Interfaces;
using Company.Menna.BLL.Repositories;
using Company.Menna.DAL.Data.Context;
using Company.Menna.PL.Services;
using Microsoft.EntityFrameworkCore;

namespace Company.Menna.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IDepartmentRepositories, DepartmentRepositories>(); // Allow DI For DepartmentRepositories
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepositories>(); // Allow DI For DepartmentRepositories
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); // Allow DI For CompanyDbContext

            // Life Time
            //builder.Services.AddScoped();    // Create Object Life Per Request - UnReachable Object 
            //builder.Services.AddTransient(); // Create Object Life Per Operation 
            //builder.Services.AddSingleton(); // Create Object Life Per Application

            builder.Services.AddScoped<IScopedService ,ScopedService>(); // Per Request
            builder.Services.AddTransient<ITransentService, TransentService>(); // Per Operation
            builder.Services.AddSingleton<ISingletonService, SingletonService>(); // Per Application



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
