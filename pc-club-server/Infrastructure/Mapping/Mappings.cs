using AutoMapper;

namespace pc_club_server.Infrastructure.Mapping
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Core.Entities.User, API.Models.UserDto>();
            CreateMap<API.Models.UserUpdateDto, API.Models.UserDto>();

            CreateMap<Core.Entities.PlaySession, API.Models.PlaySessionDto>();
        }
    }
}
