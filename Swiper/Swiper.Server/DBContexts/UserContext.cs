using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Swiper.Server.Models;

namespace Swiper.Server.DBContexts
{
    public class UserContext : IdentityDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }

        public UserContext(DbContextOptions options) : base(options)
        {

        }
    }
}
