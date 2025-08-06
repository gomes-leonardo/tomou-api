namespace Tomou.UnitTests.UseCases.Dependent.Update;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Shouldly;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Dependent.Update;
using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Communication.Responses.Dependent.Update;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;
using Tomou.TestUtils.Dependent.Update.Request;

public class UpdateDependentUseCaseTest
{
    [Fact]
    public async Task ShouldUpdateDependentSuccessfully()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var dependentUpdateOnlyRepositoryMock = new Mock<IDependentUpdateOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
        });

        var dependent = new Tomou.Domain.Entities.Dependent
        {
            Id = dependentId,
            Name = "Leonardo Gomes",
            CaregiverId = userId
        };

        dependentReadOnlyRepositoryMock
            .Setup(r => r.GetByIdAsync(dependentId))
            .ReturnsAsync(dependent);

        var request = RequestUpdateDependentJsonBuilder.Build();

        var expectedResponse = new ResponseUpdatedDependentJson
        {
            Id = dependentId,
            Name = request.Name
        };

        mapperMock
            .Setup(m => m.Map<ResponseUpdatedDependentJson>(It.IsAny<Tomou.Domain.Entities.Dependent>()))
            .Returns(expectedResponse);

        var useCase = new UpdateDependentUseCase(
            dependentUpdateOnlyRepositoryMock.Object,
            dependentReadOnlyRepositoryMock.Object,
            userReadonlyRepositoryMock.Object,
            mapperMock.Object,
            unitOfWorkMock.Object,
            userContextMock.Object
        );

        var result = await useCase.Execute(request, dependentId);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(dependentId);
        result.Name.ShouldBe(request.Name);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenDependentNotFound()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var dependentUpdateOnlyRepositoryMock = new Mock<IDependentUpdateOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
        });

        dependentReadOnlyRepositoryMock
            .Setup(r => r.GetByIdAsync(dependentId))
            .ReturnsAsync((Tomou.Domain.Entities.Dependent?)null);

        var request = RequestUpdateDependentJsonBuilder.Build();

        var useCase = new UpdateDependentUseCase(
            dependentUpdateOnlyRepositoryMock.Object,
            dependentReadOnlyRepositoryMock.Object,
            userReadonlyRepositoryMock.Object,
            mapperMock.Object,
            unitOfWorkMock.Object,
            userContextMock.Object
        );

        await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(request, dependentId));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenDependentDoesNotBelongToUser()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var dependentUpdateOnlyRepositoryMock = new Mock<IDependentUpdateOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
        });

        dependentReadOnlyRepositoryMock
            .Setup(r => r.GetByIdAsync(dependentId))
            .ReturnsAsync(new Tomou.Domain.Entities.Dependent
            {
                Id = dependentId,
                Name = "Outro Dependente",
                CaregiverId = anotherUserId
            });

        var request = RequestUpdateDependentJsonBuilder.Build();

        var useCase = new UpdateDependentUseCase(
            dependentUpdateOnlyRepositoryMock.Object,
            dependentReadOnlyRepositoryMock.Object,
            userReadonlyRepositoryMock.Object,
            mapperMock.Object,
            unitOfWorkMock.Object,
            userContextMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(request, dependentId));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotFound()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var dependentUpdateOnlyRepositoryMock = new Mock<IDependentUpdateOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync((Tomou.Domain.Entities.User?)null);

        var request = RequestUpdateDependentJsonBuilder.Build();

        var useCase = new UpdateDependentUseCase(
            dependentUpdateOnlyRepositoryMock.Object,
            dependentReadOnlyRepositoryMock.Object,
            userReadonlyRepositoryMock.Object,
            mapperMock.Object,
            unitOfWorkMock.Object,
            userContextMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(request, dependentId));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var dependentUpdateOnlyRepositoryMock = new Mock<IDependentUpdateOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = false,
            Id = userId,
        });

        var request = RequestUpdateDependentJsonBuilder.Build();

        var useCase = new UpdateDependentUseCase(
            dependentUpdateOnlyRepositoryMock.Object,
            dependentReadOnlyRepositoryMock.Object,
            userReadonlyRepositoryMock.Object,
            mapperMock.Object,
            unitOfWorkMock.Object,
            userContextMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(request, dependentId));
    }
}

