using AutoMapper;
using sport_and_joy_back_dotnet.Entities;
using sport_and_joy_back_dotnet.Model;
using sport_and_joy_back_dotnet.Models;

namespace sport_and_joy_back_dotnet.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserForCreationDTO>();
            CreateMap<User, UserForModificationDTO>();
            CreateMap<User, UserDTO>();
        }

    }
}
