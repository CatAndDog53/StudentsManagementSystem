using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services;
using Presentation.Mappings;
using Microsoft.AspNetCore.Hosting;

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
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            //var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            //IMapper mapper = mapperConfig.CreateMapper();
            //builder.Services.AddSingleton(mapper);

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