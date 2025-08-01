namespace Tomou.UnitTests.UseCases.Medications.Delete;
using AutoMapper;
using Moq;
using Shouldly;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Medications.Delete;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;

public class DeleteMedicationUseCaseTest
{
    [Fact]
    public async Task ShouldDeleteMedicationSuccessfullyWhenUserIsCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
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
            IsCaregiver = true,
            Id = userId,
            Dependents = new List<Tomou.Domain.Entities.Dependent>()
        });

        var existingMedication = new Tomou.Domain.Entities.Medication
        {
            Id = medicationId,
            Name = "Dipirona",
            Dosage = "500mg"
        };

        medicationsReadOnlyRepositoryMock
            .Setup(r => r.GetMedicationsById(userId, true, medicationId))
            .ReturnsAsync(existingMedication);

        medicationsWriteOnlyRepositoryMock
            .Setup(r => r.Delete(medicationId))
            .ReturnsAsync(true)
            .Verifiable();

        unitOfWorkMock
            .Setup(u => u.Commit())
            .Returns(Task.CompletedTask)
            .Verifiable();

        var useCase = new DeleteMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        await useCase.Execute(userId, medicationId);

        medicationsWriteOnlyRepositoryMock.Verify(r => r.Delete(medicationId), Times.Once);
        unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public async Task ShouldDeleteMedicationSuccessfullyWhenUserIsNotCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
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

        var existingMedication = new Tomou.Domain.Entities.Medication
        {
            Id = medicationId,
            Name = "Dipirona",
            Dosage = "500mg"
        };

        medicationsReadOnlyRepositoryMock
            .Setup(r => r.GetMedicationsById(userId, false, medicationId))
            .ReturnsAsync(existingMedication);

        medicationsWriteOnlyRepositoryMock
            .Setup(r => r.Delete(medicationId))
            .ReturnsAsync(true)
            .Verifiable();

        unitOfWorkMock
            .Setup(u => u.Commit())
            .Returns(Task.CompletedTask)
            .Verifiable();

        var useCase = new DeleteMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        await useCase.Execute(userId, medicationId);

        medicationsWriteOnlyRepositoryMock.Verify(r => r.Delete(medicationId), Times.Once);
        unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenMedicationNotFound()
    {
        var mapperMock = new Mock<IMapper>();
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
            IsCaregiver = true,
            Id = userId,
            Dependents = new List<Tomou.Domain.Entities.Dependent>()
        });

        medicationsReadOnlyRepositoryMock
            .Setup(r => r.GetMedicationsById(userId, true, medicationId))
            .ReturnsAsync((Tomou.Domain.Entities.Medication?)null);

        var useCase = new DeleteMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(userId, medicationId));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotFound()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var medicationsWriteOnlyRepositoryMock = new Mock<IMedicationsWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var medicationId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId))
            .ReturnsAsync((Tomou.Domain.Entities.User?)null);

        var useCase = new DeleteMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(userId, medicationId));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenMedicationDoesNotBelongToUser()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var medicationsWriteOnlyRepositoryMock = new Mock<IMedicationsWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid(); // ID diferente
        var medicationId = Guid.NewGuid();

        userContextMock.Setup(d => d.GetUserId()).Returns(userId);

        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId))
            .ReturnsAsync(new Tomou.Domain.Entities.User
            {
                IsCaregiver = true,
                Id = userId
            });

        medicationsReadOnlyRepositoryMock
            .Setup(r => r.GetMedicationsById(userId, true, medicationId))
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

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(userId, medicationId));
    }

}