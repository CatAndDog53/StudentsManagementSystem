using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services;
using Presentation.Mappings;
using Microsoft.AspNetCore.Hosting;
using Services.Interfaces;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<Infrastructure.CoursesDbContext>(options =>
                options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICoursesService, CoursesService>();
            builder.Services.AddScoped<IGroupsService, GroupsService>();
            builder.Services.AddScoped<IStudentsService, StudentsService>();
            builder.Services.AddAutoMapper(typeof(Program).Assembly);


            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Courses}/{action=Index}/{id?}");

            app.Run();
        }
    }
}