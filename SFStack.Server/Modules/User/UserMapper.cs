using AutoMapper;

namespace SFStack.Server.Modules;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, GetUserResponseDTO>();
        CreateMap<CreateUserRequestDTO, User>();
    }
}