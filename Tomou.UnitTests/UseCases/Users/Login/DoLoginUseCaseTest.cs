using Moq;
using Shouldly;
using Tomou.Application.UseCases.User.Login;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.User;
using Tomou.Domain.Security.Crypthography;
using Tomou.Domain.Security.Tokens;
using Tomou.Exception.ExceptionsBase;
using Tomou.TestUtils.User.Login.Requests;

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

        const string expectedToken = "meu-token-gerado";

        var readRepoMock = new Mock<IUserReadOnlyRepository>();
        var encrypterMock = new Mock<IEncrypter>();
        var tokenGeneratorMock = new Mock<IAccessTokenGenerator>(); 

        readRepoMock.Setup(r => r.GetUserByEmail(request.Email)).ReturnsAsync(user);
        encrypterMock.Setup(r => r.Compare(request.Password, user.Password)).Returns(true);
        tokenGeneratorMock
            .Setup(t => t.Generate(user.Id, user.Name, user.Email, user.IsCaregiver))
            .Returns(expectedToken);

        var useCase = new DoLoginUseCase(readRepoMock.Object, encrypterMock.Object, tokenGeneratorMock.Object);

        var response = await useCase.Execute(request);

        response.ShouldNotBeNull();
        response.Name.ShouldBe(user.Name);
        response.Token.ShouldBe(expectedToken);
    }


    [Fact]
    public async Task ShouldThrowException_WhenUserDoesNotExist()
    {
        var request = RequestLoginUserJsonBuilder.Build();
        
        var readRepoMock = new Mock<IUserReadOnlyRepository>();
        var encrypterMock = new Mock<IEncrypter>();
        var tokenGeneratorMock = new Mock<IAccessTokenGenerator>();

        readRepoMock.Setup(r => r.GetUserByEmail(request.Email)).ReturnsAsync((User?)null); 

        var useCase = new DoLoginUseCase(readRepoMock.Object, encrypterMock.Object, tokenGeneratorMock.Object);

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
        var tokenGeneratorMock = new Mock<IAccessTokenGenerator>();


        encrypterMock.Setup(r => r.Compare(request.Password, user.Password)).Returns(false);
        var useCase = new DoLoginUseCase(readRepoMock.Object, encrypterMock.Object, tokenGeneratorMock.Object);

        await Should.ThrowAsync<InvalidCredentialsException>(() => useCase.Execute(request));

    }
}
