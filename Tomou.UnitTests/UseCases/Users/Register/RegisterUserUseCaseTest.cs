﻿using AutoMapper;
using Moq;
using Shouldly;
using Tomou.Application.UseCases.User.Register;
using Tomou.Communication.Responses.User.Register;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Domain.Security.Crypthography;
using Tomou.Exception.ExceptionsBase;
using Tomou.TestUtils.User.Register.Requests;

namespace Tomou.UnitTests.UseCases.Users.Register;
public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task ShouldThrowExceptionWhenEmailAlreadyExists()
    {
        var readRepoMock = new Mock<IUserReadOnlyRepository>();
        var writeRepoMock = new Mock<IUserWriteOnlyRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var encrypterMock = new Mock<IEncrypter>();
        var mapperMock = new Mock<IMapper>();

        readRepoMock.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = new RegisterUserUseCase(
           readRepoMock.Object,
           writeRepoMock.Object,
           unitOfWorkMock.Object,
           mapperMock.Object,
           encrypterMock.Object
        );

        await Should.ThrowAsync<EmailAlreadyExistsException>(() => useCase.Execute(request));
    }

    [Fact]
    public async Task ShouldRegisterUserSuccessfullWhenEmailDoesNotExist()
    {
        var readRepoMock = new Mock<IUserReadOnlyRepository>();
        var writeRepoMock = new Mock<IUserWriteOnlyRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var encrypterMock = new Mock<IEncrypter>();
        var mapperMock = new Mock<IMapper>();

        readRepoMock.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        var request = RequestRegisterUserJsonBuilder.Build();
        var userEntity = new User();

        mapperMock.Setup(m => m.Map<User>(request)).Returns(userEntity);
        mapperMock.Setup(m => m.Map<ResponseRegisteredUserJson>(userEntity)).Returns(
            new ResponseRegisteredUserJson
            {
                Name = request.Name,
                Email = request.Email
            });
        encrypterMock.Setup(e => e.Encrypt(request.Password)).Returns("hashedPassword");


        var useCase = new RegisterUserUseCase(
           readRepoMock.Object,
           writeRepoMock.Object,
           unitOfWorkMock.Object,
           mapperMock.Object,
           encrypterMock.Object
        );

        var response = await useCase.Execute(request);

        response.ShouldNotBeNull();
        response.Name.ShouldBe(request.Name);
        response.Email.ShouldBe(request.Email);
        writeRepoMock.Verify(w => w.Add(It.IsAny<User>()), Times.Once);
        unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        encrypterMock.Verify(e => e.Encrypt(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ShouldNotCallCommit_WhenEmailAlreadyExists()
    {
        var readRepoMock = new Mock<IUserReadOnlyRepository>();
        var writeRepoMock = new Mock<IUserWriteOnlyRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var encrypterMock = new Mock<IEncrypter>();
        var mapperMock = new Mock<IMapper>();

        readRepoMock.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = new RegisterUserUseCase(
           readRepoMock.Object,
           writeRepoMock.Object,
           unitOfWorkMock.Object,
           mapperMock.Object,
           encrypterMock.Object
        );

        await Assert.ThrowsAsync<EmailAlreadyExistsException>(() => useCase.Execute(request));

        writeRepoMock.Verify(w => w.Add(It.IsAny<User>()), Times.Never);
        unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
        encrypterMock.Verify(e => e.Encrypt(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ShouldCallEncryptPassword_BeforeSavingUser()
    {
        var readRepoMock = new Mock<IUserReadOnlyRepository>();
        var writeRepoMock = new Mock<IUserWriteOnlyRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var encrypterMock = new Mock<IEncrypter>();
        var mapperMock = new Mock<IMapper>();
        User? savedUser = null;

        readRepoMock.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        writeRepoMock.Setup(w => w.Add(It.IsAny<User>()))
            .Callback<User>(u => savedUser = u);

        var request = RequestRegisterUserJsonBuilder.Build();
        var userEntity = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = "placeholder"
        };

        mapperMock.Setup(m => m.Map<User>(request)).Returns(userEntity);
        mapperMock.Setup(m => m.Map<ResponseRegisteredUserJson>(userEntity))
       .Returns(new ResponseRegisteredUserJson
       {
           Name = request.Name,
           Email = request.Email,

       });

        var useCase = new RegisterUserUseCase(
             readRepoMock.Object,
             writeRepoMock.Object,
             unitOfWorkMock.Object,
             mapperMock.Object,
             encrypterMock.Object
          );


        encrypterMock.Setup(e => e.Encrypt(request.Password)).Returns("hashed123");
        await useCase.Execute(request);


        savedUser!.Password.ShouldBe("hashed123");
    }
}
