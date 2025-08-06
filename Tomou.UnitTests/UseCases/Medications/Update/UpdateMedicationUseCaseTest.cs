namespace Tomou.UnitTests.UseCases.Medications.Update;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Shouldly;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Medications.Update;
using Tomou.Communication.Requests.Medications.Update;
using Tomou.Communication.Responses.Medications.Update;
using Tomou.Domain.Entities;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.Medications.Filters;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;

public class UpdateMedicationUseCaseTest
{
    [Fact]
    public async Task ShouldUpdateMedicationSuccessfully()
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

        var request = new RequestUpdateMedicationJson
        {
            Name = "Paracetamol",
            Dosage = "750mg"
        };

        var expectedResponse = new ResponseUpdatedMedicationJson
        {
            Id = medicationId,
            Name = request.Name,
            Dosage = request.Dosage
        };

        mapperMock
            .Setup(m => m.Map<ResponseUpdatedMedicationJson>(It.IsAny<Tomou.Domain.Entities.Medication>()))
            .Returns(expectedResponse);

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
        result.Name.ShouldBe("Paracetamol");
        result.Dosage.ShouldBe("750mg");
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
            .Setup(r => r.GetMedicationsById(It.IsAny<MedicationsFilterById>()))
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
    public async Task ShouldThrowForbiddenAccessExceptionWhenMedicationDoesNotBelongToUser()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var medicationsWriteOnlyRepositoryMock = new Mock<IMedicationsWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid();
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
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync((Tomou.Domain.Entities.User?)null);

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
    public async Task ShouldUpdateMedicationSuccessfullyWhenUserIsNotCaregiver()
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

        var request = new RequestUpdateMedicationJson
        {
            Name = "Paracetamol",
            Dosage = "750mg"
        };

        var expectedResponse = new ResponseUpdatedMedicationJson
        {
            Id = medicationId,
            Name = request.Name,
            Dosage = request.Dosage
        };

        mapperMock
            .Setup(m => m.Map<ResponseUpdatedMedicationJson>(It.IsAny<Tomou.Domain.Entities.Medication>()))
            .Returns(expectedResponse);

        var useCase = new UpdateMedicationUseCase(
            medicationsWriteOnlyRepositoryMock.Object,
            medicationsReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        var result = await useCase.Execute(null, medicationId, request);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(medicationId);
        result.Name.ShouldBe("Paracetamol");
        result.Dosage.ShouldBe("750mg");
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotCaregiverAndTriesToAccessDependentMedication()
    {
        var mapperMock = new Mock<IMapper>();
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

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(dependentId, medicationId, request));
    }
} 