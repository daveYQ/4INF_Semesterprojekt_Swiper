namespace Swiper.Server.Models
{
    public class UserCreationDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserCreationDTO() { }
        public UserCreationDTO(string userName, string email, string password) 
        { 
            this.UserName = userName;
            this.Email = email;
            this.Password = password;
        }
    }
}
