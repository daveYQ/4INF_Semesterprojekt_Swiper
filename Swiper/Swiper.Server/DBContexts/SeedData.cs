using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swiper.Server.Models;

namespace Swiper.Server.DBContexts
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UserContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await RemoveExistingUsers(userManager, context);
                await SeedRoles(roleManager);
                await SeedUsers(userManager);
            }
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Administrator", "Moderator", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task RemoveExistingUsers(UserManager<User> userManager, UserContext context)
        {
            var users = context.Users.ToList();
            context.Images.RemoveRange(context.Images);
            await context.SaveChangesAsync();
            foreach (var user in users)
            {
                await userManager.DeleteAsync(user);
            }
        }

        private static async Task SeedUsers(UserManager<User> userManager)
        {
            if (userManager.Users.Any())
            {
                return;   // Database has been seeded
            }

            User user1 = new User()
            {
                UserName = "Dave",
                Email = "user1@mail.com",
                Residence = "Saalfelden",
                Age = 18
            };
            User user2 = new User()
            {
                UserName = "User2",
                Email = "user2@mail.com",
                Residence = "Salzburg",
                Age = 22
            };
            User user3 = new User()
            {
                UserName = "Patrick the Sexiest Sexist",
                Email = "user3@mail.com",
                Residence = "Voglau",
                Age = 18
            };
            User moderator = new User()
            {
                UserName = "Moderator",
                Email = "moderator@mail.com",
                Residence = "Fugging",
                Age = 35
            };
            User administrator = new User()
            {
                UserName = "Administrator",
                Email = "administrator@mail.com",
                Residence = "Golling",
                Age = 60
            };
            User berni = new User()
            {
                UserName = "Berni",
                Email = "bernhard.pirchner@mail.com",
                Residence = "Niedernfritz City",
                Age = 18
            };
            User tobias = new User()
            {
                UserName = "Zylla",
                Email = "tobias.ziller@mail.com",
                Residence = "Annaberg",
                Age = 18
            };

            var result1 = await userManager.CreateAsync(user1, "ABCabc123!");
            var result2 = await userManager.CreateAsync(user2, "ABCabc123!");
            var result3 = await userManager.CreateAsync(user3, "ABCabc123!");
            var result4 = await userManager.CreateAsync(moderator, "ABCabc123!");
            var result5 = await userManager.CreateAsync(administrator, "ABCabc123!");
            var result6 = await userManager.CreateAsync(berni, "ABCabc123!");
            var result7 = await userManager.CreateAsync(tobias, "ABCabc123!");

            if (result1.Succeeded && result2.Succeeded && result3.Succeeded && result4.Succeeded && result5.Succeeded && result6.Succeeded)
            {
                await userManager.AddToRoleAsync(moderator, "Moderator");
                await userManager.AddToRoleAsync(administrator, "Administrator");

                await SeedUserImages(userManager, user1, "./Images/cat.jpg");
                await SeedUserImages(userManager, user2, "./Images/woman.jpg");
                await SeedUserImages(userManager, user3, "./Images/pat.jpeg");
                await SeedUserImages(userManager, moderator, "./Images/mod.jpg");
                await SeedUserImages(userManager, administrator, "./Images/admin.jpg");
                await SeedUserImages(userManager, berni, "./Images/BernhardPi.jpeg");
                await SeedUserImages(userManager, tobias, "./Images/Ziller.jpeg");
            }
        }

        private static async Task SeedUserImages(UserManager<User> userManager, User user, string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"Image file not found: {imagePath}");
            }

            byte[] imageData = File.ReadAllBytes(imagePath);

            // Create an image object
            Image image = new Image(imageData);

            // Add the image to the user's Images collection
            if (user.Images == null)
            {
                user.Images = new List<Image>();
            }
            user.Images.Add(image);

            // Update the user in the database
            await userManager.UpdateAsync(user);
        }

    }
}
