namespace Tomou.UnitTests.UseCases.Medications.GetById;
using AutoMapper;
using Moq;
using Shouldly;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Medications.GetById;
using Tomou.Communication.Responses.Medications.Get;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;
using Tomou.Domain.Repositories.Medications.Filters;

public class GetMedicationByIdUseCaseTest
{
    [Fact]
    public async Task ShouldGetMedicationByIdSuccessfullyWhenUserIsCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();

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
            UserId = userId,
            Id = medicationId,
            Name = "Dipirona",
            Dosage = "500mg"
        };

        medicationsReadOnlyRepositoryMock
            .Setup(r => r.GetMedicationsById(It.IsAny<MedicationsFilterById>()))
            .ReturnsAsync(medication);

        var expectedResponse = new ResponseMedicationShortJson
        {
            Id = medication.Id,
            Name = medication.Name
        };

        mapperMock
            .Setup(m => m.Map<ResponseMedicationShortJson>(It.IsAny<Tomou.Domain.Entities.Medication>()))
            .Returns(expectedResponse);

        var useCase = new GetMedicationByIdUseCase(
            medicationsReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            dependentReadOnlyRepositoryMock.Object
        );

        var result = await useCase.Execute(userId, medicationId);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(medicationId);
        result.Name.ShouldBe("Dipirona");
    }

    [Fact]
    public async Task ShouldGetMedicationByIdSuccessfullyWhenUserIsNotCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();

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
            UserId = userId,
            Id = medicationId,
            Name = "Dipirona",
            Dosage = "500mg"
        };

        medicationsReadOnlyRepositoryMock
            .Setup(r => r.GetMedicationsById(It.IsAny<MedicationsFilterById>()))
            .ReturnsAsync(medication);

        var expectedResponse = new ResponseMedicationShortJson
        {
            Id = medication.Id,
            Name = medication.Name
        };

        mapperMock
            .Setup(m => m.Map<ResponseMedicationShortJson>(It.IsAny<Tomou.Domain.Entities.Medication>()))
            .Returns(expectedResponse);

        var useCase = new GetMedicationByIdUseCase(
            medicationsReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            dependentReadOnlyRepositoryMock.Object
        );

        var result = await useCase.Execute(userId, medicationId);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(medicationId);
        result.Name.ShouldBe("Dipirona");
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionlWhenMedicationNotFound()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();


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

        var useCase = new GetMedicationByIdUseCase(
            medicationsReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            dependentReadOnlyRepositoryMock.Object
        );

        await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(userId, medicationId));

    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenDependentNotFound()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var dependentId = Guid.NewGuid();

        var userId = Guid.NewGuid();
        var medicationId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
        });

        dependentReadOnlyRepositoryMock.Setup(x => x.GetByIdAsync(dependentId)).ReturnsAsync((Tomou.Domain.Entities.Dependent)null!);

        var useCase = new GetMedicationByIdUseCase(
           medicationsReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            dependentReadOnlyRepositoryMock.Object
        );

        await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(dependentId, medicationId));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotFound()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var medicationId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId))
            .ReturnsAsync((Tomou.Domain.Entities.User?)null);

        var useCase = new GetMedicationByIdUseCase(
            medicationsReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            dependentReadOnlyRepositoryMock.Object
        );

        await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(userId, medicationId));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenMedicationDoesNotBelongToUser()
    {
        var mapperMock = new Mock<IMapper>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid(); 
        var medicationId = Guid.NewGuid();

        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            Id = userId,
            IsCaregiver = false
        });

        medicationsReadOnlyRepositoryMock
            .Setup(r => r.GetMedicationsById(It.IsAny<MedicationsFilterById>()))
            .ReturnsAsync(new Tomou.Domain.Entities.Medication
            {
                Id = medicationId,
                UserId = anotherUserId
            });

        var useCase = new GetMedicationByIdUseCase(
            medicationsReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            dependentReadOnlyRepositoryMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() => useCase.Execute(userId, medicationId));
    }

}