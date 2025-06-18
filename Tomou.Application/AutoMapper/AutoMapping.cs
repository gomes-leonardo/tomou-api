using AutoMapper;
using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Communication.Requests.User.Register;
using Tomou.Communication.Responses.Dependent.Register;
using Tomou.Communication.Responses.User.Register;
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
        CreateMap<RequestRegisterDependentJson, Tomou.Domain.Entities.Dependent>();
    }
    private void EntityToResponse()
    {
        CreateMap<User, ResponseRegisteredUserJson>();
        CreateMap<Tomou.Domain.Entities.Dependent, ResponseCreateDependentJson >();

    }
}
