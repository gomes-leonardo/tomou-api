using AutoMapper;
using Tomou.Application.Services.Auth;
using Tomou.Communication.Responses.Dependent.Get;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.User;
using Tomou.Exception;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.Dependent.GetDependentById;
public class GetDependentByIdUseCase : IGetDependentByIdUseCase
{
    private readonly IDependentReadOnlyRepository _repository;
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public GetDependentByIdUseCase(
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
    public async Task<ResponseDependentShortJson> Execute(Guid id)
    {
        var caregiverId = _userContext.GetUserId();
        var user = await _userRepository.GetUserById(caregiverId);

        if (user is null || user.IsCaregiver is false)
            throw new ForbiddenAccessException(ResourceErrorMessages.UNAUTHORIZED);
        
        var result = await _repository.GetByIdAsync(id);

        if(result is null)
            throw new NotFoundException(ResourceErrorMessages.DEPENDENT_NOT_FOUND);

        if(result.CaregiverId != user.Id)
            throw new ForbiddenAccessException(ResourceErrorMessages.FORBIDDEN_ACCESS);

        return _mapper.Map<ResponseDependentShortJson>(result);
    }
}
