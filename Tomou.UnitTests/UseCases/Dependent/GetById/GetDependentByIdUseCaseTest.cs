using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Shouldly;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Dependent.GetDependentById;
using Tomou.Communication.Responses.Dependent.Get;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.UnitTests.UseCases.Dependent.GetById;
public class GetDependentByIdUseCaseTest
{
    [Fact]
    public async Task ShouldReturnDependentSuccessfullyWhenUserIsCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var userReadOnlyRepositoryMock = new Mock<IUserReadOnlyRepository>();

        var caregiverId = Guid.NewGuid();
        userContextMock
            .Setup(c => c.GetUserId())
            .Returns(caregiverId);

        userReadOnlyRepositoryMock
            .Setup(r => r.GetUserById(caregiverId))
            .ReturnsAsync(new User
            {
                Id = caregiverId,
                IsCaregiver = true
            });

        var dependentId = Guid.NewGuid();
        var dependent = new Tomou.Domain.Entities.Dependent
        {
            Id = dependentId,
            Name = "Leonardo Gomes",
            CaregiverId = caregiverId
        };

        dependentReadOnlyRepositoryMock
            .Setup(r => r.GetByIdAsync(dependentId))
            .ReturnsAsync(dependent);

        mapperMock
            .Setup(m => m.Map<ResponseDependentShortJson>(dependent))
            .Returns(new ResponseDependentShortJson
            {
                Id = dependent.Id,
                Name = dependent.Name
            });

        var useCase = new GetDependentByIdUseCase(
            dependentReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadOnlyRepositoryMock.Object
        );

        var response = await useCase.Execute(dependentId);

        response.ShouldNotBeNull();
        response.Id.ShouldBe(dependentId);
        response.Name.ShouldBe("Leonardo Gomes");
    } 

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var userReadOnlyRepositoryMock = new Mock<IUserReadOnlyRepository>();

        var caregiverId = Guid.NewGuid();
        userContextMock
            .Setup(c => c.GetUserId())
            .Returns(caregiverId);

        userReadOnlyRepositoryMock
            .Setup(r => r.GetUserById(caregiverId))
            .ReturnsAsync(new User
            {
                Id = caregiverId,
                IsCaregiver = false
            });
        var useCase = new GetDependentByIdUseCase(
            dependentReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadOnlyRepositoryMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(caregiverId));

    }


    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIdIsNotFound()
    {
        var mapperMock = new Mock<IMapper>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var userReadOnlyRepositoryMock = new Mock<IUserReadOnlyRepository>();

        var caregiverId = Guid.NewGuid();
        userContextMock
            .Setup(c => c.GetUserId())
            .Returns(caregiverId);

        userReadOnlyRepositoryMock
        .Setup(r => r.GetUserById(caregiverId))
        .ReturnsAsync((User?)null);


        var useCase = new GetDependentByIdUseCase(
            dependentReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadOnlyRepositoryMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(caregiverId));

    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenDependentDoesNotExist()
    {
        var mapperMock = new Mock<IMapper>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var userReadOnlyRepositoryMock = new Mock<IUserReadOnlyRepository>();

        var caregiverId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();

        userContextMock.Setup(c => c.GetUserId()).Returns(caregiverId);

        userReadOnlyRepositoryMock.Setup(r => r.GetUserById(caregiverId))
            .ReturnsAsync(new User
            {
                Id = caregiverId,
                IsCaregiver = true
            });

        dependentReadOnlyRepositoryMock
            .Setup(r => r.GetByIdAsync(dependentId))
            .ReturnsAsync((Tomou.Domain.Entities.Dependent?)null);

        var useCase = new GetDependentByIdUseCase(
            dependentReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadOnlyRepositoryMock.Object
        );

        await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(dependentId));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenDependentBelongsToAnotherCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var userReadOnlyRepositoryMock = new Mock<IUserReadOnlyRepository>();

        var caregiverId = Guid.NewGuid();
        var otherCaregiverId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();

        userContextMock.Setup(c => c.GetUserId()).Returns(caregiverId);

        userReadOnlyRepositoryMock.Setup(r => r.GetUserById(caregiverId))
            .ReturnsAsync(new User
            {
                Id = caregiverId,
                IsCaregiver = true
            });

        dependentReadOnlyRepositoryMock
            .Setup(r => r.GetByIdAsync(dependentId))
            .ReturnsAsync(new Tomou.Domain.Entities.Dependent
            {
                Id = dependentId,
                Name = "Outro Dependente",
                CaregiverId = otherCaregiverId
            });

        var useCase = new GetDependentByIdUseCase(
            dependentReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadOnlyRepositoryMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(dependentId));
    }


}
