namespace Swiper.Server.Models
{
    public class RelationshipDTO
    {
        public int? Id { get; set; }
        public UserDTO? UserA { get; set; }
        public UserDTO? UserB { get; set; }
        public bool? ALikedB { get; set; } = false;
        public bool? BLikedA { get; set; } = false;
    }
}
