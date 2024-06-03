namespace SwiperModeration
{
    public class UserModDTO
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<ImageDTO>? Images { get; set; }
        public int? Age { get; set; }
        public string? Residence { get; set; }
        public bool? IsBlocked { get; set; }
    }
}
