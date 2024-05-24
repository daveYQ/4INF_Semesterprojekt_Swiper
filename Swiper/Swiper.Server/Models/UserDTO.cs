namespace Swiper.Server.Models
{
    public class UserDTO
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<ImageDTO>? Images { get; set; }
        public int? Age { get; set; }
        public string? Residence { get; set; }
    }
}
