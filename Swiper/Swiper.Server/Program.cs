
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

            var allowAngular = "allowAngular";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: allowAngular,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:4200")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowCredentials();
                                  });
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
                options.ExpireTimeSpan = TimeSpan.FromDays(5);
                options.SlidingExpiration = true;
            });

            /*
            builder.Services.AddAuthentication("CookieUserAuth")
                .AddCookie("CookieUserAuth", options =>
                {
                    options.Cookie.Name = "CookieUserAuth";
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });
            */

            builder.Services.AddAuthorization();

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

            /*
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Configure Customize password requirements, lockout settings, etc.
            });
            */

            var app = builder.Build();

            // Configure middleware pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRouting();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors(allowAngular);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllers();
            app.MapFallbackToFile("/index.html");
            app.Run();
        }
    }
}
