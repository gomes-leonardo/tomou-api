namespace Tomou.UnitTests.UseCases.Medications.Register;
using AutoMapper;
using Moq;
using Shouldly;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Medications.Register;
using Tomou.Communication.Responses.Medications.Register;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.Medications;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.Exception.ExceptionsBase;
using Tomou.TestUtils.Medication.Register.Request;

public class RegisterMedicationUseCaseTest
{
    [Fact]
    public async Task ShouldRegisterMedicationSuccessfullyWhenUserIsCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsWriteOnlyRepositoryMock = new Mock<IMedicationsWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var dependentReadonlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var request = RequestRegisterMedicationJsonBuilder.Build();

        var dependentId = request.DependentId;   
        var userId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
        });

        var dependent = new Tomou.Domain.Entities.Dependent
        {
            CaregiverId = userId,
            Id = dependentId!.Value,
            Name = "Miguel Gomes"
        };

        dependentReadonlyRepositoryMock
             .Setup(d => d.GetByIdAsync(dependentId!.Value)).ReturnsAsync(dependent);


        mapperMock
            .Setup(m => m.Map<Tomou.Domain.Entities.Medication>(It.IsAny<object>()))
            .Returns(new Tomou.Domain.Entities.Medication { Name = request.Name });

        mapperMock
            .Setup(m => m.Map<ResponseRegisterMedicationJson>(It.IsAny<Tomou.Domain.Entities.Medication>()))
            .Returns<Tomou.Domain.Entities.Medication>(ent => new ResponseRegisterMedicationJson
            {
                Name = ent.Name,
                Message = ""
            });

        medicationsWriteOnlyRepositoryMock
            .Setup(r => r.Add(It.IsAny<Tomou.Domain.Entities.Medication>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        unitOfWorkMock
            .Setup(u => u.Commit())
            .Returns(Task.CompletedTask)
            .Verifiable();

        var useCase = new RegisterMedicationUseCase(
            mapperMock.Object,
            unitOfWorkMock.Object,
            medicationsWriteOnlyRepositoryMock.Object,
            dependentReadonlyRepositoryMock.Object,
            userReadonlyRepositoryMock.Object,
            userContextMock.Object
        );

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
    }

    [Fact]
    public async Task ShouldRegisterMedicationSuccessfullyWhenUserIsNotCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsWriteOnlyRepositoryMock = new Mock<IMedicationsWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var dependentReadonlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var request = RequestRegisterMedicationJsonBuilder.Build();
        request.DependentId = null;

        var userId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = false,
            Id = userId,
        });
       

        mapperMock
            .Setup(m => m.Map<Tomou.Domain.Entities.Medication>(It.IsAny<object>()))
            .Returns(new Tomou.Domain.Entities.Medication { Name = request.Name });

        mapperMock
            .Setup(m => m.Map<ResponseRegisterMedicationJson>(It.IsAny<Tomou.Domain.Entities.Medication>()))
            .Returns<Tomou.Domain.Entities.Medication>(ent => new ResponseRegisterMedicationJson
            {
                Name = ent.Name,
                Message = ""
            });

        medicationsWriteOnlyRepositoryMock
            .Setup(r => r.Add(It.IsAny<Tomou.Domain.Entities.Medication>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        unitOfWorkMock
            .Setup(u => u.Commit())
            .Returns(Task.CompletedTask)
            .Verifiable();

        var useCase = new RegisterMedicationUseCase(
            mapperMock.Object,
            unitOfWorkMock.Object,
            medicationsWriteOnlyRepositoryMock.Object,
            dependentReadonlyRepositoryMock.Object,
            userReadonlyRepositoryMock.Object,
            userContextMock.Object
        );

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
    }

    [Fact]
    public async Task ShouldThrowForbiddenAccessExceptionWhenUserIsNotFound()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var medicationsWriteOnlyRepositoryMock = new Mock<IMedicationsWriteOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var dependentReadonlyRepositoryMock =  new Mock<IDependentReadOnlyRepository>();

        var userId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId))
            .ReturnsAsync((Tomou.Domain.Entities.User?)null);

        var request = RequestRegisterMedicationJsonBuilder.Build();

        var useCase = new RegisterMedicationUseCase(
            mapperMock.Object,
            unitOfWorkMock.Object,
            medicationsWriteOnlyRepositoryMock.Object,
            dependentReadonlyRepositoryMock.Object,
            userReadonlyRepositoryMock.Object,
            userContextMock.Object
        );

        await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(request));
    }
} 