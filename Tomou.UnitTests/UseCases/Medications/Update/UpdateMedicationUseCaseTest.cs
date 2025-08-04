namespace Tomou.UnitTests.UseCases.Medications.Update;
using AutoMapper;
using Moq;
using Shouldly;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Medications.Update;
using Tomou.Communication.Requests.Medications.Update;
using Tomou.Communication.Responses.Medications.Update;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;

public class UpdateMedicationUseCaseTest
{
    [Fact]
    public async Task ShouldUpdateMedicationSuccessfullyWhenUserIsCaregiver()
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

        var request = new RequestUpdateMedicationJson
        {
            Name = "Paracetamol",
            Dosage = "750mg",
            StartDate = DateOnly.FromDateTime(DateTime.Today),
            EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7)),
            TimesToTake = new List<string> { "08:00", "20:00" },
            DaysOfWeek = new List<string> { "monday", "tuesday", "wednesday" }
        };

        var updatedMedication = new Tomou.Domain.Entities.Medication
        {
            Id = medicationId,
            Name = request.Name,
            Dosage = request.Dosage
        };

        mapperMock
            .Setup(m => m.Map<Tomou.Domain.Entities.Medication>(It.IsAny<object>()))
            .Returns(updatedMedication);

        var expectedResponse = new ResponseUpdatedMedicationJson
        {
            Id = medicationId,
            Name = request.Name,
            Message = "Medicamento atualizado com sucesso"
        };

        mapperMock
            .Setup(m => m.Map<ResponseUpdatedMedicationJson>(It.IsAny<Tomou.Domain.Entities.Medication>()))
            .Returns(expectedResponse);

        medicationsWriteOnlyRepositoryMock
            .Setup(r => r.Update(It.IsAny<Tomou.Domain.Entities.Medication>()))
            .Verifiable();

        unitOfWorkMock
            .Setup(u => u.Commit())
            .Returns(Task.CompletedTask)
            .Verifiable();

        var useCase = new UpdateMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        var result = await useCase.Execute(userId, medicationId, request);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(medicationId);
        result.Name.ShouldBe(request.Name);
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

        var request = new RequestUpdateMedicationJson
        {
            Name = "Paracetamol",
            Dosage = "750mg"
        };

        var useCase = new UpdateMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(userId, medicationId, request));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var medicationsWriteOnlyRepositoryMock = new Mock<IMedicationsWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid(); // user logado
        var medicationId = Guid.NewGuid();

        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = false,
            Id = userId,
        });

        medicationsReadOnlyRepositoryMock
            .Setup(r => r.GetMedicationsById(It.IsAny<Guid>(), It.IsAny<bool>(), medicationId))
            .ReturnsAsync(new Tomou.Domain.Entities.Medication
            {
                Id = medicationId,
                UserId = Guid.NewGuid(),
                Name = "Paracetamol",
                Dosage = "750mg"
            });

        var request = new RequestUpdateMedicationJson
        {
            Name = "Paracetamol",
            Dosage = "750mg"
        };

        var useCase = new UpdateMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(userId, medicationId, request));
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

        var request = new RequestUpdateMedicationJson
        {
            Name = "Paracetamol",
            Dosage = "750mg"
        };

        var useCase = new UpdateMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(userId, medicationId, request));
    }
} 