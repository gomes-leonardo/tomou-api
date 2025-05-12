using AutoMapper;
using Tomou.Communication.Requests.User;
using Tomou.Domain.Entities;

namespace Tomou.Application.AutoMapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }
    private void RequestToEntity()
    {
        CreateMap<RequestRegisterUserJson, User>();
    }
    private void EntityToResponse()
    {
        CreateMap<User, RequestRegisterUserJson>();

    }
}
