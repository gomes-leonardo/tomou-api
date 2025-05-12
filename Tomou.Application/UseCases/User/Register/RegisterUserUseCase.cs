using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Tomou.Communication.Requests.User;
using Tomou.Communication.Responses.User;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Domain.Security;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.User.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _readRepository;
    private readonly IUserWriteOnlyRepository _writeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEncrypter _encrypter;
    public RegisterUserUseCase(
        IUserReadOnlyRepository readRepository,
        IUserWriteOnlyRepository writeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEncrypter encrypter)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _encrypter = encrypter;

    }
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        Validator(request);

        if(await _readRepository.ExistsByEmailAsync(request.Email))
        {
            throw new EmailAlreadyExistsException();
        }

        var entity = _mapper.Map<Domain.Entities.User>(request);
        entity.Password = _encrypter.Encrypt(request.Password);

        await _writeRepository.Add(entity);
        await _unitOfWork.Commit();
        
        return _mapper.Map<ResponseRegisteredUserJson>(entity);
        
    }

    private void Validator(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
