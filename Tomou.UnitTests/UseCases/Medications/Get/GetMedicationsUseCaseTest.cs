namespace Tomou.UnitTests.UseCases.Medications.Get;
using AutoMapper;
using Moq;
using Shouldly;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Medications.Get;
using Tomou.Communication.Responses.Medications.Get;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;

public class GetMedicationsUseCaseTest
{
    [Fact]
    public async Task ShouldGetMedicationsSuccessfullyWhenUserIsCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();

        var userId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
        });

        var dependentId = Guid.NewGuid();
        var dependent = new Tomou.Domain.Entities.Dependent
        {
            Id = dependentId,
            Name = "Elizabeth Rodrigues",
            CaregiverId = userId
        };

        dependentReadOnlyRepositoryMock
            .Setup(x => x.GetByIdAsync(dependentId))
            .ReturnsAsync(dependent);

        var medications = new List<Tomou.Domain.Entities.Medication>
        {
            new() { Id = Guid.NewGuid(), Name = "Dipirona" },
            new() { Id = Guid.NewGuid(), Name = "Paracetamol" }
        };

        medicationsReadOnlyRepositoryMock
            .Setup(r => r.GetMedications(dependentId, true, null, true))
            .ReturnsAsync(medications);

        var expectedResponse = new ResponseMedicationsJson
        {
            Medications = medications.Select(m => new ResponseMedicationShortJson { Id = m.Id, Name = m.Name }).ToList()
        };


        mapperMock
             .Setup(m => m.Map<List<ResponseMedicationShortJson>>(medications))
             .Returns(expectedResponse.Medications);

        var useCase = new GetMedicationsUseCase(
            medicationsReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            dependentReadOnlyRepositoryMock.Object
        );

        var result = await useCase.Execute(dependentId, null, true);

        result.ShouldNotBeNull();
        result.Medications.ShouldNotBeNull();
        result.Medications.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ShouldGetMedicationsSuccessfullyWhenUserIsNotCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsReadOnlyRepositoryMock = new Mock<IMedicationsReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();

        var userId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = false,
            Id = userId,
        });

        var medications = new List<Tomou.Domain.Entities.Medication>
        {
            new() { Id = Guid.NewGuid(), Name = "Dipirona" },
            new() { Id = Guid.NewGuid(), Name = "Paracetamol" }
        };

        medicationsReadOnlyRepositoryMock
            .Setup(r => r.GetMedications(userId, false, null, true))
            .ReturnsAsync(medications);

        var expectedResponse = new ResponseMedicationsJson
        {
            Medications = medications.Select(m => new ResponseMedicationShortJson { Id = m.Id, Name = m.Name }).ToList()
        };

        mapperMock
             .Setup(m => m.Map<List<ResponseMedicationShortJson>>(medications))
             .Returns(expectedResponse.Medications);

        var useCase = new GetMedicationsUseCase(
            medicationsReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadonlyRepositoryMock.Object,
            dependentReadOnlyRepositoryMock.Object
        );

        var result = await useCase.Execute(null, null, true);

        result.ShouldNotBeNull();
        result.Medications.ShouldNotBeNull();
        result.Medications.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserNotFound()
    {
        var userId = Guid.NewGuid();

        var userContextMock = new Mock<IUserContext>();
        var userRepositoryMock = new Mock<IUserReadOnlyRepository>();
        userContextMock.Setup(x => x.GetUserId()).Returns(userId);
        userRepositoryMock.Setup(x => x.GetUserById(userId)).ReturnsAsync((Tomou.Domain.Entities.User)null!);

        var useCase = new GetMedicationsUseCase(
            Mock.Of<IMedicationsReadOnlyRepository>(),
            Mock.Of<IMapper>(),
            userContextMock.Object,
            userRepositoryMock.Object,
            Mock.Of<IDependentReadOnlyRepository>()
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() =>
            useCase.Execute(null, null, true));
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenCaregiverDoesNotProvideDependentId()
    {
        var userId = Guid.NewGuid();

        var userContextMock = new Mock<IUserContext>();
        var userRepositoryMock = new Mock<IUserReadOnlyRepository>();
        userContextMock.Setup(x => x.GetUserId()).Returns(userId);
        userRepositoryMock.Setup(x => x.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            Id = userId,
            IsCaregiver = true
        });

        var useCase = new GetMedicationsUseCase(
            Mock.Of<IMedicationsReadOnlyRepository>(),
            Mock.Of<IMapper>(),
            userContextMock.Object,
            userRepositoryMock.Object,
            Mock.Of<IDependentReadOnlyRepository>()
        );

        await Should.ThrowAsync<NotFoundException>(() =>
            useCase.Execute(null, null, true));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenDependentNotFound()
    {
        var userId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();

        var userContextMock = new Mock<IUserContext>();
        var userRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentRepositoryMock = new Mock<IDependentReadOnlyRepository>();

        userContextMock.Setup(x => x.GetUserId()).Returns(userId);
        userRepositoryMock.Setup(x => x.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            Id = userId,
            IsCaregiver = true
        });
        dependentRepositoryMock.Setup(x => x.GetByIdAsync(dependentId)).ReturnsAsync((Tomou.Domain.Entities.Dependent)null!);

        var useCase = new GetMedicationsUseCase(
            Mock.Of<IMedicationsReadOnlyRepository>(),
            Mock.Of<IMapper>(),
            userContextMock.Object,
            userRepositoryMock.Object,
            dependentRepositoryMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() =>
            useCase.Execute(dependentId, null, true));
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenDependentBelongsToAnotherCaregiver()
    {
        var userId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();

        var userContextMock = new Mock<IUserContext>();
        var userRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentRepositoryMock = new Mock<IDependentReadOnlyRepository>();

        userContextMock.Setup(x => x.GetUserId()).Returns(userId);
        userRepositoryMock.Setup(x => x.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            Id = userId,
            IsCaregiver = true
        });

        dependentRepositoryMock.Setup(x => x.GetByIdAsync(dependentId)).ReturnsAsync(new Tomou.Domain.Entities.Dependent
        {
            Id = dependentId,
            CaregiverId = anotherUserId 
        });

        var useCase = new GetMedicationsUseCase(
            Mock.Of<IMedicationsReadOnlyRepository>(),
            Mock.Of<IMapper>(),
            userContextMock.Object,
            userRepositoryMock.Object,
            dependentRepositoryMock.Object
        );

        await Should.ThrowAsync<ForbiddenAccessException>(() =>
            useCase.Execute(dependentId, null, true));
    }

}