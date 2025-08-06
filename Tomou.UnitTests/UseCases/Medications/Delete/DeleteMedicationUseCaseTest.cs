using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Shouldly;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Medications.Delete;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.Medications.Filters;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.UnitTests.UseCases.Medications.Delete;
public class DeleteMedicationUseCaseTest
{
    [Fact]
    public async Task ShouldDeleteMedicationSuccessfully()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var medicationsWriteOnlyRepositoryMock = new Mock<IMedicationsWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();
        var medicationId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
            Dependents = new List<Tomou.Domain.Entities.Dependent>()
        });

        var medication = new Tomou.Domain.Entities.Medication
        {
            Id = medicationId,
            Name = "Dipirona",
            Dosage = "500mg",
            Dependent = new Tomou.Domain.Entities.Dependent
            {
                Id = dependentId,
                CaregiverId = userId
            }
        };

        medicationsReadOnlyRepositoryMock
            .Setup(r => r.GetMedicationsById(It.IsAny<MedicationsFilterById>()))
            .ReturnsAsync(medication);

        var useCase = new DeleteMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        await useCase.Execute(dependentId, medicationId);

        medicationsWriteOnlyRepositoryMock.Verify(r => r.Delete(medicationId), Times.Once);
        unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenMedicationNotFound()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var medicationsWriteOnlyRepositoryMock = new Mock<IMedicationsWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();
        var medicationId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
            Dependents = new List<Tomou.Domain.Entities.Dependent>()
        });

        medicationsReadOnlyRepositoryMock
            .Setup(r => r.GetMedicationsById(It.IsAny<MedicationsFilterById>()))
            .ReturnsAsync((Tomou.Domain.Entities.Medication?)null);

        var useCase = new DeleteMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(dependentId, medicationId));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenMedicationDoesNotBelongToUser()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var medicationsWriteOnlyRepositoryMock = new Mock<IMedicationsWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();
        var medicationId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
            Dependents = new List<Tomou.Domain.Entities.Dependent>()
        });

        medicationsReadOnlyRepositoryMock
            .Setup(r => r.GetMedicationsById(It.IsAny<MedicationsFilterById>()))
            .ReturnsAsync(new Tomou.Domain.Entities.Medication
            {
                Id = medicationId,
                UserId = anotherUserId
            });

        var useCase = new DeleteMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(dependentId, medicationId));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotFound()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var medicationsWriteOnlyRepositoryMock = new Mock<IMedicationsWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var medicationId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync((Tomou.Domain.Entities.User?)null);

        var useCase = new DeleteMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(null, medicationId));
    }

    [Fact]
    public async Task ShouldDeleteMedicationSuccessfullyWhenUserIsNotCaregiver()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var medicationsWriteOnlyRepositoryMock = new Mock<IMedicationsWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var medicationId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = false,
            Id = userId,
        });

        var medication = new Tomou.Domain.Entities.Medication
        {
            Id = medicationId,
            Name = "Dipirona",
            Dosage = "500mg",
            UserId = userId
        };

        medicationsReadOnlyRepositoryMock
            .Setup(r => r.GetMedicationsById(It.IsAny<MedicationsFilterById>()))
            .ReturnsAsync(medication);

        var useCase = new DeleteMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        await useCase.Execute(null, medicationId);

        medicationsWriteOnlyRepositoryMock.Verify(r => r.Delete(medicationId), Times.Once);
        unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotCaregiverAndTriesToAccessDependentMedication()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var medicationsWriteOnlyRepositoryMock = new Mock<IMedicationsWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();
        var medicationId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = false,
            Id = userId,
        });

        var useCase = new DeleteMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(dependentId, medicationId));
    }
}