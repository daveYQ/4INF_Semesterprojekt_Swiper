using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Swiper.Server.Models
{
    public class UserContext : IdentityDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Relationship> Relationships { get; set; }

        public UserContext(DbContextOptions options) : base(options) 
        {

        }
    }
}
