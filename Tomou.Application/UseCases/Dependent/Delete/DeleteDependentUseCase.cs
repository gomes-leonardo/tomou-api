
using Tomou.Application.Services.Auth;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;
using Tomou.Exception;

namespace Tomou.Application.UseCases.Dependent.Delete;
public class DeleteDependentUseCase : IDeleteDependentUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDependentWriteOnlyRepository _repository;
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IUserContext _userContext;

    public DeleteDependentUseCase(
        IUnitOfWork unitOfWork,
        IDependentWriteOnlyRepository repository,
        IUserContext userContext,
        IUserReadOnlyRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _repository = repository;
        _userRepository = userRepository;
    }
    public async Task Execute(Guid id)
    {
        var caregiverId = _userContext.GetUserId();
        var user = await _userRepository.GetUserById(caregiverId);

        if(user is null || user.IsCaregiver == false)
            throw new ForbiddenAccessException(ResourceErrorMessages.UNAUTHORIZED);

        var result = await _repository.DeleteAsync(id);
        if(result is false)
        {
            throw new NotFoundException(ResourceErrorMessages.DEPENDENT_NOT_FOUND);
        }

        await _unitOfWork.Commit();
    }
}
