using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Tomou.Application.Services.Email;
using Tomou.Application.Validators.User;
using Tomou.Communication.Requests.User.ForgotPassword;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.PasswordToken;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Application.UseCases.User.ForgotPassword;
public class ForgotPasswordUseCase : IForgotPasswordUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
    private readonly IEmailService _emailService;

    public ForgotPasswordUseCase(
        IUnitOfWork unitOfWork, 
        IUserReadOnlyRepository userRepository, 
        IPasswordResetTokenRepository passwordResetTokenRepository,
        IEmailService emailService)
        
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _passwordResetTokenRepository = passwordResetTokenRepository;
        _emailService = emailService;
    }
    public async Task Execute(RequestForgotPasswordJson request)
    {
        Validator(request);
        var user = await _userRepository.GetUserByEmail(request.Email);


        if (user is null)
        {
            return;
        }

        var token = GenerateAlphanumericToken(5);
        var expiresAt = DateTime.UtcNow.AddHours(1);

        var passwordToken = new PasswordResetToken
        {
            UserId = user.Id,
            Token = token,
            ExpiresAt = expiresAt,
        };

        await _passwordResetTokenRepository.Save(passwordToken);
        await _unitOfWork.Commit();


        await _emailService.Send(
            user.Email,
            "Recuperação de Senha - Tomou?",
            $"Use este token para redefinir sua senha: {token}"
        );

    }

    private static string GenerateAlphanumericToken(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var data = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(data);

        var sb = new StringBuilder(length);
        foreach (var b in data)
        {
            sb.Append(chars[b % chars.Length]);
        }
        return sb.ToString();
    }

    private void Validator(RequestForgotPasswordJson request)
    {
        var validator = new ForgotPasswordValidator();
        var result = validator.Validate(request);

        if (result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
