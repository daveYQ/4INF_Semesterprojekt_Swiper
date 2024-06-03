using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Swiper.Server.Models
{
    public class User : IdentityUser
    {
        public List<Image> Images { get; set; } 
        public List<User> LikedUsers { get; set; } 
        public int Age { get; set; }
        public string? Residence { get; set; }
        public bool IsBlocked { get; set; }
    }
}
