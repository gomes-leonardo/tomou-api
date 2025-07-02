using AutoMapper;
using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Communication.Requests.User.Register;
using Tomou.Communication.Responses.Dependent.Get;
using Tomou.Communication.Responses.Dependent.Register;
using Tomou.Communication.Responses.Dependent.Update;
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
        CreateMap<RequestRegisterDependentJson, Domain.Entities.Dependent>();
        CreateMap<RequestUpdateDependentJson, Domain.Entities.Dependent>();
    }
    private void EntityToResponse()
    {
        CreateMap<User, ResponseRegisteredUserJson>();
        CreateMap<Domain.Entities.Dependent, ResponseCreateDependentJson >();
        CreateMap<Domain.Entities.Dependent, ResponseDependentShortJson >();
        CreateMap<Domain.Entities.Dependent, ResponseUpdatedDependentJson>();

    }
}
