using AutoMapper;

namespace Swiper.Server.Models
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            this.CreateMap<User, UserDTO>();
            this.CreateMap<UserDTO, User>();
        }
    }
}
