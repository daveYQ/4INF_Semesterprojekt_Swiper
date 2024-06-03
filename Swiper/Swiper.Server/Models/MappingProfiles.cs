using AutoMapper;

namespace Swiper.Server.Models
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            this.CreateMap<User, UserDTO>();
            this.CreateMap<UserDTO, User>();

            this.CreateMap<UserCreationDTO, User>();

            this.CreateMap<UserModDTO, User>();
            this.CreateMap<User, UserModDTO>();

            this.CreateMap<Image, ImageDTO>();
            this.CreateMap<ImageDTO, Image>();
        }
    }
}
