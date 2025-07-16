using Moq;
using Tomou.Application.Services.Email;
using Tomou.Application.UseCases.User.ForgotPassword;
using Tomou.Communication.Requests.User.ForgotPassword;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.PasswordToken;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;

namespace Tomou.UnitTests.UseCases.Users.ForgotPassword;
public class ForgotPasswordUseCaseTests
{
    [Fact]
    public async Task ShouldGenerateAndSendToken_WhenEmailIsValid()
    {
        var request = new RequestForgotPasswordJson { Email = "teste@tomou.com" };
        var userId = Guid.NewGuid();

        var user = new Domain.Entities.User
        {
            Id = userId,
            Name = "Leonardo",
            Email = request.Email,
            Password = "password"
        };

        var userRepoMock = new Mock<IUserReadOnlyRepository>();
        var tokenRepoMock = new Mock<IPasswordResetTokenRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var emailServiceMock = new Mock<IEmailService>();


        userRepoMock.Setup(repo => repo.GetUserByEmail(request.Email)).ReturnsAsync(user);

        var useCase = new ForgotPasswordUseCase(
           unitOfWorkMock.Object,
           userRepoMock.Object,
           tokenRepoMock.Object,
           emailServiceMock.Object
       );

        await useCase.Execute(request);

        tokenRepoMock.Verify(repo => repo.Save(It.IsAny<PasswordResetToken>()), Times.Once);
        unitOfWorkMock.Verify(uow => uow.Commit(), Times.Exactly(2));
        emailServiceMock.Verify(email => email.Send(
            user.Email,
            It.Is<string>(s => s.Contains("Recuperação")),
            It.Is<string>(b => b.Contains("token"))), Times.Once);

    }[Fact]
    public async Task ShouldDoNothing_WhenUserDoesNotExist()
    {
        var request = new RequestForgotPasswordJson { Email = "inexistente@email.com" };


        var userRepoMock = new Mock<IUserReadOnlyRepository>();
        var tokenRepoMock = new Mock<IPasswordResetTokenRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var emailServiceMock = new Mock<IEmailService>();


        userRepoMock.Setup(repo => repo.GetUserByEmail(request.Email))
              .ReturnsAsync((User?)null);

        var useCase = new ForgotPasswordUseCase(
           unitOfWorkMock.Object,
           userRepoMock.Object,
           tokenRepoMock.Object,
           emailServiceMock.Object
       );

        await useCase.Execute(request);
        tokenRepoMock.Verify(repo => repo.Save(It.IsAny<PasswordResetToken>()), Times.Never);
        unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never);
        emailServiceMock.Verify(email => email.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);


    }
}
