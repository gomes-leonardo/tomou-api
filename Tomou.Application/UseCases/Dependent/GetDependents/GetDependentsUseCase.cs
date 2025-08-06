using AutoMapper;
using Tomou.Application.Services.Auth;
using Tomou.Communication.Responses.Dependent.Get;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;
using Tomou.Exception;
using Tomou.Domain.Repositories.Dependent.Filters;

namespace Tomou.Application.UseCases.Dependent.GetAll;
public class GetDependentsUseCase : IGetDependentsUseCase
{
    private readonly IDependentReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IUserReadOnlyRepository _userRepository;
    public GetDependentsUseCase(
        IDependentReadOnlyRepository repository,
        IMapper mapper,
        IUserContext userContext,
        IUserReadOnlyRepository userRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _userContext = userContext;
        _userRepository = userRepository;

    }
    public async Task<ResponseDependentsJson> Execute(string? nameFilter = null, bool ascending = true)
    {
        var caregiverId = _userContext.GetUserId();
        var user = await _userRepository.GetUserById(caregiverId);

        if (user is null || user.IsCaregiver is false)
            throw new ForbiddenAccessException(ResourceErrorMessages.UNAUTHORIZED);



        var filter = new DependentFilter(
            caregiverId: caregiverId,
            nameContains: nameFilter,
            ascending: ascending
        );
        var result = await _repository.GetByCaregiverId(
            filter
        );

        return new ResponseDependentsJson
        {
            Dependents = _mapper.Map<List<ResponseDependentShortJson>>(result)
        };
    }
}
