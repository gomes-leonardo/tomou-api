namespace Tomou.UnitTests.UseCases.Dependent.Get;
using AutoMapper;
using Moq;
using Shouldly;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Dependent.GetAll;
using Tomou.Communication.Responses.Dependent.Get;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;
using Tomou.Domain.Repositories.Dependent.Filters;

public class GetDependentUseCaseTest
{
    [Fact]
    public async Task ShouldGetDependentsSuccessfully()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
        });

        var dependents = new List<Tomou.Domain.Entities.Dependent>
        {
            new() { Id = Guid.NewGuid(), Name = "Leonardo Gomes" },
            new() { Id = Guid.NewGuid(), Name = "Elizabeth Rodrigues" }
        };

        dependentReadOnlyRepositoryMock
            .Setup(r => r.GetByCaregiverId(It.IsAny<DependentFilter>()))
            .ReturnsAsync(dependents);

        var expectedResponse = new ResponseDependentsJson
        {
            Dependents = dependents.Select(d => new ResponseDependentShortJson { Id = d.Id, Name = d.Name }).ToList()
        };

        mapperMock
            .Setup(m => m.Map<List<ResponseDependentShortJson>>(dependents))
            .Returns(expectedResponse.Dependents);

        var useCase = new GetDependentsUseCase(
            dependentReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object
        );

        var result = await useCase.Execute(null, true);

        result.ShouldNotBeNull();
        result.Dependents.ShouldNotBeNull();
        result.Dependents.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotCaregiver()
    {
        var userId = Guid.NewGuid();

        var userContextMock = new Mock<IUserContext>();
        var userRepositoryMock = new Mock<IUserReadOnlyRepository>();
        userContextMock.Setup(x => x.GetUserId()).Returns(userId);
        userRepositoryMock.Setup(x => x.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            Id = userId,
            IsCaregiver = false
        });

        var useCase = new GetDependentsUseCase(
            Mock.Of<IDependentReadOnlyRepository>(),
            Mock.Of<IMapper>(),
            userContextMock.Object,
            userRepositoryMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() =>
            useCase.Execute(null, true));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserNotFound()
    {
        var userId = Guid.NewGuid();

        var userContextMock = new Mock<IUserContext>();
        var userRepositoryMock = new Mock<IUserReadOnlyRepository>();
        userContextMock.Setup(x => x.GetUserId()).Returns(userId);
        userRepositoryMock.Setup(x => x.GetUserById(userId)).ReturnsAsync((Tomou.Domain.Entities.User)null!);

        var useCase = new GetDependentsUseCase(
            Mock.Of<IDependentReadOnlyRepository>(),
            Mock.Of<IMapper>(),
            userContextMock.Object,
            userRepositoryMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() =>
            useCase.Execute(null, true));
    }

    [Fact]
    public async Task ShouldGetDependentsWithFilterSuccessfully()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
        });

        var dependents = new List<Tomou.Domain.Entities.Dependent>
        {
            new() { Id = Guid.NewGuid(), Name = "Leonardo Gomes" }
        };

        dependentReadOnlyRepositoryMock
            .Setup(r => r.GetByCaregiverId(It.IsAny<DependentFilter>()))
            .ReturnsAsync(dependents);

        var expectedResponse = new ResponseDependentsJson
        {
            Dependents = dependents.Select(d => new ResponseDependentShortJson { Id = d.Id, Name = d.Name }).ToList()
        };

        mapperMock
            .Setup(m => m.Map<List<ResponseDependentShortJson>>(dependents))
            .Returns(expectedResponse.Dependents);

        var useCase = new GetDependentsUseCase(
            dependentReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object
        );

        var result = await useCase.Execute("Leonardo", true);

        result.ShouldNotBeNull();
        result.Dependents.ShouldNotBeNull();
        result.Dependents.Count.ShouldBe(1);
    }

}
