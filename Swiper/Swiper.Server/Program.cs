
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Swiper.Server.DBContexts;
using Swiper.Server.Models;

namespace Swiper.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddMvc();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<UserContext>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SwiperDB;Trusted_Connection=True;"));

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<UserContext>();

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            builder.Services.Configure<IdentityOptions>(options =>

            {
                // Configure Customize password requirements, lockout settings, etc.
            });

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
