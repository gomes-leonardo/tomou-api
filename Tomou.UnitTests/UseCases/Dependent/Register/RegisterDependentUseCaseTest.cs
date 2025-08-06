namespace Tomou.UnitTests.UseCases.Dependent.Register;
using AutoMapper;
using Moq;
using Shouldly;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Dependent.Register;
using Tomou.Communication.Responses.Dependent.Register;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;
using Tomou.TestUtils.Dependent.Register.Request;

public class RegisterDependentUseCaseTest
{
    [Fact]
    public async Task ShouldRegisterDependentSuccessfullWhenUserIsCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentWriteOnlyRepositoryMock = new Mock<IDependentWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();


        var userId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
            Dependents = new List<Tomou.Domain.Entities.Dependent>()
        });

        var request = RequestRegisterDependentJsonBuilder.Build();

         mapperMock
         .Setup(m => m.Map<Tomou.Domain.Entities.Dependent>(It.IsAny<object>()))
         .Returns(new Tomou.Domain.Entities.Dependent { Name = request.Name });

        mapperMock
          .Setup(m => m.Map<ResponseCreateDependentJson>(It.IsAny<Tomou.Domain.Entities.Dependent>()))
          .Returns<Tomou.Domain.Entities.Dependent>(ent => new ResponseCreateDependentJson
          {
              Name = ent.Name,
              Message = ""
          });
        dependentWriteOnlyRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Tomou.Domain.Entities.Dependent>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        unitOfWorkMock
        .Setup(u => u.Commit())
         .Returns(Task.CompletedTask)
        .Verifiable();

        var useCase = new RegisterDependentUseCase(
            mapperMock.Object,
            unitOfWorkMock.Object,
            dependentWriteOnlyRepositoryMock.Object,
            userReadonlyRepositoryMock.Object,
            userContextMock.Object
        );

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentWriteOnlyRepositoryMock = new Mock<IDependentWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = false,
            Id = userId,
            Dependents = new List<Tomou.Domain.Entities.Dependent>()
        });

        var request = RequestRegisterDependentJsonBuilder.Build();

        var useCase = new RegisterDependentUseCase(
            mapperMock.Object,
            unitOfWorkMock.Object,
            dependentWriteOnlyRepositoryMock.Object,
            userReadonlyRepositoryMock.Object,
            userContextMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(request));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotFound()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentWriteOnlyRepositoryMock = new Mock<IDependentWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId))
            .ReturnsAsync((Tomou.Domain.Entities.User?)null);

        var request = RequestRegisterDependentJsonBuilder.Build();

        var useCase = new RegisterDependentUseCase(
            mapperMock.Object,
            unitOfWorkMock.Object,
            dependentWriteOnlyRepositoryMock.Object,
            userReadonlyRepositoryMock.Object,
            userContextMock.Object
        );
        await Should.ThrowAsync<ForbiddenAccessException>(
       () => useCase.Execute(request));
    }

    
}


