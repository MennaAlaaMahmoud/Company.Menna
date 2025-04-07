using Company.Menna.BLL;
using Company.Menna.BLL.Interfaces;
using Company.Menna.BLL.Repositories;
using Company.Menna.DAL.Data.Context;
using Company.Menna.DAL.Models;
using Company.Menna.PL.Helpers;
using Company.Menna.PL.Mapping;
using Company.Menna.PL.Services;
using Company.Menna.PL.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Company.Menna.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

          

            builder.Services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = FacebookDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
            }).AddFacebook(Options =>
            {
                Options.ClientId = builder.Configuration["Authentiction:Facebook:ClientId"];
                Options.ClientSecret = builder.Configuration["Authentiction:Facebook:ClientSecret"];
            });




            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IDepartmentRepositories, DepartmentRepositories>(); // Allow DI For DepartmentRepositories
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepositories>(); // Allow DI For DepartmentRepositories

            builder.Services.AddScoped<IUnitOfWork, Unitofwork>();

            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); // Allow DI For CompanyDbContext


            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(M => M.AddProfile(new DepartmentProfile()));

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                             .AddEntityFrameworkStores<CompanyDbContext>()
                             .AddDefaultTokenProviders();


            // Life Time
            //builder.Services.AddScoped();    // Create Object Life Per Request - UnReachable Object 
            //builder.Services.AddTransient(); // Create Object Life Per Operation 
            //builder.Services.AddSingleton(); // Create Object Life Per Application

            builder.Services.AddScoped<IScopedService, ScopedService>(); // Per Request
            builder.Services.AddTransient<ITransentService, TransentService>(); // Per Operation
            builder.Services.AddSingleton<ISingletonService, SingletonService>(); // Per Application



            builder.Services.Configure<Mailsettings>(builder.Configuration.GetSection(nameof(Mailsettings)));
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection(nameof(TwilioSettings)));
            builder.Services.AddTransient<ITwilioService,TwilioService>();



            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SingIn";
               // config.AccessDeniedPath= "/Account/AccessDenied";

            });

            builder.Services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddGoogle(Options =>
            {
                Options.ClientId = builder.Configuration["Authentiction:Google:ClientId"];
                Options.ClientSecret = builder.Configuration["Authentiction:Google:ClientSecret"];
            });




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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
