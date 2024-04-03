namespace Swiper.Server.Models
{
    public class UserDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public List<Image>? Images { get; set; }
    }
}
