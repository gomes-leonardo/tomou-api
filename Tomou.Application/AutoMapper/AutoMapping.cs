using AutoMapper;
using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Communication.Requests.Medications.Register;
using Tomou.Communication.Requests.User.Register;
using Tomou.Communication.Responses.Dependent.Get;
using Tomou.Communication.Responses.Dependent.Register;
using Tomou.Communication.Responses.Dependent.Update;
using Tomou.Communication.Responses.Medications;
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
        CreateMap<RequestRegisterMedicationsJson, Medication>()
        .ForMember(dest => dest.TimesToTake,
                   opt => opt.MapFrom(src => src.TimesToTake.Select(TimeOnly.Parse).ToList()))
        .ForMember(dest => dest.DaysOfWeek,
                   opt => opt.MapFrom(src => src.DaysOfWeek.Select(day => Enum.Parse<DayOfWeek>(day)).ToList()))
        .ForMember(dest => dest.UserId, opt => opt.Ignore())
        .ForMember(dest => dest.DependentId, opt => opt.Ignore());
    }
    private void EntityToResponse()
    {
        CreateMap<User, ResponseRegisteredUserJson>();
        CreateMap<Domain.Entities.Dependent, ResponseCreateDependentJson >();
        CreateMap<Domain.Entities.Dependent, ResponseDependentShortJson >();
        CreateMap<Domain.Entities.Dependent, ResponseUpdatedDependentJson>();
        CreateMap<Domain.Entities.Medication, ResponseRegisterMedicationJson>();

    }
}
