using Moq;
using Shouldly;
using Tomou.Application.UseCases.User.ResetPassword;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.PasswordToken;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Domain.Security.Crypthography;
using Tomou.Exception.ExceptionsBase;
using Tomou.TestUtils.Users.ResetPassword.Requests;

namespace Tomou.UnitTests.UseCases.Users.ResetPassword;
public class ResetPasswordUseCaseTest
{
    [Fact]
    public async Task Execute_Succeeds_WhenTokenIsValid()
    {
        var passwordResetTokenRepoMock = new Mock<IPasswordResetTokenRepository>();
        var unityOfWorkMock = new Mock<IUnitOfWork>();
        var encrypterMock = new Mock<IEncrypter>();
        var userReadOnlyRepoMock = new Mock<IUserReadOnlyRepository>();
        var request = RequestResetPasswordJsonBuilder.Build();
        var tokenEntity = new PasswordResetToken
        {
            Token = request.Token,
            Used = false,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            UserId = 999
        };
        
        var user = passwordResetTokenRepoMock.Setup(r => r.GetByToken(request.Token))
                .ReturnsAsync(tokenEntity);
        
        var userEntity = new Domain.Entities.User
        {
            Id = tokenEntity.UserId,
            Password = "old-hash"
        };
        var useCase = new ResetPasswordUseCase(unityOfWorkMock.Object, passwordResetTokenRepoMock.Object, encrypterMock.Object, userReadOnlyRepoMock.Object);

        await Should.ThrowAsync<UserNotFoundException>(() => useCase.Execute(request));
    }

    [Fact]
    public async Task ShouldThrowException_WhenTokenIsInvalid()
    {
        var passwordResetTokenRepoMock = new Mock<IPasswordResetTokenRepository>();
        var unityOfWorkMock = new Mock<IUnitOfWork>();
        var encrypterMock = new Mock<IEncrypter>();
        var userReadOnlyRepoMock = new Mock<IUserReadOnlyRepository>();
        var request = RequestResetPasswordJsonBuilder.Build();
       passwordResetTokenRepoMock.Setup(r => r.GetByToken(request.Token))
                 .ReturnsAsync((PasswordResetToken?)null);

    
        var useCase = new ResetPasswordUseCase(unityOfWorkMock.Object, passwordResetTokenRepoMock.Object, encrypterMock.Object, userReadOnlyRepoMock.Object);

        await Should.ThrowAsync<InvalidTokenException>(() => useCase.Execute(request));
    }

    [Fact]
    public async Task ShouldThrowException_WhenTokenIsExpired()
    {
        var passwordResetTokenRepoMock = new Mock<IPasswordResetTokenRepository>();
        var unityOfWorkMock = new Mock<IUnitOfWork>();
        var encrypterMock = new Mock<IEncrypter>();
        var userReadOnlyRepoMock = new Mock<IUserReadOnlyRepository>();
        var request = RequestResetPasswordJsonBuilder.Build();

        var tokenEntity = new PasswordResetToken
        {
            Token = request.Token,
            Used = false,
            ExpiresAt = DateTime.UtcNow.AddHours(-1),
            UserId = 999
        };
        passwordResetTokenRepoMock.Setup(r => r.GetByToken(request.Token))
                  .ReturnsAsync((PasswordResetToken?)tokenEntity);


        var useCase = new ResetPasswordUseCase(unityOfWorkMock.Object, passwordResetTokenRepoMock.Object, encrypterMock.Object, userReadOnlyRepoMock.Object);

        await Should.ThrowAsync<InvalidTokenException>(() => useCase.Execute(request));
    }

    [Fact]
    public async Task ShouldThrowException_WhenTokenIsUsed()
    {
        var passwordResetTokenRepoMock = new Mock<IPasswordResetTokenRepository>();
        var unityOfWorkMock = new Mock<IUnitOfWork>();
        var encrypterMock = new Mock<IEncrypter>();
        var userReadOnlyRepoMock = new Mock<IUserReadOnlyRepository>();
        var request = RequestResetPasswordJsonBuilder.Build();

        var tokenEntity = new PasswordResetToken
        {
            Token = request.Token,
            Used = true,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            UserId = 999
        };
        passwordResetTokenRepoMock.Setup(r => r.GetByToken(request.Token))
                  .ReturnsAsync((PasswordResetToken?)tokenEntity);


        var useCase = new ResetPasswordUseCase(unityOfWorkMock.Object, passwordResetTokenRepoMock.Object, encrypterMock.Object, userReadOnlyRepoMock.Object);

        await Should.ThrowAsync<InvalidTokenException>(() => useCase.Execute(request));
    }
}
