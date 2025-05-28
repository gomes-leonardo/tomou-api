using Tomou.Communication.Requests.User.ForgotPassword;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.PasswordToken;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Domain.Security.Crypthography;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.User.ResetPassword;
public class ResetPasswordUseCase : IResetPasswordUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IEncrypter _encrypter;

    public ResetPasswordUseCase(
        IUnitOfWork unitOfWork,
        IPasswordResetTokenRepository passwordResetTokenRepository,
        IEncrypter encrypter,
        IUserReadOnlyRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _passwordResetTokenRepository = passwordResetTokenRepository;
        _encrypter = encrypter;
    }
    public async Task Execute(RequestResetPasswordJson request)
    {
        Validate(request);

        var token = await _passwordResetTokenRepository.GetByToken(request.Token);

        if (token is null) throw new InvalidTokenException();
        if (token.Used is true) throw new InvalidTokenException();
        if(token.ExpiresAt < DateTime.UtcNow) throw new InvalidTokenException();

        var newHashedPassword = _encrypter.Encrypt(request.NewPassword);
        var user = await _userRepository.GetUserById(token.UserId);

        if (user is null) throw new UserNotFoundException();

        user.Password = newHashedPassword;
        await _passwordResetTokenRepository.MarkAsUsed(token);

        await _unitOfWork.Commit();

    }

    private void Validate(RequestResetPasswordJson request)
    {
        var validator = new ResetPasswordValidator();
        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(result => result.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
