using AutoMapper;

namespace Swiper.Server.Models
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            this.CreateMap<User, UserDTO>();
            this.CreateMap<UserDTO, User>();

            this.CreateMap<Relationship, RelationshipDTO>();
                //.ForMember(dest => dest.UserAId, opt => opt.MapFrom(src => src.UserA.Id))
                //.ForMember(dest => dest.UserBId, opt => opt.MapFrom(src => src.UserB.Id));
            this.CreateMap<RelationshipDTO, Relationship>();
            this.CreateMap<UserCreationDTO, User>();
        }
    }
}
