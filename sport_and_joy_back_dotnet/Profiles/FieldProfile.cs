using AutoMapper;
using sport_and_joy_back_dotnet.Entities;
using sport_and_joy_back_dotnet.Models;

namespace sport_and_joy_back_dotnet.Profiles
{
    public class FieldProfile : Profile
    {
        public FieldProfile()
        {
            CreateMap<Field, FieldForCreationDTO>();
        }
    }
}
