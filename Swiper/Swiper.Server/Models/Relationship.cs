using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Swiper.Server.Models
{
    public class Relationship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public User UserA { get; set; }
        public User UserB { get; set; }

        public bool ALikedB { get; set; } = false;
        public bool BLikedA { get; set; } = false;
    }
}
