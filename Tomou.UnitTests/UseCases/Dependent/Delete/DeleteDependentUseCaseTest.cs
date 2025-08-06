using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Shouldly;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Dependent.Delete;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.UnitTests.UseCases.Dependent.Delete;
public class DeleteDependentUseCaseTest
{
    [Fact]
    public async Task ShouldDeleteDependentSuccessfully()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentWriteOnlyRepositoryMock = new Mock<IDependentWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
        });

        dependentWriteOnlyRepositoryMock
            .Setup(r => r.DeleteAsync(dependentId))
            .ReturnsAsync(true);

        var useCase = new DeleteDependentUseCase(
            unitOfWorkMock.Object,
            dependentWriteOnlyRepositoryMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object
        );

        await useCase.Execute(dependentId);

        dependentWriteOnlyRepositoryMock.Verify(r => r.DeleteAsync(dependentId), Times.Once);
        unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenDependentNotFound()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentWriteOnlyRepositoryMock = new Mock<IDependentWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
        });

        dependentWriteOnlyRepositoryMock
            .Setup(r => r.DeleteAsync(dependentId))
            .ReturnsAsync(false);

        var useCase = new DeleteDependentUseCase(
            unitOfWorkMock.Object,
            dependentWriteOnlyRepositoryMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object
        );

        await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(dependentId));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotCaregiver()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentWriteOnlyRepositoryMock = new Mock<IDependentWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = false,
            Id = userId,
        });

        var useCase = new DeleteDependentUseCase(
            unitOfWorkMock.Object,
            dependentWriteOnlyRepositoryMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(dependentId));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotFound()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentWriteOnlyRepositoryMock = new Mock<IDependentWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId))
            .ReturnsAsync((Tomou.Domain.Entities.User?)null);

        var useCase = new DeleteDependentUseCase(
            unitOfWorkMock.Object,
            dependentWriteOnlyRepositoryMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(dependentId));
    }
}
