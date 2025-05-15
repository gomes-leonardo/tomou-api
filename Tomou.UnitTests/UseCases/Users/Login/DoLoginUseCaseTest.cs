using Moq;
using Shouldly;
using Tomou.Application.UseCases.User.Login;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.User;
using Tomou.Domain.Security.Crypthography;
using Tomou.Exception.ExceptionsBase;
using Tomou.TestUtils.Users.Login.Requests;

namespace Tomou.UnitTests.UseCases.Users.Login;
public class DoLoginUseCaseTest
{
    [Fact]
    public async Task ShouldLogin_WhenCredentialsAreValid()
    {
        var request = RequestLoginUserJsonBuilder.Build();
        var user = new User
        {
            Id = 1,
            Email = request.Email,
            Password = "hashed-password"
        };

        var readRepoMock = new Mock<IUserReadOnlyRepository>();
        var encrypterMock = new Mock<IEncrypter>();

        readRepoMock.Setup(r => r.GetUserByEmail(request.Email)).ReturnsAsync(user);
        encrypterMock.Setup(r => r.Compare(request.Password, user.Password)).Returns(true);

        var useCase = new DoLoginUseCase(readRepoMock.Object, encrypterMock.Object);

        var response = await useCase.Execute(request);

        response.ShouldNotBeNull();
        response.Name.ShouldBe(user.Name);
    }

    [Fact]
    public async Task ShouldThrowException_WhenUserDoesNotExist()
    {
        var request = RequestLoginUserJsonBuilder.Build();
        
        var readRepoMock = new Mock<IUserReadOnlyRepository>();
        var encrypterMock = new Mock<IEncrypter>();

        readRepoMock.Setup(r => r.GetUserByEmail(request.Email)).ReturnsAsync((User?)null); 

        var useCase = new DoLoginUseCase(readRepoMock.Object, encrypterMock.Object);

        await Should.ThrowAsync<InvalidCredentialsException>(() => useCase.Execute(request));

    }
    [Fact]
    public async Task ShouldThrowException_WhenPasswordIsInvalid()
    {
        var request = RequestLoginUserJsonBuilder.Build();
        var user = new User
        {
            Id = 1,
            Email = request.Email,
            Password = "hashed-password"
        };

        var readRepoMock = new Mock<IUserReadOnlyRepository>();
        var encrypterMock = new Mock<IEncrypter>();

        encrypterMock.Setup(r => r.Compare(request.Password, user.Password)).Returns(false);
        var useCase = new DoLoginUseCase(readRepoMock.Object, encrypterMock.Object);

        await Should.ThrowAsync<InvalidCredentialsException>(() => useCase.Execute(request));

    }
}
